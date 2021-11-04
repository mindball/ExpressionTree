using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ParseExpression
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


                //в lambda expression-a освен body имаме и parameters                
                foreach (var parameter in lambdaExpression.Parameters)
                {
                    Console.WriteLine($"{level} Extracting parameters");
                    this.Parssing(parameter, level);
                }

                //Body is another expression
                var body = lambdaExpression.Body;
                Console.WriteLine($"{level} Extracting body: {body}");
                this.Parssing(body, level);
            }
            //example 2
            else if (expression.NodeType == ExpressionType.Constant)
            {
                var constant = (ConstantExpression)expression;
                var value = constant.Value;
                Console.WriteLine($"{level} Extracting constant: {value} value");
            }
            //Hidden cast
            else if (expression.NodeType == ExpressionType.Convert)
            {
                //Type на expression e неясен: in Debugginf go to
                //Immediate Windows and type: expression.GetType().Name -> result: UnaryExpression
                var unaryExpression = (UnaryExpression)expression;
                Console.WriteLine($"{level} Converting");
                this.Parssing(unaryExpression.Operand, level);
            }
            //Method call  - Instance or Static
            else if (expression.NodeType == ExpressionType.Call)
            {
                Console.WriteLine($"{level} Call");
                var methodExpression = (MethodCallExpression)expression;

                var methodName = methodExpression.Method.Name;
                Console.WriteLine($"{level} Method name: {methodName}");

                if (methodExpression.Object == null)
                {
                    Console.WriteLine($"{level} Method is static");
                    return;
                }

                //Тествай тук всеки път е различен expression 
                this.Parssing(methodExpression.Object, level);

                //Extracting method arguments                
                foreach (var argument in methodExpression.Arguments)
                {
                    Console.WriteLine($"{level} Extract method arguments");                    
                    this.Parssing(argument, level);
                }
            }
            else if (expression.NodeType == ExpressionType.Parameter)
            {
                var parameterExpression = (ParameterExpression)expression;
                Console.WriteLine($"{level} Extracting parameter");
                Console.WriteLine($"{level}Parameter: {parameterExpression.Name} type: {parameterExpression.Type.Name}");
            }
            else if (expression.NodeType == ExpressionType.MemberAccess)
            {
                //Това работи и за fields and properties
                var memberExpression = (MemberExpression)expression;
                Console.WriteLine($"{level} Extracting member...");

                //cat.Maw(someInteger) -> print someInteger
                Console.WriteLine($"{level}Method parameter name: ");
                Console.WriteLine($"{level}{memberExpression.Member.Name}");

                if (memberExpression.Member is PropertyInfo property)
                {
                    Console.WriteLine($"{level}Property: {property.Name} and {property.PropertyType.Name}");
                }

                if (memberExpression.Member is FieldInfo field)
                {
                    // instance of the closure class which is hidden
                    var classInstance = (ConstantExpression)memberExpression.Expression;
                    var variableValue = field.GetValue(classInstance.Value);

                    //cat.Maw(someInteger) -> print 42, someInteger = 42
                    Console.WriteLine($"{level}Variable value: {variableValue}");
                }

                this.Parssing(memberExpression.Expression, level);
            }
            else if (expression.NodeType == ExpressionType.Add
                || expression.NodeType == ExpressionType.Subtract)
            {
                Console.WriteLine($"{level} Binary operation...");
                var binaryExpression = (BinaryExpression)expression;

                //Console.WriteLine($"Left operand: {binaryExpression.Left}");
                //Console.WriteLine($"Right operand: {binaryExpression.Right}");
                //or

                this.Parssing(binaryExpression.Left, level);
                this.Parssing(binaryExpression.Right, level);
            }
            //Object with default and normal constructor
            else if (expression.NodeType == ExpressionType.New)
            {
                Console.WriteLine($"{level} object creating...");
                var newExpression = (NewExpression)expression;

                Console.WriteLine($"{level} Constructor: {newExpression.Constructor.DeclaringType.Name}");

                foreach (var argument in newExpression.Arguments)
                {
                    Console.WriteLine("Constructor arguments:");
                    this.Parssing(argument, level);
                }
            }
            //Member initialization
            else if (expression.NodeType == ExpressionType.MemberInit)
            {
                Console.WriteLine($"{level} member initializing...");
                var newExpression = (MemberInitExpression)expression;

                Console.WriteLine($"{level} member constructor");
                this.Parssing(newExpression.NewExpression, level);

                foreach (var member in newExpression.Bindings)
                {
                    Console.WriteLine($"{level}extract member");
                    Console.WriteLine($"{level}member: {member.Member.Name}");

                    var memberAssignment = (MemberAssignment)member;
                    this.Parssing(memberAssignment.Expression, level);
                }
            }
            else if (expression.NodeType == ExpressionType.Negate)
            {
                Console.WriteLine($"{level} Negate");
            }
        }
    }
}
