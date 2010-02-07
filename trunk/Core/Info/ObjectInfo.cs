using System;

namespace Lattyf.BackIt.Core.Info
{
    /// <summary>
    /// Information of object for backup.
    /// </summary>
    public class ObjectInfo
    {
        /// <summary>
        /// The full path to object.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The last time of writing to object;
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// The control sum of object;
        /// </summary>
        public byte[] ControlSum { get; set; }
    }
}