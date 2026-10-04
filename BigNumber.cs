using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp50
{
    public class BigNumber
    {
        private int[] number;
        private const int Base = 1000;
        public int ArrayLength { get { return number.Length; } }

        public BigNumber(string numberString)
        {
            numberString = numberString.TrimStart('0');
            if (numberString.Length == 0)
                numberString = "0";

            int blockCount = (numberString.Length + 2) / 3;
            number = new int[blockCount];

            int strPos = numberString.Length;
            for (int i = blockCount - 1; i >= 0; i--)
            {
                int start = Math.Max(0, strPos - 3);
                string block = numberString.Substring(start, strPos - start);
                number[i] = int.Parse(block);
                strPos = start;
            }
        }

        public override string ToString()
        {
            return ArrayToString(number);
        }

        private BigNumber Add(BigNumber bnum)
        {
            int maxLength = Math.Max(this.number.Length, bnum.number.Length);
            int[] result = new int[maxLength + 1];
            int carry = 0;

            for (int i = 0; i < maxLength; i++)
            {
                int digitA = (i < this.number.Length) ? this.number[this.number.Length - 1 - i] : 0;
                int digitB = (i < bnum.number.Length) ? bnum.number[bnum.number.Length - 1 - i] : 0;

                int sum = digitA + digitB + carry;
                result[maxLength - i] = sum % Base;
                carry = sum / Base;
            }
            result[0] = carry;

            string resultString = ArrayToString(TrimLeadingZeros(result));
            return new BigNumber(resultString);
        }

        // Метод вычитания (предполагается, что this >= bnum, отрицательные числа не поддерживаются)
        private BigNumber Subtract(BigNumber bnum)
        {
            int maxLength = this.number.Length;
            int[] result = new int[maxLength];
            int borrow = 0; // заём из предыдущего шага

            for (int i = 0; i < maxLength; i++)
            {
                int digitA = this.number[this.number.Length - 1 - i];
                int digitB = (i < bnum.number.Length) ? bnum.number[bnum.number.Length - 1 - i] : 0;

                int diff = digitA - digitB - borrow;

                if (diff < 0)
                {
                    diff += Base;  // занимаем 1000 у соседнего (старшего) разряда
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }

                result[maxLength - 1 - i] = diff;
            }

            string resultString = ArrayToString(TrimLeadingZeros(result));
            return new BigNumber(resultString);
        }
        // Умножение большого числа на обычное число (множитель), например BigNumber * 1.2
        private BigNumber Multiply(double multiplier)
        {
            int length = this.number.Length;
            int[] result = new int[length + 1]; // +1 про запас на возможный лишний разряд
            double carry = 0;

            // идём справа налево (от младшего блока к старшему)
            for (int i = 0; i < length; i++)
            {
                int digit = this.number[length - 1 - i];
                double product = digit * multiplier + carry;

                int blockValue = (int)(product % Base);
                result[length - i] = blockValue;
                carry = Math.Floor(product / Base);
            }
            result[0] = (int)carry;

            string resultString = ArrayToString(TrimLeadingZeros(result));
            return new BigNumber(resultString);
        }
        // Деление большого числа на обычное число (делитель)
        private BigNumber Divide(double divisor)
        {
            int length = this.number.Length;
            int[] result = new int[length];
            double remainder = 0; // остаток, переносимый на следующий (более младший) блок

            // идём слева направо (от старшего блока к младшему)
            for (int i = 0; i < length; i++)
            {
                double current = remainder * Base + this.number[i];
                int quotient = (int)(current / divisor);
                remainder = current - quotient * divisor;

                result[i] = quotient;
            }

            string resultString = ArrayToString(TrimLeadingZeros(result));
            return new BigNumber(resultString);
        }

        // сравнивает текущее число с другим: >0 если this больше, <0 если меньше, 0 если равны
        private int CompareTo(BigNumber bnum)
        {
            if (this.number.Length != bnum.number.Length)
                return this.number.Length - bnum.number.Length;

            for (int i = 0; i < this.number.Length; i++)
            {
                if (this.number[i] != bnum.number[i])
                    return this.number[i] - bnum.number[i];
            }
            return 0;
        }
        // создаёт точную копию текущего числа
        public BigNumber Clone()
        {
            return new BigNumber(this.ToString());
        }

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            return a.Add(b);
        }

        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            return a.Subtract(b);
        }

        public static BigNumber operator *(BigNumber a, double b)
        {
            return a.Multiply(b);
        }

        public static BigNumber operator /(BigNumber a, double b)
        {
            return a.Divide(b);
        }

        public static bool operator >(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(BigNumber a, BigNumber b)
        {
            return a.CompareTo(b) < 0;
        }

        private int[] TrimLeadingZeros(int[] arr)
        {
            int start = 0;
            while (start < arr.Length - 1 && arr[start] == 0)
                start++;

            int[] trimmed = new int[arr.Length - start];
            Array.Copy(arr, start, trimmed, 0, trimmed.Length);
            return trimmed;
        }

        private string ArrayToString(int[] arr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                    sb.Append(arr[i].ToString());
                else
                    sb.Append(arr[i].ToString("D3"));
            }
            return sb.ToString();
        }
    }
}
