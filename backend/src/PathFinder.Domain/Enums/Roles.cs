using System.ComponentModel;

namespace PathFinder.Domain.Enums
{
    public enum Roles : byte
    {
        [Description("Talent")]
        Talent,
        [Description("Manager")]
        Manager,
        [Description("Admin")]
        Admin
    }
}
