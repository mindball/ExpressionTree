using ExampleClasses;
using System.Reflection;
using System;

namespace Dynamic
{
    class Program
    {
        static void Main(string[] args)
        {
            //Неправилен подход
            //var propertyName = typeof(Cat).GetProperty("SomeHidenProperty",
            //    BindingFlags.NonPublic | BindingFlags.Instance);

            //var value = propertyName.GetValue(new Cat());

            //Console.WriteLine(value);
            //;

            //правилен подход
            //Usefull
            //dynamic someDynamic = new ExposedObject();
            //someDynamic.NotExistMethod();
            //someDynamic.KakuvtoSiIskashMethod();
            //someDynamic.HiddenMethod(); // work
            //someDynamic.HiddenMethod1(); // compile time error


            //Intelisensе нямаме но си работи
            //dynamic someDynamicExampleTwo = new ExposedObject(new Cat());            
            //Console.WriteLine(someDynamicExampleTwo.SomeHidenProperty);

            //better variant with extensions
            var newCat = new Cat();
            dynamic someCatExposed = newCat.Exposed();
            Console.WriteLine(someCatExposed.SomeHidenProperty);
        }
    }

    public static class DynamicExtensions
    {
        public static ExposedObject Exposed(this object obj)
            => new ExposedObject(obj);
    }
}
