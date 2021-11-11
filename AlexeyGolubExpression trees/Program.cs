using AlexeyGolubExpression_trees.Generic;
using AlexeyGolubExpression_trees.IdentTypeMembers;
using AlexeyGolubExpression_trees.Optimizing;
using AlexeyGolubExpression_trees.ParsingDomainSpecificLanguageIntoExpressions;
using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;
using System.Diagnostics;
using System.Reflection;

namespace AlexeyGolubExpression_trees
{
    public class Program
    {
        //https://youtu.be/US_3kUD5j2w
        public static void Main(string[] args)
        {
            //1.
            //BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());

            //2.
            //IdentifyingTypeMembers obj = new IdentifyingTypeMembers();
            //obj.field.PropertyInfoOfDto();

            //3.
            //Parsing DSLs into expressions
            var result = SimpleCalculator.Run("15 + 15");
            Console.WriteLine(result);
            result = SimpleCalculator.Run("15.5 + 15.99");
            Console.WriteLine(result);
        }
    }

    public class OptimizingReflectionHeavyCode
    {
        [Benchmark(Description = "Reflection", Baseline = true)]
        public int Reflection() => (int)
            typeof(Command)
                .GetMethod(
                "Execute", //string Execute is hardcore
                BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(new Command(), null);

        [Benchmark(Description = "Reflection (cached)")]
        public int Cached() => ReflectionCached.ReflectionCallExecute(new Command());

        [Benchmark(Description = "Reflection (delegate)")]
        public int Delegates() => ReflectionDelegate.ReflectionDelegateCallExecute(new Command());

        [Benchmark(Description = "Expression")]
        //[Benchmark]
        public int Expressions() => EpxressionTrees.CallExecute(new Command());
    }

    public class ImplementGenericCode
    {
        //static
        [Benchmark(Description = "Static", Baseline = true)]
        [Arguments(13.37)]
        public double Statics(double x) => 3 * x / 4;

        //Dynamic
        [Benchmark(Description = "Dynamic")]
        [Arguments(13.37)]
        public dynamic Dynamics(dynamic x) => 3 * x / 4;

        //Expression
        [Benchmark(Description = "Expression")]
        [Arguments(13.37)]
        public double Expressions(double x) => ThreeFourthsThreadSafty<double>.Of(x);
    }

    public class IdentifyingTypeMembers
    {
        public GetWithReflection field { get; } = new GetWithReflection();
    }

    
}
