// src/Domain/Interfaces/IForceElement.cs

namespace Lilia.Domain.Interfaces
{
    public interface IForceElement
    {
        string Name { get; }
        long Total();
        void Display(int indentLevel);
        void DisplaySimple(int indentLevel);
    }
}