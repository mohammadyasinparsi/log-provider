using System;

namespace Kernel.LogProvider.Helpers
{
    public static class StringValueCheckExtension
    {
        /// <summary>
        /// whether a string value contains a value or not
        /// basically we are using IsNullOrWhiteSpace in a reverse way
        /// </summary>
        /// <param name="str">string input to test against</param>
        /// <returns>true of it has string value and false if it does not</returns>
        public static bool ContainsString(this string str) => !string.IsNullOrWhiteSpace(str);

        /// <summary>
        /// whether a string value contains a value or not and throws exception
        /// basically we are using IsNullOrWhiteSpace in a reverse way
        /// </summary>
        /// <param name="inputString">string input to test against</param>
        /// <exception cref="T:System.ArgumentNullException">ArgumentNullException</exception>
        public static void ContainsStringWithException(this string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                throw new ArgumentNullException(nameof (inputString), "input string is null");
        }
    }
}