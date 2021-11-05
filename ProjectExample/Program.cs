using ProjectExample.SimpleTest;

namespace ProjectExample
{
    class Program
    {
        static void Main(string[] args)
        {
            ISimpleClass simpleClass = new SimpleClass();
            simpleClass.VoidMethod();
            simpleClass.IntReturnMethod();
            simpleClass.IntReturnMethod();
        }
    }
}
