using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EcommerceApp.Application.Common.Utilities
{
    public static class StringUtilities
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return Guid.NewGuid().ToString();
            }

            string slug = input.ToLowerInvariant();

            slug = Regex.Replace(slug, @"[^\p{L}\p{N}\s-]", ""); // Retain letters, numbers, spaces, and hyphens

            slug = Regex.Replace(slug, @"\s+", " ").Trim(); // Replace multiple consecutive spaces with a single space and trim

            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim(); // Replace multiple consecutive spaces with a single space and trim

            slug = Regex.Replace(slug, @"\s", "-"); // Replace all spaces with hyphens

            return slug;
        }
    }
}
