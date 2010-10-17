#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

#endregion

namespace Lattyf.BackIt.Core.Common
{
    /// <summary>
    /// Расширитель возможностей класса <see cref="SqlDataReader"/>
    /// </summary>
    public static class SqlDataReaderExtender
    {
        /// <summary>
        /// Возвращает прочитанную из ридера значение или default value, если в строке <see langword="null"/>.
        /// </summary>
        /// <param name="reader">Ридер для чтения.</param>
        /// <param name="fieldName">Имя поля, которое нужно прочитать.</param>
        /// <returns>Значение поля или default value.</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, string fieldName)
        {
            return GetValueOrDefault(reader, fieldName, default(T));
        }

        /// <summary>
        /// Возвращает прочитанную из ридера значение или default value, если в строке <see langword="null"/>.
        /// </summary>
        /// <param name="reader">Ридер для чтения.</param>
        /// <param name="fieldIndex">Индекс поля.</param>
        /// <returns>Значение поля или default value.</returns>
        public static T GetValueOrDefault<T>(this IDataReader reader, int fieldIndex)
        {
            return GetValueOrDefault(reader, fieldIndex, default(T));
        }

        /// <summary>
        /// Возвращает прочитанную из ридера значение или default value, если в строке <see langword="null"/>.
        /// </summary>
        /// <param name="reader">Ридер для чтения.</param>
        /// <param name="fieldIndex">Индекс поля.</param>
        /// <param name="defaultValue">Значение по умолчанию. Будет возвращено, если поле <see langword="null"/>.</param>
        /// <returns>Значение поля или default value.</returns>
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
        /// Возвращает прочитанную из ридера значение или default value, если в строке <see langword="null"/>.
        /// </summary>
        /// <param name="reader">Ридер для чтения.</param>
        /// <param name="fieldName">Имя поля, которое нужно прочитать.</param>
        /// <param name="defaultValue">Значение для возвращения, если в базе <see langword="null"/>.</param>
        /// <returns>Значение поля или default value.</returns>
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