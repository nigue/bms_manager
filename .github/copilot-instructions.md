# Instrucciones de GitHub Copilot para este proyecto WPF

## 1. Objetivo y contexto

Este proyecto utiliza **C# y WPF** para crear una aplicación de escritorio con una arquitectura mantenible, basada en **MVVM**, separación por capas e **inyección de dependencias**.

Al proponer código nuevo o modificar código existente, Copilot debe respetar estas instrucciones. Prioriza soluciones simples, explícitas, fáciles de probar y coherentes con la estructura actual. No agregues bibliotecas ni patrones innecesarios para resolver problemas pequeños.

Usa la versión de .NET indicada en el archivo `.csproj`. No cambies el framework objetivo ni la versión de paquetes sin una razón concreta.

## 2. Arquitectura general

Organiza el código en las siguientes capas y directorios:

```text
MiProyectoWpf/
├── .github/
│   └── copilot-instructions.md
├── App.xaml
├── App.xaml.cs
├── MiProyectoWpf.csproj
│
├── Presentation/
│   ├── Views/
│   ├── ViewModels/
│   ├── Commands/
│   └── DependencyInjection.cs       # Opcional cuando haya suficientes registros
│
├── Application/
│   ├── Services/
│   └── DependencyInjection.cs       # Opcional cuando haya suficientes registros
│
├── Domain/
│   ├── Repositories/
│   └── Models/                      # Solo cuando el dominio necesite modelos propios
│
└── Infrastructure/
    ├── Repositories/
    └── DependencyInjection.cs       # Opcional cuando haya suficientes registros
```

No es obligatorio crear de inmediato todos los directorios o archivos opcionales. Añádelos cuando exista código que lo justifique.

### 2.1 Presentation

Contiene la interfaz WPF y la lógica de presentación.

- `Views/`: ventanas y controles WPF, normalmente archivos `.xaml` y sus archivos `.xaml.cs` asociados.
- `ViewModels/`: propiedades que la interfaz muestra, estado de la pantalla y comandos disponibles para el usuario.
- `Commands/`: implementaciones reutilizables de `ICommand`, como `RelayCommand`, cuando sean necesarias.
- `DependencyInjection.cs`: registra componentes de presentación si se decide agrupar el registro por capa.

La capa de presentación puede invocar contratos de `Application`. No debe acceder directamente a los repositories concretos de `Infrastructure` ni ejecutar SQL, llamadas HTTP o persistencia.

### 2.2 Application

Coordina operaciones que la aplicación ofrece al usuario.

- `Services/`: servicios que implementan operaciones de aplicación, coordinan el flujo y aplican validaciones de aplicación sencillas cuando corresponda.
- `DependencyInjection.cs`: registra servicios de esta capa si se utiliza registro modular.

Un servicio puede depender de contratos del dominio, por ejemplo `INameRepository`. No debe depender de una implementación concreta de `Infrastructure`.

No conviertas automáticamente cada método en una clase de servicio nueva. Introduce clases y abstracciones cuando ayuden a separar responsabilidades o probar el comportamiento.

### 2.3 Domain

Contiene los conceptos y contratos del dominio que deben mantenerse independientes de WPF y de los mecanismos de almacenamiento.

- `Repositories/`: interfaces que expresan las operaciones de persistencia necesarias para el dominio o los casos de uso, por ejemplo `INameRepository`.
- `Models/`: entidades, objetos de valor u otros modelos de dominio cuando el problema los necesite.

El dominio no debe depender de `Presentation`, `Application`, `Infrastructure`, WPF, XAML ni del contenedor de inyección de dependencias.

No crees un modelo únicamente para envolver un `string` si no aporta identidad, reglas o significado de dominio. Para ejemplos pequeños, un tipo primitivo puede ser suficiente.

### 2.4 Infrastructure

Contiene los detalles técnicos concretos de acceso a recursos externos.

- `Repositories/`: implementaciones de interfaces de repository, como almacenamiento en memoria, SQLite, SQL Server o acceso a una API.
- `DependencyInjection.cs`: registra las implementaciones concretas si se utiliza registro modular.

Las implementaciones concretas deben implementar el contrato correspondiente. El resto de la aplicación debe depender de la interfaz, no de la implementación.

Una implementación `InMemory...` sirve para demostraciones y pruebas simples; no debe describirse como una base de datos persistente. Sus datos desaparecen al finalizar el proceso, salvo que se implemente una persistencia real.

## 3. Dirección de las dependencias

Respeta estas reglas al diseñar referencias entre capas:

