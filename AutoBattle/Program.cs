using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{


    /// <summary>
    /// Most methods of the Main class have been moved to Battle.cs
    /// In order to encapsulate battles and make it easier to handle multiple battles
    /// </summary>

    class Program
    {
        static void Main(string[] args)
        {
            Battle battler;
            int currentBattle = 1;
            int victories, defeats;
            int teamSize = 2;
            int currentUnitId = 0;
            int baseDamage = 30;

            Team playerTeam;
            List<Team> teams;
            Character currentCharacter;


            Setup();


            void Setup()
            {
                victories = defeats = 0;

                GetPlayerChoices();
                GenerateEnemies();


                battler = new Battle();
                battler.SetUpBattle(playerTeam, teams[currentBattle], new Action<bool>(FinishBattle), currentBattle);

            }


            void FinishBattle(bool isVictory)
            {

                if (isVictory)
                {
                    victories++;
                }
                else
                {
                    defeats++;
                }

                Console.Clear();

                Console.Write(Environment.NewLine + Environment.NewLine);
                Console.WriteLine($"Current Score: {victories}/{defeats}");
                Console.Write(Environment.NewLine + Environment.NewLine);


                if (currentBattle < 5)
                {


                    Console.WriteLine("Press any key to start the next battle...\n");
                    ConsoleKeyInfo key = Console.ReadKey();
                    Console.Clear();

                    currentBattle++;
                    battler = new Battle();
                    //battler.SetUpBattle(playerTeam.characters[0], AllPlayers[currentBattle], new Action<bool>(FinishBattle), currentBattle);
                    //battler.SetUpBattle(playerTeam.characters[0], teams[currentBattle].characters[0], new Action<bool>(FinishBattle), currentBattle);
                    //battler.SetUpBattle(playerTeam.characters, teams[currentBattle].characters, new Action<bool>(FinishBattle), currentBattle);
                    battler.SetUpBattle(playerTeam, teams[currentBattle], new Action<bool>(FinishBattle), currentBattle);


                }
                else
                {
                    Console.Clear();
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine($"Final Score: {victories}/{defeats}");
                    if (victories > defeats)
                    {
                        Console.WriteLine("THE PLAYER WAS VICTORIOUS!!");
                    }
                    else
                    {
                        Console.WriteLine("THE PLAYER HAS BEEN DEFEATED...");
                    }

                }
            }


            void GetPlayerChoices()
            {
                List<Character> characters = new List<Character>();

                for (int i = 0; i < teamSize; i++)
                {
                    GetPlayerChoice();
                    characters.Add(currentCharacter);

                }

                playerTeam = new Team(characters);
                //PlayerCharacter = playerTeam.characters[0];
            }


            void GetPlayerChoice()
            {

                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose between one of these Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();



                switch (choice)
                {
                    case "1":
                        currentCharacter = (CreatePlayerCharacter(Int32.Parse(choice)));
                        break;
                    case "2":
                        currentCharacter = (CreatePlayerCharacter(Int32.Parse(choice)));
                        break;
                    case "3":
                        currentCharacter = (CreatePlayerCharacter(Int32.Parse(choice)));
                        break;
                    case "4":
                        currentCharacter = (CreatePlayerCharacter(Int32.Parse(choice)));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }


            }

            Character CreatePlayerCharacter(int classIndex)
            {


                Character PlayerCharacter;

                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.Clear();

                //Player Index on constructor
                PlayerCharacter = new Character(characterClass, currentUnitId);
                PlayerCharacter.Health = 100;
                PlayerCharacter.baseDamage = baseDamage;
                PlayerCharacter.teamIndex = 0;


                currentUnitId++;
                return PlayerCharacter;
            }


            void GenerateEnemies()
            {

                teams = new List<Team>();

                for (int i = 1; i < 8; i++)
                {
                    teams.Add(GenerateEnemyTeam(i));
                }
            }


            Team GenerateEnemyTeam(int id)
            {
                List<Character> characters = new List<Character>();

                for (int i = 0; i < teamSize; i++)
                {
                    characters.Add(CreateEnemyCharacter(id));
                }

                return new Team(characters);
            }


            Character CreateEnemyCharacter(int id)
            {

                Character EnemyCharacter;


                var rand = new Random();
                int randomInteger = rand.Next(1, 4);
                CharacterClass enemyClass = (CharacterClass)randomInteger;
                //Console.WriteLine($"Enemy {id} Class Choice: {enemyClass}");


                //Player Index on constructor
                EnemyCharacter = new Character(enemyClass, currentUnitId);
                EnemyCharacter.Health = 100;
                EnemyCharacter.baseDamage = baseDamage;
                EnemyCharacter.teamIndex = id;


                currentUnitId++;
                return EnemyCharacter;
            }

        }
    }
}
