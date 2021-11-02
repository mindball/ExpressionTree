using System;
using System.Linq.Expressions;

namespace ExpressionTrees
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
            Expression<Func<Cat, int>> expression = cat => cat.Maw(42);
            parse.Parssing(expression, string.Empty);
        }
    }
}
