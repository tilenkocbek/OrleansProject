using System;

namespace Config
{
    /// <summary>
    /// Class that holds all configuration for SiloCluster. In the future, this class could hold values that would be read from some configuration file (ports, clusterId, serviceId, etc.)
    /// </summary>
    public sealed class Configuration
    {
        /// <summary>
        /// Unique Identifier of a cluster.
        /// </summary>
        public static readonly string ClusterId = "HaveIBeenPwnedCluster";

        /// <summary>
        /// Unique service identifier.
        /// </summary>
        public static readonly string ServiceId = "HaveIBeenPwnedService";
    }
}
