using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class CommonExtensions
    {
        public static string ToDateFormat(this DateTime date)
        {
            var culture = CultureInfo.CurrentUICulture;
            return date.ToString(culture.DateTimeFormat.ShortDatePattern);
        }
        public static string ToCurrency(this int s)
        {
            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            return s.ToString("C0", culture);
        }
        public static string ToCurrency(this double s)
        {
            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            return s.ToString("C0", culture);
        }
        public static string ToCurrency(this decimal s)
        {
            //var culture = CultureInfo.CurrentUICulture;
            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            return s.ToString("C0", culture);
        }
        public static string ToVNDecimal(this decimal s)
        {
            var culture = CultureInfo.CreateSpecificCulture("vi-VN");
            return s.ToString("F0", culture);
        }
        public static string ConvertVNString(this string stringInput)
        {
            if (!string.IsNullOrEmpty(stringInput))
            {
                Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = stringInput.Normalize(NormalizationForm.FormD);
                stringInput = regex.Replace(temp, String.Empty)
                            .Replace('\u0111', 'd').Replace('\u0110', 'D');
                string convert = "ĂÂÀẰẦÁẮẤẢẲẨÃẴẪẠẶẬỄẼỂẺÉÊÈỀẾẸỆÔÒỒƠỜÓỐỚỎỔỞÕỖỠỌỘỢƯÚÙỨỪỦỬŨỮỤỰÌÍỈĨỊỲÝỶỸỴĐăâàằầáắấảẳẩãẵẫạặậễẽểẻéêèềếẹệôòồơờóốớỏổởõỗỡọộợưúùứừủửũữụựìíỉĩịỳýỷỹỵđ";
                string To = "AAAAAAAAAAAAAAAAAEEEEEEEEEEEOOOOOOOOOOOOOOOOOUUUUUUUUUUUIIIIIYYYYYDaaaaaaaaaaaaaaaaaeeeeeeeeeeeooooooooooooooooouuuuuuuuuuuiiiiiyyyyyd";
                for (int i = 0; i < To.Length; i++)
                {
                    stringInput = stringInput.Replace(convert[i], To[i]);
                }
                stringInput = stringInput.Replace(" ", "-");
                stringInput = Regex.Replace(stringInput, @"[^0-9a-zA-Z-]+", "");
            }

            return stringInput;
        }
        public static string ToCurrencyNotUnit(this int s)
        {
            return s.ToString("N0");
        }
        public static string ToCurrencyNotUnit(this double s)
        {
            return s.ToString("N0");
        }
        public static string ToCurrencyNotUnit(this decimal s)
        {
            return s.ToString("N0");
        }

        /// <summary>
        /// Return true if list is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Return true if list is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || list.Count() == 0;
        }
    }
}
