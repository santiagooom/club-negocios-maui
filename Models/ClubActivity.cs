using SQLite;

namespace ClubNegociosUDLA.Models;

[Table("activities")]
public class ClubActivity
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = "Marketing"; // Marketing | Eventos | Otro

    public string Status { get; set; } = "PENDIENTE"; // PENDIENTE | EN_PROGRESO | PENDIENTE_REVISION | COMPLETADO

    public string Priority { get; set; } = "MEDIA"; // ALTA | MEDIA | BAJA

    public string DueDate { get; set; } = "2026-11-30";

    public string ResponsibleName { get; set; } = string.Empty;

    public string Participants { get; set; } = string.Empty;

    public int ProgressPercent { get; set; }

    public double EstimatedHours { get; set; } = 2.0;

    public bool IsPinned { get; set; }

    public string CreatedAt { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

    [Ignore]
    public double ProgressFraction => Math.Clamp(ProgressPercent / 100.0, 0.0, 1.0);

    [Ignore]
    public string StatusBadge => Status switch
    {
        "PENDIENTE" => "⏳ Pendiente",
        "EN_PROGRESO" => "⚙️ En Proceso",
        "PENDIENTE_REVISION" => "🔍 En Validación",
        "COMPLETADO" => "✅ Completada",
        _ => Status
    };

    [Ignore]
    public Color StatusColor => Status switch
    {
        "COMPLETADO" => Color.FromArgb("#16A34A"),
        "PENDIENTE_REVISION" => Color.FromArgb("#D97706"),
        "EN_PROGRESO" => Color.FromArgb("#0891B2"),
        _ => Color.FromArgb("#82091E")
    };

    [Ignore]
    public string PriorityBadge => Priority switch
    {
        "ALTA" => "🔴 Alta",
        "BAJA" => "🟢 Baja",
        _ => "🟡 Media"
    };

    [Ignore]
    public string PinLabel => IsPinned ? "📌 Fijado" : "Fijar";

    [Ignore]
    public string MetaLine => string.IsNullOrWhiteSpace(ResponsibleName)
        ? $"{Category} · Vence {DueDate} · {ProgressPercent}%"
        : $"{Category} · Líder: {ResponsibleName} · {DueDate} · {ProgressPercent}%";
}
