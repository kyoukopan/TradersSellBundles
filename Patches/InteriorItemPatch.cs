using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace TradersSellBundles.Patches
{
    /**<summary>
     * Prefixes <see cref="TradingItemView.NewTradingItemView"/>.
     * <br/>
     * When opening a container that is displayed in a trader's inventory, we flag the items inside as weapon mods so the "cannot modify traders' inventory" tooltip appears.
     * <br/>
     * We also disable selecting it, because otherwise the game will think we are able to buy the interior item. 
     * </summary>
     */
    internal class InteriorItemPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TradingItemView), nameof(TradingItemView.NewTradingItemView));
        }

        [PatchPostfix]
        static void Postfix(TradingItemView __instance, Item item, ref bool ___ModSlotView, bool modSlotView, ref bool ___bool_8, IItemOwner itemOwner)
        {
            try
            {
                if (item.CurrentAddress == null) return; // Items in your stash that you try to move to the trader's sell area will throw an error if we try to access its Parent - Checking for CurrentAddress should prevent this

                bool isInsideSomething = item.Parent?.ContainerName != null && item.Parent?.ContainerName != "hideout" && item.Parent?.ContainerName != "table";
                if (itemOwner.OwnerType != EOwnerType.Trader || modSlotView || !isInsideSomething) return;
                // Is an item within another item, not a mod, is in trader's inventory

                ___ModSlotView = true; // Treat as weapon mod so we can use the same tooltip
                ___bool_8 = false; // Not allowed to select this item to view its price in the trade UI

                return;
            }
            catch (System.Exception e)
            {
                Plugin.LogSource.LogError("TradersSellBundles Error: " + e);
            }
        }

    }
}
