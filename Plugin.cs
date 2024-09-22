using BepInEx;
using BepInEx.Logging;
using TradersSellBundles.Patches;

namespace TradersSellBundles
{
    [BepInPlugin("com.kyoukopan.TradersSellBundles", "TradersSellBundles", "0.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        private void Awake()
        {
            LogSource = Logger;
            new GetBarterSchemePatch().Enable();
            new ItemTransparencyPatch().Enable();
            LogSource.LogInfo("TradersSellBundles: Loaded");
        }
    }
}
