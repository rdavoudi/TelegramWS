using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace TWS.Common
{
    public static class GuardExt
    {
        /// <summary>
        /// Checks if the argument is null.
        /// </summary>
        public static void CheckArgumentIsNull(this object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Checks if the parameter is null.
        /// </summary>
        public static void CheckMandatoryOption(this string s, string name)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException(name);
        }

        

        public static bool HasConsecutiveChars(this string inputText, int sequenceLength = 3)
        {
            var charEnumerator = StringInfo.GetTextElementEnumerator(inputText);
            var currentElement = string.Empty;
            var count = 1;
            while (charEnumerator.MoveNext())
            {
                if (currentElement == charEnumerator.GetTextElement())
                {
                    if (++count >= sequenceLength)
                    {
                        return true;
                    }
                }
                else
                {
                    count = 1;
                    currentElement = charEnumerator.GetTextElement();
                }
            }
            return false;
        }

        public static bool IsEmailAddress(this string inputText)
        {
            return !string.IsNullOrWhiteSpace(inputText) && new EmailAddressAttribute().IsValid(inputText);
        }


        public static bool IsValidImageFile(this IFormFile photoFile, int maxWidth = 150, int maxHeight = 150)
        {
            if (photoFile == null || photoFile.Length == 0) return false;
            using (var img = Image.FromStream(photoFile.OpenReadStream()))
            {
                if (img.Width > maxWidth) return false;
                if (img.Height > maxHeight) return false;
            }
            return true;
        }

        public static bool IsImageFile(this byte[] photoFile)
        {
            if (photoFile == null || photoFile.Length == 0)
                return false;

            using (var memoryStream = new MemoryStream(photoFile))
            {
                using (var img = Image.FromStream(memoryStream))
                {
                    return img.Width > 0;
                }
            }
        }

        public static bool IsImageFile(this IFormFile photoFile)
        {
            if (photoFile == null || photoFile.Length == 0)
                return false;

            using (var img = Image.FromStream(photoFile.OpenReadStream()))
            {
                return img.Width > 0;
            }
        }

        
    }
}
