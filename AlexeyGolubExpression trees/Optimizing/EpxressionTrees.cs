using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AlexeyGolubExpression_trees.Optimizing
{
    public static class EpxressionTrees
    {
        private static Type TypeOfCommand = typeof(Command);

        private static MethodInfo ExecuteMethod { get; } = TypeOfCommand
            .GetMethod("Execute", BindingFlags.NonPublic | BindingFlags.Instance);

        private static Func<Command, int> Impl { get; }

        static EpxressionTrees()
        {
            //Lazy thread-safe
            var instance = Expression.Parameter(TypeOfCommand);
            var call = Expression.Call(instance, ExecuteMethod);

            Impl = Expression.Lambda<Func<Command, int>>(call, instance).Compile();
        }

        public static int CallExecute(Command command) => Impl(command);
    }
}
