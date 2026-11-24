using ClubNegociosUDLA.Models;
using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class HomePage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly SessionService _session;

    public HomePage(DatabaseService db, SessionService session)
    {
        InitializeComponent();
        _db = db;
        _session = session;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        var user = _session.CurrentUser;
        if (user is not null)
        {
            var hour = DateTime.Now.Hour;
            var greet = hour < 12 ? "Buenos días" : hour < 19 ? "Buenas tardes" : "Buenas noches";
            var firstName = user.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? user.Name;
            GreetingLabel.Text = $"{greet}, {firstName}";
            RoleLabel.Text = $"{user.RoleBadge} · {user.Department}";
            XpCountLabel.Text = $"{user.XpPoints} XP";
            HoursSubLabel.Text = $"{user.ApprovedHours:0.#} h aprobadas";
        }
        DateLabel.Text = DateTime.Now.ToString("dddd dd MMM yyyy · HH:mm");

        var all = await _db.GetActivitiesAsync();
        PendingCountLabel.Text = all.Count(a => a.Status is "PENDIENTE" or "PENDIENTE_REVISION").ToString();
        InProgressCountLabel.Text = all.Count(a => a.Status == "EN_PROGRESO").ToString();
        DoneCountLabel.Text = all.Count(a => a.Status == "COMPLETADO").ToString();

        var pinned = all.Where(a => a.IsPinned).ToList();
        PinnedListLayout.Children.Clear();
        NoPinnedLabel.IsVisible = pinned.Count == 0;
        foreach (var act in pinned)
        {
            PinnedListLayout.Children.Add(BuildActivityCard(act));
        }

        var upcoming = all.Where(a => a.Status != "COMPLETADO").Take(5).ToList();
        UpcomingListLayout.Children.Clear();
        foreach (var act in upcoming)
        {
            UpcomingListLayout.Children.Add(BuildActivityCard(act));
        }
    }

    private View BuildActivityCard(ClubActivity act)
    {
        var border = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#E5E5EA"),
            StrokeThickness = 1,
            Padding = new Thickness(14)
        };
        border.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 14 };

        var stack = new VerticalStackLayout { Spacing = 6 };
        var topRow = new Grid { ColumnDefinitions = [new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto)], ColumnSpacing = 8 };
        topRow.Add(new Label { Text = act.Title, FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#1D1D1F") }, 0, 0);

        var statusBadge = new Border
        {
            BackgroundColor = Color.FromArgb("#F8F8FA"),
            StrokeThickness = 0,
            Padding = new Thickness(8, 4),
            Content = new Label { Text = act.StatusBadge, FontSize = 11, FontAttributes = FontAttributes.Bold, TextColor = act.StatusColor }
        };
        statusBadge.StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 8 };
        topRow.Add(statusBadge, 1, 0);

        stack.Add(topRow);
        stack.Add(new Label { Text = act.MetaLine, FontSize = 12, TextColor = Color.FromArgb("#6E6E73") });
        stack.Add(new ProgressBar { Progress = act.ProgressFraction, ProgressColor = act.StatusColor, BackgroundColor = Color.FromArgb("#EFEFF4") });

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (_, _) => await Navigation.PushModalAsync(new ActivityFormPage(_db, act, () => MainThread.BeginInvokeOnMainThread(async () => await LoadDashboardAsync())));
        border.GestureRecognizers.Add(tap);
        border.Content = stack;
        return border;
    }

    private async void OnNewActivityClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ActivityFormPage(_db, null, () => MainThread.BeginInvokeOnMainThread(async () => await LoadDashboardAsync())));
    }
}
