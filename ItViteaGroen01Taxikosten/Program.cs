using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItViteaGroen01Taxikosten
{
    public class Program
    {
        static void Main(string[] args)
        {
            //public variables.
            int timeHour, timeMinute, kmAmount;
            decimal priceTotal;
            bool repeatPrgm;
            string strAnswer;

       
            do
            {
                kmAmount = AskInput("Hoeveel km is er gereden?", false);
                timeHour = AskInput("Welk uur was de starttijd? (Gebruik digitale tijd)", true);
                timeMinute = AskInput("Geef de minuten van de starttijd. (Gebruik digitale tijd.)", true);

                TimeSpan beginTijd = new TimeSpan(timeHour, timeMinute, 0);

                timeHour = AskInput("Welk uur was de eindtijd? (Gebruik digitale tijd)", true);
                timeMinute = AskInput("Geef de minuten van de eindtijd. (Gebruik digitale tijd)", true);
                TimeSpan eindTijd = new TimeSpan(timeHour, timeMinute, 0);

                try
                {
                    TimeSpan interval = eindTijd - beginTijd;
                    priceTotal = CalculatePrice(kmAmount, interval.Minutes);
                    Console.WriteLine("Your total price is: {0:C}", priceTotal);
                }
                catch { }

               


                ///Code om het programma eventueel te herhalen.
                Console.WriteLine("Wil je het programma herhalen? J/N");
                strAnswer = Console.ReadLine().ToString().ToUpper();
                if (strAnswer == "J")
                    repeatPrgm = true;
                else
                    repeatPrgm = false;
            }
            while (repeatPrgm);
                

        }
        /// <summary>
        /// Blijft herhalen tot dat de juiste input is gegeven.
        /// </summary>
        /// <param name="strQuestion">De string vraag die wordt gesteld.</param>
        /// <param name="hasMax">Bool voor als het antwoord een maximale lengthe mag hebben of niet.</param>
        /// <returns></returns>
        public static int AskInput(string strQuestion, bool hasMax)
        {
            bool doRepeat;
            int intAnswer = 0;
            do
            {
                Console.Write(strQuestion + " : ");
                try
                {
                    intAnswer = Convert.ToInt32(Console.ReadLine().ToString());
                    doRepeat = false;

                    if (hasMax)
                    {
                        if (intAnswer.ToString().Length > 2)
                        {
                            Console.WriteLine("Gebruik maximaal 2 nummers.");
                            doRepeat = true;
                        }
                    }       
                }
                catch
                {
                    Console.WriteLine("Gebruik alleen nummers.");
                    doRepeat = true;
                }
            }
            while (doRepeat);

            return intAnswer;
        }

        public static decimal CalculatePrice(int km, int minutes)
        {
            decimal priceTotal = 0;
            priceTotal += km;
            priceTotal += minutes * (decimal)0.25;
            return priceTotal;
        }
    }
}
