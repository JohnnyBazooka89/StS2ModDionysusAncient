using System.Reflection;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace DionysusAncient.DionysusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class WorryFree : DionysusAncientRelic
{
    private const string MinMaxHpKey = "MinMaxHp";
    private const string MaxMaxHpKey = "MaxMaxHp";

    public override RelicRarity Rarity => RelicRarity.Ancient;

    public override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new(MinMaxHpKey, 40M),
        new(MaxMaxHpKey, 60M),
    ];

    public override async Task AfterObtained()
    {
        RefreshCurrentAndMaxHp();

        await HealAnimSuppressor.Run(async () =>
        {
            await CreatureCmd.GainMaxHp(
                Owner.Creature,
                Owner.RunState.Rng.Niche.NextInt(
                    DynamicVars[MinMaxHpKey].IntValue,
                    DynamicVars[MaxMaxHpKey].IntValue + 1
                )
            );
        });
    }

    public override async Task AfterRemoved()
    {
        RefreshCurrentAndMaxHp();
    }

    private void RefreshCurrentAndMaxHp()
    {
        var field = typeof(Creature).GetField(
            "CurrentHpChanged",
            BindingFlags.Instance | BindingFlags.NonPublic);

        var del = (Action<int, int>?)field?.GetValue(Owner.Creature);

        del?.Invoke(Owner.Creature.CurrentHp, Owner.Creature.MaxHp);

        Owner.Creature.GetCreatureNode()?._stateDisplay.RefreshValues();
    }

    public static class HealAnimSuppressor
    {
        [field: ThreadStatic] public static bool SuppressHealAnim { get; private set; }

        public static async Task Run(Func<Task> action)
        {
            bool previous = SuppressHealAnim;
            SuppressHealAnim = true;

            try
            {
                await action();
            }
            finally
            {
                SuppressHealAnim = previous;
            }
        }
    }
}