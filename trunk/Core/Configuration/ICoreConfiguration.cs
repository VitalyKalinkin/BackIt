namespace Lattyf.BackIt.Core.Configuration
{
    /// <summary>
    /// The interface for configuring file system scanner.
    /// </summary>
    public interface ICoreConfiguration
    {
        /// <summary>
        /// The list of directories for backup.
        /// </summary>
        DirectoriesListConfiguration Directories { get; set; }
    }
}