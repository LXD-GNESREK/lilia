namespace Lilia.Domain.Entities
{
    public class Shop
    {
        public Guid ID { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public ICollection<ShopRegistryUnit> AvailableUnits { get; private set; } = new List<ShopRegistryUnit>();

        private Shop() {}

        public Shop(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Shop name cannot be null or empty.", nameof(name));
            Name = name;
        }
    }
}