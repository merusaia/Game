using System;
using System.Collections.Generic;
using Microsoft.International;
using Microsoft.International.Collections;
using Microsoft.International.Converters;

namespace YomiganaFrameworkSample
{
    class Program
    {
        private static List<string> sources = new List<string> { "今日", "は", "良", "い", "天気", "。" };
        private static List<string> annotations = new List<string> { "きょう", "", "よ", "", "てんき", "" };

        static void Main(string[] args)
        {
            StringAnnotationInterLinearAnnotationFormatProviderSample();

            StringAnnotationJisX4052FormatProviderSample();

            StringAnnotationInterOpenCloseBracketsFormatProviderSample();

            StringAnnotationCompareSample();

            InterlinearAnnotationCompareSample();

            CombinedComparisonSample();

            StringAnnotationCombinedComparisonSample();

            AnnotatedStringSortingSample();

            IgnoreKanaWidthComparisionSample();

            MergeTwoSegmentsSample();

            SplitSegmentSample();

            AutoAssignYomiganaSample();

            Console.Read();
        }

        /// <summary>
        /// How do I format annotated string in StringAnnotations with Unicode's interlinear annotation characters?
        ///</summary>
        public static void StringAnnotationInterLinearAnnotationFormatProviderSample()
        {
            StringAnnotations stringAnnotations = new StringAnnotations(Program.sources, Program.annotations);
            StringAnnotationInterLinearAnnotationFormatProvider formatProvider = new StringAnnotationInterLinearAnnotationFormatProvider();
            string formatResult = formatProvider.Format(formatProvider.FormatString, stringAnnotations, formatProvider);

            // Expected: "\ufff9今日\ufffaきょう\ufffbは\ufff9良\ufffaよ\ufffbい\ufff9天気\ufffaてんき\ufffb。"
            Console.WriteLine("StringAnnotationInterLinearAnnotationFormatProviderSample: {0}", formatResult);
        }

        /// <summary>
        /// How do I format annotated string in StringAnnotations  with JIS X 4052 format?
        ///</summary>
        public static void StringAnnotationJisX4052FormatProviderSample()
        {
            StringAnnotations stringAnnotations = new StringAnnotations(Program.sources, Program.annotations);
            StringAnnotationJisX4052FormatProvider formatProvider = new StringAnnotationJisX4052FormatProvider();
            string formatResult = formatProvider.Format(formatProvider.FormatString, stringAnnotations, formatProvider);

            // Expected: "＿＾今日（きょう）＾＿は＿＾良（よ）＾＿い＿＾天気（てんき）＾＿。"
            Console.WriteLine("StringAnnotationJisX4052FormatProviderSample: {0}", formatResult);
        }

        /// <summary>
        /// How do I format annotated string in StringAnnotations with parenthesis?
        ///</summary>
        public static void StringAnnotationInterOpenCloseBracketsFormatProviderSample()
        {
            StringAnnotations stringAnnotations = new StringAnnotations(Program.sources, Program.annotations);
            StringAnnotationInterOpenCloseBracketsFormatProvider formatProvider = new StringAnnotationInterOpenCloseBracketsFormatProvider();
            formatProvider.OpenBracket = "\u0028";
            formatProvider.CloseBracket = "\u0029";
            string formatResult = formatProvider.Format(formatProvider.FormatString, stringAnnotations, formatProvider);

            // Expected: "今日(きょう)は良(よ)い天気(てんき)。"
            Console.WriteLine("StringAnnotationInterOpenCloseBracketsFormatProviderSample: {0}", formatResult);
        }

        /// <summary>
        /// How do I compare annotated string using StringAnnotation? 
        ///</summary>
        public static void StringAnnotationCompareSample()
        {
            StringAnnotations stringAnnotationsX = new StringAnnotations(Program.sources, Program.annotations);
            StringAnnotations stringAnnotationsY = new StringAnnotations(Program.sources, Program.annotations);

            int compareResult = stringAnnotationsX.CompareTo(stringAnnotationsY);

            // Expected: 0.
            Console.WriteLine("StringAnnotationCompareSample: {0}", compareResult);
        }

