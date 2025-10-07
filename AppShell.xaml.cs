using CounterJO.Views;

namespace CounterJO;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.CounterPage), typeof(CounterPage));
    }
}