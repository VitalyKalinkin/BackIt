using System.Configuration;

namespace Lattyf.BackIt.Core.Configuration
{
    /// <summary>
    /// Configuration of directory for backup.
    /// </summary>
    public class DirectoryConfiguration : ConfigurationElement
    {
        /// <summary>
        /// Name of directory.
        /// </summary>
        [ConfigurationProperty("Directory", IsRequired = true)]
        public string Directory
        {
            get { return (string) this["Directory"]; }
            set { this["Directory"] = value; }
        }
    }
}