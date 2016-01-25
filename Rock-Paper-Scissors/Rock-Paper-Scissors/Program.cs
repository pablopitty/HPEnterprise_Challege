using System;
using System.Collections.Generic;
using System.Linq;

namespace Rock_Paper_Scissors
{
    public class Elements
    {
        public string playerName { get; set; }
        public char playerStrategy { get; set; } //R - Rock, P - Paper & S = Scissor
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Elements> elementsList = new List<Elements>();

            Console.WriteLine("Hi, welcome to Rock-Paper-Scissor!\nPlease enter your name:");
            string playerName = Console.ReadLine();

            Console.WriteLine("Great, now enter your strategy:");
            char playerStrategy = ' ';

            ValidatePlayerStrategy(playerName, playerStrategy, elementsList);

            char newPlayer = ' ';

            while (newPlayer != 'n')
            {
                Console.WriteLine("Would you like to add a new player?\nEnter the letter y for yes and the letter n for no");

                try
                {
                    newPlayer = Convert.ToChar(Console.ReadLine().ToLower());
                }

                catch (Exception)
                {
                    Console.WriteLine("You did not enter a valid character.");
                }

                if (elementsList.Count() == 1 && newPlayer == 'n')
                {
                    Console.WriteLine("There needs to be at least 2 players listed to begin playing.");
                    newPlayer = 'y';
                }

                if (newPlayer != 'n')
                {
                    Console.WriteLine("Please enter the name of the new player:");
                    playerName = Console.ReadLine();

                    Console.WriteLine("Great, now enter your strategy:");
                    playerStrategy = ' ';

                    ValidatePlayerStrategy(playerName, playerStrategy, elementsList);
                }
            }

            List<Elements> rockPlayers = elementsList.Where(x => x.playerStrategy == 'r').ToList();
            List<Elements> paperPlayers = elementsList.Where(x => x.playerStrategy == 'p').ToList();
            List<Elements> scissorPlayers = elementsList.Where(x => x.playerStrategy == 's').ToList();

            if (paperPlayers.Count() != 0)
            {
                if (scissorPlayers.Count() != 0)
                {
                    if (rockPlayers.Count() != 0)
                    {
                        Console.WriteLine(String.Format("...and the winner is {0} with rock.", rockPlayers.First().playerName));
                        Console.ReadLine();
                    }

                    else
                    {
                        Console.WriteLine(String.Format("...and the winner is {0} with scissor.", scissorPlayers.First().playerName));
                        Console.ReadLine();
                    }
                }

                else
                {
                    Console.WriteLine(String.Format("...and the winner is {0} with paper.", paperPlayers.First().playerName));
                    Console.ReadLine();
                }
            }

            else if (scissorPlayers.Count() != 0)
            {
                if (rockPlayers.Count() != 0)
                {
                    Console.WriteLine(String.Format("...and the winner is {0} with rock.", rockPlayers.First().playerName));
                    Console.ReadLine();
                }

                else
                {
                    Console.WriteLine(String.Format("...and the winner is {0} with scissor.", scissorPlayers.First().playerName));
                    Console.ReadLine();
                }
            }
        }

        private static void ValidatePlayerStrategy(string playerName, char playerStrategy, List<Elements> elementsList)
        {
            while (playerStrategy != 'r' && playerStrategy != 'p' && playerStrategy != 's')
            {
                Console.WriteLine("Enter the letter r for rock\nEnter the letter p for paper\nEnter the letter s for scissors");

                try
                {
                    playerStrategy = Convert.ToChar(Console.ReadLine().ToLower());
                }

                catch (Exception)
                {
                    playerStrategy = ' ';
                    Console.WriteLine("You did not enter a valid character.");
                }
            }

            elementsList.Add(new Elements { playerName = playerName, playerStrategy = playerStrategy });
        }
    }
}