        /// <summary>
        /// How do I compare annotated string using Unicode's interlinear annotation? 
        ///</summary>
        public static void InterlinearAnnotationCompareSample()
        {
            string annotatedStringX = "\ufff9今日\ufffaきょう\ufffbは\ufff9良\ufffaよ\ufffbい\ufff9天気\ufffaてんき\ufffb。";
            string annotatedStringY = "\ufff9今日\ufffaきょう\ufffbは\ufff9良\ufffaよ\ufffbい\ufff9天気\ufffaてんき\ufffb。";

            InterlinearAnnotationComparer comparer = new InterlinearAnnotationComparer(StringComparer.Ordinal);
            int compareResult = comparer.Compare(annotatedStringX, annotatedStringY);

            // Expected: 0.
            Console.WriteLine("interlinearAnnotationCompareSample: {0}", compareResult);
        }

        /// <summary>
        /// How do I combine comparisons ignoring kana kinds and small kana letters? 
        ///</summary>
        public static void CombinedComparisonSample()
        {
            StringComparer comparer = new InterlinearAnnotationComparer(StringComparer.Ordinal);
            IgnoreSmallKanaLetterComparison ignoreSmallKanaLetterComparison = new IgnoreSmallKanaLetterComparison(comparer);
            IgnoreKanaKindComparison combinedComparison = new IgnoreKanaKindComparison(ignoreSmallKanaLetterComparison);

            string annotatedStringX = "\ufff9今日\ufffaきょう\ufffbは\ufff9良\ufffaよ\ufffbい\ufff9天気\ufffaテンキ\ufffb。ょ";
            string annotatedStringY = "\ufff9今日\ufffaキョウ\ufffbハ\ufff9良\ufffaよ\ufffbい\ufff9天気\ufffaてんき\ufffb。よ";
            int compareResult = combinedComparison.Compare(annotatedStringX, annotatedStringY);

            // Expected: 0.
            Console.WriteLine("CombinedComparisonSample: {0}", compareResult);
        }
         
        /// <summary>
        /// How do I combine comparisons ignoring kana kinds and small kana letters in annotated string? 
        ///</summary>
        public static void StringAnnotationCombinedComparisonSample()
        {
            StringAnnotations stringAnnotationsX = new StringAnnotations(Program.sources, Program.annotations);
            StringAnnotations stringAnnotationsY = new StringAnnotations(Program.sources, Program.annotations);

            StringAnnotationComparer comparer = new StringAnnotationComparer();
            IgnoreSmallKanaLetterComparison ignoreSmallKanaLetterComparison = new IgnoreSmallKanaLetterComparison(new InterlinearAnnotationComparer(StringComparer.Ordinal));
            IgnoreKanaKindComparison combinedComparison = new IgnoreKanaKindComparison(ignoreSmallKanaLetterComparison);
            Comparison<string> comparison = combinedComparison.GetComparison();

            int compareResult = comparer.Compare(stringAnnotationsX, stringAnnotationsY, comparison);

            // Expected: 0.
            Console.WriteLine("StringAnnotationCombinedComparisonSample: {0}", compareResult);
        }

        /// <summary>
        /// How do I sort a list of interlinear annotation strings?
        ///</summary>
        public static void AnnotatedStringSortingSample()
        {
            StringComparer comparer = new InterlinearAnnotationComparer(StringComparer.Ordinal);
            List<string> stringList = new List<string> { "\ufff9雨\ufffaあめ\ufffb", "\ufff9雨\ufffaあま\ufffb" };

            stringList.Sort(comparer);

            // Expected:
            //    "\ufff9雨\ufffaあま\ufffb"
            //    "\ufff9雨\ufffaあめ\ufffb"
            Console.Write("AnnotatedStringSortingSample: \n");

            foreach (string item in stringList)
            {
                Console.WriteLine("[{0}]", item);
            }
        }

