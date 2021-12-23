using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public class Types
    {




        public struct Team
        {
            public List<Character> characters;

            public Team(List<Character> characters)
            {
                this.characters = characters;
            }
        }


        public struct MovementOption
        {
            public string name;
            public GridBox gridBox;

            public int mod;


            public MovementOption(string name, GridBox gridBox, int mod = 0)
            {
                this.gridBox = gridBox;

                this.name = name;
                this.mod = mod;

            }
        }




        public struct GridBox
        {
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;

            //Character id for display
            public string charId;

            //Character id for combat
            public int unitId;


            //Changed constructor to have teamId
            public GridBox(int x, int y, bool ocupied, int index, int unitId = -1, string charId = "")
            {
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.Index = index;

                //Added character identifier
                this.charId = charId;
                this.unitId = unitId;
            }

            public int CheckDistance(GridBox pos)
            {
                double distance;
                distance = Math.Sqrt(Math.Pow(xIndex - pos.xIndex, 2) + Math.Pow(yIndex - pos.yIndex, 2));
                //Console.WriteLine($"CheckDistance() {xIndex} , {yIndex}  -> {pos.xIndex},{pos.yIndex}");

                return (int)distance;
            }


            public void SetOccupied(bool active)
            {
                ocupied = false;
                if (!ocupied)
                {
                    charId = "*";
                }
            }


        }


        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

        #region Unused types
        public struct CharacterSkills
        {
            string Name;
            float damage;
            float damageMultiplier;
        }

        public struct CharacterClassSpecific
        {
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;

        }
        #endregion


    }
}
