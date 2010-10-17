using System.Collections.Generic;
using System.Text;

namespace Lattyf.BackIt.Core.Common
{
    /// <summary>
    /// �����-������� ������ ����������� �������.
    /// </summary>
    public static class CommonUtil
    {
        #region Delegates

        /// <summary>
        /// Empty-args delegate.
        /// </summary>
        public delegate void VoidDelegate();

        #endregion

        /// <summary>
        /// �����-�������� ��� ����, ����� ���������� �� ������� �� �������������� ����������.
        /// �� ������ ����� �������� � ���� ����� :)
        /// </summary>
        /// <param name="arg">�������������� ����������.</param>
        public static void UseIt<T>(T arg)
        {
#pragma warning disable 168
            T variable = arg;
#pragma warning restore 168
        }

        /// <summary>
        /// Converts enumeberable to string with elements of sequence.
        /// </summary>
        /// <typeparam name="T">Type of enumeberable items.</typeparam>
        /// <param name="enumerable">List of items.</param>
        /// <returns>String representations.</returns>
        public static string ToReadableString<T>(this IEnumerable<T> enumerable)
        {
            var builder = new StringBuilder();

            builder.Append("[");

            bool isFirstItem = true;
            foreach (T item in enumerable)
            {
                builder.AppendFormat("{1}{0}", item, isFirstItem ? "" : ",");
                isFirstItem = false;
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}