        /// <summary>
    　　///How do I extend the JapaneseComparison class to compare string ignoring the Japanese Kana width?
        ///</summary>
        public static void IgnoreKanaWidthComparisionSample()
        {
            string stringX = "ニホン";
            string stringY = "ﾆﾎﾝ";

            StringComparer comparer = new InterlinearAnnotationComparer(StringComparer.Ordinal);
            IgnoreKanaWidthComparision ignoreKanaWidthComparision = new IgnoreKanaWidthComparision(comparer);
            int compareResult = ignoreKanaWidthComparision.Compare(stringX, stringY);

            // Expected: 0.
            Console.WriteLine("IgnoreKanaWidthComparisionSample: {0}", compareResult);
        }

        /// <summary>
    　　///How do I merge two segments in StringAnnotation? 
        ///</summary>
        public static void MergeTwoSegmentsSample()
        {
            CustomTextSegments segments = new CustomTextSegments(Program.sources);
            StringAnnotations stringAnnotations = new StringAnnotations(segments, Program.annotations);

            // Merges "良" and "い".
            segments.MergeNextAt(2);
            stringAnnotations.MergeNextAt(2);

            // Expected:
            //    今日(きょう)
            //    は
            //    良い(よい)
            //    天気(てんき)
            //    。
            Console.Write("MergeTwoSegmentsSample: \n");
            if (segments.Count == stringAnnotations.Count)
            {
                int index = 0;
                int count = segments.Count;
                while (index < count)
                {
                    if (stringAnnotations[index] == String.Empty)
                    {
                        Console.WriteLine(segments[index]);
                    }
                    else
                    {
                        Console.WriteLine("{0}({1})", segments[index], stringAnnotations[index]);
                    }
                    index++;
                }
            }
        }

        /// <summary>
    　　///　How do I split a segment into two in StringAnnotation? 
        ///</summary>
        public static void SplitSegmentSample()
        {
            CustomTextSegments segments = new CustomTextSegments(Program.sources);
            StringAnnotations stringAnnotations = new StringAnnotations(segments, Program.annotations);

            // Splits "今日".
            stringAnnotations.SplitAt(0);                 
            segments.SplitAt(0);

            // Expected: 
            //    今(きょう)
            //    日
            //    は
            //    良(よ)
            //    い
            //    天気(てんき)
            //    。
            Console.Write("SplitSegmentSample: \n");

            if (segments.Count == stringAnnotations.Count)
            {
                int index = 0;
                int count = segments.Count;
                while (index < count)
                {
                    if (stringAnnotations[index] == String.Empty)
                    {
                        Console.WriteLine(segments[index]);
                    }
                    else
                    {
                        Console.WriteLine("{0}({1})", segments[index], stringAnnotations[index]);
                    }
                    index++;
                }
            }
        }

        /// <summary>
        ///　How do I automatically assign Japanese Yomigana to StringAnnotation? 
        ///</summary>
        public static void AutoAssignYomiganaSample()
        {
            TextSegments segments = new HanIdeographSequenceBoundaries("今日は良い天気。");
            StringAnnotations stringAnnotations = new StringAnnotations(segments);
            int index = 0;

            // Expected: 
            //    今日(きょう)
            //    は
            //    良(よ)
            //    い
            //    天気(てんき)
            //    。
            Console.WriteLine("AutoAssignYomiganaSample: ");
            foreach (string source in segments)
            {
                stringAnnotations[index++] = YomiHelper.GetYomiByIME(source, NativeMethods.FELANG_CMODE_HIRAGANAOUT);
            }

            if (segments.Count == stringAnnotations.Count)
            {
                index = 0;
                int count = segments.Count;
                while (index < count)
                {
                    if (stringAnnotations[index] == String.Empty)
                    {
                        Console.WriteLine(segments[index]);
                    }
                    else
                    {
                        Console.WriteLine("{0}({1})", segments[index], stringAnnotations[index]);
                    }
                    index++;
                }
            }
        }

        /// <summary>
        /// How do I parse the interlinear annotation?
        /// </summary>
        /// <returns></returns>
        private static void ParseInterlinearAnnotationSample()
        {
            string annotatedString = "\ufff9今日\ufffaきょう\ufffbは\ufff9良\ufffaよ\ufffbい\ufff9天気\ufffaてんき\ufffb。";
            StringAnnotations stringAnnotations = StringAnnotations.Parse(annotatedString);
        }
    }
}
