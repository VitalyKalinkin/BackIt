using System.Configuration;

namespace Lattyf.BackIt.Core.Configuration
{
    /// <summary>
    /// Configuration of list of directories for backup.
    /// </summary>
    public class DirectoriesListConfiguration : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates new element of collection.
        /// </summary>
        /// <returns>New element of collection.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DirectoryConfiguration();
        }

        /// <summary>
        /// Returns key of element.
        /// </summary>
        /// <param name="element">Element those key will be returned.</param>
        /// <returns>Key of element.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DirectoryConfiguration) element).Directory;
        }
    }
}