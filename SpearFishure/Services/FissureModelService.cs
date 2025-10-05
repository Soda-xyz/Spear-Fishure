namespace SpearFishure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using SpearFishure.Models;

    /// <summary>
    /// Service that periodically fetches fissure data from the Warframe API and stores it in memory.
    /// </summary>
    public class FissureModelService : IDisposable // TODO: EXpain this
    {
        private readonly Timer timer;
        private readonly HttpClient httpClient = new ();
        private List<FissureModel> fissures = new ();
        private readonly object @lock = new ();

        public IReadOnlyList<FissureModel> Fissures
        {
            get
            {
                lock (this.@lock)
                {
                    return this.fissures;
                }
            }
        }

        public FissureModelService()
        {
            // Fetch immediately, then every 60 seconds
            this.timer = new Timer(async _ => await this.FetchAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private async Task FetchAsync()
        {
            try
            {
                var json = await this.httpClient.GetStringAsync("https://content.warframe.com/dynamic/worldState.php");
                var fissures = FissureModel.FromJson(json);
                lock (this.@lock)
                {
                    this.fissures = fissures ?? new List<FissureModel>();
                }
            }
            catch (Exception ex)
            {
                // Handle/log error as needed
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.timer?.Dispose();
            this.httpClient?.Dispose();
        }
    }
}
