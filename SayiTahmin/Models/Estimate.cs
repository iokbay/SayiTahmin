using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace SayiTahmin
{
    public class TahminList:Collection<Guess>
    {
       public int CountX
        {
            get
            {
                if (this.Count > 2)
                    return this.Count - 1;
                else
                    return this.Count;
            }
        }
       public Guess ElementAtX(int index)
        {
            var temp = this.OrderBy(x => x.sequence);
            if (index > 1)
                return temp.ElementAt(index + 1);
            else
                return temp.ElementAt(index);
        }
        public void Sort() 
        { 
            ((List<Guess>)Items).Sort(); 
        }
    }

    public static class Estimate
    {
        public static TahminList TahminDizisi { get; set; }
        static StringBuilder redlist;
        public static void Set(string number) 
        {
            TahminDizisi = Guess.Shuffling(number); 
        }
        public static Guess Çöz()
        {
            Guess newGuess;
            switch (TahminDizisi.Count)
            {
                case 0:
                    newGuess = Guess.Number(true);
                    break;
                case 1:
                    newGuess = Guess.Number(false);
                    break;
                case 2:
                    newGuess = Guess.Number(false);
                    newGuess.eksi = 4 - (TahminDizisi[0].hepsi + TahminDizisi[1].hepsi);
                    TahminDizisi.Add(newGuess);
                    goto atla2;;
                default:
                atla2:
                    TahminDizisi.Sort();
                    newGuess = new Guess();
                atla:
                    redlist = new StringBuilder();
                    string newNumber = "    ";
                    foreach (Guess guess in TahminDizisi)
                    {
                        string retval = Yerleştir(guess, newNumber);
                        if (retval == null)
                        {
                            ++newGuess.Retry;
                            if (newGuess.Retry > 200)
                            {
                                //DisplayAlert("Puanlama hatası", "Oyunu kaybettiniz", "OK");
                                return null;
                            }
                            goto atla;
                        }
                        newNumber = retval;
                    }
                    if (newNumber.IndexOf(' ') > -1)
                        { ++newGuess.blank_digit; goto atla; } //uğramazsa kaldır
                    /*if (Consist(newNumber))
                        { ++newGuess.err_dublicate; goto atla;} */
                    newGuess.number = newNumber;
                    break;
            }
            TahminDizisi.Add(newGuess);
            return newGuess;
        }

        static string Yerleştir(Guess guess, string _newNumber_)
        {
            Guess guessCopy = guess.Extract(_newNumber_);
            if (guessCopy == null)
                //Fazla yerleşmiş
                return null;
            int j, k;
            char c;
            StringBuilder newNumber = new StringBuilder(_newNumber_);
            StringBuilder guessNumber = new StringBuilder(guessCopy.number);
            newNumber.FindSpaces();

            for (int i = 0; i < guessCopy.arti; ++i)
            {
                do
                {
                    j = newNumber.NextSpace();
                    if (j == -1)
                        return null;
                    c = guessNumber[j];
                    if (c == ' ')
                        continue;
                } while (redlist.ToString().IndexOf(c) > -1);

                newNumber[j] = guessNumber[j];
                guessNumber[j] = ' ';
            }
            guessNumber.FindChars();
            for (int i = 0; i < guessCopy.eksi; ++i)
            {
                dön:
                do
                {
                    k = guessNumber.NextChar();
                    if (k == -1)
                        return null;
                    c = guessNumber[k];
                } while (redlist.ToString().IndexOf(c) > -1);

                newNumber.FindSpaces();
                for (;;)
                {
                    j = newNumber.NextSpace();
                    if (j == -1)
                        goto dön;
                    if (guess.number.Length == 4 && j == k)
                        continue;
                    if (c == '0' && j == 0)
                        continue;
                     break;
                }
                newNumber[j] = c;
                guessNumber[k] = ' ';
            }
            redlist.Append(guessNumber.RemoveWhitespace());
            return newNumber.ToString();
        }
    }
}
