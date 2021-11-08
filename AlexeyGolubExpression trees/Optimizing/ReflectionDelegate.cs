using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AlexeyGolubExpression_trees.Optimizing
{
    public static class ReflectionDelegate
    //public class ReflectionDelegate
    {
        private static MethodInfo ExecuteMethod { get; } =
            typeof(Command)
                .GetMethod(
                "Execute", //string Execute is hardcore
                BindingFlags.NonPublic | BindingFlags.Instance);

        private static Func<Command, int> Impl { get; } =
            (Func<Command, int>)Delegate
            .CreateDelegate(typeof(Func<Command, int>), ExecuteMethod);

        public static int ReflectionDelegateCallExecute(Command command) => Impl(command);
    }
}