- `Domain` no depende de ninguna otra capa del proyecto.
- `Application` puede depender de `Domain`.
- `Infrastructure` implementa contratos definidos por `Domain` y puede depender de `Domain`.
- `Presentation` puede depender de contratos de `Application`.
- El punto de entrada (`App.xaml.cs`) actúa como **composition root** y conecta las implementaciones concretas con sus interfaces.
- Evita que `Application` dependa de `Infrastructure` para construir repositories concretos.
- Evita referencias circulares entre proyectos o capas.

Para un proyecto en un único `.csproj`, estas reglas se aplican como límites de arquitectura y organización. Si el proyecto crece, se pueden separar las capas en proyectos `.csproj` independientes.

## 4. MVVM en WPF

### 4.1 View (XAML y code-behind)

La vista define los controles, el diseño y los bindings.

- Usa `Binding` para conectar los controles con propiedades y comandos del ViewModel.
- Para entradas de texto que deben actualizarse mientras se escribe, considera `Mode=TwoWay` y `UpdateSourceTrigger=PropertyChanged`.
- Los botones que ejecutan una acción deben enlazarse a un `ICommand` mediante `Command`, en vez de incluir lógica de negocio en un manejador `Click`.
- El code-behind debe limitarse a tareas específicas de la vista, como `InitializeComponent`, inicialización visual o asignación del `DataContext` cuando corresponda.
- No pongas reglas de negocio, persistencia, consultas a repositories ni coordinación de servicios en archivos `.xaml.cs`.

### 4.2 ViewModel

Un ViewModel representa el estado y las acciones de una pantalla.

- Expón propiedades públicas para los valores que necesite la vista.
- Implementa `INotifyPropertyChanged` cuando los cambios deban reflejarse en los bindings, o usa una solución equivalente ya adoptada por el proyecto.
- Expón comandos mediante `ICommand`.
- Recibe las dependencias por constructor.
- Coordina la interacción entre la vista y los servicios de `Application`.
- Actualiza el estado de presentación, por ejemplo un mensaje de éxito o error.
- No accede directamente a infraestructura ni crea repositories concretos.
- Evita colocar reglas de negocio complejas dentro del ViewModel.

### 4.3 Commands

Un `RelayCommand` permite ejecutar un método del ViewModel desde XAML.

- Implementa `ICommand` y delega la ejecución en un método o delegado proporcionado.
- Implementa `CanExecute` cuando la disponibilidad de una acción dependa del estado actual.
- Si cambia una condición que afecta a `CanExecute`, asegúrate de que WPF vuelva a evaluar el comando usando un mecanismo apropiado.
- No introduzcas un comando para cada operación interna; úsalo para acciones que necesita invocar la interfaz.
- Si el proyecto incorpora una biblioteca MVVM, utiliza sus comandos de forma consistente en vez de mantener dos sistemas paralelos sin necesidad.

## 5. Servicios y repositories

### 5.1 Servicios de Application

Los servicios implementan operaciones que el ViewModel necesita solicitar.

- Declara una interfaz para un servicio cuando el consumidor deba depender de un contrato o sea útil sustituirlo en pruebas.
- Recibe sus dependencias por constructor.
- Valida las reglas propias de la operación en la capa adecuada.
- Delega el almacenamiento en una interfaz de repository.
- No crea instancias de repositories concretos.
- No conoce detalles de la interfaz visual ni actualiza controles WPF.
- Define de manera explícita si una operación debe ser síncrona o asíncrona. Si se trabaja con I/O real, favorece `async`/`await` cuando el proveedor lo soporte y evita bloquear el hilo de la interfaz.

### 5.2 Contratos de repository

Las interfaces de repository declaran las operaciones de almacenamiento requeridas, sin revelar su mecanismo concreto.

- Usa nombres que expresen intención, como `Save`, `FindById` o `GetAll`, ajustados al lenguaje del dominio.
- No incluyas tipos específicos de SQLite, SQL Server, archivos o HTTP en un contrato de dominio si no son parte esencial del dominio.
- Mantén las firmas pequeñas y enfocadas en las operaciones necesarias.

### 5.3 Implementaciones de repository

Las clases de `Infrastructure/Repositories` implementan esos contratos.

- Concentra allí el acceso concreto al almacenamiento.
- No pongas reglas de presentación en un repository.
- No dupliques en el repository la coordinación que corresponde al servicio.
- Si se usa una lista en memoria para una demostración, deja claro que no persiste entre ejecuciones.
- Maneja y transforma errores externos solo cuando sea responsabilidad de esa implementación; no ocultes fallos devolviendo resultados falsos o vacíos sin justificación.

