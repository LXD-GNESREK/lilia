// src/Domain/Entities/Player.cs

namespace Lilia.Domain.Entities
{
    public class Player
    {
        public ulong DiscordID { get; private set; }
        public UnitCount UnitCount { get; private set; }

        private readonly List<Organisation> _organisations = new();
        public IReadOnlyList<Organisation> Organisations => _organisations.AsReadOnly();

        public Player(ulong discordID)
        {
            DiscordID = discordID;
            UnitCount = new UnitCount();
        }

        public void AddOrganisation(Organisation organisation)
        {
            if(organisation == null)
            {
                throw new ArgumentNullException(nameof(organisation));
            }
            _organisations.Add(organisation);
        }

        public void RemoveOrganisation(Organisation organisation)
        {
            if(organisation == null)
            {
                throw new ArgumentNullException(nameof(organisation));
            }
            _organisations.Remove(organisation);
        }

        public int GetUnassignedUnitCount(string unitName)
        {
            int totalOwned = 0;
            if(UnitCount.Units.TryGetValue(unitName, out int count))
            {
                totalOwned = count;
            }

            int totalAssigned = _organisations.Sum(org => org.CountUnitsByName(unitName));
            return totalOwned - totalAssigned;
        }
    }
}