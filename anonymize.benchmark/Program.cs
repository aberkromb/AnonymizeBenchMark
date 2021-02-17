using System;
using System.IO;
using Anonymize.Benchmark.Models;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Anonymize.Benchmark
{
    public class Program
    {
        public static AnonymizePattern AnonymizePattern = new AnonymizePattern
        {
            Params = new [] {"Type2", "Type3", "Type4"}
        };
        
        [MemoryDiagnoser]
        public class AnonymizeBenchmark
        {
            private IReplace _recursiveReplace;
            private IReplace _regexReplace;
            
            private string _jsonString;
            
            public AnonymizeBenchmark()
            {
                _jsonString = File.ReadAllText("Resources/JsonSample.json");
            
                _recursiveReplace = new RecursiveReplace();
                _regexReplace = new RegexReplace();
            }

            [Benchmark]
            public string RecursiveAnonymize() => _recursiveReplace.ReplaceJsonForbiddenVariables(_jsonString);
            
            [Benchmark]
            public void RegexReplace() => _regexReplace.ReplaceJsonForbiddenVariables(_jsonString);
        }
        
        public static void Main(string[] args)
        { 
            var jsonString = File.ReadAllText("Resources/JsonSample.json");
            var recursiveReplace = new RecursiveReplace();
            var regexReplace = new RegexReplace();

            var recursiveRepalceResult = recursiveReplace.ReplaceJsonForbiddenVariables(jsonString);
            Console.WriteLine(recursiveRepalceResult);


            var regexReplaceResult = regexReplace.ReplaceJsonForbiddenVariables(jsonString);
            Console.WriteLine(regexReplaceResult);
            
            
            // var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}