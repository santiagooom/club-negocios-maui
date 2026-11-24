using System.Globalization;
using ClubNegociosUDLA.Models;
using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class ActivityFormPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly ClubActivity _editing;
    private readonly Action? _onSaved;

    private static readonly string[] Categories = ["Marketing", "Eventos", "Otro"];
    private static readonly string[] Priorities = ["ALTA", "MEDIA", "BAJA"];
    private static readonly string[] Statuses = ["PENDIENTE", "EN_PROGRESO", "PENDIENTE_REVISION", "COMPLETADO"];

    public ActivityFormPage(DatabaseService db, ClubActivity? existing = null, Action? onSaved = null)
    {
        InitializeComponent();
        _db = db;
        _editing = existing ?? new ClubActivity();
        _onSaved = onSaved;

        CategoryPicker.ItemsSource = Categories;
        PriorityPicker.ItemsSource = Priorities;
        StatusPicker.ItemsSource = Statuses;

        if (_editing.Id > 0)
        {
            Title = "Editar Actividad";
            HeaderTitleLabel.Text = $"Editar Actividad #{_editing.Id}";
            DeleteButton.IsVisible = true;
        }
        else
        {
            Title = "Nueva Actividad";
            HeaderTitleLabel.Text = "Nueva Actividad del Club";
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var members = await _db.GetMembersAsync();
        var names = new List<string> { "Sin responsable asignado" };
        names.AddRange(members.Select(m => m.Name));
        ResponsiblePicker.ItemsSource = names;

        TitleEntry.Text = _editing.Title;
        DescriptionEditor.Text = _editing.Description;
        CategoryPicker.SelectedItem = Categories.Contains(_editing.Category) ? _editing.Category : "Marketing";
        PriorityPicker.SelectedItem = Priorities.Contains(_editing.Priority) ? _editing.Priority : "MEDIA";
        StatusPicker.SelectedItem = Statuses.Contains(_editing.Status) ? _editing.Status : "PENDIENTE";
        ResponsiblePicker.SelectedItem = !string.IsNullOrWhiteSpace(_editing.ResponsibleName) && names.Contains(_editing.ResponsibleName)
            ? _editing.ResponsibleName
            : "Sin responsable asignado";
        ParticipantsEntry.Text = _editing.Participants;

        if (DateTime.TryParseExact(_editing.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            DueDatePicker.Date = parsedDate;
        else
            DueDatePicker.Date = DateTime.Today.AddDays(3);

        ProgressSlider.Value = _editing.ProgressPercent;
        ProgressValueLabel.Text = $"Progreso: {_editing.ProgressPercent}%";
        HoursEntry.Text = _editing.EstimatedHours.ToString("0.#", CultureInfo.InvariantCulture);
        PinSwitch.IsToggled = _editing.IsPinned;
    }

    private void OnProgressChanged(object? sender, ValueChangedEventArgs e)
    {
        var pct = (int)Math.Round(e.NewValue);
        ProgressValueLabel.Text = $"Progreso: {pct}%";
    }

    private async void OnSaveClicked(object? sender, EventArgs e)
    {
        FormErrorLabel.IsVisible = false;
        var title = (TitleEntry.Text ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(title))
        {
            FormErrorLabel.Text = "El título de la actividad es obligatorio.";
            FormErrorLabel.IsVisible = true;
            return;
        }

        var resp = ResponsiblePicker.SelectedItem as string;
        if (resp == "Sin responsable asignado")
            resp = string.Empty;

        _editing.Title = title;
        _editing.Description = (DescriptionEditor.Text ?? string.Empty).Trim();
        _editing.Category = CategoryPicker.SelectedItem as string ?? "Marketing";
        _editing.Priority = PriorityPicker.SelectedItem as string ?? "MEDIA";
        _editing.Status = StatusPicker.SelectedItem as string ?? "PENDIENTE";
        _editing.ResponsibleName = resp ?? string.Empty;
        _editing.Participants = (ParticipantsEntry.Text ?? string.Empty).Trim();
        _editing.DueDate = (DueDatePicker.Date ?? DateTime.Today).ToString("yyyy-MM-dd");
        _editing.ProgressPercent = (int)Math.Round(ProgressSlider.Value);
        if (double.TryParse((HoursEntry.Text ?? "2").Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var hrs))
            _editing.EstimatedHours = Math.Max(0.5, hrs);
        _editing.IsPinned = PinSwitch.IsToggled;

        await _db.SaveActivityAsync(_editing);
        _onSaved?.Invoke();
        await Navigation.PopModalAsync();
    }

    private async void OnDeleteClicked(object? sender, EventArgs e)
    {
        if (_editing.Id <= 0)
            return;
        var confirm = await DisplayAlertAsync("Eliminar actividad", $"¿Eliminar '{_editing.Title}' de SQLite?", "Eliminar", "Cancelar");
        if (!confirm)
            return;

        await _db.DeleteActivityAsync(_editing.Id);
        _onSaved?.Invoke();
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
