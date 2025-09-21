using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum Roles : byte
    {
        /// <summary>
        /// Talent
        /// </summary>
        [Description("Talent")]
        Talent,
        /// <summary>
        /// Manager
        /// </summary>
        [Description("Manager")]
        Manager,
        /// <summary>
        /// Admin
        /// </summary>
        [Description("Admin")]
        Admin
    }
}
