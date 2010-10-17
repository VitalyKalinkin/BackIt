#region

using System;
using System.IO;

#endregion

namespace Lattyf.BackIt.Core.Common
{
    /// <summary>
    /// Утилитарный класс-"защитник" (содержит методы для проверки чего-либо).
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Проверяет существование папки. Если папка не существует, то будет выкинуто исключение.
        /// </summary>
        /// <param name="folderName">Имя папки для проверки.</param>
        /// <exception cref="DirectoryNotFoundException">Выкидывается если папка не существует.</exception>
        public static void CheckFolderExistence(string folderName)
        {
            var info = new DirectoryInfo(folderName);

            if (!info.Exists)
                throw new DirectoryNotFoundException("Can't find directory \"" + folderName + "\".");
        }

        /// <summary>
        /// Проверяет аргумент на равенство <see langword="null"/>.
        /// </summary>
        /// <param name="arg">Аргумент для проверки.</param>
        /// <param name="argName">Имя аргумента</param>
        public static void IsNotNull(object arg, string argName)
        {
            if (ReferenceEquals(arg, null))
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Проверяет существование файла на жёстком диске компьютера.
        /// </summary>
        /// <param name="fileName">Имя файла для проверки.</param>
        /// <exception cref="FileNotFoundException">Выбрасывается если проверяемый файл не существует.</exception>
        public static void CheckFileExistence(string fileName)
        {
            var info = new FileInfo(fileName);

            if (!info.Exists)
                throw new FileNotFoundException("Can't find file \"" + fileName + "\"");
        }
    }
}