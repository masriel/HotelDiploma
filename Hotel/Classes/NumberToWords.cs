using System;

public class NumberToWords
{
    private string[] Units = {
        "", "один", "два", "три", "четыре", "пять",
        "шесть", "семь", "восемь", "девять", "десять",
        "одиннадцать", "двенадцать", "тринадцать", "четырнадцать",
        "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать",
        "девятнадцать"
    };

    private string[] Tens = {
        "", "десять", "двадцать", "тридцать", "сорок", "пятьдесят",
        "шестьдесят", "семьдесят", "восемьдесят", "девяносто"
    };

    private string[] Hundreds = {
        "", "сто", "двести", "триста", "четыреста", "пятьсот",
        "шестьсот", "семьсот", "восемьсот", "девятьсот"
    };

    private string[] Thousands = {
        "", "тысяча", "тысячи", "тысяч"
    };

    public string NumberToWordsRussian(double number)
    {
        if (number == 0)
            return "ноль";

        if (number < 0)
            return "минус " + NumberToWordsRussian(Math.Abs(number));

        string words = "";

        if ((number / 1000) > 0)
        {
            words += ThousandsToWords(number / 1000) + " ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += Hundreds[(int)number / 100] + " ";
            number %= 100;
        }

        if (number > 0)
        {
            if (number < 20)
                words += Units[(int)number] + " ";
            else
            {
                words += Tens[(int)number / 10] + " ";
                if ((number % 10) > 0)
                    words += Units[(int)number % 10] + " ";
            }
        }

        return words.Trim();
    }

    private string ThousandsToWords(double number)
    {
        if (number == 1)
            return "одна тысяча";
        if (number >= 2 && number <= 4)
            return Units[(int)number] + " тысячи";
        return Units[(int)number] + " тысяч";
    }
}
