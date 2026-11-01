---
applyTo: "**"
---

# Arquitectura del proyecto WPF

El proyecto utiliza C# y WPF con MVVM, separación por capas e inyección de dependencias. Respeta la estructura existente y prioriza cambios simples, explícitos y fáciles de probar. Usa el framework y las versiones de paquetes definidos en el `.csproj`; no los cambies sin una razón concreta.

## Capas y responsabilidades

- **Presentation** contiene las vistas WPF, ViewModels, comandos y el proveedor de localización de la interfaz. Puede invocar contratos de Application, pero no debe acceder a repositories concretos ni ejecutar SQL, llamadas HTTP o persistencia.
- **Application** coordina las operaciones que ofrece la aplicación, aplica validaciones propias del caso de uso y delega el almacenamiento mediante contratos de Domain. No depende de implementaciones de Infrastructure ni conoce la interfaz visual.
- **Domain** contiene los conceptos y contratos independientes de WPF y del almacenamiento, como modelos necesarios para el dominio e interfaces de repositories. No depende de ninguna otra capa.
- **Infrastructure** implementa los contratos de Domain y concentra los detalles técnicos de persistencia y comunicación externa. El resto de la aplicación depende de las interfaces, no de estas implementaciones.
- **Localization** contiene recursos de texto por idioma, por ejemplo `en.json` y `es.json`. Los recursos son datos; la lógica para cargarlos y exponerlos a las vistas pertenece a `Presentation/Localization`. Mantén las claves consistentes entre idiomas.

Organiza el código en directorios por capa, como `Presentation/Views`, `Presentation/ViewModels`, `Presentation/Commands`, `Application/Services`, `Domain/Repositories` e `Infrastructure/Repositories`. No crees directorios, modelos, interfaces ni capas vacías por obligación: añádelos cuando exista una responsabilidad que lo justifique. No crees un modelo que solo envuelva un tipo primitivo si no aporta identidad, reglas o significado de dominio.

## Dirección de dependencias

- `Domain` no depende de otras capas, WPF, XAML ni del contenedor de DI.
- `Application` puede depender de `Domain`.
- `Infrastructure` implementa contratos de `Domain` y puede depender de `Domain`.
- `Presentation` puede depender de contratos de `Application`.
- `App.xaml.cs` es el composition root: conecta contratos e implementaciones concretas.
- Evita referencias circulares y evita que `Application` construya o dependa de repositories concretos.

Estas reglas se aplican también si todas las capas están en un único `.csproj`. Si el proyecto crece, pueden separarse en proyectos independientes.

## MVVM en WPF

- Las vistas declaran controles, diseño y bindings. Usa bindings para propiedades y comandos; para entradas que deban actualizarse mientras se escribe, considera `Mode=TwoWay` y `UpdateSourceTrigger=PropertyChanged`.
- Los botones invocan acciones mediante `ICommand`, no mediante lógica de negocio en manejadores `Click`.
- El code-behind se limita a tareas propias de la vista, como `InitializeComponent`, inicialización visual o asignación del `DataContext`.
- Los ViewModels exponen el estado y los comandos que necesita la vista, implementan `INotifyPropertyChanged` o la solución equivalente ya adoptada, reciben dependencias por constructor y coordinan llamadas a servicios de Application.
- Un ViewModel no accede a Infrastructure, no crea repositories concretos y no contiene reglas de negocio complejas.
- Usa `RelayCommand` u otro `ICommand` reutilizable cuando sea necesario. Implementa `CanExecute` si corresponde y vuelve a evaluar el comando cuando cambie la condición. Si el proyecto ya usa una biblioteca MVVM, sigue ese patrón en vez de mantener dos sistemas paralelos.

## Servicios y repositories

- Los servicios de Application reciben sus dependencias por constructor, coordinan la operación, validan lo que corresponda y delegan el almacenamiento mediante interfaces. Declara una interfaz de servicio cuando aporte un contrato útil o facilite sustituirlo en pruebas.
- Los contratos de repository expresan operaciones necesarias con nombres orientados a intención, como `Save`, `FindById` o `GetAll`, sin exponer detalles de SQLite, SQL Server, archivos o HTTP.
- Las implementaciones de repositories concentran el acceso concreto a recursos externos. No contienen lógica de presentación ni duplican la coordinación propia de los servicios.
- Define explícitamente si una operación es síncrona o asíncrona. Para I/O real, favorece `async`/`await` cuando el proveedor lo soporte y evita bloquear el hilo de interfaz.
- Una implementación en memoria es útil para demostraciones y pruebas, pero no es persistente: sus datos desaparecen al finalizar el proceso, salvo que se implemente persistencia real.

## Inyección de dependencias

Cuando la aplicación necesite DI y gestión del ciclo de vida, prefiere `Microsoft.Extensions.DependencyInjection` y `Microsoft.Extensions.Hosting`.

- `App.xaml.cs` configura e inicia el host, registra dependencias, resuelve y muestra la ventana principal, y cierra el host al finalizar. No concentra lógica de negocio ni implementaciones de servicios o repositories.
- Registra interfaces con sus implementaciones. En una aplicación pequeña los registros pueden estar en `App.xaml.cs`; cuando aumenten, agrúpalos en métodos de extensión por capa, como `AddApplication()`, `AddInfrastructure()` y `AddPresentation()`.
- Elige el ciclo de vida según el estado y la responsabilidad: `Singleton` para instancias compartidas durante la vida del contenedor cuando sea seguro; `Transient` para objetos ligeros que deban crearse en cada resolución; `Scoped` solo si la aplicación gestiona explícitamente ese ámbito.
- Evita que una instancia singleton capture una dependencia de vida más corta. No registres todo como singleton por comodidad.
- Usa inyección por constructor para dependencias requeridas. No uses `IServiceProvider` como Service Locator fuera del composition root.
- Si el inicio se controla desde código, no configures a la vez un `StartupUri` que cree otra ventana principal.

## Errores y estado de la interfaz

- No uses excepciones como flujo normal para validaciones que puedan expresarse naturalmente como resultados; sigue el estilo ya establecido.
- Presenta los errores de entrada con mensajes útiles y no expongas trazas técnicas ni detalles sensibles al usuario.
- No ocultes errores de persistencia devolviendo valores falsos o colecciones vacías sin justificación; distingue validaciones de fallos inesperados de infraestructura.
- Actualiza las propiedades enlazadas mediante notificación. No limpies el formulario ni muestres éxito hasta que la operación haya finalizado correctamente.
- Para operaciones asíncronas, gestiona errores y estado de ejecución de forma compatible con WPF.

## Pruebas y convenciones

- Diseña la lógica para probarla sin abrir ventanas WPF ni iniciar el host. Sustituye contratos de repositories o servicios por implementaciones de prueba al comprobar servicios y ViewModels.
- Prueba las implementaciones concretas de repositories por separado cuando corresponda. Evita acoplar pruebas unitarias a detalles visuales.
- Usa PascalCase para tipos y miembros públicos, camelCase para parámetros y variables locales, y `_camelCase` para campos privados de instancia. Mantén namespaces coherentes con los directorios.
- Respeta la configuración de nullable reference types del `.csproj`; no la desactives para ocultar advertencias.
- Mantén clases pequeñas y responsabilidades claras. Usa interfaces cuando definan un límite útil o faciliten sustitución; evita abstracciones especulativas, lógica duplicada y dependencias globales o singletons estáticos.