using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTrees
{
    public class ParseExpression
    {
        public void Parssing(Expression expression, string level)
        {
            level += "-";
            //example 1
            //if(expression is ExpressionType)
            //{
                //  това е същото като долното но с повече overhead performance
                // защото сравняването на enums е по бързо.
            //}
            //example 1
            //Good practice
            if (expression.NodeType == ExpressionType.Lambda)
            {
                //cast is not secure
                //var lambda = expression as LambdaExpression;
                var lambdaExpression = (LambdaExpression)expression;
                Console.WriteLine($"{level} Extracting lambda: {lambdaExpression} value");

                //Body is another expression
                var body = lambdaExpression.Body;
                Console.WriteLine($"{level} Extracting body: {body}");
                this.Parssing(body, level);

                //в lambda expression-a освен body имаме и parameters
                Console.WriteLine($"{level} Extracting parameters");
                foreach (var parameter in lambdaExpression.Parameters)
                {
                    this.Parssing(parameter, level);
                }
            }
            //example 2
            else if (expression.NodeType == ExpressionType.Constant)
            {
                var constant = (ConstantExpression)expression;
                var value = constant.Value;
                Console.WriteLine($"{level} Extracting constant: {value} value");
            }            
            //Hidden cast
            else if(expression.NodeType == ExpressionType.Convert)
            {
                //Type на expression e неясен: in Debugginf go to
                //Immediate Windows and type: expression.GetType().Name -> result: UnaryExpression
                var unaryExpression = (UnaryExpression)expression;
                Console.WriteLine($"{level} Converting");
                this.Parssing(unaryExpression.Operand, level);                
            }
            //Method call  - Instance or Static
            else if(expression.NodeType == ExpressionType.Call)
            {
                Console.WriteLine($"{level} Call");
                var methodExpression = (MethodCallExpression)expression;

                var methodName = methodExpression.Method.Name;
                Console.WriteLine($"{level} Method name: {methodName}");

                if(methodExpression.Object == null)
                {
                    Console.WriteLine($"{level} Method is static");
                    return;
                }

                //Тествай тук всеки път е различен expression 
                this.Parssing(methodExpression.Object, level);
            }
            else if (expression.NodeType == ExpressionType.Parameter)
            {
                var parameterExpression = (ParameterExpression)expression;
                Console.WriteLine($"{level} Extracting parameter");
            }
        }
    }
}
