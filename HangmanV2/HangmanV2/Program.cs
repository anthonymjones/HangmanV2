using HangmanV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class Program
    {
        private static string returnString;
        static void Main(string[] args)
        {
            //call the hangman function to start the game
            hangMan();
        }
        //create a function for the hangman game
        static void hangMan()
        {

            // Create wordBank list
            List<string> wordBank = new List<string>() { "Bills", "Dolphins", "Patriots", "Jets", "Ravens", "Bengals", "Browns", "Steelers", "Texans", "Colts", "Jaguars", "Titans", "Broncos", "Chiefs", "Raiders", "Chargers", "Cowboys", "Giants", "Eagles", "Redskins", "Bears", "Lions", "Packers", "Vikings",
                "Falcons", "Panthers", "Saints", "Buccaneers", "Cardinals", "Rams", "FortyNiners", "Seahawks" };
            // Create random word generator
            Random randGen = new Random();
            // Declare variable to hold lives
            int lives = 7;
            //declare variable to hold letters guessed
            string lettersGuessed = String.Empty;
            // Get random string from wordBank
            string wordtoGuess = wordBank[randGen.Next(0, wordBank.Count)].ToLower();
            // User has not won yet
            bool won = false;

            // Set window size
            Console.SetWindowSize(90, 50);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            // Display user greeting and instructions
            Console.WriteLine();
            Console.WriteLine("Hi! Let's play Hangman.");
            Console.WriteLine();
            Console.WriteLine("I'll pick a word, and you guess a letter or the word if you think you know it.");
            Console.WriteLine();

            Console.WriteLine("If you guess wrong, you lose one of your 7 lives.");
            Console.WriteLine();

            //request a guess from the user
            Console.WriteLine(maskedWord(wordtoGuess, lettersGuessed));
            Console.WriteLine();

            Console.WriteLine("Please enter a letter or guess the word.");

            // Create loop while still playing game
            while (lives > 0 && !won)
            {
                //declare input string
                string input = String.Empty;
                //declare your input variable
                input = Console.ReadLine().ToLower();

                // Is guess more than one character
                if (input.Length > 1)
                {
                    if (input == wordtoGuess.ToLower())
                    {
                        won = true;
                        //tell the user they won, and thank them for playing
                        Console.WriteLine();

                        Console.WriteLine("Yes, it's " + wordtoGuess + "! Thanks for playing.");
                    }
                    //their guess was not correct, tell them to guess again
                    else
                    {
                        won = false;
                        Console.WriteLine();

                        Console.WriteLine("That's not the word I was thinking of. Guess again!");
                        lives--;
                        // Display masked word, number of guesses, letters used
                        Console.WriteLine();

                        Console.WriteLine(maskedWord(wordtoGuess, lettersGuessed));
                        Console.WriteLine();

                        Console.WriteLine("Lives left: " + lives);
                        Console.WriteLine();

                        Console.WriteLine("Letters guessed: " + lettersGuessed);
                    }
                }

                    //guess is one character, compare to letters in word to guess
                else
                {
                    //add guessed letter
                    lettersGuessed += input;

                    //does the letter match?
                    if (wordtoGuess.ToLower() == maskedWord(wordtoGuess, lettersGuessed).ToLower())
                    {
                        won = true;
                        //tell the user they won, and thank them for playing
                        Console.WriteLine();

                        Console.WriteLine("Yes, it's " + wordtoGuess + "! Thanks for playing.");
                    }

                    else if (wordtoGuess.ToLower().Contains(input))
                    {
                        //letter matches, reveal in masked word, subtract a life, add to letters guessed
                        Console.WriteLine();

                        Console.WriteLine("You got a letter!");

                        // Display masked word, number of guesses, letters used
                        Console.WriteLine(maskedWord(wordtoGuess, lettersGuessed));
                        Console.WriteLine("Lives left: " + lives);
                        Console.WriteLine("Letters guessed: " + lettersGuessed);

                    }
                    //letter does not match, subtract a life, add to letters guessed
                    else
                    {
                        Console.WriteLine();

                        Console.WriteLine("Guess again.");
                        lives--;
                        // Display masked word, number of guesses, letters used
                        Console.WriteLine(maskedWord(wordtoGuess, lettersGuessed));
                        Console.WriteLine("Lives left: " + lives);
                        Console.WriteLine("Letters guessed: " + lettersGuessed);
                    }


                }
            }
            if (lives == 0)
            {
                Console.WriteLine();
                Console.WriteLine("I'm sorry, you lose. The word was " + wordtoGuess);
            }

            //game over
            System.Threading.Thread.Sleep(1500);
            if (won)
            {
                AddHighScore(lives);
                DisplayHighScores();
            }
        }
        static void AddHighScore(int playerScore)
        {
            //get the player name for high scores
            Console.Write("Your name: ");
            string playerName = Console.ReadLine();

            //create a gateway to the database
            AnthonyEntities db = new AnthonyEntities();

            //create a new highscore object
            HighScore newHighScore = new HighScore();
            newHighScore.DateCreated = DateTime.Now;
            newHighScore.Game = "Hangman";
            newHighScore.Name = playerName;
            newHighScore.Score = playerScore;

            //add to the database
            db.HighScores.Add(newHighScore);

            //save our changes
            db.SaveChanges();
        }
        static void DisplayHighScores()
        {
            Console.SetWindowSize(40, 30);
            //clear the console
            Console.Clear();
            //Write the High Score Text
            Console.WriteLine();
            Console.WriteLine("         Hangman High Scores");
            Console.WriteLine("    *****************************");

            //create a new connection to the db
            AnthonyEntities db = new AnthonyEntities();
            //get the high score list
            List<HighScore> highScoreList = db.HighScores.Where(x => x.Game == "Hangman").OrderByDescending(x => x.Score).Take(10).ToList();

            foreach (HighScore highScore in highScoreList)
            {
                Console.WriteLine("    {0}. {1} - {2} on {3}", highScoreList.IndexOf(highScore) + 1, highScore.Name, highScore.Score, highScore.DateCreated.Value.ToShortDateString());
            }
            Console.ReadKey();
        }
        //create function for Display Masked Word
        static string maskedWord(string wordtoGuess, string lettersGuessed)
        {
            //get word to guess and letters guessed list

            //declare output
            returnString = "";

            int i = 0;

            while (i < wordtoGuess.Length)
            {
                //get a letter from the word
                string letter = wordtoGuess[i].ToString();
                //did the user guess the current letter in the word?
                if (lettersGuessed.Contains(letter))
                {
                    //user guessed the right letter
                    //add letter to the return string
                    returnString += letter;
                    i++;
                }
                else
                {
                    //letter not found
                    //add an underscore to the return string
                    returnString += "_ ";
                    i++;
                }
                returnString += "";

            }
            //display return string
            return returnString;
        }
    }
}