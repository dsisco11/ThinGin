using System.Numerics;

namespace ThinGin.Core.Common.Noise
{
    // https://jobtalle.com/cubic_noise.html

    public class CubicNoise
    {
        #region Values
        public readonly uint Seed;

        public readonly int Octaves;
        private readonly float OctavesF;
        private readonly float[] OctaveInfluence;

        public readonly float PeriodX;
        public readonly float PeriodY;
        public readonly float PeriodZ;

        private readonly Vector3 Period = Vector3.One;

        public readonly INoiseProvider Provider;
        #endregion

        #region Constructors
        public CubicNoise(uint seed, INoiseProvider provider, int octaves, float octave_p_factor = 2f, int periodX = int.MaxValue, int periodY = int.MaxValue, int periodZ = int.MaxValue)
        {
            Seed = seed;
            Octaves = octaves;
            OctavesF = (float)Octaves;

            PeriodX = periodX;
            PeriodY = periodY;
            PeriodZ = periodZ;
            
            Provider = provider;
            Period = new Vector3(periodX, periodY, periodZ);

            // Calculate octave factors
            OctaveInfluence = new float[Octaves];
            float f = octave_p_factor;
            float fmo = f - 1f;
            float fpo = f;// F^o

            for (int o = 0; o < Octaves; o++)
            {
                // Formula:
                // n = (f-1)*fpo / (fpo - 1)
                // i = n / f;

                float n = (fmo * fpo) / (fpo - 1f);
                float i = n / f;
                fpo *= f;

                OctaveInfluence[o] = i;
            }
        }
        #endregion

        #region Interpolation
        public static float Interpolate(float a, float b, float c, float d, float factor)
        {// Formula: n=x(x(x(−a+b−c+d)+2a−2b+c−d)−a+c)+b
            //float ba = (b - a);
            //float ca = (c - a);
            //float badc = ba - (d - c);
            //float a2 = (a * 2);
            //float b2 = (b * 2);
            //return factor * (factor * (factor * badc + (ba - badc)) + ca) + b;


            // Expanded: n = x(x(x((d-c)-(a-b))+((a-b)-p)+(c-a))+b
            float ab = (a - b);
            float dc = (d - c);
            float ca = (c - a);
            float dcab = dc - ab;

            return factor * (factor * ((factor * dcab) + (ab - dcab)) + ca) + b;
        }
        #endregion

        #region Sampling
        public float Sample(float pos)
        {
            float Sum = 0f;
            var Factor = 1f / OctavesF;
            for (int oct = 0; oct < Octaves; oct++)
            {
                Sum += OctaveInfluence[oct] * SubSample(pos, oct);
            }

            return Sum;
        }

        public float Sample(Vector2 pos)
        {
            float Sum = 0f;
            var Factor = 1f / OctavesF;
            for (int oct = 0; oct < Octaves; oct++)
            {
                Sum += OctaveInfluence[oct] * SubSample(pos, oct);
            }

            return Sum;
        }

        public float Sample(Vector3 pos)
        {
            float Sum = 0f;
            var Factor = 1f / OctavesF;
            for (int oct = 0; oct < Octaves; oct++)
            {
                Sum += OctaveInfluence[oct] * SubSample(pos, oct);
            }

            return Sum;
        }
        #endregion

        #region Sub-Sampling
        public float SubSample(float pos, float octave)
        {
            var x = pos / octave;
            var xi = (int)(long)x;
            var xf = x - xi;

            var n0 = Provider.SampleF(Seed, xi - 1);
            var n1 = Provider.SampleF(Seed, xi);
            var n2 = Provider.SampleF(Seed, xi + 1);
            var n3 = Provider.SampleF(Seed, xi + 2);

            return Interpolate(n0, n1, n2, n3, xf);
        }

