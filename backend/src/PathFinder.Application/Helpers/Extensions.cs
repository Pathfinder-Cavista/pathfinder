using Microsoft.AspNetCore.Http;
using PathFinder.Application.DTOs;
using PathFinder.Domain.Enums;
using System.ComponentModel;

namespace PathFinder.Application.Helpers
{
    public static class Extensions
    {
        private const long MaxImageSize = 2048000; //2mb
        private const long MaxDocSize = 1024000; //1mb

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

        public static DateTime ToDayEnd(this DateTime date, bool isUtc = true)
        {
            return isUtc ? new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, DateTimeKind.Utc) :
                new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static Paginator<T> Paginate<T>(this IEnumerable<T> source, int page, int size)
        {
            var count = source.LongCount();
            var items = source.Skip((page - 1) * size)
                .Take(size).ToList();

            return new Paginator<T>(items, count, page, size);
        }

        public static (bool Valid, string Message) IsAValidImage(this IFormFile file, UploadMediaType mediaType)
        {
            if(!Enum.IsDefined(typeof(UploadMediaType), mediaType)) 
                return (false, "Invalid media type");

            if(file is null || file.Length <= 0) 
                return (false, "Please upload a file");

            if(mediaType == UploadMediaType.Image)
            {
                var allowedFormats = mediaType.GetDescription().Split('|');
                if (!allowedFormats.Any(f => file.FileName.EndsWith(f))) 
                    return (false, string.Format("Invalid image type. Accepted extensions: {0}",string.Join(',', allowedFormats)));

                if(file.Length > MaxImageSize) 
                    return (false, $"File size too large. Max image size: ({MaxImageSize/(1024 * 1014)}mb)");
            }
            else if(mediaType == UploadMediaType.Document)
            {
                var allowedFormats = mediaType.GetDescription().Split('|');
                if (!allowedFormats.Any(f => file.FileName.EndsWith(f)))
                    return (false, string.Format("Invalid document type. Accepted extensions: {0}", string.Join(',', allowedFormats)));

                if (file.Length > MaxDocSize)
                        return (false, $"File size too large. Max doc size: ({MaxDocSize / (1024 * 1024)}mb)");
            }

            return (true, "Valid");
        }
    }
}
