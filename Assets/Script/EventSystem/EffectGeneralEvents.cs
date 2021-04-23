namespace Script.EventSystem
{
    public class EffectGeneralEvents
    {
        public delegate void OnEffectStarted(Effect effect, Player player);
        public static OnEffectStarted onEffectStarted;
        
        public delegate void OnEffectEnded(Effect effect, Player player);
        public static OnEffectEnded onEffectEnded;
    }
}