## 6. Inyección de dependencias con Generic Host

Usa preferentemente el contenedor integrado de Microsoft (`Microsoft.Extensions.DependencyInjection`) y `Microsoft.Extensions.Hosting` cuando el proyecto necesite inyección de dependencias y ciclo de vida de la aplicación.

### 6.1 Composition root

`App.xaml.cs` es el punto de entrada donde se configura e inicia el host de .NET y se muestra la ventana principal.

- Registra interfaces junto a sus implementaciones.
- Resuelve la ventana principal desde el contenedor.
- Gestiona el inicio y el cierre del host.
- No concentres aquí la lógica de negocio ni las implementaciones de servicios/repositories.
- No crees manualmente toda la cadena de dependencias si ya está configurada en el contenedor.

En aplicaciones pequeñas, los registros pueden permanecer en `App.xaml.cs`. Cuando aumenten, agrúpalos en métodos de extensión por capa, como `AddApplication()`, `AddInfrastructure()` y `AddPresentation()`. Estos métodos deben registrar únicamente tipos de su área y ser llamados desde el composition root.

### 6.2 Ciclos de vida

Elige el ciclo de vida de cada servicio según su responsabilidad:

- `AddSingleton`: una instancia compartida durante la vida del contenedor. Adecuado para componentes sin estado mutable por usuario y para un almacenamiento en memoria que deba conservar una única colección durante la ejecución.
- `AddTransient`: crea una nueva instancia cada vez que se solicita. Adecuado para ViewModels u objetos ligeros cuando se necesita una instancia nueva por resolución.
- `AddScoped`: comparte una instancia dentro de un scope explícito. En WPF no existe automáticamente un scope por petición HTTP; úsalo solo cuando la aplicación gestione ese ámbito conscientemente.

No registres todo como singleton por comodidad. Revisa el estado mutable, el ciclo de vida de las ventanas y las dependencias de cada clase. Evita que una instancia singleton capture una dependencia de vida más corta de forma incorrecta.

### 6.3 Constructores

- Utiliza inyección por constructor como mecanismo normal.
- Las dependencias requeridas deben recibirse explícitamente en el constructor.
- Evita el patrón Service Locator: las clases de negocio no deben recibir `IServiceProvider` para buscar arbitrariamente sus propias dependencias.
- Solo el composition root debe resolver servicios de manera directa cuando necesite iniciar componentes, como la ventana principal.

## 7. Guía de archivos del ejemplo base

La siguiente lista explica el propósito de los archivos usados en el flujo de una caja de texto y un botón Guardar.

| Archivo | Responsabilidad |
|---|---|
| `App.xaml` | Declara la aplicación WPF y sus eventos de ciclo de vida. Si el inicio se controla en código, no configures simultáneamente un `StartupUri` que cree otra ventana principal. |
| `App.xaml.cs` | Crea y configura el Generic Host, registra dependencias, resuelve y muestra la ventana principal, y cierra el host al finalizar. |
| `Presentation/Views/MainWindow.xaml` | Define la caja de texto, el botón y los mensajes; enlaza `Text` a `Name`, `Command` a `SaveCommand` y el mensaje a `StatusMessage`. |
| `Presentation/Views/MainWindow.xaml.cs` | Inicializa la vista y asigna el `DataContext`; no almacena la lógica de guardar. |
| `Presentation/ViewModels/MainViewModel.cs` | Mantiene el nombre y el mensaje de estado, expone `SaveCommand`, llama al servicio y notifica cambios a WPF. |
| `Presentation/Commands/RelayCommand.cs` | Implementa `ICommand` y conecta la acción del botón con un método del ViewModel. |
| `Application/Services/INameService.cs` | Declara la operación de aplicación para guardar un nombre. |
| `Application/Services/NameService.cs` | Valida y normaliza el nombre, y llama a `INameRepository`. |
| `Domain/Repositories/INameRepository.cs` | Declara el contrato abstracto de almacenamiento. |
| `Infrastructure/Repositories/InMemoryNameRepository.cs` | Implementa el contrato agregando nombres a una lista en memoria. No es una base de datos persistente. |
| `Presentation/DependencyInjection.cs` | Opcional: agrupa registros de Views y ViewModels. |
| `Application/DependencyInjection.cs` | Opcional: agrupa registros de los servicios de aplicación. |
| `Infrastructure/DependencyInjection.cs` | Opcional: agrupa registros de las implementaciones concretas. |

