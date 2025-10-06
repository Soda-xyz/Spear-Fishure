namespace SpearFishure.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SpearFishure.Models;

    public class PlanetViewModel : ViewModelBase
    {
        public string PlanetName { get; }

        public ObservableCollection<NodeDataModel> Nodes { get; }

        public PlanetViewModel(string planetName, IEnumerable<NodeDataModel> nodes)
        {
            this.PlanetName = planetName;
            this.Nodes = new ObservableCollection<NodeDataModel>(nodes);
        }
    }
}
