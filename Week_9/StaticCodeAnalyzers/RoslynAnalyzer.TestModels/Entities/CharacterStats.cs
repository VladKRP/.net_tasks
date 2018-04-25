using System.Runtime.Serialization;

namespace RoslynAnalyzer.TestModels.Entities
{
    [DataContract]
    public class CharacterStats
    {
        public int Id { get; set; }

        public int AttackRate { get; set; }

        public int AttackPower { get; set; }

        public int MovingRate { get; set; }

        public int HealthPoint { get; set; }

        public int? ManaPoint { get; set; }
    }
}