using InterpetingExpressionVisitorPatternDemo.Visitors;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Expression<Func<int, int, int>> exp = (a, b) => a + b;
            //var v = Visitor.CreateFromExpression(exp);
            //v.Visit("-");


            //Console.WriteLine(new string('-', 50));

            //Expression<Func<int>>  exp2 = () => 1 + 2 + 3 + 4;
            //v = Visitor.CreateFromExpression(exp2);
            //v.Visit("-");

            //Console.WriteLine(new string('-', 50));

            //Expression<Func<int, int>> sum = (a) => 1 + a + 3 + 4;
            //v = Visitor.CreateFromExpression(exp2);
            //v.Visit("-");

            //complicated expression
            Expression<Func<int, int>> factorial = (n) =>
                    n == 0 ?
                    1 :
                    Enumerable.Range(1, n).Aggregate((product, factor) => product * factor);

            var v = Visitor.CreateFromExpression(factorial);
            v.Visit("-");

            var func = factorial.Compile();

            var fib = func(10);
        }
    }
}
