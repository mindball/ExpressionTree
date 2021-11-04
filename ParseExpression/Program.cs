using ExampleClasses;
using System;
using System.Linq.Expressions;

namespace ParseExpression
{
    class Program
    {
        static void Main(string[] args)
        {

            var parse = new ParseExpression();

            //Example 1,2
            //Expression<Func<int>> expression = () => 42;
            //parse.Parssing(expression, string.Empty);

            //Console.WriteLine();

            //Hidden cast
            /*Тук дървото има повече children.  Или по конкретно body-то 
                има повече children(object cast and constant). При горния случай
                в body-то имахме само constant.
            */
            //Expression<Func<object>> hiddenCast = () => 42;
            //parse.Parssing(hiddenCast, string.Empty);

            //Console.WriteLine();

            //Method call  - Instance or Static
            //print different becaus new Cat in lambda, expression не го разбира още не сме го прихванали
            //Expression<Func<Cat, int>> expression = cat => cat.Maw(42);

            //Expression<Func<Cat, int>> expression2 = c => new Cat().Maw(42);
            //parse.Parssing(expression2, string.Empty);

            //Console.WriteLine();

            //Variable extract variable value
            int someInteger = 42;
            Expression<Func<Cat, int>> expression2 = cat => cat.Maw(someInteger);
            ////test when is constant and variable
            ////Expression<Func<Cat, int>> expression2 = cat => cat.Maw(42);
            parse.Parssing(expression2, string.Empty);

            //Console.WriteLine();

            //Member
            //int someInteger = 42;
            //Expression<Func<Cat, int>> property = cat => cat.Maw(someInteger);
            //parse.Parssing(property, string.Empty);

            //Operator expression (Binary) 
            //Expression<Func<int, int, int>> sum = (x, y) => x + y;
            //Expression<Func<int, int, int>> sumWithConstant = (x, y) => x + 55;
            //Expression<Func<int, int, int>> twoExpression = (x, y) => x + y + 55;
            //Expression<Func<int, int, int>> sub = (x, y) => x - y;
            //parse.Parssing(sum, string.Empty);
            //parse.Parssing(sumWithConstant, string.Empty);
            //parse.Parssing(twoExpression, string.Empty);
            //parse.Parssing(sub, string.Empty);

            //Console.WriteLine();

            //Object with default and normal constructor
            //Expression<Func<string, string, Cat>> cat =
            //    (catName, ownerName) => new Cat(catName);
            //Expression<Func<string, string, Cat>> cat2 =
            //   (catName, ownerName) => new Cat(new string('s', 42));
            //parse.Parssing(cat, string.Empty);
            //Console.WriteLine();
            //parse.Parssing(cat2, string.Empty);

            //Console.WriteLine();

            //Member initialization
            //Expression<Func<string, string, Cat>> catWithOwner =
            //   (catName, ownerName) => new Cat(catName)
            //   {
            //       Owner = new Owner
            //       {
            //           FullName = "Bob"
            //       }
            //   };
            //parse.Parssing(catWithOwner, string.Empty);

            //Console.WriteLine();

            //Negate
            //
            //int someInteger = 42;
            ////sign - convert to operator
            //Expression<Func<Cat, int>> negate = cat => cat.Maw(-someInteger);
            //Expression<Func<Cat, int>> negate1 = cat => cat.Maw(-42);
            //parse.Parssing(negate, string.Empty);
            //Console.WriteLine();
            //parse.Parssing(negate1, string.Empty);
        }
    }
}
