namespace DockerWebAPI.Pulumi;

public static class CpuHelper
{
    public enum ContainerSize
    {
        cpu025_512,
        cpu025_1024,
        cpu025_2048,
        cpu05_1024,
        cpu05_2048,
        cpu05_3072,
        cpu05_4096,
        cpu1_2048,
        cpu1_3072,
        cpu1_4096,
        cpu2_4096,
    }
    private enum MemorySize
    {
        _512,
        _1024,
        _2048,
        _3072,
        _4096,
    }

    public static int ToMemory(this ContainerSize size)
    {
        return Int32.Parse(size.ToString().Split("_").Last());
    }
    
    public static int ToCpuUnits(this ContainerSize cpu)
    {
        var _cpu = Enum.Parse<CpuSize>(cpu.ToString().Split("_").First());
        return ToCpuUnits(_cpu);
    }

    public static int ToCpuUnits(this CpuSize cpu)
    {
        var i = 1024;
        var cpuUnits = 0m;
        switch (cpu)
        {
            case CpuSize.cpu025:
                cpuUnits = 0.25m * i;
                break;
            case CpuSize.cpu05:
                cpuUnits = 0.5m * i;
                break;
            case CpuSize.cpu1:
                cpuUnits = 1 * i;
                break;
            case CpuSize.cpu2:
                cpuUnits = 2 * i;
                break;
            case CpuSize.cpu4:
                cpuUnits = 4 * i;
                break;
            case CpuSize.cpu8:
                cpuUnits = 8 * i;
                break;
            case CpuSize.cpu16:
                cpuUnits = 16 * i;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(cpu), cpu, null);
        }

        return (int)cpuUnits;
    }
}

public enum CpuSize
{
    cpu025,
    cpu05,
    cpu1,
    cpu2,
    cpu4,
    cpu8,
    cpu16,
}
