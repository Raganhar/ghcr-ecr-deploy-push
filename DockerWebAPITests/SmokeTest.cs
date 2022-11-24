using DockerWebAPI.Pulumi;
using DockerWebAPI.Pulumi.InfrastructureTemplates;
using NUnit.Framework;

namespace DockerWebAPITests;

public class SmokeTest
{
    [Test]
    public void Smoke([Values]CpuSize cpus)
    {
        Console.WriteLine(cpus.ToCpuUnits());
    }
}