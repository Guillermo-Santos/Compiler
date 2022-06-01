namespace Compiler.Services.Interfaces
{
    internal interface IAutomaton
    {

        char NextChar(ref int i);
        bool Recognize(string texto, int iniToken, ref int i, int noAuto);
    }
}
