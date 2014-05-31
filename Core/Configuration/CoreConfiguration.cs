using System.Configuration;

namespace Lattyf.BackIt.Core.Configuration
{
    /// <summary>
    /// Main configuration.
    /// </summary>
    public class CoreConfiguration : ConfigurationSection, ICoreConfiguration
    {
        /// <summary>
        /// List of directories for backup.
        /// </summary>
        [ConfigurationProperty("Directories", IsRequired = true)]
        public DirectoriesListConfiguration Directories
        {
            get { return (DirectoriesListConfiguration) this["Directories"]; }
            set { this["Directories"] = value; }
        }
    }
}