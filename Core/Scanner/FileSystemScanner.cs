using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

        private readonly SHA1CryptoServiceProvider _provider = new SHA1CryptoServiceProvider();
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
            var totalFiles = new List<ObjectInfo>();

            _log.InfoFormat("Starting scanning process...");

            foreach (DirectoryConfiguration directory in _configuration.Directories)
            {
                var dirInfo = new DirectoryInfo(directory.Directory);

                if (!dirInfo.Exists)
                {
                    _log.InfoFormat("Directory \"{0}\" does not exists.", directory.Directory);
                    continue;
                }

                _log.InfoFormat("Scanning root directory \"{0}\"...", dirInfo.FullName);

                // Scan folders recursive.
                IEnumerable<ObjectInfo> objects = ScanFolder(dirInfo);

                totalFiles.AddRange(objects);
            }

            _log.InfoFormat("Found total files: {0}.", totalFiles.Count);

            // Checking file modification status.
            IList<ObjectInfo> modifiedFiles = CheckFilesModificationStatus(totalFiles);

            _log.InfoFormat("Found {0} modified files", modifiedFiles.Count);

            return modifiedFiles;
        }

        #endregion

        /// <summary>
        /// Checks files modification status by database.
        /// </summary>
        /// <param name="totalFiles">Total list of files for checking.</param>
        /// <returns>Only modified files.</returns>
        private IList<ObjectInfo> CheckFilesModificationStatus(IEnumerable<ObjectInfo> totalFiles)
        {
            IEnumerable<ObjectInfo> modified = from objectInfo in totalFiles
                                               let checkingResult = CheckFile(objectInfo)
                                               where checkingResult
                                               select objectInfo;

            _log.InfoFormat("Evaluating files control sum.");

            IEnumerable<ObjectInfo> result = from objectInfo in modified
                                             let controlSum = EvaluateHash(objectInfo)
                                             select
                                                 new ObjectInfo
                                                     {
                                                         FullPath = objectInfo.FullPath,
                                                         LastWriteTime = objectInfo.LastWriteTime,
                                                         ControlSum = controlSum
                                                     };

            return result.ToList();
        }

        /// <summary>
        /// Check file modification status.
        /// </summary>
        /// <param name="objectInfo">Info of file for checking modification.</param>
        /// <returns>True, if file modified.</returns>
        private bool CheckFile(ObjectInfo objectInfo)
        {
            try
            {
                _log.DebugFormat("Checking file \"{0}\".", objectInfo.FullPath);

                bool isFileModifiedByDate = LocalStorage.LocalStorage.Instance.IsFileModified(objectInfo);

                if (!isFileModifiedByDate)
                    return false;

                // Evaluate control sum.
                byte[] controlSum = EvaluateHash(objectInfo);

                bool isFileModifiedByContent = LocalStorage.LocalStorage.Instance.IsFileModifiedByContent(objectInfo,
                                                                                                          controlSum);

                return isFileModifiedByContent;
            }
            catch (Exception ex)
            {
                _log.WarnFormat("Exception while checking file modification status \"{0}\": {1}", objectInfo.FullPath,
                                ex);
                return false;
            }
        }

        private byte[] EvaluateHash(ObjectInfo objectInfo)
        {
            using (
                Stream inputStream = new FileStream(objectInfo.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                )
            {
                byte[] hash = _provider.ComputeHash(inputStream);
                return hash;
            }
        }


        /// <summary>
        /// Scans folder recursive.
        /// </summary>
        /// <param name="dirInfo">Directory for scanning.</param>
        /// <returns>Objects in directory.</returns>
        private static IEnumerable<ObjectInfo> ScanFolder(DirectoryInfo dirInfo)
        {
            _log.DebugFormat("Scanning folder \"{0}\"", dirInfo.FullName);

            var result = new List<ObjectInfo>();

            try
            {
                // Get files in folder.
                IEnumerable<ObjectInfo> objects = from file in dirInfo.GetFiles()
                                                  select
                                                      new ObjectInfo
                                                          {
                                                              FullPath = file.FullName,
                                                              ControlSum = null,
                                                              LastWriteTime = file.LastAccessTimeUtc
                                                          };

                result.AddRange(objects);
            }
            catch (Exception ex)
            {
                _log.WarnFormat("Exception while scanning directory \"{0}\": {1}", dirInfo, ex);
            }

            try
            {
                // Get all folders. Recurse
                DirectoryInfo[] nestedDirs = dirInfo.GetDirectories();

                foreach (DirectoryInfo directoryInfo in nestedDirs)
                {
                    IEnumerable<ObjectInfo> nestedObjects = ScanFolder(directoryInfo);

                    result.AddRange(nestedObjects);
                }
            }
            catch (Exception ex)
            {
                _log.WarnFormat("Exception while scanning directory \"{0}\": {1}", dirInfo, ex);
            }

            return result;
        }
    }
}