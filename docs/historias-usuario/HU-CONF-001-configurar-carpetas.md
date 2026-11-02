## Código
HU-CONF-001 — Configurar carpetas de descarga y extracción

## Historia de usuario
Como usuario,
quiero configurar las carpetas donde se descargan y descomprimen los archivos,
para decidir dónde se almacenan en mi equipo.

## Requerimientos
- La aplicación permite elegir una carpeta de descarga.
- La aplicación permite elegir una carpeta de extracción.
- Las rutas se conservan al cerrar y volver a abrir la aplicación.

## Criterios de aceptación
- Dado que no existe configuración, cuando inicio la aplicación,
  entonces se usan las carpetas predeterminadas bajo `%LocalAppData%\\BmsManager`.
- Dado que cambio una carpeta y guardo la configuración, cuando reinicio la aplicación,
  entonces se muestra la ruta seleccionada.
- Dado que una carpeta configurada ya no está disponible, cuando intento usarla,
  entonces la aplicación informa el problema y me permite elegir otra.

## Ejemplo de configuración (`settings.json`)
```json
{
  "download_dir": "%LocalAppData%\\BmsManager\\Downloads",
  "selected_language": "English",
  "decompress_dir": "%LocalAppData%\\BmsManager\\Decompress"
}