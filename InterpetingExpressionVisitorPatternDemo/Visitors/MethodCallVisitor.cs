using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace InterpetingExpressionVisitorPatternDemo.Visitors
{
    public class MethodCallVisitor : Visitor
    {
        private readonly MethodCallExpression node;
        public MethodCallVisitor(MethodCallExpression node)
            : base(node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {
            Console.WriteLine(this.ToString());
            if (this.node.Object == null)
            {
                Console.WriteLine($"{prefix}This is static method call");
            }
            else
            {
                Console.WriteLine($"{prefix}The receiver (this) is");
                var receiverVisitor = CreateFromExpression(this.node.Object);
                receiverVisitor.Visit(prefix + "\t");
            }

            var methodInfo = this.node.Method;
            Console.WriteLine($"{prefix} The method name is {methodInfo.DeclaringType}.{methodInfo.Name}");
            Console.WriteLine($"{prefix}The Arguments are:");
            foreach (var arg in node.Arguments)
            {
                var argVisitor = Visitor.CreateFromExpression(arg);
                argVisitor.Visit(prefix + "\t");
            }
        }
    }
}
