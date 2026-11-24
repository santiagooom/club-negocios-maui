using ClubNegociosUDLA.Pages;
using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA;

public partial class App : Application
{
    private readonly DatabaseService _db;
    private readonly SessionService _session;

    public App(DatabaseService db, SessionService session)
    {
        InitializeComponent();
        UserAppTheme = AppTheme.Light;
        _db = db;
        _session = session;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new SplashPage(_db, _session))
        {
            Title = "Club de Negocios Digitales · UDLA (.NET MAUI)"
        };
    }
}
