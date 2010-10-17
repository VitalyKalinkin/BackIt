using System.Collections.Generic;
using Lattyf.BackIt.Core.Configuration;
using Lattyf.BackIt.Core.Info;

namespace Lattyf.BackIt.Core.Scanner
{
    /// <summary>
    /// Interface of object for scanning.
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Initialize instance of object.
        /// </summary>
        /// <param name="config">Parameters of configuration.</param>
        void Initialize(ICoreConfiguration config);

        /// <summary>
        /// Scans scan area and returns found objects.
        /// </summary>
        /// <returns>List of modified and new objects.</returns>
        IList<ObjectInfo> Scan();
    }
}