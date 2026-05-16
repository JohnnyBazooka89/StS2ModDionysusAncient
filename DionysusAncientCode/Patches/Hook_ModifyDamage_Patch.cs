using DionysusAncient.DionysusAncientCode.Hooks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace DionysusAncient.DionysusAncientCode.Patches;

[HarmonyPatch(typeof(Hook), nameof(Hook.ModifyDamage))]
public static class Hook_ModifyDamage_Patch
{
    static void Postfix(
        IRunState? runState,
        ICombatState? combatState,
        Creature? target,
        Creature? dealer,
        decimal damage,
        ValueProp props,
        CardModel? cardSource,
        ModifyDamageHookType modifyDamageHookType,
        CardPreviewMode previewMode,
        ref IEnumerable<AbstractModel> modifiers,
        ref decimal __result)
    {
        __result = DionysusHooks.ModifyDamageToFinalValue(
            runState,
            combatState,
            target,
            __result,
            props,
            dealer,
            cardSource,
            previewMode,
            ref modifiers
        );
    }
}