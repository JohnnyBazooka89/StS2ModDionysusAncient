using DionysusAncient.DionysusAncientCode.Relics;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;
using MegaCrit.sts2.Core.Nodes.TopBar;
using MegaCrit.Sts2.Core.Saves;

namespace DionysusAncient.DionysusAncientCode.Patches;

[HarmonyPatch]
static class WorryFree_Patches
{
    [HarmonyPatch(typeof(NHealthBar), nameof(NHealthBar.RefreshText))]
    [HarmonyPrefix]
    static void HideHpOnBarInit(NHealthBar __instance)
    {
        if (__instance._creature.IsPlayer)
        {
            __instance.HpBarContainer.Visible = true;
        }
    }

    [HarmonyPatch(typeof(NHealthBar), nameof(NHealthBar.RefreshText))]
    [HarmonyPostfix]
    static void HideHpOnBar(NHealthBar __instance)
    {
        if (__instance._creature.IsPlayer)
        {
            bool hasWorryFreeRelic = HasWorryFreeRelic(__instance._creature.Player);
            __instance._hpLabel.Visible = __instance._hpLabel.Visible && !hasWorryFreeRelic;
            __instance.HpBarContainer.Visible = __instance.HpBarContainer.Visible && !hasWorryFreeRelic;
        }
    }

    [HarmonyPatch(typeof(NTopBarHp), nameof(NTopBarHp.UpdateHealth))]
    [HarmonyPostfix]
    static void HideHpTopBar(NTopBarHp __instance)
    {
        if (HasWorryFreeRelic(__instance._player))
        {
            __instance._hpLabel.SetTextAutoSize("?/?");
        }
    }

    [HarmonyPatch(typeof(NContinueRunInfo), nameof(NContinueRunInfo.ShowInfo))]
    [HarmonyPostfix]
    static void HideHpContinueRun(NContinueRunInfo __instance, SerializableRun save)
    {
        if (save.Players[0].Relics.Any(relic => relic.Id == ModelDb.GetId<WorryFree>()))
        {
            __instance._healthLabel.Text = "[red]?/?[/red]";
        }
    }

    static bool HasWorryFreeRelic(Player? player)
    {
        return player?.GetRelic<WorryFree>() != null;
    }

    [HarmonyPatch(typeof(CreatureCmd), nameof(CreatureCmd.Heal))]
    public static class CreatureCmd_Heal_Patch
    {
        static void Prefix(ref bool playAnim)
        {
            if (WorryFree.HealAnimSuppressor.SuppressHealAnim)
            {
                playAnim = false;
            }
        }
    }
}