// src/Domain/Interfaces/IForceElement.cs

namespace Lilia.Domain.Interfaces
{
    public interface IForceElement
    {
        int Total();
        void Display(int indentLevel);
        void DisplaySimple(int indentLevel);
    }
}