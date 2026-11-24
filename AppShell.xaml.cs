using ClubNegociosUDLA.Pages;
using ClubNegociosUDLA.Services;

namespace ClubNegociosUDLA;

public partial class AppShell : Shell
{
    public AppShell(DatabaseService db, SessionService session)
    {
        InitializeComponent();

        MainTabBar.Items.Add(new ShellContent
        {
            Title = "Inicio",
            Route = "HomePage",
            ContentTemplate = new DataTemplate(() => new HomePage(db, session))
        });

        MainTabBar.Items.Add(new ShellContent
        {
            Title = "Actividades",
            Route = "ActivitiesPage",
            ContentTemplate = new DataTemplate(() => new ActivitiesPage(db))
        });

        MainTabBar.Items.Add(new ShellContent
        {
            Title = "Agenda UDLA",
            Route = "AgendaPage",
            ContentTemplate = new DataTemplate(() => new AgendaPage(db))
        });

        MainTabBar.Items.Add(new ShellContent
        {
            Title = "Perfil",
            Route = "ProfilePage",
            ContentTemplate = new DataTemplate(() => new ProfilePage(db, session))
        });
    }
}
