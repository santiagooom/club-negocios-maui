using System.Security.Cryptography;
using System.Text;
using ClubNegociosUDLA.Models;
using SQLite;

namespace ClubNegociosUDLA.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection? _db;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public string DbPath { get; } = Path.Combine(FileSystem.AppDataDirectory, "club_negocios_maui.db3");

    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public async Task InitializeAsync()
    {
        if (_db is not null)
            return;

        await _initLock.WaitAsync();
        try
        {
            if (_db is not null)
                return;

            _db = new SQLiteAsyncConnection(DbPath);
            await _db.CreateTableAsync<Member>();
            await _db.CreateTableAsync<ClubActivity>();
            await _db.CreateTableAsync<ClubEvent>();

            await SeedInitialDataAsync(_db);
        }
        finally
        {
            _initLock.Release();
        }
    }

    private static async Task SeedInitialDataAsync(SQLiteAsyncConnection db)
    {
        var demoHash = HashPassword("udla2027");

        if (await db.Table<Member>().CountAsync() == 0)
        {
            var members = new List<Member>
            {
                new() { Name = "Santiago Viteri", Email = "santiago.viteri.alcivar@udla.edu.ec", PasswordHash = demoHash, Role = "Desarrollador · Admin TI", IsAdmin = 2, Department = "Tecnología e Innovación", ApprovedHours = 24.5, XpPoints = 480 },
                new() { Name = "Arlette Luque", Email = "arlette.luque@udla.edu.ec", PasswordHash = demoHash, Role = "Presidenta del Club", IsAdmin = 1, Department = "Administración", ApprovedHours = 22.0, XpPoints = 430 },
                new() { Name = "Justin Frutos", Email = "justin.frutos@udla.edu.ec", PasswordHash = demoHash, Role = "Coordinador de Operaciones", IsAdmin = 1, Department = "Operaciones", ApprovedHours = 18.5, XpPoints = 390 },
                new() { Name = "Xavier Rojas", Email = "xavier.rojas@udla.edu.ec", PasswordHash = demoHash, Role = "Especialista UI/UX", IsAdmin = 0, Department = "Marketing", ApprovedHours = 16.0, XpPoints = 345 },
                new() { Name = "Juan Marcos Vallejo", Email = "juan.vallejo@udla.edu.ec", PasswordHash = demoHash, Role = "Gestión de Datos", IsAdmin = 0, Department = "Analítica", ApprovedHours = 15.0, XpPoints = 310 },
                new() { Name = "Anderson Calderón", Email = "anderson.calderon@udla.edu.ec", PasswordHash = demoHash, Role = "Calidad y Pruebas", IsAdmin = 0, Department = "Eventos", ApprovedHours = 14.5, XpPoints = 295 },
                new() { Name = "Jhoaho Velásquez", Email = "jhoaho.velasquez@udla.edu.ec", PasswordHash = demoHash, Role = "Miembro del Club", IsAdmin = 0, Department = "Marketing", ApprovedHours = 12.0, XpPoints = 260 }
            };
            await db.InsertAllAsync(members);
        }

        if (await db.Table<ClubActivity>().CountAsync() == 0)
        {
            var today = DateTime.Today.ToString("yyyy-MM-dd");
            var plus2 = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
            var plus5 = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd");

            var activities = new List<ClubActivity>
            {
                new()
                {
                    Title = "Campaña Reels Digital Talks 2027-10",
                    Description = "Producción audiovisual y parrilla de contenido para Instagram y TikTok del Club.",
                    Category = "Marketing",
                    Status = "EN_PROGRESO",
                    Priority = "ALTA",
                    DueDate = today,
                    ResponsibleName = "Xavier Rojas",
                    Participants = "Xavier Rojas, Jhoaho Velásquez",
                    ProgressPercent = 65,
                    EstimatedHours = 4.0,
                    IsPinned = true
                },
                new()
                {
                    Title = "Logística Visita Empresarial Tech",
                    Description = "Confirmar listado de asistentes UDLA, transporte y agenda con ponentes.",
                    Category = "Eventos",
                    Status = "PENDIENTE_REVISION",
                    Priority = "ALTA",
                    DueDate = plus2,
                    ResponsibleName = "Justin Frutos",
                    Participants = "Justin Frutos, Anderson Calderón",
                    ProgressPercent = 90,
                    EstimatedHours = 5.0,
                    IsPinned = true
                },
                new()
                {
                    Title = "Consolidación de Horas y Portafolio CSV",
                    Description = "Validar horas aprobadas del primer corte y exportar reporte de miembros.",
                    Category = "Otro",
                    Status = "PENDIENTE",
                    Priority = "MEDIA",
                    DueDate = plus5,
                    ResponsibleName = "Juan Marcos Vallejo",
                    Participants = "Juan Marcos Vallejo, Santiago Viteri",
                    ProgressPercent = 25,
                    EstimatedHours = 3.0,
                    IsPinned = false
                },
                new()
                {
                    Title = "Reunión General de Arranque 2027-10",
                    Description = "Alineación inicial de metas del semestre con directiva y nuevos miembros.",
                    Category = "Eventos",
                    Status = "COMPLETADO",
                    Priority = "MEDIA",
                    DueDate = "2026-11-14",
                    ResponsibleName = "Arlette Luque",
                    Participants = "Arlette Luque, Santiago Viteri, Justin Frutos",
                    ProgressPercent = 100,
                    EstimatedHours = 2.0,
                    IsPinned = false
                }
            };
            await db.InsertAllAsync(activities);
        }

        if (await db.Table<ClubEvent>().CountAsync() == 0)
        {
            var events = new List<ClubEvent>
            {
                new() { Title = "Digital Talk: IA aplicada a Negocios", EventType = "CLUB", CategoryLabel = "Evento CND", StartDate = "2026-11-26", EndDate = "2026-11-26", Location = "Auditorio UDLA Park", Description = "Charla abierta con casos reales de transformación digital.", IsPinned = true },
                new() { Title = "PROGRESO 2: Evaluación y Retroalimentación", EventType = "UDLA", CategoryLabel = "Calendario UDLA 2027-10", StartDate = "2026-11-30", EndDate = "2026-12-08", Location = "Campus UDLA", Description = "Ventana oficial de evaluaciones Progreso 2.", IsPinned = false },
                new() { Title = "Feriado Fundación de Quito", EventType = "UDLA", CategoryLabel = "Feriado UDLA", StartDate = "2026-12-05", EndDate = "2026-12-07", Location = "Quito", Description = "Suspensión oficial de actividades académicas.", IsPinned = false },
                new() { Title = "Feria de Emprendimiento & Día FACEA", EventType = "CLUB", CategoryLabel = "Evento CND", StartDate = "2026-12-12", EndDate = "2026-12-12", Location = "Plaza Central UDLA Park", Description = "Stand interactivo del Club de Negocios Digitales.", IsPinned = false },
                new() { Title = "Fin del Semestre 2027-10 Grado", EventType = "UDLA", CategoryLabel = "Fin de Semestre", StartDate = "2027-01-23", EndDate = "2027-01-23", Location = "Campus UDLA", Description = "Cierre regular de clases del periodo académico.", IsPinned = false }
            };
            await db.InsertAllAsync(events);
        }
    }

    public async Task<Member?> AuthenticateAsync(string email, string password)
    {
        await InitializeAsync();
        var cleanEmail = (email ?? string.Empty).Trim().ToLowerInvariant();
        var hash = HashPassword(password ?? string.Empty);
        var member = await _db!.Table<Member>().Where(m => m.Email == cleanEmail).FirstOrDefaultAsync();
        if (member is null)
            return null;
        return string.Equals(member.PasswordHash, hash, StringComparison.OrdinalIgnoreCase) ? member : null;
    }

    public async Task<List<Member>> GetMembersAsync()
    {
        await InitializeAsync();
        return await _db!.Table<Member>().OrderByDescending(m => m.XpPoints).ToListAsync();
    }

    public async Task<List<ClubActivity>> GetActivitiesAsync(string? statusFilter = null, string? search = null)
    {
        await InitializeAsync();
        var items = await _db!.Table<ClubActivity>().OrderByDescending(a => a.IsPinned).ThenBy(a => a.DueDate).ToListAsync();
        if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "TODAS")
        {
            items = items.Where(a => string.Equals(a.Status, statusFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (!string.IsNullOrWhiteSpace(search))
        {
            var q = search.Trim();
            items = items.Where(a =>
                a.Title.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.ResponsibleName.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                a.Category.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return items;
    }

    public async Task<ClubActivity?> GetActivityByIdAsync(int id)
    {
        await InitializeAsync();
        return await _db!.Table<ClubActivity>().Where(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveActivityAsync(ClubActivity activity)
    {
        await InitializeAsync();
        if (activity.Id == 0)
            return await _db!.InsertAsync(activity);
        return await _db!.UpdateAsync(activity);
    }

    public async Task<int> DeleteActivityAsync(int id)
    {
        await InitializeAsync();
        return await _db!.DeleteAsync<ClubActivity>(id);
    }

    public async Task<bool> TogglePinActivityAsync(int id)
    {
        await InitializeAsync();
        var act = await GetActivityByIdAsync(id);
        if (act is null)
            return false;
        act.IsPinned = !act.IsPinned;
        await _db!.UpdateAsync(act);
        return act.IsPinned;
    }

    public async Task<List<ClubEvent>> GetEventsAsync(string? typeFilter = null)
    {
        await InitializeAsync();
        var items = await _db!.Table<ClubEvent>().OrderBy(e => e.StartDate).ToListAsync();
        if (!string.IsNullOrWhiteSpace(typeFilter) && typeFilter != "TODOS")
        {
            items = items.Where(e => string.Equals(e.EventType, typeFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return items;
    }

    public async Task<int> SaveEventAsync(ClubEvent ev)
    {
        await InitializeAsync();
        if (ev.Id == 0)
            return await _db!.InsertAsync(ev);
        return await _db!.UpdateAsync(ev);
    }

    public async Task<int> DeleteEventAsync(int id)
    {
        await InitializeAsync();
        return await _db!.DeleteAsync<ClubEvent>(id);
    }
}
