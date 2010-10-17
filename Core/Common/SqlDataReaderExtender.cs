#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

#endregion

namespace Lattyf.BackIt.Core.Common
{
    /// <summary>
    /// ����������� ������������ ������ <see cref="SqlDataReader"/>
    /// </summary>
    public static class SqlDataReaderExtender
    {
        /// <summary>
        /// ���������� ����������� �� ������ �������� ��� default value, ���� � ������ <see langword="null"/>.
        /// </summary>
        /// <param name="reader">����� ��� ������.</param>
        /// <param name="fieldName">��� ����, ������� ����� ���������.</param>
        /// <returns>�������� ���� ��� default value.</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, string fieldName)
        {
            return GetValueOrDefault(reader, fieldName, default(T));
        }

        /// <summary>
        /// ���������� ����������� �� ������ �������� ��� default value, ���� � ������ <see langword="null"/>.
        /// </summary>
        /// <param name="reader">����� ��� ������.</param>
        /// <param name="fieldIndex">������ ����.</param>
        /// <returns>�������� ���� ��� default value.</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, int fieldIndex)
        {
            return GetValueOrDefault(reader, fieldIndex, default(T));
        }

        /// <summary>
        /// ���������� ����������� �� ������ �������� ��� default value, ���� � ������ <see langword="null"/>.
        /// </summary>
        /// <param name="reader">����� ��� ������.</param>
        /// <param name="fieldIndex">������ ����.</param>
        /// <param name="defaultValue">�������� �� ���������. ����� ����������, ���� ���� <see langword="null"/>.</param>
        /// <returns>�������� ���� ��� default value.</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, int fieldIndex, T defaultValue)
        {
            Guard.IsNotNull(reader, "reader");

            if (reader.IsDBNull(fieldIndex))
                return defaultValue;

            object value = reader.GetValue(fieldIndex);

            if (!(value is T))
            {
                string fieldName = reader.GetName(fieldIndex);
                throw new InvalidDataException(
                    string.Format(
                                     "Value of field \"{0}\" has type {1} but readed as {2}",
                                     fieldName,
                                     value.GetType(),
                                     typeof (T)));
            }

            return (T) value;
        }

        /// <summary>
        /// ���������� ����������� �� ������ �������� ��� default value, ���� � ������ <see langword="null"/>.
        /// </summary>
        /// <param name="reader">����� ��� ������.</param>
        /// <param name="fieldName">��� ����, ������� ����� ���������.</param>
        /// <param name="defaultValue">�������� ��� �����������, ���� � ���� <see langword="null"/>.</param>
        /// <returns>�������� ���� ��� default value.</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, string fieldName, T defaultValue)
        {
            Guard.IsNotNull(reader, "reader");

            try
            {
                int fieldIndex = reader.GetOrdinal(fieldName);
                return GetValueOrDefault(reader, fieldIndex, defaultValue);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException(
                    string.Format("Field with name \"{0}\" does not exists in reader.", fieldName));
            }
        }
    }
}