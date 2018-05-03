using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RoslynAnalyzer.TestModels.Entities
{
    [DataContract]
    public class Player
    {
        public int Id { get; set; }

        public string  Nickname { get; set; } 

        public string Email { get; set; }    

        public CharacterStats Characteristics { get; set; }

        public IEnumerable<PlayerAchievement> PlayerAchievements {get;set;}
    }
}
