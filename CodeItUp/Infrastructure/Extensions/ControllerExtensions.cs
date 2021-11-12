using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CodeItUp.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult RedirectTo<TController>(
            this Controller controller,
            Expression<Action<TController>> redirectExpression)
        {
            if (redirectExpression.Body.NodeType != ExpressionType.Call)
            {
                throw new InvalidOperationException("The provided expression is not valid");
            }

            var methodCallExpression = (MethodCallExpression)redirectExpression.Body;

            var actionName = GetActionName(methodCallExpression);
            var controllerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty);

            var routeValue = ExtractRouteValues(methodCallExpression);

            return controller.RedirectToAction(actionName, controllerName, routeValue);
        }

        private static RouteValueDictionary ExtractRouteValues(MethodCallExpression expression)
        {
            //["id", "query"]
            var names = expression.Method.GetParameters()
                .Select(pi => pi.Name)
                .ToArray();

            var arguments = expression.Arguments
                .Select(arg =>
                {
                    if (arg.NodeType == ExpressionType.Constant)
                    {
                        var constantExpression = (ConstantExpression)arg;
                        return constantExpression;
                    }
                    if (arg.NodeType == ExpressionType.MemberAccess
                        && ((MemberExpression)arg).Member is FieldInfo)
                    {
                        var memberAccessExpr = (MemberExpression)arg;
                        return GetArgument(memberAccessExpr);
                    }    

                        return null;
                })
                .ToArray();          

            var routeValueDictionary = new RouteValueDictionary();

            for (int i = 0; i < names.Length; i++)
            {
                routeValueDictionary.Add(names[i], arguments[i]);
            }

            return routeValueDictionary;
        }

        private static object GetArgument(MemberExpression expression)
        {
            // Expression of type c => c.Action(id)
                // Value can be extracted without compiling.           

            var constantExpression = (ConstantExpression)expression.Expression;
            if (constantExpression == null)
            {
                return null;                
            }

            var innerMemberName = expression.Member.Name;
            var compiledLambdaScopeField = constantExpression.Value.GetType().GetField(innerMemberName);
            return compiledLambdaScopeField.GetValue(constantExpression.Value);
        }

        private static string GetActionName(MethodCallExpression expression)
        {
            var methodName = expression.Method.Name;
            if (expression == null)
            {
                throw new ArgumentException("Not a " + nameof(MethodCallExpression));
            }

            var actionname = expression.Method
                .GetCustomAttribute<ActionNameAttribute>()? //? If ActionName not set
                .Name;

            return actionname ?? methodName;
        }
    }
}
