using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace ConsoleBasedGame
{
    class UserInterface
    {
        public IEnumerable<PlayerInfo> GetPlayerInfoFromUserLazily()
        {
            HashSet<char> invalidKeys = new HashSet<char>();
            bool again;
            do
            {
                Console.Write("Enter player name: ");
                var playerName = Console.ReadLine();

                var playCardKey = AskForKey("Enter play card key: ", invalidKeys);
                invalidKeys.Add(playCardKey);

                var snapKey = AskForKey("Enter snap key: ", invalidKeys);
                invalidKeys.Add(snapKey);

                yield return new PlayerInfo(playerName, playCardKey, snapKey);

                var createPlayerKey = AskForKey("Create another player? (y|n): ");
                again = createPlayerKey.Equals('y');
            } while (again);
        }

        private static char AskForKey(string prompt, HashSet<char> invalidKeys = null)
        {
            Console.Write(prompt);
            var response = Console.ReadKey().KeyChar;
            while (invalidKeys != null && invalidKeys.Contains(response))
            {
                Console.WriteLine("Already used, " + prompt);
                response = Console.ReadKey().KeyChar;
            }
            Console.WriteLine();
            return response;
        }

        public bool TryReadUserInput(out char userInput)
        {
            ConsoleKeyInfo keyPress = Console.ReadKey();
            Console.WriteLine();
            userInput = keyPress.KeyChar;
            return keyPress.Key != ConsoleKey.Escape;
        }
    }
}