using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Coins
{
    class Program
    {
        static void Main(string[] args)
        {
            string inFileName = "in.txt";
            string outFileName = "out.txt";
            string[] lines = File.ReadAllLines(inFileName, Encoding.Default);
            var outFile = new StreamWriter(outFileName, false, Encoding.Default);

            char[] separator = new char[] { ';' };
            foreach (string line in lines)
            {
                string[] data = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                if (data.Count() < 2)
                {
                    Console.WriteLine("Слишком мало параметров в строке: " + line);
                    continue;
                }

                var coins = data.ToList();
                coins.RemoveAt(0);
                
                string answer = "";
                try
                {
                    CheckCoins checker = new CheckCoins(data[0], coins);
                    answer = checker.GetAnswer();
                }
                catch (CheckCoinsExeption e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                Console.WriteLine("Для '" + line + "' ответ: " + answer);
                outFile.WriteLine(answer);
            }

            outFile.Close();
        }

    }

    public class CheckCoins
    {
        private int sum_;
        private List<int> coins_ = new List<int>();

        public CheckCoins(string sum, List<string> coins)
        {
            if (!int.TryParse(sum, out sum_))
                throw new CheckCoinsExSum(sum);

            foreach (string coin in coins)
            {
                int tmp;
                if (!int.TryParse(coin, out tmp))
                    throw new CheckCoinsExCoin(coin);
                coins_.Add(tmp);
            }
        }

        public string GetAnswer()
        {
            //List<int> arr = new List<int>();

            for (int i = 1; i < Math.Pow(2, coins_.Count); ++i)
            {
                int tmp = 0;
                for (int j = 0; j < coins_.Count; ++j)
                {
                    if ((i & (int)Math.Pow(2, j)) != 0)
                        tmp += coins_[j];
                }
                //arr.Add(tmp);
                if (tmp == sum_)
                {
                    string ret = "";
                    for (int j = 0; j < coins_.Count; ++j)
                    {
                        if ((i & (int)Math.Pow(2, j)) != 0)
                        {
                            if (ret.Length > 0)
                                ret += ";";
                            ret += coins_[j];
                        }
                    }
                    return ret;
                }
            }

            return "NO";

            //int pos = arr.FindIndex(r => r == sum_);
            //if (pos < 0)
            //    return "NO";
            //else
            //{
            //    string ret = "";
            //    for (int j = 0; j < coins_.Count; ++j)
            //    {
            //        if (((pos + 1) & (int)Math.Pow(2, j)) != 0)
            //        {
            //            if (ret.Length > 0)
            //                ret += ";";
            //            ret += coins_[j];
            //        }
            //    }
            //    return ret;
            //}
        }
    }

    public class CheckCoinsExeption : System.Exception
    {
        public CheckCoinsExeption(string err) : base(err) { }        
    }

    public class CheckCoinsExSum : CheckCoinsExeption
    {
        public CheckCoinsExSum(string err) : base("В качестве суммы задано не число: " + err) { }
    }

    public class CheckCoinsExCoin : CheckCoinsExeption
    {
        public CheckCoinsExCoin(string err) : base("В качестве номинала монеты задано не число: " + err) { }
    }
}
