using DionysusAncient.DionysusAncientCode.Hooks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Odds;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace DionysusAncient.DionysusAncientCode.Patches;

[HarmonyPatch(typeof(UnknownMapPointOdds), nameof(UnknownMapPointOdds.Roll))]
public static class UnknownMapPointOdds_Roll_Patch
{
    public static void Postfix(IRunState runState, RoomType __result)
    {
        DionysusHooks.AfterRoomTypeRolled(runState, __result);
    }
}