No crees todos estos archivos por obligación en cada funcionalidad. Reutiliza componentes existentes y añade archivos únicamente cuando haya una responsabilidad diferenciada.

## 8. Manejo de errores y estado de la interfaz

- No uses excepciones como flujo normal para validaciones que puedan representarse de manera natural con resultados; sigue el estilo ya establecido por el proyecto.
- Cuando se usen excepciones para comunicar una entrada inválida, captura en el nivel que pueda presentar un mensaje útil al usuario.
- No muestres trazas técnicas ni detalles sensibles al usuario final.
- No silencies errores de persistencia. Distingue una validación de entrada de un fallo inesperado de infraestructura.
- Actualiza propiedades enlazadas mediante notificación para que la interfaz refleje los cambios.
- No limpies el formulario ni muestres éxito hasta que la operación de guardado haya terminado correctamente.
- Si una operación de I/O es asíncrona, maneja sus errores y estado de ejecución de forma compatible con WPF; evita operaciones largas síncronas en el hilo UI.

## 9. Pruebas y testabilidad

Diseña clases de forma que se puedan probar sin abrir ventanas WPF ni utilizar una base de datos real.

- Prueba las reglas y validaciones de los servicios de forma independiente.
- Sustituye `INameRepository` por una implementación de prueba cuando evalúes un servicio.
- Prueba el ViewModel usando un servicio falso o simulado, comprobando el estado resultante y las interacciones relevantes.
- Prueba la implementación concreta del repository por separado cuando corresponda.
- No requieras que una prueba unitaria inicie `App.xaml.cs`, arranque el Generic Host o muestre una ventana.
- Evita acoplar pruebas a detalles visuales que no sean necesarios para comprobar el comportamiento.

## 10. Convenciones de código

- Sigue las convenciones habituales de C#: PascalCase para tipos y miembros públicos, camelCase para parámetros y variables locales, y `_camelCase` para campos privados de instancia.
- Usa `namespace` coherentes con los directorios.
- Usa nullable reference types de acuerdo con la configuración del `.csproj`; no desactives la nulabilidad para ocultar advertencias.
- Prefiere clases pequeñas con una responsabilidad clara.
- Usa interfaces cuando definan un límite útil o permitan sustituir una dependencia; no crees interfaces sin propósito.
- Prefiere métodos y propiedades con nombres que expliquen su intención.
- Evita duplicar lógica, crear capas vacías o añadir abstracciones especulativas.
- Mantén las dependencias por constructor y evita variables globales o singletons estáticos para acceder a servicios.
- Conserva los patrones y convenciones ya establecidos salvo que exista una razón clara para cambiarlos.

## 11. Qué debe hacer Copilot al proponer cambios

Antes de generar o modificar código:

1. Identifica la responsabilidad de la funcionalidad y la capa donde pertenece.
2. Revisa si ya existe una clase, interfaz, servicio, comando o repository que pueda reutilizarse.
3. Respeta la dirección de dependencias y evita acoplar Presentation a Infrastructure.
4. Explica brevemente qué archivos nuevos o modificados se necesitan y por qué.
5. Si cambia una interfaz o constructor, identifica los lugares donde deben actualizarse los registros de DI y las pruebas.
6. Prefiere la solución mínima que mantenga la separación de responsabilidades.
7. No inventes APIs de paquetes ni cambies versiones del framework por iniciativa propia; utiliza las dependencias declaradas en el proyecto y verifica compatibilidad.
8. Cuando una decisión de arquitectura importante no esté definida, elige la alternativa más sencilla compatible con estas instrucciones y señala la suposición.

## 12. Criterios de aceptación arquitectónica

Una implementación se considera coherente con esta arquitectura cuando:

- La vista declara controles y bindings sin lógica de negocio.
- Los botones llaman a comandos y estos invocan métodos del ViewModel.
- El ViewModel coordina el estado de la interfaz y depende de contratos de Application.
- Los servicios de Application coordinan operaciones y dependen de contratos, no de implementaciones concretas de almacenamiento.
- Los contratos de repository están separados de sus implementaciones.
- Infrastructure contiene los detalles concretos de persistencia o comunicación externa.
- El contenedor de DI conecta las interfaces con implementaciones en el composition root o en módulos de registro por capa.
- Se puede probar la lógica principal sin iniciar la interfaz WPF.
- No se añaden complejidad, paquetes ni archivos que no estén justificados por la necesidad actual.
