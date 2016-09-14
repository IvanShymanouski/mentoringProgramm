using System;

namespace CustomParsers
{
    public static class IntParsers
    {
        public static int ParseCustom(this string src)
        {
            long result = 0,
                 length;
            bool sign = true;

            if (src == null) throw new ArgumentNullException();
            if (src.Length == 0) throw new FormatException("String is empty");

            src = src.Trim();
            length = src.Length;

            if (length == 0) throw new FormatException("String contains only white spaces");

            var i = 0;
            switch (src[i])
            {
                case '-':
                    sign = false;
                    i++;
                    break;
                case '+':
                    i++;
                    break;
            }

            for (; i < length; i++)
            {
                if (char.IsDigit(src[i]))
                {
                    result = result * 10 + (src[i] - '0');
                    if (sign)
                    {
                        if (result > int.MaxValue) throw new OverflowException("Tried to parse value greater than int.MaxValue");
                    }
                    else
                    {
                        if (-result < int.MinValue) throw new OverflowException("Tried to parse value less than int.MinValue");
                    }
                }
                else
                {
                    throw new FormatException();
                }
            }

            return (int)((sign) ? result : -result);
        }

        public static bool TryParseCustom(this string src, out int result)
        {
            bool parsed = true;
            try
            {
                result = src.ParseCustom();
            }
            catch (ArgumentNullException)
            {
                result = 0;
                parsed = false;
            }
            catch (FormatException)
            {
                result = 0;
                parsed = false;
            }
            catch(OverflowException)
            {
                result = 0;
                parsed = false;
            }

            return parsed;
        }
    }
}
