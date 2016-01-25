using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Rock_Paper_Scissors_Tournament
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
            List<List<List<Elements>>> elementsList = new List<List<List<Elements>>>();
            List<List<Elements>> doubleElements = new List<List<Elements>>();
            List<Elements> singleElement = new List<Elements>();

            Console.WriteLine("Hi, welcome to Rock-Paper-Scissor!\nPlease enter your name:");
            string playerName = Console.ReadLine();

            Console.WriteLine("Great, now enter your strategy:");
            char playerStrategy = ' ';

            ValidatePlayerStrategy(playerName, playerStrategy, ref singleElement, ref doubleElements, ref elementsList);

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
                    newPlayer = ' ';
                    Console.WriteLine("You did not enter a valid character.");
                }

                if (newPlayer == 'n')
                {
                    if (singleElement.Count() == 1 && doubleElements.Count() == 0 && elementsList.Count() == 0)
                    {
                        Console.WriteLine("There needs to be at least 2 players listed to begin playing.");
                        newPlayer = 'y';
                    }

                    else if (singleElement.Count() == 1)
                    {
                        Console.WriteLine("There needs to be 1 more player to begin playing.");
                        newPlayer = 'y';
                    }

                    else if (doubleElements.Count() % 2 != 0)
                    {
                        Console.WriteLine("There needs to be another pair of players to begin playing.");
                        newPlayer = 'y';
                    }

                    else if (doubleElements.Count() == 2)
                    {
                        elementsList.Add(doubleElements);
                    }
                }

                if (newPlayer == 'y')
                {
                    Console.WriteLine("Please enter the name of the new player:");
                    playerName = Console.ReadLine();

                    Console.WriteLine("Great, now enter your strategy:");
                    playerStrategy = ' ';

                    ValidatePlayerStrategy(playerName, playerStrategy, ref singleElement, ref doubleElements, ref elementsList);
                }
            }

            ValidateWinner(elementsList);
            SQLManagement(elementsList);

            Console.WriteLine(String.Format("...and the winner is {0} with the {1} strategy.", elementsList[0][0][0].playerName, GetStrategy(elementsList[0][0][0].playerStrategy)));
            Console.ReadLine();
        }

        private static void ValidateWinner(List<List<List<Elements>>> elementsList)
        {
            for (int i = 0; i < elementsList.Count(); i++)
            {
                for (int x = 0; x < elementsList[i].Count(); x++)
                {
                    elementsList[i][x].Remove(elementsList[i][x][GetLoser(elementsList[i][x][0].playerStrategy, elementsList[i][x][1].playerStrategy)]);
                }

                for (int x = 0; x < elementsList[i].Count(); x++)
                {
                    elementsList[i].Remove(elementsList[i][GetLoser(elementsList[i][0][0].playerStrategy, elementsList[i][1][0].playerStrategy)]);
                }
            }

            for (int i = 0; i < elementsList.Count(); i++)
            {
                if (elementsList[i + 1][0] != null)
                {
                    elementsList.Remove(elementsList[GetLoser(elementsList[i][0][0].playerStrategy, elementsList[i + 1][0][0].playerStrategy)]);
                }
            }
        }

        private static void ValidatePlayerStrategy(string playerName, char playerStrategy, ref List<Elements> singleElement, ref List<List<Elements>> doubleElements, ref List<List<List<Elements>>> elementsList)
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

                singleElement.Add(new Elements { playerName = playerName, playerStrategy = playerStrategy });

                if (singleElement.Count() == 2)
                {
                    doubleElements.Add(singleElement);
                    singleElement = new List<Elements>();
                }

                if (doubleElements.Count() == 2)
                {
                    elementsList.Add(doubleElements);
                    doubleElements = new List<List<Elements>>();
                }
            }
        }

        private static void SQLManagement(List<List<List<Elements>>> elementsList)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "Server=.;Database=RockPaperScissor_Tournament_DB;Trusted_Connection=true;MultipleActiveResultSets=True;";
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT PlayerPoints FROM Leaderboard WHERE PlayerName like @0", conn);
                command.Parameters.Add(new SqlParameter("0", elementsList[0][0][0].playerName));

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        SqlCommand executeCommand = new SqlCommand("UPDATE Leaderboard SET PlayerPoints = @0 WHERE PlayerName like @1;", conn);
                        executeCommand.Parameters.Add(new SqlParameter("0", (int)reader[0] + 3));
                        executeCommand.Parameters.Add(new SqlParameter("1", elementsList[0][0][0].playerName));
                        executeCommand.ExecuteNonQuery();
                    }

                    else
                    {
                        SqlCommand executeCommand = new SqlCommand("INSERT INTO Leaderboard (PlayerName, PlayerStrategy, PlayerPoints) VALUES (@0, @1, @2)", conn);
                        executeCommand.Parameters.Add(new SqlParameter("0", elementsList[0][0][0].playerName));
                        executeCommand.Parameters.Add(new SqlParameter("1", elementsList[0][0][0].playerStrategy));
                        executeCommand.Parameters.Add(new SqlParameter("2", 3));
                        executeCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private static string GetStrategy(char strategy)
        {
            switch (strategy)
            {
                case 'r':
                    return "rock";

                case 'p':
                    return "paper";

                case 's':
                    return "scissor";
            }

            return null;
        }

        private static int GetLoser(char player1, char player2)
        {
            switch (player1)
            {
                case 'p':
                    if (player2 == 's')
                    {
                        return 0;
                    }

                    else
                    {
                        return 1;
                    }

                case 'r':
                    if (player2 == 'p')
                    {
                        return 0;
                    }

                    else
                    {
                        return 1;
                    }

                case 's':
                    if (player2 == 'r')
                    {
                        return 0;
                    }

                    else
                    {
                        return 1;
                    }
            }

            return 0;
        }
    }
}