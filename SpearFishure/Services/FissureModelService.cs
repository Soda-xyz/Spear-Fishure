namespace SpearFishure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using SpearFishure;
    using SpearFishure.Models;

    /// <summary>
    /// Service that periodically fetches fissure data from the Warframe API and stores it in memory.
    /// </summary>
    public class FissureModelService : IDisposable
    {
        public FissureModelService()
        {
            this.logger = Logging.LoggerFactory!.CreateLogger<FissureModelService>();
            System.Diagnostics.Debug.WriteLine("FissureModelService constructor called");
            this.logger.LogInformation("FissureModelService constructor called");

            // Fetch immediately, then every 60 seconds
            this.timer = new Timer(async _ => await this.FetchAsync(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private readonly object @lock = new ();
        private readonly ILogger<FissureModelService> logger;
        private readonly Timer timer;
        private readonly HttpClient httpClient = new ();
        private List<FissureModel> fissures = new ();

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

        private async Task FetchAsync()
        {
            System.Diagnostics.Debug.WriteLine("FetchAsync called");
            this.logger.LogInformation("FetchAsync called");
            try
            {
                var json = await this.httpClient.GetStringAsync("https://content.warframe.com/dynamic/worldState.php");
                using var doc = System.Text.Json.JsonDocument.Parse(json);

                var allFissures = new List<FissureModel>();
                bool foundAny = false;

                if (doc.RootElement.TryGetProperty("ActiveMissions", out var activeMissions))
                {
                    var parsedFissures = System.Text.Json.JsonSerializer.Deserialize<List<FissureModel>>(activeMissions.GetRawText());
                    if (parsedFissures != null)
                    {
                        allFissures.AddRange(parsedFissures);
                        foundAny = true;
                    }
                }

                if (doc.RootElement.TryGetProperty("VoidStorms", out var voidStorms))
                {
                    var parsedFissures = System.Text.Json.JsonSerializer.Deserialize<List<FissureModel>>(voidStorms.GetRawText());
                    if (parsedFissures != null)
                    {
                        allFissures.AddRange(parsedFissures);
                        foundAny = true;
                    }
                }

                if (foundAny)
                {
                    System.Diagnostics.Debug.WriteLine($"Fetched {allFissures.Count} fissures (ActiveMissions + VoidStorms)");
                    this.logger.LogInformation("Fetched {Count} fissures (ActiveMissions + VoidStorms)", allFissures.Count);
                    lock (this.@lock)
                    {
                        this.fissures = allFissures;
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No fissure arrays found in JSON");
                    this.logger.LogInformation("No fissure arrays found in JSON");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FetchAsync error: {ex.Message}");
                this.logger.LogError(ex, "FetchAsync error");
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
