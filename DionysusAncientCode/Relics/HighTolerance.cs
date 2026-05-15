using BaseLib.Utils;
using DionysusAncient.DionysusAncientCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace DionysusAncient.DionysusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class HighTolerance : DionysusAncientRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
        new PowerVar<HangoverPower>(5M)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this),
        HoverTipFactory.FromPower<HangoverPower>()
    ];

    public override decimal ModifyMaxEnergy(Player player, decimal amount)
    {
        return player != Owner ? amount : amount + DynamicVars.Energy.IntValue;
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is not CombatRoom)
        {
            return;
        }

        Flash();
        await PowerCmd.Apply<HangoverPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
            DynamicVars["HangoverPower"].BaseValue,
            Owner.Creature, null);
    }
}