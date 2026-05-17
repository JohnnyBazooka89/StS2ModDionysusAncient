using System.Reflection;
using DionysusAncient.DionysusAncientCode.Relics;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.HoverTips;
using MegaCrit.Sts2.Core.Runs;
using RichTextLabel = Godot.RichTextLabel;

namespace DionysusAncient.DionysusAncientCode.Patches;

[HarmonyPatch]
public class WorryFree_HistoryHoverTip_Patch
{
    static MethodBase TargetMethod()
    {
        return AccessTools.Method(
            typeof(NMapPointHistoryHoverTip),
            "PopulateActionStats",
            new[] { typeof(PlayerMapPointHistoryEntry) }
        );
    }

    static void Postfix(NMapPointHistoryHoverTip __instance, PlayerMapPointHistoryEntry playerEntry)
    {
        bool gainedWorryFree = playerEntry?.RelicChoices?.Any(r =>
            r.wasPicked &&
            r.choice != null &&
            r.choice.ToString().Contains(ModelDb.GetId<WorryFree>().Entry, StringComparison.OrdinalIgnoreCase)
        ) ?? false;

        if (!gainedWorryFree)
            return;

        var actionStats = AccessTools
            .Field(typeof(NMapPointHistoryHoverTip), "_actionStats")
            .GetValue(__instance) as RichTextLabel;

        if (actionStats == null)
            return;

        string text = actionStats.Text;

        // Hide Max HP gained
        if (playerEntry.MaxHpGained > 0)
        {
            var normal = new LocString("run_history", "MAP_POINT_HISTORY.maxHpGained");
            normal.Add("HP", playerEntry.MaxHpGained);

            var hidden = new LocString("run_history", "MAP_POINT_HISTORY.maxHpGained");
            hidden.Add("HP", "?");

            text = text.Replace(
                normal.GetFormattedText(),
                hidden.GetFormattedText()
            );
        }

        // Hide healing
        if (playerEntry.HpHealed > 0)
        {
            var normal = new LocString("run_history", "MAP_POINT_HISTORY.healed");
            normal.Add("HP", playerEntry.HpHealed);

            var hidden = new LocString("run_history", "MAP_POINT_HISTORY.healed");
            hidden.Add("HP", "?");

            text = text.Replace(
                normal.GetFormattedText(),
                hidden.GetFormattedText()
            );
        }

        actionStats.Text = text;
    }
}