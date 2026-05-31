using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.ValueProps;

namespace DionysusAncient.DionysusAncientCode.Hooks;

public class DionysusHooks
{
    private static void Dispatch<T>(IRunState? runState, ICombatState? combatState, Action<T> action)
        where T : class
    {
        foreach (var model in runState?.IterateHookListeners(combatState).OfType<T>() ?? [])
        {
            action(model);
        }
    }

    public static Decimal ModifyDamageToFinalValue(IRunState? runState, ICombatState? combatState, Creature? target,
        Decimal amount,
        ValueProp props, Creature? dealer, CardModel? cardSource, CardPreviewMode previewMode,
        ref IEnumerable<AbstractModel> modifiers)
    {
        Boolean changed = false;
        foreach (IModifyDamageToFinalValue model in runState?.IterateHookListeners(combatState)
                     .OfType<IModifyDamageToFinalValue>() ?? [])
        {
            decimal num = model.ModifyDamageToFinalValue(target, amount, props, dealer, cardSource, previewMode);
            if (num != amount)
            {
                if (model is AbstractModel abstractModel)
                {
                    modifiers = modifiers.Append(abstractModel);
                }

                amount = num;
                changed = true;
            }
        }

        if (changed)
        {
            Decimal cappedDamage = Decimal.MaxValue;
            foreach (AbstractModel iterateHookListener in runState.IterateHookListeners(combatState))
            {
                Decimal capToCompare = iterateHookListener.ModifyDamageCap(target, props, dealer, cardSource);
                if (capToCompare < cappedDamage)
                {
                    cappedDamage = capToCompare;
                    if (amount > cappedDamage)
                    {
                        amount = cappedDamage;
                    }
                }
            }
        }

        return amount;
    }

    public static void AfterRoomTypeRolled(IRunState runState, RoomType roomType)
    {
        Dispatch<IAfterRoomTypeRolled>(runState, null, m => m.AfterRoomTypeRolled(runState, roomType));
    }
}