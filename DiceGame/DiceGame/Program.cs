using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DiceGame
{
    class Program
    {
        static void Main(string[] args)
        {

            int defaultSimulation = 100;
            int defaultDices = 5;
            Console.WriteLine("* For executing with default values like 100 simulation and 5 dices (Y/N)");

            if (Convert.ToString(Console.ReadLine()).Trim().ToUpper() == "N")
            {

                Console.WriteLine("Please enter simulation");
                while (!int.TryParse(Console.ReadLine(), out defaultSimulation))
                {
                    Console.WriteLine("Please enter valid simulation");
                }

                Console.WriteLine("Please enter number of Dices");
                while (!int.TryParse(Console.ReadLine(), out defaultDices))
                {
                    Console.WriteLine("Please enter valid number of Dices");
                }

            }


            DiceGameSimulator ds = new DiceGameSimulator(defaultSimulation, defaultDices);
            ds.DiceRolling();
            ds.GetResult();

        }
       
    }


    public class DiceGameSimulator
    {
        #region Private variable
        private int totalSimulationCount = 0;
        private int totalDicesCount = 0;
        private int takenOffNumber = 0;
        private Dictionary<int, int> diceRes = null;
        Stopwatch timer = null;
        #endregion


        public DiceGameSimulator(int simulations, int totalDices, int elapsNumber = 3)
        {
            totalSimulationCount = simulations;
            totalDicesCount = totalDices;
            takenOffNumber = elapsNumber;
        }

        internal void DiceRolling()
        {
            timer = new Stopwatch();
            timer.Start();

            var diceTrown = new Random();
            int rollCount = totalSimulationCount;
            int totalDices = totalDicesCount;
            int remainingDices = totalDices;

            diceRes = new Dictionary<int, int>();
            List<int> resultset = new List<int>(); ;
            //int tmpRes = 0;
            while (rollCount > 0)
            {
                remainingDices = totalDices;
                List<int> noDiceResult = new List<int>();
                while (remainingDices > 0)
                {
                    noDiceResult.Add(diceTrown.Next(1, 7));
                    remainingDices -= 1;
                }

                totalDices -= (noDiceResult.Where(x => x == takenOffNumber).Count() > 0) ? noDiceResult.Where(x => x == takenOffNumber).Count() : 1;
                resultset.Add((noDiceResult.Where(x => x == takenOffNumber).Count() > 0) ? 0 : noDiceResult.OrderBy(x => x).FirstOrDefault());

                totalDices = totalDices <= 0 ? totalDicesCount : totalDices;
                if (totalDices == totalDicesCount)
                {
                    if (!diceRes.ContainsKey(resultset.Sum(x => x)))
                        diceRes.Add(resultset.Sum(x => x), 1);
                    else
                        diceRes[resultset.Sum(x => x)] = (diceRes.Where(x => x.Key == resultset.Sum(y => y))?.FirstOrDefault().Value ?? 0) + 1;

                    rollCount -= 1;
                    resultset = new List<int>();
                }
                //Console.WriteLine("-----------------------------" + rollCount);
            }


        }

        internal void GetResult()
        {
            foreach (var item in diceRes.OrderBy(x => x.Key))
            {
                Console.WriteLine("Total " + item.Key + " occurs " + (Convert.ToDecimal(string.Format("{0,12:N2}", item.Value)) / totalSimulationCount) + "  occured " + item.Value + " times.");
            }
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("total simulation took " + timeTaken.ToString(@"m\:ss\.fff"));

           // Console.WriteLine(diceRes.Sum(x => x.Value));
            //
            Console.ReadLine();
        }
    }
}

