using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly SessionService _session;

    public ProfilePage(DatabaseService db, SessionService session)
    {
        InitializeComponent();
        _db = db;
        _session = session;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var u = _session.CurrentUser;
        if (u is not null)
        {
            UserNameLabel.Text = u.Name;
            UserEmailLabel.Text = u.Email;
            UserRoleLabel.Text = $"{u.RoleBadge} · {u.ApprovedHours:0.#} h · {u.XpPoints} XP";
        }
        DbPathLabel.Text = $"Archivo DB: {_db.DbPath}";

        var members = await _db.GetMembersAsync();
        LeaderboardLayout.Children.Clear();
        int rank = 1;
        foreach (var m in members)
        {
            var card = new Border
            {
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb("#E5E5EA"),
                StrokeThickness = 1,
                Padding = new Thickness(14, 10)
            };
            card.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 };

            var grid = new Grid { ColumnDefinitions = [new ColumnDefinition(32), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto)], ColumnSpacing = 10 };
            grid.Add(new Label { Text = $"#{rank}", FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb(rank == 1 ? "#D97706" : "#82091E"), VerticalOptions = LayoutOptions.Center }, 0, 0);

            var mid = new VerticalStackLayout { Spacing = 2 };
            mid.Add(new Label { Text = m.Name, FontSize = 14, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#1D1D1F") });
            mid.Add(new Label { Text = m.StatsLabel, FontSize = 11, TextColor = Color.FromArgb("#6E6E73") });
            grid.Add(mid, 1, 0);

            grid.Add(new Label { Text = m.RoleBadge, FontSize = 11, TextColor = Color.FromArgb("#6E6E73"), VerticalOptions = LayoutOptions.Center }, 2, 0);
            card.Content = grid;
            LeaderboardLayout.Children.Add(card);
            rank++;
        }
    }

    private void OnLogoutClicked(object? sender, EventArgs e)
    {
        _session.SignOut();
        if (Application.Current?.Windows.FirstOrDefault() is Window win)
        {
            win.Page = new LoginPage(_db, _session);
        }
    }
}
