using DockerWebAPI.Pulumi.InfrastructureTemplates;
using Pulumi;

namespace DockerWebAPI.Pulumi;

class Program
{
    static Task<int> Main() => Deployment.RunAsync<CrosswalkPulumiEcs>();
}