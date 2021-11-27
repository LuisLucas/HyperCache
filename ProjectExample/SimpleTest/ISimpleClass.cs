namespace ProjectExample.SimpleTest
{
    using System.Threading.Tasks;

    public interface ISimpleClass
    {
        void VoidMethod();

        int IntReturnMethod();

        int IntReturnMethod(int param1);

        Task<string> StringReturnMethod(int param1);

        int IntReturnMethod(int param1, string param2);

        int IntReturnMethod(int? param1, string param2);

        int IntReturnMethod(string param1, int? param2);
    }
}
