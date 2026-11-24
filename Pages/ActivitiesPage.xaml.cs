using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class ActivitiesPage : ContentPage
{
    private readonly DatabaseService _db;
    private string _activeFilter = "TODAS";

    public ActivitiesPage(DatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        UpdateFilterButtonStyles();
        await RefreshListAsync();
    }

    private void UpdateFilterButtonStyles()
    {
        var map = new Dictionary<Button, string>
        {
            [FilterAllBtn] = "TODAS",
            [FilterPendingBtn] = "PENDIENTE",
            [FilterProgressBtn] = "EN_PROGRESO",
            [FilterDoneBtn] = "COMPLETADO"
        };
        foreach (var (btn, key) in map)
        {
            var selected = key == _activeFilter;
            btn.BackgroundColor = selected ? Color.FromArgb("#82091E") : Colors.White;
            btn.TextColor = selected ? Colors.White : Color.FromArgb("#1D1D1F");
        }
    }

    private async Task RefreshListAsync()
    {
        var items = await _db.GetActivitiesAsync(_activeFilter, ActivitySearchBar.Text);
        ActivitiesListView.ItemsSource = items;
    }

    private async void OnFilterClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string filter)
        {
            _activeFilter = filter;
            UpdateFilterButtonStyles();
            await RefreshListAsync();
        }
    }

    private async void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        await RefreshListAsync();
    }

    private async void OnCreateClicked(object? sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new ActivityFormPage(_db, null, () => MainThread.BeginInvokeOnMainThread(async () => await RefreshListAsync())));
    }

    private async void OnEditClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            var item = await _db.GetActivityByIdAsync(id);
            if (item is not null)
            {
                await Navigation.PushModalAsync(new ActivityFormPage(_db, item, () => MainThread.BeginInvokeOnMainThread(async () => await RefreshListAsync())));
            }
        }
    }

    private async void OnPinClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            await _db.TogglePinActivityAsync(id);
            await RefreshListAsync();
        }
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int id)
        {
            var item = await _db.GetActivityByIdAsync(id);
            if (item is null)
                return;

            var ok = await DisplayAlertAsync("Confirmar eliminación", $"¿Eliminar la actividad '{item.Title}'?", "Eliminar", "Cancelar");
            if (!ok)
                return;

            await _db.DeleteActivityAsync(id);
            await RefreshListAsync();
        }
    }
}
