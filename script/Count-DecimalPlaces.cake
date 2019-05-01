#l "Common.cake"

using System;
using System.Linq;

void Script() {
    Decimal argument = 123.456m;
    Int32 count = BitConverter.GetBytes(decimal.GetBits(argument)[3])[2];
    Console.WriteLine(count);
    Console.WriteLine();
    
    Console.WriteLine(argument);
    Console.WriteLine(String.Join(", ", decimal.GetBits(argument)));
    Console.WriteLine(decimal.GetBits(argument)[3]);
    Console.WriteLine(String.Join(", ", BitConverter.GetBytes(decimal.GetBits(argument)[3])));
    
    Console.WriteLine(argument);
    Console.WriteLine(CountDecimalDigits(argument));
    
    // NOTE: https://stackoverflow.com/questions/13477689/find-number-of-decimal-places-in-decimal-value-regardless-of-culture
}

Int32 CountDecimalDigits(Decimal n) {
    return n.ToString(System.Globalization.CultureInfo.InvariantCulture)
        //.TrimEnd('0') uncomment if you don't want to count trailing zeroes
        .SkipWhile(c => c != '.')
        .Skip(1)
        .Count();
}
