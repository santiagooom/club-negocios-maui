# DESARROLLO MULTIPLATAFORMA — AVANCE PROYECTO (Progreso 2)

**TEMA:** Aplicación Multiplataforma en .NET MAUI y SQLite para la Gestión de Actividades, Eventos y Participación del Club de Negocios Digitales (UDLA)  
**ESTUDIANTES:** Justin Frutos, Xavier Rojas, Juan Marcos Vallejo, Anderson Calderón y Santiago Viteri  
**PROFESOR:** PhD (c). Luis Fernando Aguas Bucheli  
**CIUDAD:** QUITO – ECUADOR  
**FECHA:** 24 de noviembre de 2026  

---

## 0. Contexto de Negocio, Entorno e Inteligencia de Mercados (`EVA360_NEDI_RC1`)

### 0.1. Identificación de la Oportunidad de Mercado
- **Brecha operativa (sustento cualitativo):** La coordinación de organizaciones estudiantiles universitarias (como el *Club de Negocios Digitales · UDLA*) suele fragmentarse entre chats de WhatsApp, hojas de cálculo manuales y calendarios aislados. Esto provoca pérdida de trazabilidad sobre líderes vs. participantes, ausencia de control formal de horas de participación y desconocimiento de cruces con fechas críticas académicas.
- **Sustento cuantitativo del proyecto y entorno:** En la arquitectura de origen del club (`club-negocios-bot`, periodo `2027-10`) operan **3 niveles de roles** (Administración/Directiva, Admin TI y Miembros), **3 categorías operativas** (`Marketing`, `Eventos`, `Otro`), una estructura inicial de **7+ integrantes/directiva activos** y **16 tablas relacionales** en web adaptadas a **3 entidades núcleo móviles** (`Member`, `ClubActivity`, `ClubEvent`) junto al calendario oficial UDLA `2027-10`. En el contexto nacional ecuatoriano, el **80,1%** de la población ya utiliza internet y el **59,3%** cuenta con smartphone (INEC, 2025), habilitando una adopción directa de herramientas móviles en campus.
- **Proyecciones a corto y mediano plazo (metas del proyecto):**
  - *Corto plazo (Periodo 2027-10 · Progreso 2–3):* Unificar el **100%** de la gestión de tareas, calendario UDLA/Club y seguimiento de horas/XP del CND en la app `.NET MAUI`, reemplazando 3 canales dispersos por una sola aplicación offline-first en SQLite.
  - *Mediano plazo (2027–2028):* Sincronizar la aplicación `.NET MAUI` mediante API REST con el servidor central del club (`club-negocios-bot`) y replicar la plantilla hacia otros clubes y asociaciones estudiantiles de FACEA / UDLA.

### 0.2. Evaluación del Entorno (Ecuador · Ecosistema Universitario UDLA)

| Variable del Entorno | Hallazgo e Interpretación (2022–2025 / Periodo 2027-10) | Relación Directa con `.NET MAUI` (`ClubNegociosUDLA`) |
|---|---|---|
| **Tecnológica** | Según el INEC (2025), el acceso a internet en hogares pasó de **60,4% (2022)** a **71,3% (2025)** (**76,6%** en área urbana como Quito), la tenencia de smartphones subió de **52,2%** a **59,3%** y el analfabetismo digital cayó del **8,2%** al **2,1%**. | Valida lanzar una app móvil Android sin barreras de adopción, compilando desde un único código C#/XAML para **Android y Windows**, mientras **SQLite local** garantiza continuidad incluso en aulas o auditorios sin Wi-Fi estable. |
| **Académica / Institucional** | El semestre UDLA `2027-10` exige cumplir ventanas estrictas (Progreso 1, 2 y 3, retiros y asentamiento de notas) compatibilizando clases con eventos del club (*Digital Talks*, *Día FACEA*). | Justifica el módulo **Agenda UDLA**, que cruza fechas académicas oficiales con eventos propios del Club en color cyan y logo institucional. |
| **Económica** | Los clubes universitarios operan con presupuesto limitado y no disponen de fondos recurrentes para licencias SaaS por miembro. | Exige una arquitectura con costo **\$0 en licencias** (`.NET 10 SDK`, `SQLite`, `GitHub`), evitando suscripciones mensuales por usuario. |
| **Organizacional / Social** | La rotación semestral de directivas y miembros en clubes genera pérdida de historial cuando los datos quedan en teléfonos personales. | Impone autenticación institucional (`@udla.edu.ec`), roles diferenciados (Admin TI / Directiva / Miembro) y registro estructurado de horas y XP. |

