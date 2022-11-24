using Pulumi;
using Pulumi.Awsx.Ecs.Inputs;
using Aws = Pulumi.Aws;
using Ecr = Pulumi.Awsx.Ecr;
using Ecs = Pulumi.Awsx.Ecs;
using Lb = Pulumi.Awsx.Lb;

namespace DockerWebAPI.Pulumi.InfrastructureInProgress;

class CrosswalkPulumiEcs_autoscaling : Stack
{
    public CrosswalkPulumiEcs_autoscaling()
    {
        var serviceName = nameof(CrosswalkPulumiEcs_autoscaling);
        var repo = new Ecr.Repository($"{serviceName}-repo");

        var image = new Ecr.Image($"{serviceName}-img", new Ecr.ImageArgs
        {
            RepositoryUrl = repo.Url,
            Path = "../",
        });

        var cluster = new Aws.Ecs.Cluster($"{serviceName}-cluster");
        // var scalingPlan = new Aws.AutoScalingPlans.ScalingPlan($"{serviceName}-scalingplan",new ScalingPlanArgs
        // {
        //     ApplicationSource = new ScalingPlanApplicationSourceArgs()
        //     {
        //         CloudformationStackArn = 
        //     }
        // });
        var lb = new Lb.ApplicationLoadBalancer($"{serviceName}-lb");

        var service = new Ecs.FargateService($"{serviceName}-service", new Ecs.FargateServiceArgs
        {
            DesiredCount = 1,
            
            Cluster = cluster.Arn,
            TaskDefinitionArgs = new FargateServiceTaskDefinitionArgs
            {
                Container = new TaskDefinitionContainerDefinitionArgs
                {
                    Memory = CpuHelper.ContainerSize.cpu1_2048.ToMemory(),
                    Cpu = CpuHelper.ContainerSize.cpu1_2048.ToCpuUnits(),
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