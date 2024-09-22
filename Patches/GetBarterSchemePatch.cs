using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Collections.Generic;
using System.Reflection;

namespace TradersSellBundles.Patches
{
    /**<summary>
     * Postfixes <see cref="TraderAssortmentControllerClass.GetSchemeForItem"/>.
     * <br/>
     * When trying to get the <see cref="BarterScheme"/> for an item containing other items (e.g. bag), 
     * the default behavior for <c>GetSchemeForItem()</c> is to return <c>null</c>, which makes it unpurchasable.
     * <br/>
     * This patch runs the price-getting logic from <c>GetSchemeForItem()</c> on anything that was rejected by the default implementation.
     * </summary>
     */
    internal class GetBarterSchemePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TraderAssortmentControllerClass), nameof(TraderAssortmentControllerClass.GetSchemeForItem));
        }


        [PatchPostfix]
        static BarterScheme Postfix(BarterScheme __result, TraderAssortmentControllerClass __instance, Item item, Dictionary<string, BarterScheme> ___dictionary_1)
        {

            BarterScheme barterScheme = __result;

            if (barterScheme == null && item != null)
            {
                // Default returns null when item is null or is a bag with stuff inside
                // We handled item == null above, so now it should only be the case where item is a bag with stuff

                // Plugin.LogSource.LogDebug($"{item} is not empty - manually getting barter scheme & price"); // DEBUG

                ___dictionary_1.TryGetValue(item.Id, out barterScheme); // This is pulled directly from GetSchemeForItem()
            }

            // Do nothing if we already had a valid BarterScheme. Pass thru null value if item was null (an error).
            return barterScheme;
        }
    }
}
