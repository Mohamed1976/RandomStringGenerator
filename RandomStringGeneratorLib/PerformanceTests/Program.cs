// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using PerformanceTests;

Console.WriteLine("Running Benchmarks.");

Summary summary1 = BenchmarkRunner.Run<RandomNumberGeneratorsBenchmarks>();
//Summary summary2 = BenchmarkRunner.Run<RandomStringGeneratorsBenchmarks>();

Console.ReadLine();