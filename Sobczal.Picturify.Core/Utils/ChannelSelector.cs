namespace Sobczal.Picturify.Core.Utils
{
    public class ChannelSelector
    {
        public bool UseRed { get; set; }
        public bool UseGreen { get; set; }
        public bool UseBlue { get; set; }
        public bool UseAlpha { get; set; }

        public static ChannelSelector RGB => new ChannelSelector
            {UseRed = true, UseGreen = true, UseBlue = true, UseAlpha = false};
        public static ChannelSelector ARGB => new ChannelSelector
            {UseRed = true, UseGreen = true, UseBlue = true, UseAlpha = true};
        public static ChannelSelector R => new ChannelSelector
            {UseRed = true, UseGreen = false, UseBlue = false, UseAlpha = false};
        public static ChannelSelector G => new ChannelSelector
            {UseRed = false, UseGreen = true, UseBlue = false, UseAlpha = false};
        public static ChannelSelector B => new ChannelSelector
            {UseRed = false, UseGreen = false, UseBlue = true, UseAlpha = false};
    }
}