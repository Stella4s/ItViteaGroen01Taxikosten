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

                priceTotal = CalcPrice(kmAmount, beginTijd, eindTijd);
                Console.WriteLine("Your total price is: {0:C}", priceTotal);


                ///Code om het programma eventueel te herhalen.
                Console.WriteLine("Wil je het programma herhalen? Y/N");
                strAnswer = Console.ReadLine().ToString().ToUpper();
                if (strAnswer == "Y")
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

        /// <summary>
        /// Berekend de tijd gebaseerd op de hoeveelheid km en wanneer de rid begon en eindigte.
        /// </summary>
        /// <param name="km">Aantal km gereden.</param>
        /// <param name="tBegin">Timespan begin rid.</param>
        /// <param name="tEnd">TimeSpan eind rid.</param>
        /// <returns></returns>
        public static decimal CalcPrice(int km, TimeSpan tBegin, TimeSpan tEnd)
        {
            
            bool weekendToeslag = false;
            decimal priceTotal = 0;
            int intAnswer, 
                minExtra = 0,
                minStand = (int)(tEnd - tBegin).TotalMinutes;
            //Alle tijdspans nodig om te vergelijken met begin en eindtijd.
            TimeSpan tDTimeStart = new TimeSpan(8, 0, 0);
            TimeSpan tDTimeEnd = new TimeSpan(18, 0, 0);
            TimeSpan tFriWeekend = new TimeSpan(22, 0, 0);
            TimeSpan tMonWeekend = new TimeSpan(7, 0, 0);

            //Als rid begint voor 8 uur of eindigt na 18 bereken hoeveel van die minuten buiten normale tijden waren.
            if (tBegin.Hours < 8)
                minExtra += (int)(tDTimeStart - tBegin).TotalMinutes;
            if ((tDTimeEnd - tEnd).TotalMinutes < 0)
                minExtra += (int)(tEnd - tDTimeEnd).TotalMinutes;
            //Trek de aantal ExtraKosten minuten af van de totale minuten tussen de begin en eindtijd. 
            //Zodat je de minuten met normale tarieven en extra tarieven apart overhoud.
            minStand -= minExtra;

            //Vraag welke dag het is om te bepalen ofdat er weekendbijslag bij opgeteld moet worden.
            //Voor vrijdag alleen weekendbijslag wanneer de rid na 22:00 start, voor Maandag alleen als deze voor 7:00 uur start.
            intAnswer = AskInput("Is het [1]Vrijdag, [2]Zaterdag/Zondag, [3]Maandag of [4]een andere dag.", false);
            switch (intAnswer)
            {
                case 1:
                    if ((tBegin - tFriWeekend).TotalMinutes > 0)
                        weekendToeslag = true;
                    break;
                case 2:
                    weekendToeslag = true;
                    break;
                case 3:
                    if ((tBegin - tMonWeekend).TotalMinutes < 0)
                        weekendToeslag = true;
                    break;
                default:
                    weekendToeslag = false;
                    break;
            }

            //Bereken het prijs totaal met beide tarieven en verhoog de eindprijs met 15% als er weekendtoeslag geld.
            priceTotal += km;
            priceTotal += minStand * (decimal)0.25;
            priceTotal += minExtra * (decimal)0.45;

            if (weekendToeslag)
                priceTotal *= (decimal)1.15;
                
            return priceTotal;
        }
    }
}
