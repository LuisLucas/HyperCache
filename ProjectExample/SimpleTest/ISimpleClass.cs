namespace ProjectExample.SimpleTest
{
    public interface ISimpleClass
    {
        void VoidMethod();

        int IntReturnMethod();

        int IntReturnMethod(int param1);

        int IntReturnMethod(int param1, string param2);

        int IntReturnMethod(int? param1, string param2);

        int IntReturnMethod(string param1, int? param2);
    }
}
