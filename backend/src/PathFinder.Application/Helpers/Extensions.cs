using System.ComponentModel;

namespace PathFinder.Application.Helpers
{
    public static class Extensions
    {
        public static string GetDescription(this Enum value)
        {
            if (Attribute.GetCustomAttribute(value.GetType()?.GetField(value.ToString())!, 
                typeof(DescriptionAttribute)) is DescriptionAttribute descriptionAttribute)
            {
                return descriptionAttribute.Description;
            }

            throw new ArgumentNullException(nameof(value));
        }
    }
}
