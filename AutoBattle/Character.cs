using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public string Name { get; set; }
        public float Health;
        public float DamageMultiplier { get; set; } // not used yet
        public Character Target { get; set; }

        public float baseDamage;
        public int playerIndex;

        public char classTag;
        public int teamIndex;

        public GridBox currentBox;
        public CharacterClass characterClass;
        public bool isAlive = true;

        //Create method here as callback
        public Action<int, int> unitDied;



        public Character(CharacterClass characterClass, int unitId)
        {
            this.characterClass = characterClass;
            playerIndex = unitId;


            if (characterClass == CharacterClass.Paladin)
            {
                DamageMultiplier = 0.9f;
                Health = (int)Math.Round(Health * 1.25f);
            }
            else if (characterClass == CharacterClass.Cleric)
            {
                DamageMultiplier = 1.25f;
                Health = (int)Math.Round(Health * 0.8f);
            }
            else if (characterClass == CharacterClass.Archer)
            {
                DamageMultiplier = 1.5f;
                Health = (int)Math.Round(Health * 0.75f);

            }
            else if (characterClass == CharacterClass.Warrior)
            {
                DamageMultiplier = 1.25f;
            }

            classTag = characterClass.ToString()[0];
            isAlive = true;
        }


        public bool TakeDamage(float amount)
        {

            //Changed to deal damage based on value generated
            if ((Health -= amount) <= 0)
            {
                Die();
                return true;
            }


            Console.WriteLine($"\t{characterClass.ToString()} ({playerIndex}) has {Health} HP left\n");

            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
            Console.WriteLine($"\t{characterClass.ToString()} ({playerIndex})  has died T-T");
            Health = 0;
            isAlive = false;

            unitDied(teamIndex, playerIndex);
        }

        public void StartTurn(Grid battlefield)
        {


            if (CheckCloseTargets(battlefield))
            {

                Attack(Target);
                return;
            }
            else
            {
                CalculateMovement(currentBox, battlefield);

            }
        }

        public void Attack(Character target)
        {
            var rand = new Random();
            bool killingBlow = false;

            //Added variable to store the damage generated
            int damage;
            damage = rand.Next(5, (int)(baseDamage * DamageMultiplier));
            Console.WriteLine($"\t{characterClass.ToString()} ({playerIndex}) strikes {Target.characterClass.ToString()} ({Target.playerIndex}) for {damage} damage");

            killingBlow = target.TakeDamage(damage);

        }

        public string GenerateCharacterId()
        {
            return "T" + teamIndex + "_" + classTag + playerIndex;
        }


        void CalculateMovement(GridBox currentBox, Grid battlefield)
        {
            List<MovementOption> moveOptions = new List<MovementOption>();
            GridBox nextPos;


            //Left
            if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
            {
                nextPos = battlefield.grids.Find(x => x.Index == currentBox.Index - 1);

                if (!nextPos.ocupied)
                {
                    moveOptions.Add(new MovementOption("left", nextPos, -1));
                }

            }

            //Right
            if ((battlefield.grids.Exists(x => x.Index == currentBox.Index + 1)))
            {
                nextPos = battlefield.grids.Find(x => x.Index == currentBox.Index + 1);

                if (!nextPos.ocupied)
                {
                    moveOptions.Add(new MovementOption("right", nextPos, +1));
                }

            }

            //Up
            if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - battlefield.xLength)))
            {
                nextPos = battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLength);

                if (!nextPos.ocupied)
                {
                    moveOptions.Add(new MovementOption("up", nextPos, -battlefield.xLength));
                }


            }

            //Down
            if ((battlefield.grids.Exists(x => x.Index == currentBox.Index + battlefield.xLength)))
            {
                nextPos = battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLength);

                if (!nextPos.ocupied)
                {
                    moveOptions.Add(new MovementOption("down", nextPos, battlefield.xLength));
                }

            }


            int id = -1;
            int minDist = int.MaxValue;
            int dist;


            for (int i = 0; i < moveOptions.Count; i++)
            {

                dist = Target.currentBox.CheckDistance(moveOptions[i].gridBox);

                //Console.WriteLine(moveOptions[i].name + " - Dist: " + dist);

                if (dist < minDist)
                {
                    id = i;
                    minDist = dist;
                }
            }

            //Console.WriteLine("Target: " + moveOptions[id].gridBox.Index);


            if (id != -1 && Target != null)
            {
                MoveTo(battlefield, moveOptions[id].mod);
                Console.WriteLine($"{characterClass.ToString()} walked {moveOptions[id].name} towards: {Target.characterClass} ({Target.playerIndex})");
                battlefield.DrawBattlefield();
            }



        }

        void MoveTo(Grid battlefield, int mod)
        {
            currentBox.ocupied = false;
            currentBox.charId = "";
            currentBox.unitId = -1;

            battlefield.grids[currentBox.Index] = currentBox;

            currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + mod));

            currentBox.ocupied = true;
            currentBox.charId = GenerateCharacterId();
            currentBox.unitId = playerIndex;


            battlefield.grids[currentBox.Index] = currentBox;

            if (CheckCloseTargets(battlefield))
            {
                Attack(Target);
            }

        }


        //// Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {

            int targetId = Target.playerIndex;


            //Check if position is valid, then checks if the unit in that spot is the target

            if (currentBox.xIndex - 1 >= 0)
            {
                bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).unitId == targetId);
                if (left) return true;
            }

            if (currentBox.xIndex + 1 > battlefield.xLength)
            {
                bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).unitId == targetId);
                if (right) return true;
            }


            if (currentBox.yIndex - 1 >= 0)
            {
                bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLength).unitId == targetId);
                if (up) return true;
            }



            if (currentBox.yIndex + 1 < battlefield.yLength)
            {
                bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLength).unitId == targetId);
                if (down) return true;
            }

            return false;
        }


    }
}
