// src/Domain/Entities/BaseUnit.cs

using Lilia.Domain.Interfaces;

namespace Lilia.Domain.Entities
{
    public abstract class BaseUnit : IForceElement
    {
        public string Name { get; private set; }
        public Guid InternalID { get; private set; } = Guid.NewGuid();
        public abstract string Prefix { get; }
        public int Price { get; private set; }
        public int Modslots { get; private set; }
        public string RegistryNumber { get; private set;}
        public string DisplayTag => $"{Name} ({Prefix}-{RegistryNumber})";

        public BaseUnit(string name, int price, int modslots, string registryNumber)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if(price < 0) throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
            if(modslots < 0) throw new ArgumentOutOfRangeException(nameof(modslots), "Modslots cannot be negative.");
            if(string.IsNullOrWhiteSpace(registryNumber)) throw new ArgumentException("Registry number cannot be null or empty.", nameof(registryNumber));
            Name = name;
            Price = price;
            Modslots = modslots;
            RegistryNumber = registryNumber;
        }

        public int Total()
        {
            return Price;
        }

        public abstract void Display(int indentLevel);

        public void DisplaySimple(int indentLevel)
        {
            string indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}- {DisplayTag}");
        }
    }
}