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
        private readonly List<IForceElement> _elements = new List<IForceElement>();
        public IReadOnlyList<IForceElement> Elements => _elements.AsReadOnly();

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

        public void Add(IForceElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (element == this) throw new InvalidOperationException("An organisation cannot contain itself.");

            _elements.Add(element);
        }

        public void Remove(IForceElement element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            _elements.Remove(element);
        }

        public int Total()
        {
            return _elements.Sum(e => e.Total());
        }

        public void Display(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}[+] {Name} [{ShortCode}] (Total: {Total()})");

            foreach(var element in _elements)
            {
                element.Display(indentLevel + 1);
            }
        }

        public int CountUnitsByName(string unitName)
        {
            int count = 0;
            foreach (var element in _elements)
            {
                if(element is BaseUnit unit && unit.Name.Equals(unitName, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
                else if(element is Organisation subOrg)
                {
                    count += subOrg.CountUnitsByName(unitName);
                }
            }
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