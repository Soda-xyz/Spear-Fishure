namespace SpearFishure.Models
{
    /// <summary>
    /// Datamodel to hold information about mission nodes.
    /// </summary>
    public class NodeDataModel
    {
        required public string Name { get; set; }

        required public string Planet { get; set; }

        required public string Type { get; set; }

        required public string InternalName { get; set; }
    }
}
