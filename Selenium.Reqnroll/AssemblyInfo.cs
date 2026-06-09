using NUnit.Framework;

[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]

[assembly: Parallelizable(ParallelScope.Children)]

[assembly: LevelOfParallelism(6)]