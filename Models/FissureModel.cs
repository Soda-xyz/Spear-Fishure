namespace SpearFishure.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;

    /// <summary>
    /// Model for fissure data.
    /// </summary>
    public class FissureModel
    {
        public string Id { get; set; }

        public DateTime Expiry { get; set; }

        public string Node { get; set; }

        public bool? Hard { get; set; }

        /// <summary>
        /// Json Deserializer for WF API.
        /// </summary>
        /// <returns></returns>
        public static List<FissureModel> FromJson(string json)
        {
            return JsonSerializer.Deserialize<List<FissureModel>>(json) ?? new List<FissureModel>();
        }
    }
}
