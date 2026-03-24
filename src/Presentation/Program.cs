// src/Presentation/Program.cs

using Lilia.Domain.Entities;
using Lilia.Domain.Entities.Units;
using Lilia.Domain.Builders;
using Lilia.Domain.Enums;

namespace Lilia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Organisation Imperial_11th_Division = new Organisation("Imperial 11th Division");
            Organisation Black_Squadron = new Organisation("Black Squadron");
            Imperial_11th_Division.Add(Black_Squadron);
            Starfighter TIE_LN = new StarfighterBuilder("TIE/LN", 60000, 0, "IMP001")
                                                                                    .WithShields(RelativeRating.VeryWeak)
                                                                                    .WithHull(RelativeRating.Weak)
                                                                                    .WithManeuverability(RelativeRating.AboveAverage)
                                                                                    .WithDimensions(6.4, 6.4, 7.5)
                                                                                    .WithSpaceSpeed(1000)
                                                                                    .WithAtmosphericSpeed(1200)
                                                                                    .WithHyperdriveRating(0.0)
                                                                                    .WithCargoCapacity(65)
                                                                                    .WithConsumables("2 days")
                                                                                    .Build();
            Starfighter TIE_SA = new StarfighterBuilder("TIE/SA", 80000, 1, "IMP002")
                                                                                    .WithShields(RelativeRating.Weak)
                                                                                    .WithHull(RelativeRating.BelowAverage)
                                                                                    .WithManeuverability(RelativeRating.Strong)
                                                                                    .WithDimensions(6.4, 6.4, 7.5)
                                                                                    .WithSpaceSpeed(1100)
                                                                                    .WithAtmosphericSpeed(1300)
                                                                                    .WithHyperdriveRating(0.0)
                                                                                    .WithCargoCapacity(65)
                                                                                    .WithConsumables("2 days")
                                                                                    .Build();
            Black_Squadron.Add(TIE_LN);
            Black_Squadron.Add(TIE_SA);
            Imperial_11th_Division.Display(0);
        }
    }
}