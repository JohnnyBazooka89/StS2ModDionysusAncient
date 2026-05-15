using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace DionysusAncient.DionysusAncientCode.Powers;

public class HangoverPower : DionysusAncientPower
{
    private static readonly Color Color = new(122 / 255f, 46 / 255f, 138 / 255f);

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != Owner.Side)
        {
            return;
        }

        await CreatureCmd.Damage(choiceContext, Owner, Amount, ValueProp.Unpowered, Owner);
        if (Owner.IsAlive)
        {
            await PowerCmd.Decrement(this);
        }
    }

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, Color, HealthBarForecastDirection.FromRight)];
    }
}