using System;
using System.Collections.Generic;
using System.Text;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using static AutoBattle.Types;
using System.Linq;







namespace AutoBattle
{


    /// <summary>
    /// This class is responsible for running all the flow of an entire battle and then returns the result to Program.cs
    /// in order to keep scores
    /// </summary>

    class Battle
    {

        Grid grid;
        List<Character> allPlayers = new List<Character>();

        List<Character> playerSide = new List<Character>();
        List<Character> enemySide = new List<Character>();
        int playersLeft, enemiesLeft;

        Action<bool> finishCombat;
        Action<int, int> unitDied;
        int currentTurn = 0;
        Random rand = new Random();

        #region Update Status
        void DrawBattlefield(bool wait = true)
        {
            grid.DrawBattlefield();


            string bar = "";

            for (int i = 0; i < 49; i++)
            {
                bar += "-";
            }

            Console.WriteLine(bar);
            Console.WriteLine($"| Player Side: {playersLeft}/{playerSide.Count}\t|\tEnemy Side: {enemiesLeft}/{enemySide.Count} |");
            Console.WriteLine(bar);

            if (wait)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine("\nPress any key to continue");
            }
        }



        //Team id and unitId of the target that just died
        void UnitDiedCallback(int teamId, int unitId)
        {
            bool isPlayerTarget;


            if (teamId == 0)
            {
                playersLeft--;
                isPlayerTarget = false;
            }
            else
            {
                enemiesLeft--;
                isPlayerTarget = true;
            }

            int id = 0;
            int gridPos;

            for (int i = 0; i < allPlayers.Count; i++)
            {
                if (allPlayers[i].playerIndex == unitId)
                {
                    id = i;
                    break;
                }
            }


            gridPos = allPlayers[id].currentBox.Index;


            GridBox pos = grid.grids.ElementAt(gridPos);
            pos.ocupied = false;
            pos.charId = "*";

            grid.grids[gridPos] = pos;


            allPlayers.RemoveAt(id);

            //Console.WriteLine($"UnitDiead gridpos: {gridPos} | Count: {allPlayers.Count}");

            UpdateTargets(unitId, teamId);
            DrawBattlefield();

        }


        void UpdateTargets(int unitId, int teamId)
        {
            for (int i = 0; i < allPlayers.Count; i++)
            {

                if (allPlayers[i].Target.playerIndex == unitId)
                {
                    allPlayers[i].Target = FindValidTarget(teamId);
                }
            }


        }


        Character FindValidTarget(int teamId)
        {
            //Find target from the same team as the one that died

            for (int i = 0; i < allPlayers.Count; i++)
            {
                if (allPlayers[i].teamIndex == teamId)
                {
                    return allPlayers[i];
                }
            }

            return null;
        }

        void StartTurn()
        {

            //Added turn debug
            //Clear screen
            Console.Clear();
            //Console.WriteLine("Start Turn: " + (currentTurn + 1));

            if (currentTurn == 0)
            {
                //allPlayers.Sort();  
            }

            //foreach (Character character in allPlayers)
            //{
            //    Console.WriteLine($"\nTeam {character.teamIndex}'s {character.characterClass.ToString()} ({character.playerIndex}) action");
            //    character.StartTurn(grid);
            //}


            for (int i = 0; i < allPlayers.Count; i++)
            {
                Console.Clear();
                Console.WriteLine("Turn: " + (currentTurn + 1));
                Console.WriteLine($"\n(Move {i + 1}) Team {allPlayers[i].teamIndex}'s {allPlayers[i].characterClass.ToString()} ({allPlayers[i].playerIndex}) action");
                allPlayers[i].StartTurn(grid);

                Console.WriteLine("\nPress any key to show next move...");
                ConsoleKeyInfo key = Console.ReadKey();

            }


            currentTurn++;
            HandleTurn();
        }

        void HandleTurn()
        {
            if (playersLeft == 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("The Player has been defeated\nYou've lost");
                Console.Write(Environment.NewLine + Environment.NewLine);

                EndCombat(false);
                return;
            }
            else if (enemiesLeft == 0)
            {
                Console.Write(Environment.NewLine + Environment.NewLine);
                // endgame?
                Console.WriteLine("The Enemy has been defeated\nYou've won");
                Console.Write(Environment.NewLine + Environment.NewLine);

                EndCombat(true);
                return;
            }
            else
            {
                //Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine("Click on any key to start the next turn...\n");
                Console.Write(Environment.NewLine + Environment.NewLine);

                ConsoleKeyInfo key = Console.ReadKey();
                StartTurn();
            }
        }



