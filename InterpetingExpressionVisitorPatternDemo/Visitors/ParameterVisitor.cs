using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo.Visitors
{
    public class ParameterVisitor : Visitor
    {
        private readonly ParameterExpression node;

        public ParameterVisitor(ParameterExpression node)
            : base(node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {
            Console.WriteLine($"{prefix}This is an {this.NodeType} expression type");
            Console.WriteLine($"{prefix}Type: " +
                $"{this.node.Type.ToString()}, " +
                $"Name: {this.node.Name}, " +
                $"ByRef: {this.node.IsByRef}");
        }
    }
}
