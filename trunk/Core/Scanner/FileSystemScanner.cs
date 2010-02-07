using System;
using System.Collections.Generic;
using Lattyf.BackIt.Core.Configuration;
using Lattyf.BackIt.Core.Info;
using log4net;

namespace Lattyf.BackIt.Core.Scanner
{
    /// <summary>
    /// Scanner of file system.
    /// </summary>
    public class FileSystemScanner : IScanner
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (FileSystemScanner));

        private ICoreConfiguration _configuration;

        #region IScanner Members

        /// <summary>
        /// Initialize a instance of object.
        /// </summary>
        /// <param name="config">Configuration of directories for scanning.</param>
        public void Initialize(ICoreConfiguration config)
        {
            _configuration = config;

            _log.InfoFormat("Initializing file system scanner...");

            _log.InfoFormat("Directories for scanning:");
            foreach (DirectoryConfiguration directory in _configuration.Directories)
            {
                _log.InfoFormat(" * {0}", directory.Directory);
            }
        }

        /// <summary>
        /// Scans file system. Searching new and modified files in file system.
        /// </summary>
        /// <returns>List of new or modified files.</returns>
        public IList<ObjectInfo> Scan()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}