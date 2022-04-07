using Sobczal.Picturify.Core.Data.Operators.EdgeDetection;
using Sobczal.Picturify.Core.Processing.EdgeDetection;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.Core.Tests.Processing.Processors.EdgeDetection
{
    public class QuadOperatorProcessorTests : AbstractProcessorTests<QuadOperatorProcessor>
    {
        protected override void PopulateWorkingAreaCheckProcessors()
        {
            WorkingAreaCheckProcessor = new QuadOperatorProcessor(new QuadOperatorParams(ChannelSelector.ARGB,
                new SobelOperator3(), OperatorBeforeNormalizationFunc.Linear, 0f, 1f,
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateChannelSelectorCheckProcessors()
        {
            ChannelSelectorCheckProcessor = new QuadOperatorProcessor(new QuadOperatorParams(ChannelSelector.ARGB,
                new SobelOperator3(), OperatorBeforeNormalizationFunc.Linear, 0f, 1f,
                EdgeBehaviourSelector.Type.Constant, null));
        }

        protected override void PopulateSizeCheckProcessors()
        {
            SizeCheckProcessors.Add(new QuadOperatorProcessor(new QuadOperatorParams(ChannelSelector.ARGB,
                new SobelOperator3(), OperatorBeforeNormalizationFunc.Linear, 0f, 1f,
                EdgeBehaviourSelector.Type.Constant, null)));
            SizeCheckProcessors.Add(new QuadOperatorProcessor(new QuadOperatorParams(ChannelSelector.ARGB,
                new SobelOperator3(), OperatorBeforeNormalizationFunc.Linear, 0f, 1f,
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(2, 8, 2, 8))));
            SizeCheckProcessors.Add(new QuadOperatorProcessor(new QuadOperatorParams(ChannelSelector.ARGB,
                new SobelOperator3(), OperatorBeforeNormalizationFunc.Linear, 0f, 1f,
                EdgeBehaviourSelector.Type.Constant, new SquareAreaSelector(3, 7, 3, 7))));
        }
    }
}