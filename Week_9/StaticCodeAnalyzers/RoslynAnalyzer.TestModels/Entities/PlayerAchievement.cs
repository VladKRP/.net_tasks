using System;
using System.Runtime.Serialization;

namespace RoslynAnalyzer.TestModels.Entities
{
    [DataContract]
    public class PlayerAchievement
    {
        public int Id { get; set; }

        public string Name {get;set;}

        public int ReputationPoints { get; set; }
    }
}
