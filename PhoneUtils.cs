using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WAChatFlow.Shared.Common
{
    public static class PhoneUtils
    {
        public static string NormalizeToWhatsAppId(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var digits = Regex.Replace(input, @"\D", "");

            if (digits.StartsWith("00")) digits = digits[2..];

            if (Regex.IsMatch(digits, @"^(044|045)")) digits = digits[3..];

            if (digits.StartsWith("52") && digits.Length == 12) digits = "521" + digits[2..];

            if (digits.StartsWith("521") && digits.Length >= 13) return "521" + digits[^10..];

            if (digits.StartsWith("1") && digits.Length == 11) return "521" + digits[1..];

            if (digits.Length == 10) return "521" + digits;

            if (digits.Length > 13) return "521" + digits[^10..];

            return digits;
        }

        public static string FormatPhoneForDisplay(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            var digits = Regex.Replace(input, @"\D", "");

            if (digits.StartsWith("521") && digits.Length == 13)
            {
                digits = digits[2..];
            }

            if (digits.Length == 10)
            {
                return $"+52 {digits.Substring(0, 2)} {digits.Substring(2, 2)} {digits.Substring(4, 2)} {digits.Substring(6, 2)} {digits.Substring(8, 2)}";
            }

            if (digits.Length == 11 && digits.StartsWith("1"))
            {
                var national = digits[1..];
                return $"+52 {national.Substring(0, 2)} {national.Substring(2, 2)} {national.Substring(4, 2)} {national.Substring(6, 2)} {national.Substring(8, 2)}";
            }

            return "+52 " + digits;
        }
    }
}


