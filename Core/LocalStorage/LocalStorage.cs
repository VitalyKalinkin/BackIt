using System;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using Lattyf.BackIt.Core.Common;
using Lattyf.BackIt.Core.Info;
using log4net;

namespace Lattyf.BackIt.Core.LocalStorage
{
    /// <summary>
    /// Local storage.
    /// </summary>
    public class LocalStorage
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (LocalStorage));

        private static LocalStorage _instance;
        private static readonly object _instanceMutex = new object();

        /// <summary>
        /// „тобы никто не уволок.
        /// </summary>
        private LocalStorage()
        {
            _log.InfoFormat("Testing Local Storage...");

            try
            {
                using (var connection = new SqlCeConnection(GetConnectionString()))
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT COUNT(*) FROM [File]";

                    connection.Open();

                    var result = (int) command.ExecuteScalar();
                    CommonUtil.UseIt(result);

                    _log.InfoFormat("Local Storage has been tested successfully.");
                }
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception while testing local storage.", ex);
            }
        }

        ///<summary>
        /// Ёкземпл€р класса.
        ///</summary>
        public static LocalStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceMutex)
                        if (_instance == null)
                            _instance = new LocalStorage();
                }

                return _instance;
            }
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["LocalStorageConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Checks file modification status by last write time.
        /// </summary>
        /// <param name="objectInfo">Information of file for checking.</param>
        /// <returns>True, if file modified.</returns>
        public bool IsFileModified(ObjectInfo objectInfo)
        {
            Guard.IsNotNull(objectInfo, "objectInfo");
            Guard.IsNotNull(objectInfo.FullPath, "objectInfo.FullPath");

            var lastWriteTime = new DateTime(
                objectInfo.LastWriteTime.Year,
                objectInfo.LastWriteTime.Month,
                objectInfo.LastWriteTime.Day,
                objectInfo.LastWriteTime.Hour,
                objectInfo.LastWriteTime.Minute,
                objectInfo.LastWriteTime.Second);

            try
            {
                bool isFileExists;

                using (var connection = new SqlCeConnection(GetConnectionString()))
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "SELECT COUNT(*) FROM [File] WHERE Name = @fileName AND LastWriteTime = @lastWriteTime";

                    command.Parameters.AddWithValue("@fileName", objectInfo.FullPath);
                    command.Parameters.AddWithValue("@lastWriteTime", lastWriteTime);

                    connection.Open();

                    isFileExists = (int)command.ExecuteScalar() == 0;
                }

                return isFileExists;
            }
            catch (SqlCeException ex)
            {
                _log.Fatal("Exception while adding file to local storage.", ex);
                return false;
            }
        }

        /// <summary>
        /// Checks file modification status by content hash code.
        /// </summary>
        /// <param name="objectInfo">Information of file for checking.</param>
        /// <param name="controlSum">Control sum of file content.</param>
        /// <returns>True, if file modified.</returns>
        public bool IsFileModifiedByContent(ObjectInfo objectInfo, byte[] controlSum)
        {
            Guard.IsNotNull(objectInfo, "objectInfo");
            Guard.IsNotNull(objectInfo.FullPath, "objectInfo.FullPath");

            try
            {
                bool isFileExists;

                using (var connection = new SqlCeConnection(GetConnectionString()))
                using (SqlCeCommand command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "SELECT TOP (1) Id FROM [File] WHERE Name = @fileName AND ControlSum = @controlSum ORDER BY LastWriteTime DESC";

                    command.Parameters.AddWithValue("@fileName", objectInfo.FullPath);
                    command.Parameters.Add("@controlSum", SqlDbType.Binary).Value = controlSum;

                    connection.Open();

                    var reader = command.ExecuteReader();

                    isFileExists =  reader != null && !reader.Read();
                }

                return isFileExists;
            }
            catch (SqlCeException ex)
            {
                _log.Fatal("Exception while adding file to local storage.", ex);
                return false;
            }
        }
    }
}