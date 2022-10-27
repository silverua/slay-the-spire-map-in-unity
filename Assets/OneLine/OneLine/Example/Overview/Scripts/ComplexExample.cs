using System;
using UnityEngine;

namespace OneLine.Examples {
    [CreateAssetMenu(menuName = "OneLine/Examples/ComplexExample")]
    public class ComplexExample : ScriptableObject {

        public Character[] characters;

        [Serializable]
        public class Character {
            [SerializeField, OneLine, Separator]
            private FullName fullName;
            [SerializeField, OneLineWithHeader]
            private Stats stats;
            [SerializeField, OneLine]
            private Skill[] skills;
        }

        [Serializable]
        public class FullName {
            [SerializeField, Tooltip("First name is not good")] private string firstName;
            [SerializeField] private string secondName;
            [SerializeField,Separator] private string nickName;
        }

        [Serializable]
        public class Stats {
            [SerializeField] private int strength;
            [SerializeField] private int intelligence;
            [SerializeField] private int agility;
            [SerializeField] private int charisma;
            [SerializeField] private int luck;
        }

        // Skills are from the legendary game `Dwarf Fortress`
        [Serializable]
        public class Skill {
            [SerializeField] private SkillType type;
            [SerializeField] private SkillLevel level;
        }

        public enum SkillType {
            Miner,
            Bowyer,
            Carpenter,
            WoodCutter,
            Engraver,
            Mason,
        }

        public enum SkillLevel {
            Dabbling,
            Novice,
            Adequate,
            Competent,
            Skilled,
            Proficient,
            Talented,
            Adept,
            Expert,
            Professional,
            Accomplished,
            Great,
            Master,
            HighMaster,
            GrandMaster,
            Legendary,
        }
    }
}