### 0.3. Integración de Herramientas de Inteligencia de Mercados (Benchmark Comparativo)
Para validar el diseño frente a alternativas reales del mercado, se compararon los planes gratuitos y límites vigentes de herramientas líderes de productividad frente a nuestra solución a medida:

| Criterio Evaluado | WhatsApp + Excel | Trello (Free / Standard) | Notion (Free / Plus) | **ClubNegociosUDLA (.NET MAUI)** |
|---|---|---|---|---|
| **Límite en plan gratuito** | Sin límite formal, pero sin base relacional ni control de estados | Máx. **10 tableros** y **10 colaboradores** por workspace | Límite de **1,000 bloques** totales cuando hay 2+ miembros | **Ilimitado** en registros y usuarios (SQLite embebido) |
| **Costo al escalar equipo** | \$0 (alto costo en tiempo manual y errores) | ~**\$5–\$6** / usuario / mes (Standard) para quitar límites | ~**\$10** / usuario / mes (Plus) para equipos colaborativos | **\$0 en licencias** (desarrollo académico propio) |
| **Vista de Agenda / Calendario** | No integrada | Bloqueada en plan Free (requiere plan pago) | Incluida, pero requiere configuración manual | **Integrada** con calendario oficial UDLA `2027-10` + Club |
| **Control de horas, XP y roles UDLA** | Manual en hojas sueltas | No nativo (solo tarjetas genéricas) | Parcial mediante plantillas manuales | **Nativo** (roles `@udla.edu.ec`, horas, XP, pines y CRUD) |
| **Operación sin conexión** | Limitada / desincronizada | Parcial | Limitada | **100% funcional offline** con SQLite (`club_negocios_maui.db3`) |

---

## 1. Estructura de Gestión y Avance del Proyecto en .NET MAUI

### 1.1. Porcentaje de Avance del Proyecto MAUI

| Módulo MAUI | Funcionalidad Principal | Estado Actual | Porcentaje de Avance |
|---|---|---|:---:|
| Pantalla de Inicio (`SplashPage.xaml` y `HomePage.xaml`) | Bienvenida institucional con identidad UDLA/CND, saludo dinámico, KPIs en vivo y sección `📌 Fijados en Inicio` | Implementado | 100% |
| Inicio de Sesión (`LoginPage.xaml`) | Acceso institucional (`@udla.edu.ec`) validado contra SQLite con hash criptográfico SHA-256 y perfiles de demostración | Implementado | 95% |
| Navegación (`AppShell.xaml`) | Shell con barra de pestañas (`Inicio`, `Actividades`, `Agenda UDLA`, `Perfil`) y ventanas modales para formularios | Implementado | 100% |
| Formularios (`ActivityFormPage.xaml` y `AgendaPage.xaml`) | Registro y edición de actividades y eventos con `Entry`, `Editor`, `Picker`, `DatePicker`, `Slider` y `Switch` | Implementado | 95% |
| Base de Datos SQLite (`DatabaseService.cs`) | Persistencia local asíncrona con `sqlite-net-pcl` (tablas `members`, `activities`, `events`), datos semilla UDLA `2027-10` y CRUD completo | Implementado | 95% |

