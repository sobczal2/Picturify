using System;
using System.CommandLine.Parsing;
using Sobczal.Picturify.Core.Utils;

namespace Sobczal.Picturify.CLI.Util
{
    public class CommonParseArguments
    {
        public static PSize ParseSize(ArgumentResult result)
        {
            if (result.Tokens.Count != 1)
            {
                result.ErrorMessage = "Only a single token can be parsed";
                return default;
            }

            var value = result.Tokens[0].Value.Split('x');
            if (value.Length != 2)
            {
                result.ErrorMessage = "Size must be in format [Width]x[Height]";
                return default;
            }
            try
            {
                var x = int.Parse(value[0]);
                var y = int.Parse(value[1]);
                return new PSize(x, y);
            }
            catch (Exception)
            {
                result.ErrorMessage = "Size must be in format [Width]x[Height]";
                return default;
            }
        }
        
        public static ChannelSelector ParseChannel(ArgumentResult result)
        {
            if (result.Tokens.Count != 1)
            {
                result.ErrorMessage = "Only a single token can be parsed";
                return default;
            }
            
            switch (result.Tokens[0].Value.ToUpper())
            {
                case "A":
                    return ChannelSelector.A;
                case "R":
                    return ChannelSelector.R;
                case "G":
                    return ChannelSelector.G;
                case "B":
                    return ChannelSelector.B;
                case "RG":
                    return ChannelSelector.RG;
                case "RB":
                    return ChannelSelector.RB;
                case "GB":
                    return ChannelSelector.GB;
                case "RGB":
                    return ChannelSelector.RGB;
                case "ARGB":
                    return ChannelSelector.ARGB;
                default:
                    result.ErrorMessage = "Invalid channel.";
                    return default;
            }
        }

        public static EdgeBehaviourSelector.Type ParseEdge(ArgumentResult result)
        {
            switch (result.Tokens[0].Value.ToLower())
            {
                case "constant":
                    return EdgeBehaviourSelector.Type.Constant;
                case "crop":
                    return EdgeBehaviourSelector.Type.Crop;
                case "extend":
                    return EdgeBehaviourSelector.Type.Extend;
                case "mirror":
                    return EdgeBehaviourSelector.Type.Mirror;
                case "wrap":
                    return EdgeBehaviourSelector.Type.Wrap;
                default:
                    throw new FormatException("Edge bad format.");
            }
        }
    }
}