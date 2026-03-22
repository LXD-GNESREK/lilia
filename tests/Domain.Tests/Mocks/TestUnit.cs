// tests/Domain.Tests/Mocks/TestUnit.cs

using Lilia.Domain.Entities;

namespace Lilia.Domain.Tests.Mocks
{
    public class TestUnit : BaseUnit
    {
        public override string Prefix => "TST";

        public TestUnit(string name, int price, int modslots, string registryNumber) : base(name, price, modslots, registryNumber)
        {
            name = Name;
            price = Price;
            modslots = Modslots;
            registryNumber = RegistryNumber;
        }

        public override void Display(int indentLevel)
        {
            Console.WriteLine($"{new string(' ', indentLevel * 2)}{DisplayTag} - Price: {Price}, Modslots: {Modslots}");
        }
    }
}