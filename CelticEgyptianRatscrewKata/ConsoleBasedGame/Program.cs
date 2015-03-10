using System;
using System.Collections.Generic;
using System.Linq;
using CelticEgyptianRatscrewKata;
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

                playerActions.Add(playerInfo.PlayCardKey, new PlayCardCommand(game, player).Execute);
                playerActions.Add(playerInfo.SnapKey, new SnapCommand(game, player).Execute);

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

    internal class PlayCardCommand
    {
        private readonly GameController m_Game;
        private readonly Player m_Player;

        public PlayCardCommand(GameController game, Player player)
        {
            m_Game = game;
            m_Player = player;
        }

        internal void Execute()
        {
            Card card = m_Game.PlayCard(m_Player);

            if (card == null)
            {
                Console.WriteLine("{0} had no cards. Sucks to be him/her", m_Player.Name);
            }
            else
            {
                Console.WriteLine("{0} has played {1}", m_Player.Name, card);
            }
        }
    }

    internal class SnapCommand
    {
        private readonly GameController m_Game;
        private readonly Player m_Player;

        public SnapCommand(GameController game, Player player)
        {
            m_Game = game;
            m_Player = player;
        }

        internal void Execute()
        {
            m_Game.AttemptSnap(m_Player);
            Console.WriteLine("{0} attempted to snap", m_Player.Name);
        }
    }
}
