using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA.Pages;

public partial class LoginPage : ContentPage
{
    private readonly DatabaseService _db;
    private readonly SessionService _session;

    public LoginPage(DatabaseService db, SessionService session)
    {
        InitializeComponent();
        _db = db;
        _session = session;
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        await TrySignInAsync(EmailEntry.Text ?? string.Empty, PasswordEntry.Text ?? string.Empty);
    }

    private async void OnDemoAdminClicked(object? sender, EventArgs e)
    {
        EmailEntry.Text = "santiago.viteri.alcivar@udla.edu.ec";
        PasswordEntry.Text = "udla2027";
        await TrySignInAsync(EmailEntry.Text, PasswordEntry.Text);
    }

    private async void OnDemoCoordClicked(object? sender, EventArgs e)
    {
        EmailEntry.Text = "justin.frutos@udla.edu.ec";
        PasswordEntry.Text = "udla2027";
        await TrySignInAsync(EmailEntry.Text, PasswordEntry.Text);
    }

    private async Task TrySignInAsync(string email, string password)
    {
        ErrorLabel.IsVisible = false;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorLabel.Text = "Ingresa tu correo institucional y contraseña.";
            ErrorLabel.IsVisible = true;
            return;
        }

        LoginButton.IsEnabled = false;
        try
        {
            var member = await _db.AuthenticateAsync(email, password);
            if (member is null)
            {
                ErrorLabel.Text = "Credenciales inválidas. Prueba con tu correo @udla.edu.ec y clave demo.";
                ErrorLabel.IsVisible = true;
                return;
            }

            _session.SignIn(member);
            if (Application.Current?.Windows.FirstOrDefault() is Window win)
            {
                win.Page = new AppShell(_db, _session);
            }
        }
        finally
        {
            LoginButton.IsEnabled = true;
        }
    }
}
