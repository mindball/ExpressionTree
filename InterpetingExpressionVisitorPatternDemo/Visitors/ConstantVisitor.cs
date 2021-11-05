using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo.Visitors
{
    public class ConstantVisitor : Visitor
    {
        private readonly ConstantExpression node;
        public ConstantVisitor(ConstantExpression node)
            : base (node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {
            Console.WriteLine($"{prefix}{this.ToString()}");
            Console.WriteLine($"{prefix}The type of the constant value is {this.node.Type}");
            Console.WriteLine($"{prefix}The value of the constant value is {this.node.Value}");
        }
    }
}
