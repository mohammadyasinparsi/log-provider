using System;
using Kernel.LogProvider.Helpers;

namespace Kernel.Utilites.ValueCheck
{
    /// <summary>
    /// A class with NullCheck function to check any object if its null
    /// </summary>
    public class Precondition
    {
        /// <summary>
        /// heck any object if its null and throws ArgumentNullException
        /// </summary>
        /// <param name="input">Object to check against</param>
        /// <param name="objectName">name of the object(using in exception message)</param>
        /// <exception cref="T:System.ArgumentNullException">ArgumentNullException</exception>
        public static void NullCheck(object input, string objectName = "")
        {
            if (input != null)
                return;
            if (objectName.ContainsString())
                throw new ArgumentNullException(objectName, objectName + " is null");
            throw new ArgumentNullException(nameof (input), "input is null");
        }
    }
}