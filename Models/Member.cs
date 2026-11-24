using SQLite;

namespace ClubNegociosUDLA.Models;

[Table("members")]
public class Member
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    [Indexed(Unique = true)]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Miembro del Club";

    public int IsAdmin { get; set; } // 2 = Admin TI / Dev, 1 = Directiva, 0 = Miembro

    public string Department { get; set; } = "Operaciones";

    public double ApprovedHours { get; set; }

    public int XpPoints { get; set; }

    [Ignore]
    public string RoleBadge => IsAdmin >= 2
        ? "🛠️ Desarrollador · Admin TI"
        : IsAdmin == 1
            ? "👑 Administración"
            : "👤 Miembro del Club";

    [Ignore]
    public string StatsLabel => $"{XpPoints} XP · {ApprovedHours:0.#} h aprobadas · {Department}";
}
