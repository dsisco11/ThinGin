namespace ThinGin.Core.Common.Noise
{
    public interface INoiseProvider
    {
        uint SampleUInt(uint seed, int pos);
        uint SampleUInt(uint seed, int posX, int posY);
        uint SampleUInt(uint seed, int posX, int posY, int posZ);

        float SampleF(uint seed, int pos);
        float SampleF(uint seed, int posX, int posY);
        float SampleF(uint seed, int posX, int posY, int posZ);
    }
}