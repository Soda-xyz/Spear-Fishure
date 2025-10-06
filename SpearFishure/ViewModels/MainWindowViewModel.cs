namespace SpearFishure.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Timers;
    using Avalonia.Threading;
    using SpearFishure.Models;
    using SpearFishure.Services;

    /// <summary>
    /// The main window view model for the SpearFishure application.
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly FissureModelService fissureService;
        private readonly Timer refreshTimer;

        public ObservableCollection<FissureModel> Fissures { get; } = new ObservableCollection<FissureModel>();

        public ObservableCollection<PlanetViewModel> Planets { get; } = new ObservableCollection<PlanetViewModel>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="fissureService">The shared FissureModelService instance.</param>
        public MainWindowViewModel(FissureModelService fissureService)
        {
            this.fissureService = fissureService;

            // Initial load
            this.RefreshFissures();

            // Set up timer to refresh every 10 seconds for demo/testing
            this.refreshTimer = new Timer(10000);
            this.refreshTimer.Elapsed += (s, e) => this.RefreshFissures();
            this.refreshTimer.Start();

            var allNodes = NodeDataService.Nodes;
            var grouped = allNodes.GroupBy(n => n.Planet);

            foreach (var group in grouped)
            {
                this.Planets.Add(new PlanetViewModel(group.Key, group.ToList()));
            }
        }

        private void RefreshFissures()
        {
            var fissures = this.fissureService.Fissures;
            Dispatcher.UIThread.Post(() =>
            {
                this.Fissures.Clear();
                foreach (var fissure in fissures)
                {
                    this.Fissures.Add(fissure);
                }
            });
        }
    }
}
