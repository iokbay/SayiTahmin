using System;
using System.Collections.Generic;
using System.Text;

namespace SayiTahmin
{
    public class Guess : IComparable
    {
        static int counter=0;
        public string number { get; set; }
        public int arti { get; set; }
        public int eksi { get; set; }
        public int hepsi { get => arti + eksi; }
        public bool IsGoal { get => arti == 4; }
        public bool IsExemined { get; set; }
        public int sequence;

        public int Retry;
        public int blank_digit;
        public Guess(string number, int arti, int eksi)
        {
            this.number = number;
            this.arti = arti;
            this.eksi = eksi;
            this.sequence = counter++;
        }
        public string valueString
        {
            get
            {
                if (this.IsExemined == false)
                    return string.Empty;
                string artiString = "", eksiString = "";
                if (this.arti > 0)
                {
                    artiString = $"+{this.arti}";
                }
                if (this.eksi > 0)
                {
                    eksiString = $"-{this.eksi}";
                }
                if (this.hepsi == 0)
                    artiString = "0";
                return artiString + eksiString;
            }
        }

        public Guess()
        {
            this.sequence = counter++;
            this.number = "";
        }

        static string temp = "0123456789";
        static StringBuilder shape;

        static public TahminList Shuffling(string number)
        {
            TahminList numbers =  new TahminList();
            shape = new StringBuilder(temp);

            int tguess = 0;
            counter = 0;
            while (shape.Length > 0)
            {
                Guess guess = Number(false);

                if (shape.Length == 0)
                    guess.eksi = 4 - tguess;
                else
                {
                    guess.Exemine(number);
                    tguess += guess.hepsi;
                }

                numbers.Add(guess);
            }
            return numbers;
        }

        static public Guess Number(bool yeni)
        {
            if (yeni)
                shape = new StringBuilder(temp);
            Random rnd = new Random();
            StringBuilder newNumber = new StringBuilder();
            while (shape.Length > 0)
            {
                int i = rnd.Next(shape.Length);
                char c = shape[i];
                if (newNumber.Length == 0 && c == '0')
                    continue;
                newNumber.Append(c);
                shape.Remove(i,1);
                if (newNumber.Length == 4)
                    break;
            }
            return new Guess(newNumber.ToString(), 0, 0);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) 
                throw new ArgumentException("Object is null");

            Guess guess2 = obj as Guess;
            int fark;
            if (guess2 == null)
                throw new ArgumentException("Object is not the GuessNumber");
            fark = guess2.arti - this.arti;
            if (fark != 0)
                return fark;
            fark = guess2.hepsi - this.hepsi;
            if (fark != 0)
                return fark;
            return guess2.sequence - this.sequence;
        }
        public Guess Extract(string sayı)
        {
            int arti = this.arti;
            int eksi = this.eksi;
            StringBuilder _number = new StringBuilder(this.number);
            for (int i = 0; i < _number.Length; i++)
            {
                char c = _number[i];
                int j = sayı.IndexOf(c);
                if (j == -1)
                    continue;
                _number[i] = ' ';
                if (i == j && this.number.Length == 4)
                    --arti;
                else
                    --eksi;
            }
            if (arti < 0 || eksi < 0)
                return null;
            return new Guess(_number.ToString(), arti, eksi);
        }
        public bool Exemine(string sayi)
        {
            if (this.number[0] == '0')
                return false;

            this.arti = 0;
            this.eksi = 0;
            for (int i = 0; i < this.number.Length; i++)
            {
                char c = this.number[i];
                if (this.number.LastIndexOf(c) != i)
                    return false;
                int j = sayi.IndexOf(c);
                if (i == j)
                    ++this.arti;
                else if (j > -1)
                    ++this.eksi;
            }
            this.IsExemined = true;
            return true;
        }

        public override string ToString()
        {
            return $"{this.number} {this.valueString}";
        }
    }
}