        public float SubSample(Vector2 pos, float octave)
        {
            var p = Vector2.Divide(pos, octave);
            var xi = (int)(long)p.X;
            var yi = (int)(long)p.Y;
            var pi = new Vector2(xi, yi);
            var lerp = Vector2.Subtract(p, pi);

            // Save computation by just offsetting our base coordinates ahead of time.
            xi -= 1;
            yi -= 1;

            // Gather noise samples
            float[] Samples = new float[4];
            for (int n = 0; n < 4; n++)
            {
                var y = yi + n;
                var n0 = Provider.SampleF(Seed, xi, y);
                var n1 = Provider.SampleF(Seed, xi + 1, y);
                var n2 = Provider.SampleF(Seed, xi + 2, y);
                var n3 = Provider.SampleF(Seed, xi + 3, y);

                Samples[n] = Interpolate(n0, n1, n2, n3, lerp.X);
            }

            return Interpolate(Samples[0], Samples[1], Samples[2], Samples[3], lerp.Y);
        }

        public float SubSampleInline(Vector2 pos, float octave)
        {
            var p = Vector2.Divide(pos, octave);
            var xi = (int)(long)p.X;
            var yi = (int)(long)p.Y;
            var pi = new Vector2(xi, yi);
            var lerp = Vector2.Subtract(p, pi);

            // Save computation by just offsetting our base coordinates ahead of time.
            xi -= 1;
            yi -= 1;

            // Gather noise samples

            var n00 = Provider.SampleF(Seed, xi, yi);
            var n10 = Provider.SampleF(Seed, xi + 1, yi);
            var n20 = Provider.SampleF(Seed, xi + 2, yi);
            var n30 = Provider.SampleF(Seed, xi + 3, yi);

            var n01 = Provider.SampleF(Seed, xi, yi + 1);
            var n11 = Provider.SampleF(Seed, xi + 1, yi + 1);
            var n21 = Provider.SampleF(Seed, xi + 2, yi + 1);
            var n31 = Provider.SampleF(Seed, xi + 3, yi + 1);

            var n02 = Provider.SampleF(Seed, xi, yi + 2);
            var n12 = Provider.SampleF(Seed, xi + 1, yi + 2);
            var n22 = Provider.SampleF(Seed, xi + 2, yi + 2);
            var n32 = Provider.SampleF(Seed, xi + 3, yi + 2);

            var n03 = Provider.SampleF(Seed, xi, yi + 3);
            var n13 = Provider.SampleF(Seed, xi + 1, yi + 3);
            var n23 = Provider.SampleF(Seed, xi + 2, yi + 3);
            var n33 = Provider.SampleF(Seed, xi + 3, yi + 3);

            var f0 = Interpolate(n00, n10, n20, n30, lerp.X);
            var f1 = Interpolate(n01, n11, n21, n31, lerp.X);
            var f2 = Interpolate(n02, n12, n22, n32, lerp.X);
            var f3 = Interpolate(n03, n13, n23, n33, lerp.X);

            return Interpolate(f0, f1, f2, f3, lerp.Y);
        }

        public float SubSample(Vector3 pos, float octave)
        {
            var p = Vector3.Divide(pos, octave);
            var xi = (int)(long)p.X;
            var yi = (int)(long)p.Y;
            var zi = (int)(long)p.Z;
            var pi = new Vector3(xi, yi, zi);
            var lerp = Vector3.Subtract(p, pi);

            // Save computation by just offsetting our base coordinates ahead of time.
            xi -= 1;
            yi -= 1;
            zi -= 1;

            const int TABLE_SIZE = 4;
            float[] xbuf = new float[TABLE_SIZE];
            float[] ybuf = new float[TABLE_SIZE];
            float[] zbuf = new float[TABLE_SIZE];

            for (int xoff = 0; xoff < TABLE_SIZE; xoff++)
            {
                var x = xi + xoff;
                for (int yoff = 0; yoff < TABLE_SIZE; yoff++)
                {
                    var y = yi + yoff;
                    for (int zoff = 0; zoff < TABLE_SIZE; zoff++)
                    {
                        var z = zi + zoff;
                        zbuf[zoff] = Provider.SampleF(Seed, x, y, z);
                    }

                    ybuf[yoff] = Interpolate(zbuf[0], zbuf[1], zbuf[2], zbuf[3], lerp.Z);
                }

                xbuf[xoff] = Interpolate(ybuf[0], ybuf[1], ybuf[2], ybuf[3], lerp.Y);
            }

            return Interpolate(xbuf[0], xbuf[1], xbuf[2], xbuf[3], lerp.X);
        }
        #endregion
    }
}
