using Pulumi;
using Pulumi.Awsx.Ecs.Inputs;
using Aws = Pulumi.Aws;
using Ecr = Pulumi.Awsx.Ecr;
using Ecs = Pulumi.Awsx.Ecs;
using Lb = Pulumi.Awsx.Lb;

namespace DockerWebAPI.Pulumi.InfrastructureTemplates;

class CrosswalkPulumiEcs : Stack
{
    public CrosswalkPulumiEcs()
    {
        var serviceName = nameof(CrosswalkPulumiEcs);
        var repo = new Ecr.Repository($"{serviceName}-repo");

        var image = new Ecr.Image($"{serviceName}-img", new Ecr.ImageArgs
        {
            RepositoryUrl = repo.Url,
            Path = "../",
        });

        var cluster = new Aws.Ecs.Cluster($"{serviceName}-cluster");

        var lb = new Lb.ApplicationLoadBalancer($"{serviceName}-lb");

        var service = new Ecs.FargateService($"{serviceName}-service", new Ecs.FargateServiceArgs
        {
            DesiredCount = 1,
            
            Cluster = cluster.Arn,
            TaskDefinitionArgs = new FargateServiceTaskDefinitionArgs
            {
                Container = new TaskDefinitionContainerDefinitionArgs
                {
                    Memory = CpuHelper.ContainerSize.cpu025_512.ToMemory(),
                    Cpu = CpuHelper.ContainerSize.cpu025_512.ToCpuUnits(),
                    Image = image.ImageUri,
                    Essential = true,
                    PortMappings = new TaskDefinitionPortMappingArgs
                    {
                        ContainerPort = 80,
                        TargetGroup = lb.DefaultTargetGroup,
                    },
                }
            }
        });

        this.Url = lb.LoadBalancer.Apply(lb => lb.DnsName);
    }
    [Output] public Output<string> Url { get; set; }
}