        #endregion


        public void SetUpBattle(Team playerTeam, Team enemyTeam, Action<bool> finishCombat, int battleId)
        {

            //grid = new Grid(5, 10);
            GenerateBattlefield();

            playerSide = playerTeam.characters;
            enemySide = enemyTeam.characters;


            //this.playerSide.Add(playerTeam.characters[0]);
            //this.enemySide.Add(enemyTeam.characters[0]);

            this.finishCombat = finishCombat;



            StartGame(battleId);
        }

        void GenerateBattlefield()
        {

            int lines, columns;
            Random rand = new Random();

            columns = rand.Next(5, 11);
            lines = rand.Next(5, 11);


            //grid = new Grid(columns, lines);
            grid = new Grid(5, 5);


        }


        void ResetCombatantsData()
        {

            foreach (Character character in allPlayers)
            {
                character.Health = 100;
            }

        }

        void SetTargets()
        {

            //populates the character variables and targets
            unitDied = new Action<int, int>(UnitDiedCallback);

            foreach (Character character in playerSide)
            {


                character.Target = enemySide[rand.Next(0, enemySide.Count)];
                character.unitDied = unitDied;

                playersLeft++;
                allPlayers.Add(character);
            }



            foreach (Character character in enemySide)
            {

                character.Target = playerSide[rand.Next(0, playerSide.Count)];
                character.unitDied = unitDied;

                enemiesLeft++;
                allPlayers.Add(character);
            }

            //Shuffle list
            allPlayers.Shuffle();

            ResetCombatantsData();
        }


        void StartGame(int battleId)
        {

            //Console.WriteLine($"<Battle {battleId} - Player {PlayerCharacter.playerIndex} vs Enemy {EnemyCharacter.playerIndex}>");
            Console.WriteLine($"<Battle {battleId} - Player {playerSide[0].playerIndex} vs Enemy {enemySide[0].playerIndex}>\n");

            SetTargets();
            AlocatePlayers();


            DrawBattlefield(false);
            Console.Write(Environment.NewLine + Environment.NewLine);

            Console.WriteLine("Press any key to start the battle...\n");
            ConsoleKeyInfo key = Console.ReadKey();


            StartTurn();


        }

        void EndCombat(bool isVictory)
        {
            finishCombat(isVictory);
        }



        #region Setup
        void AlocatePlayers()
        {
            AlocatePlayerCharacters();
            AlocateEnemyCharacters();
        }

        void AlocatePlayerCharacters()
        {
            for (int i = 0; i < playerSide.Count; i++)
            {
                AlocatePlayerCharacter(i);
            }
        }

        void AlocateEnemyCharacters()
        {
            for (int i = 0; i < enemySide.Count; i++)
            {
                AlocateEnemyCharacter(i);
            }
        }



        void AlocatePlayerCharacter(int id)
        {
            int random = 0;
            random = rand.Next(0, grid.xLength);



            GridBox RandomLocation = (grid.grids.ElementAt(random));

            RandomLocation.charId = playerSide[id].GenerateCharacterId();

            //Console.Write($"{random}\n");
            if (!RandomLocation.ocupied)
            {
                //sGridBox PlayerCurrentLocation = RandomLocation;
                RandomLocation.ocupied = true;
                grid.grids[random] = RandomLocation;
                playerSide[id].currentBox = grid.grids[random];

                //Set team id
                playerSide[id].teamIndex = 0;
            }
            else
            {
                AlocatePlayerCharacter(id);
            }
        }




        void AlocateEnemyCharacter(int id)
        {
            int random = 24;
            random = rand.Next(grid.grids.Count - grid.xLength, grid.grids.Count);

            GridBox RandomLocation = (grid.grids.ElementAt(random));
            RandomLocation.charId = enemySide[id].GenerateCharacterId();


            if (!RandomLocation.ocupied)
            {
                //sEnemyCurrentLocation = RandomLocation;
                RandomLocation.ocupied = true;
                grid.grids[random] = RandomLocation;
                enemySide[id].currentBox = grid.grids[random];


                //Set team id
                enemySide[id].teamIndex = 2;
            }
            else
            {
                AlocateEnemyCharacter(id);
            }


        }



        #endregion


    }
}
