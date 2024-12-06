using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Mastermind___Ryan_Sumpter_Quadax_Coding_Exercise
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Establish constant game parameters for quick reference and ease of modification
            const int minNumber = 1;
            const int maxNumber = 6;
            const int solutionLength = 4;
            const int guessesAmount = 10;

            // Present game to the player
            Console.WriteLine("Welcome to Simple Mastermind!");
            Console.WriteLine("You will have " + guessesAmount + " chances to guess the " + solutionLength + " digit secret answer");
            Console.WriteLine("Each digit will be a number between and including " + minNumber + " through " + maxNumber);
            Console.WriteLine("For every correct digit in the correct place in your guess, your hint will include a plus (+) symbol");
            Console.WriteLine("For every correct digit in the incorrect place in your guess, your hint will include a minus (-) symbol");
            Console.WriteLine("You must guess a " + solutionLength + " digit number");

            // This encompassing loop allows the game to be played as long as the
            // application is open
            while (true)
            {
                // Let the player know a new game is beginning, with two line breaks to help
                // separate the instructions or previous game
                Console.WriteLine("\n\nStarting new game");

                // Solution Generation
                Random rnd = new Random();
                string secretAnswer = string.Empty;

                // Generate the solution to the length specified 
                for (int i = 0; i < solutionLength; i++)
                {
                    // The Next() method in Random treats the minimum number as inclusive and the
                    // maximum number as exclusive, so 1 will need to be added to the max to allow
                    // Next() the chance to generate that number
                    secretAnswer += rnd.Next(minNumber, maxNumber + 1).ToString();
                }

                Debug.WriteLine("Generated answer: " + secretAnswer);

                // Create an array of the number of possible digits in the secret answer for more
                // efficient referencing. Digits will need to be checked by subtracting the minimum
                // playable number from the digit to get the proper index in the array
                int[] answerArray = new int[maxNumber - minNumber + 1];

                for (int i = 0; i < solutionLength; i++)
                {
                    int position = (int)char.GetNumericValue(secretAnswer[i]) - minNumber;
                    answerArray[position]++;
                }

                // Keep track of the number of guesses with an integer and whether or not the player
                // has won the game
                int guesses = 0;
                bool won = false;
                while (guesses < guessesAmount && !won)
                {
                    // Add 1 to the number of guesses the player has made to get the current guess
                    Console.WriteLine("Guess " + (guesses + 1) + "/" + guessesAmount + ":");
                    // Prompt the user for input, fall back to string.Empty if null is returned
                    string currentGuess = Console.ReadLine() ?? string.Empty;

                    // Check for errors in input length or quality
                    if (currentGuess.Length == solutionLength && int.TryParse(currentGuess, out _))
                    {
                        // Keep track of strings for each symbol to help assemble the final hint
                        string plusString = string.Empty;
                        string minusString = string.Empty;

                        // Step through each digit of the guess
                        for (int i = 0; i < solutionLength; i++)
                        {
                            // Get the position for the current guess digit to compare against the answer
                            // array. If the answer array at that position has a value greater than 0, then
                            // that digit must exist in the answer. This comparison keeps complexity to O(n)
                            // time and avoids having to check each character of the secret answer against
                            // each character of the guess
                            int position = (int)char.GetNumericValue(currentGuess[i]) - minNumber;

                            try
                            {
                                if (answerArray[position] > 0)
                                {
                                    // This digit of the guess exists somewhere in the answer, so the hint will include
                                    // at least one more symbol. If the secret answer contains the same digit at the
                                    // same position of the guess, then the hint will include a plus instead of a minus
                                    if (currentGuess[i] == secretAnswer[i])
                                        plusString += "+";
                                    else
                                        minusString += "-";

                                    Debug.WriteLine("Digit " + i + " was found in the array");
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Debug.WriteLine("Guessed number out of range");
                            }
                        }

                        // If there are less pluses in the hint than numbers in the solution, then the
                        // game has not yet been won
                        if (plusString.Length < solutionLength)
                        {
                            // Assemble and display the hint string
                            Console.WriteLine("Hint: " + plusString + minusString + "\n");

                            // Finally, increment the guess count and return to the start of the loop
                            guesses++;
                        }
                        else
                        {
                            // Set the won bool to true to close the gameplay loop
                            won = true;
                        }
                        
                    }
                    else
                    {
                        // Reject the guess and prompt the user to try again
                        // This does not increment the guess count
                        Console.WriteLine("This is not a valid guess. Please try again.");
                    }
                }

                // Finish this round and display a message depending on if the player guessed the
                // correct number or not
                if (won)
                    Console.WriteLine("Congratuations! You guessed the correct number: " + secretAnswer);
                else
                    Console.WriteLine("No more guesses! The secret answer you were looking for was: " + secretAnswer);
            }
        }
    }
}
