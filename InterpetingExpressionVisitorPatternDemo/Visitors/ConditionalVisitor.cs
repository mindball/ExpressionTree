using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo.Visitors
{
    public class ConditionalVisitor : Visitor
    {
        private readonly ConditionalExpression node;

        public ConditionalVisitor(ConditionalExpression node)
            : base (node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {
            Console.WriteLine(this.ToString());
            var testVisitor = CreateFromExpression(node.Test);
            Console.WriteLine($"The Test for this expression is: ");
            testVisitor.Visit(prefix + "\t");
            var falseVisitor = CreateFromExpression(node.IfFalse);
            Console.WriteLine($"{prefix}The False clause for this expression is:");
            falseVisitor.Visit(prefix + "\t");
        }
    }
}
