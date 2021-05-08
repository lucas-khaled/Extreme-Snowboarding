using ExtremeSnowboarding.Script.Items.Effects;

namespace ExtremeSnowboarding.Script.EventSystem
{
    public class EffectGeneralEvents
    {
        public delegate void OnEffectStarted(Effect effect, Player.Player player);
        public static OnEffectStarted onEffectStarted;
        
        public delegate void OnEffectEnded(Effect effect, Player.Player player);
        public static OnEffectEnded onEffectEnded;
    }
}