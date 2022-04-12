using System.CommandLine;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Util
{
    public static class CommonOptions
    {
        public static Option<PSize> GetPSizeOption(PSize maxSize, string[] aliases, string description, PSize defaultValue)
        {
            var sizeOption =
                new Option<PSize>(aliases, CommonParseArguments.ParseSize, false,
                    $"{description} Can't be bigger than {maxSize.Width}x{maxSize.Height}.");
            sizeOption.AddValidator(x =>
            {
                if (x.GetValueOrDefault<PSize>().Width > maxSize.Width ||
                    x.GetValueOrDefault<PSize>().Height > maxSize.Height)
                    x.ErrorMessage = $"Size can't be bigger than {maxSize.Width}x{maxSize.Height}";
            });
            sizeOption.SetDefaultValue(defaultValue);
            return sizeOption;
        }

        public static Option<ChannelSelector> GetChannelOption(
            string description = "Choose what color channels should get edited.", ChannelSelector defaultValue = null)
        {
            var channelOption = new Option<ChannelSelector>(new[] {"-c", "--channel"}, CommonParseArguments.ParseChannel,
                false, description);
            if(defaultValue == null) defaultValue = ChannelSelector.RGB;
            channelOption.SetDefaultValue(defaultValue);
            return channelOption;
        }

        public static Option<EdgeBehaviourSelector.Type> GetEdgeOption(string description = "Define edge behaviour.",
            EdgeBehaviourSelector.Type defaultValue = EdgeBehaviourSelector.Type.Mirror)
        {
            var edgeOption = new Option<EdgeBehaviourSelector.Type>(new[] {"-e", "--edge"},
                CommonParseArguments.ParseEdge, false, description);
            edgeOption.SetDefaultValue(defaultValue);
            return edgeOption;
        }
    }
}