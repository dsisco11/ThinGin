namespace ThinGin.Core.RenderHardware.Resources
{
    public struct VRamAllocation
    {
        /// <summary> Allocated Size (in bytes) of the resource in memory </summary>
        public readonly uint Size;
        /// <summary> Unused atm, might have a virtualized resource management system later. </summary>
        public readonly uint Start;

        public VRamAllocation(uint size, uint start) : this()
        {
            Size = size;
            Start = start;
        }
    }

    public struct RHIResourceInfo
    {
        public readonly VRamAllocation Allocation;

        public RHIResourceInfo(VRamAllocation allocation)
        {
            Allocation = allocation;
        }
    }
}
