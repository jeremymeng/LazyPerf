using System;
using System.Diagnostics;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace LazyPerf
{
    public class LazyBenchmarks
    {
        static readonly object syncObject = new object();
        static readonly Stopwatch stopwatch = new Stopwatch();
        const int iterations = 10000000;

        [Benchmark]
        public void CreateWithLazyDefaultCtor()
        { 
            var lazyVar = new Lazy<List<int>>(); 
            var count = lazyVar.Value.Count; 
        }

        [Benchmark]
        public void CreateWithLazyFactory()
        { 
            var lazyVar = new Lazy<List<int>>(() => new List<int>()); 
            var count = lazyVar.Value.Count; 
        }

        [Benchmark]
        public void CreateWithDoubleCheckedLocking()
        {
            List<int> lazyVar = null;
            if (lazyVar == null)
            {
                lock (syncObject)
                {
                    if (lazyVar == null)
                    {
                        lazyVar = new List<int>();
                    }
                }
            }
            var count = lazyVar.Count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<LazyBenchmarks>();
        }
    }
    }
