using ExampleClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CreatingExpression
{
    class Program
    {
        static void Main(string[] args)
        {
            //Simple lambda () => 42
            //SimpleExample();

            //() => new Cat();
            //create object with empty constructor            
            //CreateObject();

            //cat => cat.SayMew(42)
            //Need it: constant, method call, parameter, lambda expression
            //CallMethodWithConstant();

            //MVC Style fast property getters ASp.net
            //RedirectToAction("Index", new {id = 5, query = "Test"});
            //Това генерира речник с key=id, value=5, key=query, value="Test"
            //
            var obj = new { id = 5, query = "Test" };
            var cat = new Cat { Age = 3, Name = "Pesho"};
            //MakeWithReflection(obj);
            MakeWithExpression(obj);            
            MakeWithExpression(cat);           

        }

        private static void MakeWithExpression(object obj)
        {

            var a = PropertyHelper.Get(obj);
            var dict = new Dictionary<string, object>();
            PropertyHelper
                .Get(obj)
                .Select(pr => new
                {
                    Name = pr.Name,
                    //bottleneck
                    Value = pr.Getter(obj)
                })
                .ToList()
                .ForEach(pr =>
                {
                    dict[pr.Name] = pr.Value;
                    ;
                });

        }

        public  static void MakeWithReflection(object obj)
        {            
            var dict = new Dictionary<string, object>();

            obj.GetType()
                .GetProperties()
                .Select(o => new
                {
                    Name = o.Name,
                    Value = o.GetValue(obj)
                })
                .ToList()
                .ForEach(pr =>
                {
                    dict[pr.Name] = pr.Value;
                });
            ;
        }

        public static void CallMethodWithConstant()
        {
            //(a)cat => (b)cat.(c)SayMew((d)42)
            var typeCat = typeof(Cat);
            //1. const - (d)
            var constant = Expression.Constant(42);

            //2 object (a)(b)
            var parameter = Expression.Parameter(typeCat, "cat");

            //3. Method info (c)
            var methodInfo = typeCat.GetMethod(nameof(Cat.Maw));

            //4. Call (b) (c) (d)
            var call = Expression.Call(parameter, methodInfo, constant);

            //5. (a)(b)(c)(d)
            //Expression.Lambda<Func<Cat, int>>(call, parameter);
            var lambda = Expression.Lambda<Func<Cat, int>>(call, parameter);

            var func = lambda.Compile();
            var result = func(new Cat());

            Console.WriteLine(result);

            //((a)cat, (b)number) => (c)cat.(d)SayMew((e)number)
            //Expression<Func<cat, number, int>> func = (cat, number) => cat.SayMaw(number);

            // (cat, number) (a)(b)
            var catParam = Expression.Parameter(typeCat, "cat");
            var numberParam = Expression.Parameter(typeof(int), "number");

            //3. Method info (d)
            var methodTwo = typeCat.GetMethod(nameof(Cat.Maw));

            //4. Call (b) (c) (d)
            var callTwo = Expression.Call(catParam, methodInfo, numberParam);

            //5. (a)(b)(c)(d)
            //Expression.Lambda<Func<Cat, int, int>>(call, parameter);
            var lambdaTwo = Expression.Lambda<Func<Cat, int, int>>(callTwo, catParam, numberParam);

            var funcTwo = lambdaTwo.Compile();
            int number = 100;
            var resultTwo = funcTwo(new Cat(), number);

            Console.WriteLine(resultTwo);

        }

        public static void CreateObject()
        {
            //Expression-ните са по бързо от Activator.CreateInstance
            //https://vagifabilov.wordpress.com/2010/04/02/dont-use-activator-createinstance-or-constructorinfo-invoke-use-compiled-lambda-expressions/
            var newExpression = Expression.New(typeof(Cat));
            var lambdaExp = Expression.Lambda<Func<Cat>>(newExpression);

            var func = lambdaExp.Compile();
            var newCat = func();

            Console.WriteLine(newCat.GetType().Name);

            //хитър начин за object creating работи с default-тен конструктор, 
            //ако искаме обекти с non-default конструктор да използваме SuperFast 
            var funcOfCat = New<Cat>.Instance;
            var funcOfOwner = New<Owner>.Instance;

           

            var newCat2 = funcOfCat();
            var newOwner = funcOfOwner();
            newOwner.FullName = "Pesho";
            
            Console.WriteLine(newCat2.GetType().Name);
            Console.WriteLine(newOwner.FullName);
        }

        public static void SimpleExample()
        {
            //1. create a constant with value 42
            var constant = Expression.Constant(42);

            //2. create lambda expression without parameters
            var lambdaExp = Expression.Lambda<Func<int>>(constant);

            //3. Тук е bottleneck на програмата иначе другите 1, 2, 4 са бързи
            //Даже извикването на ф-я е бързо равнозначно на: да създадем ф-я и да я извикаме 5.
            var func = lambdaExp.Compile();

            //4.
            Console.WriteLine(func());
            Console.WriteLine(constant.Value);
        }

        //5
        public static int GetInt(int number)
        {
            return number;
        }
    }

    public class New<T>
        where T : class
    {
        /*
         * Статичните полета се excecute-ват веднъж когато ги достъпим за първи път
         * и всеки следващ път те са кеширани и са готови.
         * Или казано по друг начин: първият път след като достъпим Instance
         * static field/property ще се компилира и следващият път след като го 
         * достъпваме ще имам готова ф-я. Също така ако ни трябва за малки неща 
         * това е излишно но ако ни трябва за супер супер много обекти това е идеално 
         */
        public static Func<T> Instance
            = Expression.Lambda<Func<T>>(Expression.New(typeof(T))).Compile();
    }
}
