namespace SpearFishure.Services
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Linq;
    using Microsoft.Extensions.Logging;
    using SpearFishure;
    using SpearFishure.Models;

    /// <summary>
    /// Provide a static class containing XML info on nodes.
    /// </summary>
    public static class NodeDataService
    {
        private static readonly ILogger Logger = Logging.LoggerFactory!.CreateLogger("NodeDataService");

        public static void Initialize()
        {
            try
            {
                Nodes = NodesFromXml("NodeData.xml", Logger);
                Logger.LogInformation("Nodes loaded from XML");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to load from NodeData.xml");
                throw new InvalidOperationException("Failed to load XML", ex);
            }
        }

        /// <summary>
        /// Gets all nodes.
        /// </summary>
        public static List<NodeDataModel> Nodes { get; private set; } = new List<NodeDataModel>();

        private static List<NodeDataModel> NodesFromXml(string filePath, ILogger logger)
        {
            var nodes = new List<NodeDataModel>();
            var doc = XDocument.Load(filePath);
            foreach (var node in doc.Descendants("Node"))
            {
                try
                {
                    string name = (string?)node.Attribute("Name") ?? throw new InvalidDataContractException("Null value in Name field");
                    string planet = (string?)node.Attribute("Planet") ?? throw new InvalidDataContractException("Null value in Planet field");
                    string type = (string?)node.Attribute("Type") ?? throw new InvalidDataContractException("Null value in Type field");
                    string internalName = (string?)node.Attribute("InternalName") ?? throw new InvalidDataContractException("Null value in InternalName field");
                    nodes.Add(new NodeDataModel
                    {
                        Name = name,
                        Planet = planet,
                        Type = type,
                        InternalName = internalName,
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Exception thrown when parsing: {Node}", node);
                    throw new InvalidOperationException($"Failed to parse node: {node}", ex);
                }
            }

            return nodes;
        }
    }
}
