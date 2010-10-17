#region

using System;
using System.IO;

#endregion

namespace Lattyf.BackIt.Core.Common
{
    /// <summary>
    /// ����������� �����-"��������" (�������� ������ ��� �������� ����-����).
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// ��������� ������������� �����. ���� ����� �� ����������, �� ����� �������� ����������.
        /// </summary>
        /// <param name="folderName">��� ����� ��� ��������.</param>
        /// <exception cref="DirectoryNotFoundException">������������ ���� ����� �� ����������.</exception>
        public static void CheckFolderExistence(string folderName)
        {
            var info = new DirectoryInfo(folderName);

            if (!info.Exists)
                throw new DirectoryNotFoundException("Can't find directory \"" + folderName + "\".");
        }

        /// <summary>
        /// ��������� �������� �� ��������� <see langword="null"/>.
        /// </summary>
        /// <param name="arg">�������� ��� ��������.</param>
        /// <param name="argName">��� ���������</param>
        public static void IsNotNull(object arg, string argName)
        {
            if (ReferenceEquals(arg, null))
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// ��������� ������������� ����� �� ������ ����� ����������.
        /// </summary>
        /// <param name="fileName">��� ����� ��� ��������.</param>
        /// <exception cref="FileNotFoundException">������������� ���� ����������� ���� �� ����������.</exception>
        public static void CheckFileExistence(string fileName)
        {
            var info = new FileInfo(fileName);

            if (!info.Exists)
                throw new FileNotFoundException("Can't find file \"" + fileName + "\"");
        }
    }
}