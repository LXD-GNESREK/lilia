namespace Lilia.Domain.Entities
{
    public class UnitCount
    {
        public Dictionary<BaseUnit, int> Units { get; private set; }
        public UnitCount()
        {
            Units = new Dictionary<BaseUnit, int>();
        }

        public void AddUnit(BaseUnit unit, int count)
        {
            if (Units.ContainsKey(unit))
            {
                Units[unit] += count;
            }
            else
            {
                Units[unit] = count;
            }
        }
    }
}