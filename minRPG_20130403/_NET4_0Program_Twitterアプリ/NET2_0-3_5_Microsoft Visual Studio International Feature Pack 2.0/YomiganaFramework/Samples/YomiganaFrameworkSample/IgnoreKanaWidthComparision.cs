using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.International;
using Microsoft.International.Converters;

namespace YomiganaFrameworkSample
{
    /// <summary>
    /// A Japanese comparison option to ignore Kana Width.
    /// Indicates that the string comparison must ignore the character width. For
    /// example, Japanese katakana characters can be written as full-width or half-width.
    /// If this value is selected, the katakana characters written as full-width
    /// are considered equal to the same characters written as half-width.
    /// </summary>
    class IgnoreKanaWidthComparision : JapaneseComparison
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreKanaKindComparison"/> class.
        /// </summary>
        /// <param name="comparer">to be decorated with an Japanese comparison option.</param>
        /// <remarks>a derived class for this abstract class must implements ctor(StringComparer compare):base(compare) as its constructor.</remarks>
        public IgnoreKanaWidthComparision(StringComparer comparer)
            : base(comparer)
        {
        }

        /// <summary>
        /// Gets Comparison&lt;string&gt;for the generic delegate used for comparison.
        /// </summary>
        /// <returns>A Comparison&lt;string&gt; generic delegate.</returns>
        public override Comparison<string> GetComparison()
        {
            return this.Compare;
        }

        /// <summary>
        /// Compares two  strings after ignore Kana Width and returns an indication of their relative sort order.
        /// </summary>
        /// <param name="x">A string to be compared.</param>
        /// <param name="y">A string to be compared.</param>
        /// <returns>
        /// A value less than zero means <paramref name="x"/> is less than <paramref name="y"/>; 
        /// a value of zero means <paramref name="x"/> equals to <paramref name="y"/>; 
        /// a value greater than zero means <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public override int Compare(string x, string y)
        {
            string kanaIgnoredX = IgnoreKanaWidth(x);
            string kanaIgnoredY = IgnoreKanaWidth(y);

            if (comparer != null)
            {
                return comparer.Compare(kanaIgnoredX, kanaIgnoredY);
            }
            return string.Compare(kanaIgnoredX, kanaIgnoredY);
        }

        /// <summary>
        /// Gets the hash code for the specified string after ignore Kana Width.
        /// </summary>
        /// <param name="obj">A string.</param>
        /// <returns>
        /// A 32-bit signed hash code calculated from the value of the <paramref name="obj"/> parameter.
        /// </returns>
        public override int GetHashCode(string obj)
        {
            string kanaIgnoredObj = IgnoreKanaWidth(obj);

            if (comparer != null)
            {
                return comparer.GetHashCode(kanaIgnoredObj);
            }
            return kanaIgnoredObj.GetHashCode();
        }

        /// <summary>
        /// Indicates whether two strings are equal if ignore Kana Width.
        /// </summary>
        /// <param name="x">A string to be compared.</param>
        /// <param name="y">A string to be compared.</param>
        /// <returns>
        /// true if <paramref name="x"/> and <paramref name="y"/> refer to the same object, or <paramref name="x"/> and <paramref name="y"/> are equal; otherwise, false.
        /// </returns>
        public override bool Equals(string x, string y)
        {
            string kanaIgnoredX = IgnoreKanaWidth(x);
            string kanaIgnoredY = IgnoreKanaWidth(y);

            // TODO: see other codes.
            if (comparer != null)
            {
                return comparer.Equals(kanaIgnoredX, kanaIgnoredY);
            }
            return string.Equals(kanaIgnoredX, kanaIgnoredY);
        }

        private static string IgnoreKanaWidth(string src)
        {
            string ret = KanaConverter.HalfwidthKatakanaToKatakana(src);
            ret = ret.Normalize(NormalizationForm.FormD);

            return ret;
        }
    }
}
