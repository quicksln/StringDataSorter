using BenchmarkDotNet.Attributes;
using HPCsharp;
using StringDataSorter.Benchmark.Settings;
using StringDataSorter.Core.Comparer;
using StringDataSorter.Core.Helpers;
using System;
using System.Linq;
using StringDataSorter.Core;

namespace StringDataSorter.Benchmark
{
    [MemoryDiagnoser] 
    public class StringDataSorterBenchmarks
    {
        private string[] dataNotSorted = null!;

        [GlobalSetup]
        public void Setup()
        {
            var fileDataManager = new FileDataManager();
            var settings = ApplicationConfiguration.GetSettings();
            dataNotSorted = fileDataManager.ReadDataAsync(settings.FilePath!).GetAwaiter().GetResult();
        }

        [Benchmark]
        public string[] SortMergeParallel()
        {
            // Parallel Merge Sort
            return dataNotSorted.SortMergePar(comparer: new StringDataComparer());
        }

        [Benchmark]
        public string[] SortMergeSingleThread()
        {
            // Single-threaded Merge Sort
            return dataNotSorted.SortMerge(comparer: new StringDataComparer());
        }

        [Benchmark]
        public List<string> SortMergePseudoInPlacePar()
        {
            return dataNotSorted.ToList().SortMergePseudoInPlacePar(comparer: new StringDataComparer());
        }

        [Benchmark]
        public string[] SortAsParallelOrderBy()
        {
            return dataNotSorted.AsParallel().AsUnordered().OrderBy(x => x, new StringDataComparer()).ToArray();
        }
    }
}