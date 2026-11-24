# Club de Negocios Digitales · UDLA (.NET MAUI)

Aplicación multiplataforma desarrollada en **.NET MAUI (.NET 10, XAML y C#)** con persistencia local en **SQLite (`sqlite-net-pcl`)** para la gestión operativa de actividades, eventos, calendario académico UDLA `2027-10`, roles y participación del **Club de Negocios Digitales (UDLA)**.

## Módulos Implementados
- **Splash & Inicialización (`Pages/SplashPage.xaml`)**: Pantalla de bienvenida con identidad UDLA/CND e inicialización asíncrona de SQLite.
- **Inicio de Sesión (`Pages/LoginPage.xaml`)**: Validación de credenciales `@udla.edu.ec` con hash SHA-256 y accesos rápidos de demostración.
- **Inicio / Dashboard (`Pages/HomePage.xaml`)**: KPIs en tiempo real (`Pendientes`, `En Proceso`, `Completadas`, `XP/Horas`), sección `📌 Fijados en Inicio` y próximas actividades.
- **Gestión de Actividades — CRUD Completo (`Pages/ActivitiesPage.xaml` y `Pages/ActivityFormPage.xaml`)**: Búsqueda, filtros por estado, creación, edición, fijado personal (`📌`) y eliminación con confirmación.
- **Agenda Híbrida (`Pages/AgendaPage.xaml`)**: Calendario oficial UDLA `2027-10` + eventos del Club (distintivo cyan + logo CND) con registro y eliminación en SQLite.
- **Perfil y Ranking (`Pages/ProfilePage.xaml`)**: Datos de sesión, ranking de participación por XP y horas aprobadas, y ruta de base de datos SQLite local.

## Equipo Desarrollador (UDLA · Desarrollo Multiplataforma)
- **Santiago Viteri** — Arquitecto .NET MAUI / Líder Técnico
- **Justin Frutos** — Coordinador del Proyecto y Flujo Operativo
- **Xavier Rojas** — Diseñador UI/UX (XAML y Paleta UDLA)
- **Juan Marcos Vallejo** — Base de Datos SQLite y Modelos
- **Anderson Calderón** — Pruebas Funcionales (CRUD) y Documentación

## Compilación
```bash
# Windows (.NET MAUI)
dotnet build -f net10.0-windows10.0.19041.0

# Android (APK)
dotnet build -f net10.0-android
```
