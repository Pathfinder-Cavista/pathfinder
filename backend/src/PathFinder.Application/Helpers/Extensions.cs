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

        public static string CapitalizeFirstLetterOnly(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                var word = words[i];
                if (char.IsLetter(word[0]))
                {
                    if(word.Length > 1)
                    {
                        words[i] = char.ToUpper(word[0]) + word[1..];
                    }
                    else
                    {
                        words[i] = char.ToUpper(word[0]).ToString();
                    }
                }
            }

            return string.Join(" ", words);
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list is not null && list.Any();
        }

        public static DateTime ToDayEnd(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
    }
}