**Evidencias técnicas (XAML, C#, navegación y compilación):**
- Compilación verificada sin errores en **.NET 10.0 (`10.0.403`)** tanto para **Windows (`net10.0-windows10.0.19041.0`)** como para **Android (`net10.0-android` → `ec.edu.udla.clubnegocios.maui-Signed.apk`)**.
- Archivos clave en el repositorio:
  - `Models/Member.cs`, `Models/ClubActivity.cs`, `Models/ClubEvent.cs`
  - `Services/DatabaseService.cs`, `Services/SessionService.cs`
  - `Pages/SplashPage.xaml(.cs)`, `Pages/LoginPage.xaml(.cs)`, `Pages/HomePage.xaml(.cs)`, `Pages/ActivitiesPage.xaml(.cs)`, `Pages/ActivityFormPage.xaml(.cs)`, `Pages/AgendaPage.xaml(.cs)`, `Pages/ProfilePage.xaml(.cs)`
- Fragmento representativo de persistencia SQLite en `Services/DatabaseService.cs`:
  ```csharp
  _db = new SQLiteAsyncConnection(DbPath);
  await _db.CreateTableAsync<Member>();
  await _db.CreateTableAsync<ClubActivity>();
  await _db.CreateTableAsync<ClubEvent>();
  ```

---

### 1.2. Avance de Funcionalidades de la Aplicación

| Funcionalidad | Descripción | Estado Actual | Porcentaje de Avance |
|---|---|---|:---:|
| Menú Principal (`HomePage`) | Panel principal con KPIs (`Pendientes`, `En Proceso`, `Completadas`, `XP/Horas`) y actividades fijadas | Implementado | 100% |
| Registro de Datos (Create) | Creación de nuevas actividades (`ActivityFormPage`) y nuevos eventos (`AgendaPage`) guardados en SQLite | Implementado | 100% |
| Listado y Búsqueda (Read) | Listado con `CollectionView`, buscador en tiempo real por título/líder/categoría y filtros por estado o tipo (`Club CND` / `UDLA 2027-10`) | Implementado | 100% |
| Edición de Datos (Update) | Carga de registros existentes en `ActivityFormPage` (`Editar Actividad #N`), actualización de avance con `Slider` (`0-100%`) y botón de fijado `📌` | Implementado | 100% |
| Eliminación de Datos (Delete) | Eliminación de actividades y eventos con cuadro de confirmación (`DisplayAlertAsync`) y persistencia en SQLite | Implementado | 100% |
| Ranking y Perfil (`ProfilePage`) | Perfil del usuario autenticado, ranking por XP/Horas aprobadas y cierre de sesión | Implementado | 90% |

**Evidencias funcionales del CRUD:**
- **Create / Read:** En `ActivitiesPage` y `AgendaPage` los botones `+ Nueva` y `+ Evento` insertan registros reales en `club_negocios_maui.db3` mediante `SaveActivityAsync` / `SaveEventAsync` y refrescan la vista automáticamente.
- **Update:** Al presionar `✏️ Editar` en `Campaña Reels Digital Talks 2027-10`, se abre `Editar Actividad #1` precargada desde SQLite; además el botón `📌 Fijar / Fijado` actualiza `IsPinned` al instante.
- **Delete:** El botón `🗑️ Eliminar` solicita confirmación antes de invocar `DeleteActivityAsync(id)` / `DeleteEventAsync(id)`.

---

## 2. Presupuesto del Proyecto MAUI

| Recurso | Descripción | Cantidad | Costo Unitario | Costo Total |
|---|---|:---:|---:|---:|
| Laptops de desarrollo | Equipos personales del equipo (MSI / PCs Windows para programación y compilación MAUI) | 5 | $0 (propios) | $0 |
| Internet residencial | Conectividad para descarga de paquetes NuGet (`sqlite-net-pcl`), SDK Android y GitHub | 1 | $30 | $30 |
| .NET 10 SDK / Visual Studio | Entorno gratuito para desarrollo multiplataforma C# / XAML (Windows y Android) | 5 | Gratuito | $0 |
| Git y GitHub | Control de versiones distribuido y alojamiento público del código fuente | 1 | Gratuito | $0 |
| SQLite (`sqlite-net-pcl`) | Motor de base de datos local embebido sin servidor ni costo de licencia | 1 | Gratuito | $0 |
| Desarrollo e Integración | Horas invertidas en diseño XAML, lógica C#, SQLite, pruebas y documentación (5 integrantes) | 60 | $5 | $300 |
| **Total Estimado** | | | | **$330** |

**Observaciones:**  
Todas las licencias de software utilizadas (**.NET 10 SDK**, **Visual Studio / .NET MAUI**, **SQLite / `sqlite-net-pcl`**, **Android SDK** y **GitHub**) son gratuitas para uso académico y desarrollo. El presupuesto refleja principalmente el valor simbólico de horas-hombre invertidas por el equipo y la conectividad a internet para restaurar dependencias y validar compilaciones Windows y Android.

---

## 3. Prototipos de la Aplicación MAUI

### 3.1. Prototipo Inicial
Como punto de partida se tomaron los prototipos visuales móviles diseñados previamente para la identidad del **Club de Negocios Digitales · UDLA** (paleta vino `#82091E`, fondo institucional UDLA Park, login institucional y tarjetas limpias estilo móvil) y se diseñó la transición hacia vistas nativas en `.NET MAUI`:
- **`docs/screenshots/01_splash.png`** *(Pantalla Splash inicial con identidad UDLA/CND e inicialización asíncrona SQLite)*.
- **`docs/screenshots/02_login.png`** *(Prototipo de inicio de sesión móvil/multiplataforma con `@udla.edu.ec` y accesos de prueba)*.

### 3.2. Prototipo Funcional
Las capturas de la aplicación ejecutándose en vivo demuestran la integración real con SQLite, navegación por `Shell`, diseño responsive y flujo CRUD completo:
- **`docs/screenshots/03_home.png`** — Dashboard `Inicio` con KPIs (`2 Pendientes`, `1 En Proceso`, `1 Completadas`, `480 XP · 24,5 h aprobadas`) y sección `📌 Fijados en Inicio`.
- **`docs/screenshots/04_activities.png`** — Listado de `Actividades` con buscador, filtros por estado, barras de progreso y botones `📌 Fijado`, `✏️ Editar`, `🗑️ Eliminar`.
- **`docs/screenshots/05_activity_form.png`** — Formulario modal `Nueva Actividad del Club` (Create).
- **`docs/screenshots/05b_activity_edit.png`** — Formulario `Editar Actividad #1` precargado desde SQLite (Read + Update).
- **`docs/screenshots/06_agenda.png`** — `Agenda UDLA` combinando eventos del club (distintivo cyan + logo CND) y fechas oficiales UDLA `2027-10`.
- **`docs/screenshots/07_profile.png`** — `Perfil` con sesión activa, ranking de participación por XP/Horas y datos del equipo desarrollador.

---

## 4. Distribución de Tareas en los Miembros del Grupo

| Integrante | Rol | Responsabilidades |
|---|---|---|
| **Santiago Viteri** | Líder Técnico y Arquitecto .NET MAUI | Estructura del proyecto `.NET MAUI`, inyección de dependencias (`MauiProgram.cs`), navegación `AppShell` y coordinación de compilación Windows/Android |
| **Justin Frutos** | Líder de Proyecto y Gestión Operativa | Definición de reglas de negocio del Club, organización de módulos, validación de flujos CRUD y redacción del informe de avance |
| **Xavier Rojas** | Diseñador UI/UX (XAML) | Adaptación de la paleta oficial UDLA (`Colors.xaml`), diseño visual de `SplashPage`, `LoginPage`, tarjetas con progreso y jerarquía responsive |
| **Juan Marcos Vallejo** | Especialista en Base de Datos SQLite | Diseño de modelos (`Member`, `ClubActivity`, `ClubEvent`), implementación de `DatabaseService.cs`, consultas filtradas y carga semilla `2027-10` |
| **Anderson Calderón** | Tester / Control de Calidad (QA) | Pruebas funcionales de login, filtros, operaciones CRUD (crear, editar, fijar, eliminar), captura de evidencias y validación en GitHub |

**Descripción de tareas:**  
El equipo dividió el avance en cuatro frentes coordinados: definición de la arquitectura y reglas del Club (Justin y Santiago), capa de datos local SQLite (Juan Marcos), maquetación XAML con identidad visual UDLA/CND (Xavier y Santiago) y pruebas funcionales extremo a extremo con recolección de capturas y control de commits (Anderson y todo el equipo).

---

## 5. Repositorios y Herramientas Utilizadas

| Plataforma / Herramienta | Tipo | Uso Principal |
|---|---|---|
| **GitHub** | Repositorio remoto | Alojamiento del código fuente, control de versiones por módulos e historial de commits |
| **.NET 10 SDK & .NET MAUI** | Framework multiplataforma | Desarrollo en C# y XAML para compilar en Windows y Android (`ec.edu.udla.clubnegocios.maui-Signed.apk`) |
| **Visual Studio / IDE** | Entorno de desarrollo (IDE) | Edición de XAML/C#, depuración y gestión de cargas de trabajo MAUI/Android |
| **SQLite (`sqlite-net-pcl`)** | Base de datos local | Persistencia asíncrona en dispositivo (`club_negocios_maui.db3`) |
| **Git / GitHub CLI (`gh`)** | CLI de versionamiento | Creación de commits estructurados por fase y publicación del repositorio |

- **Nombre del proyecto:** `club-negocios-maui` (`ClubNegociosUDLA`)
- **URL del repositorio:** `https://github.com/santiagooom/club-negocios-maui`
- **Commits registrados en `main`:**
  - `501fe50` — `chore: setup .NET MAUI project, UDLA colors, and branding assets`
  - `94b857b` — `feat(data): add SQLite models (Member, ClubActivity, ClubEvent) and DatabaseService`
  - `85008e1` — `feat(ui): add Splash, Login, Home, Activities CRUD, Agenda UDLA, and Profile XAML pages`
  - `fc9dce8` — `docs: add README and live MAUI application screenshots`

---

## 6. Declaratoria de Uso de Inteligencia Artificial

El grupo declara que durante el desarrollo del proyecto se utilizó asistencia de **Inteligencia Artificial** (asistente de programación **Antigravity / Gemini**) como herramienta de apoyo técnico para acelerar la adaptación de nuestro sistema previo del Club de Negocios Digitales hacia **.NET MAUI**, estructurar vistas XAML y consultas asíncronas con `sqlite-net-pcl`, resolver detalles de compilación multiplataforma (Windows y Android) y redactar la estructura del documento de avance.

Todos los resultados generados fueron **revisados, compilados, ejecutados y validados** directamente por el equipo mediante pruebas funcionales reales (inicio de sesión, navegación por pestañas, creación/edición/fijado/eliminación de registros en SQLite y verificación de compilación con 0 errores para Windows y Android), garantizando un uso ético, transparente y alineado con los lineamientos académicos de la Universidad de Las Américas (UDLA).

---

## 7. Bibliografía (Formato APA 7)

- Hipp, D. R. (2025). *SQLite Documentation*. SQLite Consortium. https://www.sqlite.org/docs.html
- Instituto Nacional de Estadística y Censos [INEC]. (2025). *Estadísticas de Tecnologías de la Información y Comunicación (TIC) – ENEMDU 2022–2025*. Gobierno del Ecuador. https://www.ecuadorencifras.gob.ec/
- Krueger, F. A. (2024). *sqlite-net: Simple, powerful, cross-platform SQLite client and ORM for .NET* [Software]. GitHub. https://github.com/praeclarum/sqlite-net
- Microsoft. (2025). *.NET MAUI local databases (SQLite-net)*. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/database-sqlite
- Microsoft. (2025). *.NET MAUI Shell overview*. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/
- Microsoft. (2025). *What is .NET MAUI? - .NET MAUI documentation*. Microsoft Learn. https://learn.microsoft.com/en-us/dotnet/maui/what-is-maui
