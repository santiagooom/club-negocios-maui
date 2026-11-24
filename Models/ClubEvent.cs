using SQLite;

namespace ClubNegociosUDLA.Models;

[Table("events")]
public class ClubEvent
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string EventType { get; set; } = "CLUB"; // CLUB | UDLA

    public string CategoryLabel { get; set; } = "Evento del Club";

    public string StartDate { get; set; } = string.Empty;

    public string EndDate { get; set; } = string.Empty;

    public string Location { get; set; } = "Campus UDLA Park";

    public string Description { get; set; } = string.Empty;

    public bool IsPinned { get; set; }

    [Ignore]
    public bool IsClubEvent => string.Equals(EventType, "CLUB", StringComparison.OrdinalIgnoreCase);

    [Ignore]
    public Color AccentColor => IsClubEvent
        ? Color.FromArgb("#0891B2")
        : Color.FromArgb("#82091E");

    [Ignore]
    public Color BadgeBg => IsClubEvent
        ? Color.FromArgb("#E0F2FE")
        : Color.FromArgb("#FDF2F4");

    [Ignore]
    public string DateRangeText => StartDate == EndDate || string.IsNullOrWhiteSpace(EndDate)
        ? StartDate
        : $"{StartDate} al {EndDate}";
}
