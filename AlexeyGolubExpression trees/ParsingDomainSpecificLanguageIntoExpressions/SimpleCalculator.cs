using Sprache;
using System;
using System.Globalization;
using System.Linq.Expressions;

namespace AlexeyGolubExpression_trees.ParsingDomainSpecificLanguageIntoExpressions
{
    public static class SimpleCalculator
    {
        private static readonly Parser<Expression> Constant =
            Parse.DecimalInvariant
                .Select(n => double.Parse(n, CultureInfo.InvariantCulture))
                .Select(n => Expression.Constant(n, typeof(double)))
                .Token();

        private static readonly Parser<ExpressionType> Operator =
            Parse.Char('+').Return(ExpressionType.Add)
                .Or(Parse.Char('-').Return(ExpressionType.Subtract))
                .Or(Parse.Char('*').Return(ExpressionType.Multiply))
                .Or(Parse.Char('/').Return(ExpressionType.Divide));

        private static readonly Parser<Expression> Operation =
            Parse.ChainOperator(Operator, Constant, Expression.MakeBinary);

        private static readonly Parser<Expression> FullExpression =
            Operation.Or(Constant).End();

        public static double Run(string expression)
        {
            var operation = FullExpression.Parse(expression);
            var func = Expression.Lambda<Func<double>>(operation).Compile();

            return func();
        }
    }
}
