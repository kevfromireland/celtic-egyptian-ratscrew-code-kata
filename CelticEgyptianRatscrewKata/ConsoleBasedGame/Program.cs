using System;
using System.Collections.Generic;
using System.Linq;
using CelticEgyptianRatscrewKata.Game;

namespace ConsoleBasedGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var playerActions = new Dictionary<char, Action>();

            GameController game = new GameFactory().Create();

            var userInterface = new UserInterface();
            IEnumerable<PlayerInfo> playerInfos = userInterface.GetPlayerInfoFromUserLazily().ToList();

            foreach (PlayerInfo playerInfo in playerInfos)
            {
                var player = new Player(playerInfo.PlayerName);

                playerActions.Add(playerInfo.PlayCardKey, () => game.PlayCard(player));
                playerActions.Add(playerInfo.SnapKey, () => game.AttemptSnap(player));

                game.AddPlayer(player);
            }

            game.StartGame(GameFactory.CreateFullDeckOfCards());

            char userInput;
            while (userInterface.TryReadUserInput(out userInput))
            {
                Action playerAction;
                if (playerActions.TryGetValue(userInput, out playerAction))
                {
                    playerAction();
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }
    }
}
