using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo.Visitors
{
    public class LambdaVisitor : Visitor
    {
        private readonly LambdaExpression node;

        public LambdaVisitor(LambdaExpression node)
            : base(node)
        {
            this.node = node;
        }

        public override void Visit(string prefix)
        {
            var nodeName = ((this.node.Name == null) ? "<null>" : this.node.Name);
            var returnType = this.node.ReturnType.ToString();
            var parameters = this.node.Parameters;

            Console.WriteLine($"{prefix}{this.ToString()}");
            Console.WriteLine($"{prefix}The name of the lambda is {nodeName}");
            Console.WriteLine($"{prefix}The return type is {returnType}");
            Console.WriteLine($"{prefix}The expression has {parameters.Count} arguments. They are:");

            foreach (var argumentExpression in parameters)
            {
                var argumentVisitor = CreateFromExpression(argumentExpression);
                argumentVisitor.Visit(prefix + "\t");
            }

            Console.WriteLine($"{prefix}The expression body is:");
            //Visit the body
            var bodyVisitor = CreateFromExpression(this.node.Body);
            bodyVisitor.Visit(prefix + "\t");
        }
    }
}
