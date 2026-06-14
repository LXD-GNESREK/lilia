namespace Lilia.Domain.Entities
{
    public class UnitCount
    {
        public Dictionary<string, int> Units { get; private set; }
        public UnitCount()
        {
            Units = new Dictionary<string, int>();
        }

        public void AddUnit(string unitName, int count)
        {
            if(string.IsNullOrWhiteSpace(unitName)) throw new ArgumentException("Unit name cannot be null or empty.", nameof(unitName));
            if(count < 0 && (!Units.ContainsKey(unitName) || Units[unitName] + count < 0))
            {
                throw new InvalidOperationException("Cannot reduce unit count below zero.");
            }

            if (Units.ContainsKey(unitName))
            {
                Units[unitName] += count;
            }
            else
            {
                Units[unitName] = count;
            }
        }
    }
}