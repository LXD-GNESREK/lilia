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
    }
}