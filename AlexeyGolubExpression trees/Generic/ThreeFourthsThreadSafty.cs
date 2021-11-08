using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AlexeyGolubExpression_trees.Generic
{
    //Generic, thread-safe and lazy initialization
    public static class ThreeFourthsThreadSafty<T>
    {
        private static class Impl<T>
        {
            public static Func<T, T> Of { get; }

            static Impl()
            {
                var typeOf = typeof(T);

                //(T x) => 3 * x / 4;
                //x
                var param = Expression.Parameter(typeOf);

                //3 into T model
                var threeConst = Expression.Convert(Expression.Constant(3), typeOf);
                //4 into T model
                var fourConst = Expression.Convert(Expression.Constant(4), typeOf);

                //3 * x / 4
                var body = Expression.Divide((Expression.Multiply(param, threeConst)), fourConst);

                //(x) => 3 * x / 4
                var lambda = Expression.Lambda<Func<T, T>>(body, param);

                Of = lambda.Compile();
            }
        }

        public static T Of(T x) => Impl<T>.Of(x);
    }
}
