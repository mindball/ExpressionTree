using System.Reflection;

namespace AlexeyGolubExpression_trees.Optimizing
{
    //public  static class ReflectionCached
    public  class ReflectionCached
    {
        private static MethodInfo ExecuteMethod { get; } =
            typeof(Command)
                .GetMethod(
                "Execute", //string Execute is hardcore
                BindingFlags.NonPublic | BindingFlags.Instance);

        public static int ReflectionCallExecute(Command command) =>
        //public static int ReflectionCallExecute(Command command) =>
            (int)ExecuteMethod.Invoke(command, null);
    }
}
