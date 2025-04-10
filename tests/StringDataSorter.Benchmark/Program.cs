using BenchmarkDotNet.Running;
using StringDataSorter.Benchmark;
using StringDataSorter.Benchmark.Settings;
using StringDataSorter.Core.Helpers;
using System;
using System.Linq;
using StringDataSorter.Core;

var fileDataManager  = new FileDataManager();
var dataSampler = DataSampler.Get();
var settings = ApplicationConfiguration.GetSettings();

Task.Run(async () => 
{
    // Create a file with 100 MB of data
    await fileDataManager.CreateDataAsync(dataSampler, settings.FilePath!, 10); 

}).Wait();

// Run the benchmarks
var summary = BenchmarkRunner.Run<StringDataSorterBenchmarks>();