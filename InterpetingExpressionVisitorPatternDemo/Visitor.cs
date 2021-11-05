using InterpetingExpressionVisitorPatternDemo.Visitors;
using System;
using System.Linq.Expressions;

namespace InterpetingExpressionVisitorPatternDemo
{
    public abstract class Visitor
    {
        private readonly Expression node;

        protected Visitor(Expression node)
        {
            this.node = node;
        }

        public abstract void Visit(string prefix);

        public ExpressionType NodeType 
            => this.node.NodeType;

        public static Visitor CreateFromExpression(Expression node)
        {
            switch(node.NodeType)
            {
                case ExpressionType.Constant:
                    return new ConstantVisitor((ConstantExpression)node);
                case ExpressionType.Parameter:
                    return new ParameterVisitor((ParameterExpression)node);
                case ExpressionType.Lambda:
                    return new LambdaVisitor((LambdaExpression)node);
                case ExpressionType.Add:
                    return new BinaryVisitor((BinaryExpression)node);
                default:
                    Console.Error.WriteLine($"Node not processed yet: {node.NodeType}");
                    return default(Visitor);
            }
        }

        public override string ToString()
        {
            string expressionType = $"This is an {this.NodeType} expression type";
            return expressionType;
        }
    }
}
