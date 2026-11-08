// src/Domain/Entities/RegistryStats/WeaponMount.cs

using Lilia.Domain.Enums;

namespace Lilia.Domain.Entities.RegistryStats
{
    public class WeaponMount
    {
        public int Quantity { get; set; }
        public WeaponType WeaponType { get; set; }
        public string? Configuration { get; set; }
    }
}