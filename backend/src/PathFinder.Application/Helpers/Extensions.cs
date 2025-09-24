using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using PathFinder.Application.DTOs;
using PathFinder.Domain.Enums;
using System.ComponentModel;
using System.Globalization;

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

        public static bool IsValid(this DateTime value)
        {
            return value != DateTime.MinValue && value < DateTime.MaxValue;
        }

        public static T ParseEnum<T>(this string value) where T : struct, Enum
        {
            if (int.TryParse(value, out var num))
            {
                return (T)Enum.ToObject(typeof(T), num);
            }

            if (Enum.TryParse<T>(value, out var enumValue)) { return enumValue; }

            throw new ArgumentNullException(nameof(value));
        }

        public static T TryParseEnum<T>(this string? value, T defaultvalue) where T : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultvalue;
            }

            if(int.TryParse(value, out var num) && Enum.IsDefined(typeof(T), num))
            {
                return (T)Enum.ToObject(typeof(T), num);
            }
            
            if(Enum.TryParse<T>(value, out var enumValue))
            {
                return enumValue;
            }

            return defaultvalue;
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

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list is null || !list.Any();
        }

        public static DateTime ToDayEnd(this DateTime date, bool isUtc = true)
        {
            return isUtc ? new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, DateTimeKind.Utc) :
                new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        public static DateTime ParseToDate(this string dateString)
        {
            DateTime date = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(dateString))
            {
                if(DateTime.TryParseExact(dateString, "yyyy-MM-dd", 
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                {
                    date = parsed;
                }
            }

            return date;
        }

        public static DateTime? ParseToNullableDate(this string dateString)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(dateString))
            {
                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
                {
                    date = parsed;
                }
            }

            return date;
        }

        public static Paginator<T> Paginate<T>(this IEnumerable<T> source, int page, int size)
        {
            var count = source.LongCount();
            var items = source.Skip((page - 1) * size)
                .Take(size).ToList();

            return new Paginator<T>(items, count, page, size);
        }

        public static (bool Valid, string Message) IsAValidFile(this IFormFile file, UploadMediaType mediaType)
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
                    return (false, $"File size too large. Max image size: 2mb");
            }
            else if(mediaType == UploadMediaType.Document)
            {
                var allowedFormats = mediaType.GetDescription().Split('|');
                if (!allowedFormats.Any(f => file.FileName.EndsWith(f)))
                    return (false, string.Format("Invalid document type. Accepted extensions: {0}", string.Join(',', allowedFormats)));

                if (file.Length > MaxDocSize)
                        return (false, $"File size too large. Max doc size: 1mb");
            }

            return (true, "Valid");
        }

        public static (List<DataloadJob> Jobs, List<DataloadUserProfile> Users, List<DataloadJobApplication> Applications) 
            LoadDataloadSeedData(this Stream stream)
        {
            var jobs = new List<DataloadJob>();
            var users = new List<DataloadUserProfile>();
            var applications = new List<DataloadJobApplication>();

            using var workbook = new XLWorkbook(stream);

            //Jobs
            var jobSheet = workbook.Worksheet("Jobs");
            foreach (var row in jobSheet.RowsUsed().Skip(1))
            {
                jobs.Add(new DataloadJob
                {
                    Id = row.Cell(1).GetValue<long>(),
                    Title = row.Cell(2).GetString(),
                    Description = row.Cell(3).GetString(),
                    Location = row.Cell(4).GetString(),
                    Type = row.Cell(5).GetString().TryParseEnum(EmploymentType.Fulltime),
                    Level = row.Cell(6).GetString().TryParseEnum(JobLevel.Entry),
                    Skills = [.. row.Cell(7).GetString().Split(',').Select(s => s.Trim())],
                    Requirements = [.. row.Cell(8).GetString().Split(',').Select(r => r.Trim())],
                    DateOpened = row.Cell(9).GetString().ParseToDate(),
                    DateClosed = row.Cell(10).GetString().ParseToNullableDate(),
                    Status = row.Cell(11).GetString().TryParseEnum(JobStatus.Published),
                });
            }

            //Users
            var userSheet = workbook.Worksheet("Users");
            foreach (var row in userSheet.RowsUsed().Skip(1))
            {
                users.Add(new DataloadUserProfile
                {
                    Id = row.Cell(1).GetValue<long>(),
                    FirstName = row.Cell(2).GetString(),
                    LastName = row.Cell(3).GetString(),
                    Email = row.Cell(4).GetString(),
                    PhoneNumber = row.Cell(5).GetString(),
                    City = row.Cell(6).GetString(),
                    Summary = row.Cell(7).GetString(),
                    YearsOfExperience = row.Cell(8).GetValue<int>(),
                    ProfileSkills = [.. row.Cell(9).GetString().Split(',').Select(s => s.Trim())],
                });
            }

            //Applications
            var applicationSheet = workbook.Worksheet("Applications");
            foreach (var row in applicationSheet.RowsUsed().Skip(1))
            {
                applications.Add(new DataloadJobApplication
                {
                    Id = row.Cell(1).GetValue<long>(),
                    UserId = row.Cell(2).GetValue<long>(),
                    JobId = row.Cell(3).GetValue<long>(),
                    ApplicationDate = row.Cell(4).GetString().ParseToDate(),
                    Status = row.Cell(5).GetString().TryParseEnum(JobApplicationStatus.Applied)
                });
            }

            return (jobs,  users, applications);
        }
    }
}
