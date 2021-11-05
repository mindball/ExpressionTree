using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo.Visitors
{
    public class BinaryVisitor : Visitor
    {
        private readonly BinaryExpression node;

        public BinaryVisitor(BinaryExpression node)
            : base(node)
        {
            this.node = node;
        }
        public override void Visit(string prefix)
        {
            Console.WriteLine($"{prefix}{this.ToString()}");

            var left = CreateFromExpression(this.node.Left);           
            Console.WriteLine($"{prefix}The Left argument is:");
            left.Visit(prefix + "\t");

            var right = CreateFromExpression(this.node.Right);
            Console.WriteLine($"{prefix}The Right argument is:");
            right.Visit(prefix + "\t");
        }
    }
}
