using InterpetingExpressionVisitorPatternDemo.Visitors;
using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int, int>> exp = (a, b) => a + b;
            var v = Visitor.CreateFromExpression(exp);
            v.Visit("-");

            Console.WriteLine(new string('-', 50));

            Expression<Func<int>>  exp2 = () => 1 + 2 + 3 + 4;
            v = Visitor.CreateFromExpression(exp2);
            v.Visit("-");

            Console.WriteLine(new string('-', 50));

            Expression<Func<int, int>> sum = (a) => 1 + a + 3 + 4;
            v = Visitor.CreateFromExpression(exp2);
            v.Visit("-");
        }
    }
}
