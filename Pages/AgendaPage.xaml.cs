using ClubNegociosUDLA.Models;
using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class AgendaPage : ContentPage
{
    private readonly DatabaseService _db;
    private string _activeFilter = "TODOS";

    public AgendaPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
        EventTypePicker.ItemsSource = new[] { "CLUB", "UDLA" };
        EventTypePicker.SelectedIndex = 0;
        EventDatePicker.Date = DateTime.Today.AddDays(1);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        UpdateFilterStyles();
        await RefreshEventsAsync();
    }

    private void UpdateFilterStyles()
    {
        var map = new Dictionary<Button, string>
        {
            [AllEventsBtn] = "TODOS",
            [ClubEventsBtn] = "CLUB",
            [UdlaEventsBtn] = "UDLA"
        };
        foreach (var (btn, key) in map)
        {
            var selected = key == _activeFilter;
            btn.BackgroundColor = selected ? Color.FromArgb(key == "CLUB" ? "#0891B2" : "#82091E") : Colors.White;
            btn.TextColor = selected ? Colors.White : Color.FromArgb("#1D1D1F");
        }
    }

    private async Task RefreshEventsAsync()
    {
        EventsListView.ItemsSource = await _db.GetEventsAsync(_activeFilter);
    }

    private async void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string filter)
        {
            _activeFilter = filter;
            UpdateFilterStyles();
            await RefreshEventsAsync();
        }
    }

    private void OnToggleFormClicked(object? sender, EventArgs e)
    {
        NewEventPanel.IsVisible = !NewEventPanel.IsVisible;
    }

    private async void OnSaveEventClicked(object? sender, EventArgs e)
    {
        var title = (EventTitleEntry.Text ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            await DisplayAlertAsync("Dato requerido", "Escribe el título del evento.", "OK");
            return;
        }

        var type = EventTypePicker.SelectedItem as string ?? "CLUB";
        var date = (EventDatePicker.Date ?? DateTime.Today).ToString("yyyy-MM-dd");
        var ev = new ClubEvent
        {
            Title = title,
            EventType = type,
            CategoryLabel = type == "CLUB" ? "Evento CND" : "Calendario UDLA 2027-10",
            StartDate = date,
            EndDate = date,
            Location = string.IsNullOrWhiteSpace(EventLocationEntry.Text) ? "Campus UDLA Park" : EventLocationEntry.Text.Trim(),
            Description = "Registrado desde la aplicación .NET MAUI."
        };
        await _db.SaveEventAsync(ev);
        EventTitleEntry.Text = string.Empty;
        EventLocationEntry.Text = string.Empty;
        NewEventPanel.IsVisible = false;
        await RefreshEventsAsync();
    }

    private async void OnDeleteEventClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            var confirm = await DisplayAlertAsync("Eliminar evento", "¿Eliminar este evento de la agenda SQLite?", "Eliminar", "Cancelar");
            if (!confirm)
                return;
            await _db.DeleteEventAsync(id);
            await RefreshEventsAsync();
        }
    }
}
