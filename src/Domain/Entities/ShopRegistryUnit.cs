namespace Lilia.Domain.Entities
{
    public class ShopRegistryUnit
    {
        public Guid ShopID { get; private set; }
        public Shop Shop { get; private set; } = null!;

        public Guid RegistryUnitID { get; private set; }
        public RegistryUnit RegistryUnit { get; private set; } = null!;
    }
}