using NUnit.Framework;

// Run test fixtures (test classes) in parallel
[assembly: Parallelizable(ParallelScope.Fixtures)]

// Max number of tests running at the same time
[assembly: LevelOfParallelism(4)]