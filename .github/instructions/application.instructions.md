---
applyTo: "Application/**/*.cs"
---

# Instrucciones para la capa Application

La capa `Application` coordina las operaciones que la aplicación ofrece a Presentation. 
Sus servicios ejecutan los casos de uso, delegan el acceso a datos mediante contratos de repositories definidos en `Domain`.
No depende de WPF, de `Presentation` ni de implementaciones concretas de `Infrastructure`.

## Interfaces de servicios

- Ubica los contratos de servicios en `Application/Services`.

## Servicios y lógica de aplicación

- Coloca las clases que implementan los casos de uso en `Application/Services/Implementations`
- Mantén aquí la coordinación del flujo de aplicación y las validaciones que correspondan al caso de uso. 
- Llama al repository mediante las operaciones de su contrato.

## Registro y pruebas

- Registra los servicios de esta capa en `Application/DependencyInjection.cs` si el proyecto agrupa los registros por capa; de lo contrario, el composition root puede registrarlos. El registro no debe introducir dependencias de `Infrastructure` en el código de Application.
- Elige el ciclo de vida de acuerdo con el estado del servicio y de sus dependencias; prefiere servicios sin estado y evita retener estado de una operación entre llamadas.
- Mantén los servicios comprobables sin iniciar WPF ni el host. Prueba las validaciones, el flujo del caso de uso y las llamadas esperadas usando implementaciones de prueba de los contratos de `Domain`.
- Sigue las convenciones del proyecto para nullable reference types, nombres, namespaces y estilo asíncrono. Cambia únicamente los contratos y consumidores necesarios cuando evolucione una operación.