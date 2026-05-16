using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace DionysusAncient.DionysusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class HappyHaze : DionysusAncientRelic
{
    private const string TurnKey = "Turn";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(TurnKey, 3M),
        new PowerVar<StrengthPower>(4M),
        new PowerVar<DexterityPower>(3M),
        new PowerVar<FocusPower>(2M),
    ];

    public override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromPower<FocusPower>(),
    ];

    public override async Task AfterSideTurnStart(CombatSide side, ICombatState combatState)
    {
        if (side != Owner.Creature.Side)
            return;
        if (combatState.RoundNumber == DynamicVars[TurnKey].IntValue)
        {
            Flash();
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
                DynamicVars.Strength.BaseValue, Owner.Creature, null);
            await PowerCmd.Apply<DexterityPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
                DynamicVars.Dexterity.BaseValue, Owner.Creature, null);
            await PowerCmd.Apply<FocusPower>(new ThrowingPlayerChoiceContext(), Owner.Creature,
                DynamicVars["FocusPower"].BaseValue, Owner.Creature, null);
        }

        InvokeDisplayAmountChanged();
    }
}