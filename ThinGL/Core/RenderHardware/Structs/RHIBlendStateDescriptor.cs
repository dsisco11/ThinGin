namespace ThinGin.Core.RenderHardware
{
    public readonly ref struct RHIBlendStateDescriptor
    {
        public readonly bool UseAlphaToCoverage;
        public readonly bool UseIndependentRenderTargetBlendStates;

        public readonly RHIRenderTargetDescriptor[] RenderTargets;
    }
}
