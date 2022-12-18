namespace Kernel.LogProvider.SerilogProvider.Enum
{
    public enum LogCategories
    {
        /// <summary>
        /// The lifeblood of operational intelligence - things
        /// happen.
        /// </summary>
        Information = 2,

        /// <summary>
        /// Service is degraded or endangered.
        /// </summary>
        Warning = 3,

        /// <summary>
        /// Functionality is unavailable, invariants are broken
        /// or data is lost.
        /// </summary>
        Error = 4,

        /// <summary>
        /// If you have a pager, it goes off when one of these
        /// occurs.
        /// </summary>
        Fatal = 5
    }
}