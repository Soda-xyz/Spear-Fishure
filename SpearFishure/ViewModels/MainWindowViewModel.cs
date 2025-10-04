namespace SpearFishure.ViewModels;

    /// <summary>
    /// The main window view model for the SpearFishure application.
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// Gets a greeting message for the main window.
    /// </summary>
    public string Greeting { get; } = "Welcome to Avalonia!";
}
