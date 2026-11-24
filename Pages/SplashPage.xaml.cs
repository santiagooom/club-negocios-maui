using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class SplashPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly SessionService _session;

    public SplashPage(DatabaseService db, SessionService session)
    {
        InitializeComponent();
        _db = db;
        _session = session;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _db.InitializeAsync();
        StatusLabel.Text = "Base SQLite lista · Cargando acceso institucional...";
        await Task.Delay(700);

        if (Application.Current?.Windows.FirstOrDefault() is Window win)
        {
            win.Page = new LoginPage(_db, _session);
        }
    }
}
