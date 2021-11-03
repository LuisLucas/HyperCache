namespace MyApp
{
    public interface IClassToCache
    {
        string GetSomething(int anInt);

        string GetSomethingWithMoreParams(int anInt, string aString);

        bool GetSomethingWitEvenMoreParams(int anInt, string aString, bool itsABool);
    }
}
