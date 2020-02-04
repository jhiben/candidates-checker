using System.Globalization;
using System.Text;

namespace CandidatesChecker.Web.Check.Common
{
    public static class StringExtensions
    {
        public static string RemoveDiacritics(this string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return s;

            string normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
