// src/Domain/Entities/Organisation.cs

using Lilia.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Lilia.Domain.Entities
{
    public partial class Organisation : IForceElement
    {
        public string Name { get; private set; }
        public string ShortCode { get; private set; }
        public Guid InternalID { get; private set; } = Guid.NewGuid();
        public ulong? PlayerDiscordID { get; private set; }
        public Player? Player { get; private set; }
        public Guid? ParentOrganisationID { get; private set; }
        public Organisation? ParentOrganisation { get; private set; }
        private readonly List<Organisation> _subOrganisations = new();
        public IReadOnlyCollection<Organisation> SubOrganisations => _subOrganisations.AsReadOnly();
        private readonly List<BaseUnit> _assignedUnits = new();
        public IReadOnlyCollection<BaseUnit> AssignedUnits => _assignedUnits.AsReadOnly();
        public IReadOnlyList<IForceElement> Elements => _subOrganisations.Cast<IForceElement>().Concat(_assignedUnits).ToList().AsReadOnly();


        [GeneratedRegex("^[a-zA-Z0-9_-]+$")]
        private static partial Regex AlphanumericRegex();

        public Organisation(string name, string shortCode)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Organisation name cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(shortCode)) throw new ArgumentException("ShortCode cannot be null or empty.", nameof(shortCode));
            if (!AlphanumericRegex().IsMatch(shortCode)) throw new ArgumentException("ShortCode must be alphanumeric and contain no spaces.", nameof(shortCode));

            Name = name;
            ShortCode = shortCode.ToUpperInvariant();
        }

        internal void SetPlayer(Player? player)
        {
            PlayerDiscordID = player?.DiscordID;
            Player = player;
        }

        internal void SetParent(Organisation? parent)
        {
            ParentOrganisationID = parent?.InternalID;
            ParentOrganisation = parent;
        }

        public void Add(IForceElement element)
        {
            if(element == null) throw new ArgumentNullException(nameof(element));
            if(element == this) throw new InvalidOperationException("An organisation cannot add itself as an element.");

            if(element is Organisation org)
            {
                org.SetParent(this);
                _subOrganisations.Add(org);
            }
            else if(element is BaseUnit unit)
            {
                unit.SetOrganisation(this);
                _assignedUnits.Add(unit);
            }
        }

        public void Remove(IForceElement element)
        {
            if(element == null) throw new ArgumentNullException(nameof(element));

            if(element is Organisation org)
            {
                org.SetParent(null);
                _subOrganisations.Remove(org);
            }
            else if(element is BaseUnit unit)
            {
                unit.SetOrganisation(null!);
                _assignedUnits.Remove(unit);
            }
        }

        public int Total()
        {
            return Elements.Sum(e => e.Total());
        }

        public void Display(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}[+] {Name} [{ShortCode}] (Total: {Total()})");

            foreach(var element in Elements)
            {
                element.Display(indentLevel + 1);
            }
        }

        public int CountUnitsByName(string unitName)
        {
            int count = _assignedUnits.Count(u => u.Name.Equals(unitName, StringComparison.OrdinalIgnoreCase));
            count += _subOrganisations.Sum(org => org.CountUnitsByName(unitName));
            return count;
        }

        public void DisplaySimple(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}[+] {Name} [{ShortCode}] (Total: {Total()})");

            this.Elements.GroupBy(e => e.Name)
                        .OrderBy(g => g.Key)
                        .ToList()
                        .ForEach(g =>
                        {
                            var element = g.First();
                            if (g.Count() > 1)
                            {
                                if(element is BaseUnit unit)
                                {
                                    Console.WriteLine($"{indent}    - {g.Count()} {g.Key}");
                                }
                                else
                                {
                                    Console.WriteLine($"{indent}    - {g.Count()} {g.Key} ({g.First().Total()})");
                                }
                            }
                            else
                            {
                                element.DisplaySimple(indentLevel + 1);
                            }
                        });
        }
    }
}