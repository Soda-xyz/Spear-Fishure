namespace SpearFishure
{
    using System.Linq;
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Data.Core;
    using Avalonia.Data.Core.Plugins;
    using Avalonia.Markup.Xaml;
    using SpearFishure.Services;
    using SpearFishure.ViewModels;
    using SpearFishure.Views;

    /// <summary>
    /// Represents the main application class for SpearFishure.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the global FissureModelService instance, kept alive for the app lifetime.
        /// </summary>
        public static Services.FissureModelService? FissureServiceInstance { get; private set; }

        /// <summary>
        /// Initializes the application and loads XAML.
        /// </summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// Called when the framework initialization is completed.
        /// </summary>
        public override void OnFrameworkInitializationCompleted()
        {
            // Create and keep alive the global FissureModelService
            FissureServiceInstance = new Services.FissureModelService();

            // Initialize Node Data for program wide use.
            NodeDataService.Initialize();
            if (this.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                this.DisableAvaloniaDataAnnotationValidation();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(FissureServiceInstance),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Make this method 'static'", Justification = "Called for pre-configuration; instance context preferred for future extensibility.")]
        private void DisableAvaloniaDataAnnotationValidation()
        {
            // Get an array of plugins to remove
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            // remove each entry found
            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}
