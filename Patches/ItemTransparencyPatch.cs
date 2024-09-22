using EFT.InventoryLogic;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace TradersSellBundles.Patches
{
    /**<summary>
     * Postfixes <see cref="TradingItemView.method_36"/>.
     * <br/>
     * <c>method_36()</c> by default checks if the item is holding anything inside it, and if it does, it calls <see cref="GClass761.SetUnlockStatus"/> to make the item transparent in the menu.
     * <br/>
     * We use the same method to undo this so that our shop item is not greyed out.
     * </summary>
     */
    internal class ItemTransparencyPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(TradingItemView), nameof(TradingItemView.method_36));
        }

        [PatchPostfix]
        static void Postfix(CanvasGroup ___CanvasGroup, EOwnerType ___eownerType_0)
        {
            if (___eownerType_0 != EOwnerType.Trader) return; // Only change the behavior for trader inventories, just in case this method is used elsewhere

            ___CanvasGroup.SetUnlockStatus(true, false); // Undo transparency
            return;
        }

    }
}
