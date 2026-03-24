// src/Domain/Entities/Organisation.cs

using Lilia.Domain.Interfaces;

namespace Lilia.Domain.Entities
{
    public class Organisation : IForceElement
    {
        public string Name { get; private set; }
        public Guid InternalID { get; private set; } = Guid.NewGuid();
        private readonly List<IForceElement> _elements = new List<IForceElement>();
        public IReadOnlyList<IForceElement> Elements => _elements.AsReadOnly();

        public Organisation(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Organisation name cannot be null or empty.", nameof(name));
            Name = name;
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
            Console.WriteLine($"{indent}[+] {Name} (Total: {Total()})");

            foreach(var element in _elements)
            {
                element.Display(indentLevel + 1);
            }
        }

        public void DisplaySimple(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}[+] {Name} (Total: {Total()})");

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