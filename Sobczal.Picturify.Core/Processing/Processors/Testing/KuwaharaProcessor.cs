// # ==============================================================================
// # Solution: Picturify
// # File: KuwaharaProcessor.cs
// # Author: Łukasz Sobczak
// # Created: 16-02-2024
// # ==============================================================================

using System.Linq;
using System.Threading;
using Sobczal.Picturify.Core.Data;

namespace Sobczal.Picturify.Core.Processing.Testing
{
    public class KuwaharaProcessor : BaseProcessor<ProcessorParams, FastImageF>
    {
        public KuwaharaProcessor(
            ProcessorParams processorParams
        )
            : base(processorParams)
        {
        }

        public override IFastImage Process(
            IFastImage fastImage,
            CancellationToken cancellationToken
        )
        {
            ((FastImageF)fastImage).Process(ProcessingFunction, cancellationToken);
            return base.Process(fastImage, cancellationToken);
        }

        private float[,,] ProcessingFunction(
            float[,,] pixels,
            CancellationToken cancellationToken
        )
        {
            for (var i = 2; i < pixels.GetLength(0) - 2; i++)
            {
                for (var j = 2; j < pixels.GetLength(1) - 2; j++)
                {
                    ProcessPoint(pixels, i, j);
                }
            }
            
            return pixels;
        }

        private void ProcessPoint(
            float[,,] pixels,
            int x,
            int y
        )
        {
            var quadrantA = new float[9];
            var quadrantB = new float[9];
            var quadrantC = new float[9];
            var quadrantD = new float[9];

            quadrantA[0] = pixels[x - 2, y - 2, 0];
            quadrantA[1] = pixels[x - 2, y - 1, 0];
            quadrantA[2] = pixels[x - 2, y, 0];
            quadrantA[3] = pixels[x - 1, y - 2, 0];
            quadrantA[4] = pixels[x - 1, y - 1, 0];
            quadrantA[5] = pixels[x - 1, y, 0];
            quadrantA[6] = pixels[x, y - 2, 0];
            quadrantA[7] = pixels[x, y - 1, 0];
            quadrantA[8] = pixels[x, y, 0];

            quadrantB[0] = pixels[x - 2, y, 0];
            quadrantB[1] = pixels[x - 2, y + 1, 0];
            quadrantB[2] = pixels[x - 2, y + 2, 0];
            quadrantB[3] = pixels[x - 1, y, 0];
            quadrantB[4] = pixels[x - 1, y + 1, 0];
            quadrantB[5] = pixels[x - 1, y + 2, 0];
            quadrantB[6] = pixels[x, y, 0];
            quadrantB[7] = pixels[x, y + 1, 0];
            quadrantB[8] = pixels[x, y + 2, 0];

            quadrantC[0] = pixels[x, y - 2, 0];
            quadrantC[1] = pixels[x, y - 1, 0];
            quadrantC[2] = pixels[x, y, 0];
            quadrantC[3] = pixels[x + 1, y - 2, 0];
            quadrantC[4] = pixels[x + 1, y - 1, 0];
            quadrantC[5] = pixels[x + 1, y, 0];
            quadrantC[6] = pixels[x + 2, y - 2, 0];
            quadrantC[7] = pixels[x + 2, y - 1, 0];
            quadrantC[8] = pixels[x + 2, y, 0];

            quadrantD[0] = pixels[x, y, 0];
            quadrantD[1] = pixels[x, y + 1, 0];
            quadrantD[2] = pixels[x, y + 2, 0];
            quadrantD[3] = pixels[x + 1, y, 0];
            quadrantD[4] = pixels[x + 1, y + 1, 0];
            quadrantD[5] = pixels[x + 1, y + 2, 0];
            quadrantD[6] = pixels[x + 2, y, 0];
            quadrantD[7] = pixels[x + 2, y + 1, 0];
            quadrantD[8] = pixels[x + 2, y + 2, 0];

            var meanA = quadrantA.Average();
            var meanB = quadrantB.Average();
            var meanC = quadrantC.Average();
            var meanD = quadrantD.Average();

            var varianceA = quadrantA.Select(val => (val - meanA) * (val - meanA)).Sum() / 9;
            var varianceB = quadrantB.Select(val => (val - meanB) * (val - meanB)).Sum() / 9;
            var varianceC = quadrantC.Select(val => (val - meanC) * (val - meanC)).Sum() / 9;
            var varianceD = quadrantD.Select(val => (val - meanD) * (val - meanD)).Sum() / 9;

            var minVariance = new[] { varianceA, varianceB, varianceC, varianceD }.Min();
            var minVarianceIndex = new[] { varianceA, varianceB, varianceC, varianceD }.ToList().IndexOf(minVariance);

            var mean = new[] { meanA, meanB, meanC, meanD }[minVarianceIndex];

            pixels[x, y, 0] = mean;
            pixels[x, y, 1] = mean;
            pixels[x, y, 2] = mean;

            pixels[x, y, 3] = 255;
        }
    }
}