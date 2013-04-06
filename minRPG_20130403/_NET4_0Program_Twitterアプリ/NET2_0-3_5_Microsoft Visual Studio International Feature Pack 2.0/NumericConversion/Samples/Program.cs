using System;
using Microsoft.International.Formatters;
using Microsoft.International;

using System.Globalization;
using System.Diagnostics;

// sample code to convert the number to a string with local numeric representation in East Asian and Arabic languages.
namespace InternationalNumericFormatterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.WriteLine("The number of 12345 with Normal format and Chinese-Simplified is: " + InternationalNumericFormatter.FormatWithCulture("Ln", 12345, null, new CultureInfo("zh-CHS")));
            Debug.WriteLine("The number of 12345 with Currency format and Chinese-Traditional is: " + InternationalNumericFormatter.FormatWithCulture("Lc", 12345, null, new CultureInfo("zh-CHT")));
            Debug.WriteLine("The number of 12345 with standard format and Japanese is: " + InternationalNumericFormatter.FormatWithCulture("L", 12345, null, new CultureInfo("ja")));
            Debug.WriteLine("The number of 12345 with standard format and Korean is: " + InternationalNumericFormatter.FormatWithCulture("L", 12345, null, new CultureInfo("ko")));
            Debug.WriteLine("The number of 12345 with standard format and Arabic is: " + InternationalNumericFormatter.FormatWithCulture("L", 12345, null, new CultureInfo("ar")));
        }
    }
}
/*
This code produces the following debug output.
            
The number of 12345 with Normal format and Chinese-Simplified is: 一万二千三百四十五
The number of 12345 with Currency format and Chinese-Traditional is: 壹萬貳仟參佰肆拾伍
The number of 12345 with Standard format and Japanese is: 壱萬弐阡参百四拾伍
The number of 12345 with standard format and Korean is: 일만 이천삼백사십오
The number of 12345 with standard format and Arabic is:اثنا عشر آلاف و ثلاثة مائة خمسة و أربعون
             
*/