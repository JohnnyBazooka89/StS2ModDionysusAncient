using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using DionysusAncient.DionysusAncientCode.Extensions;
using DionysusAncient.DionysusAncientCode.Relics;
using MegaCrit.Sts2.Core.Models;

namespace DionysusAncient.DionysusAncientCode.Ancients;

[Pool(typeof(AncientEventModel))]
public class DionysusAncient : CustomAncientModel
{
    public override string CustomScenePath => "Dionysus.tscn".AncientImagePath();
    public override string CustomMapIconPath => "map_icon.png".AncientImagePath();
    public override string CustomMapIconOutlinePath => "map_icon_outline.png".AncientImagePath();
    public override string CustomRunHistoryIconPath => "run_history_icon.png".AncientImagePath();
    public override string CustomRunHistoryIconOutlinePath => "run_history_icon_outline.png".AncientImagePath();

    protected override OptionPools MakeOptionPools
    {
        get
        {
            List<AncientOption> relics =
            [
                AncientOption<DrunkenDash>(),
                AncientOption<DrunkenStupor>(),
                AncientOption<HighTolerance>(),
                AncientOption<TipsyShot>(),
            ];

            return new OptionPools(
                MakePool(relics.ToArray())
            );
        }
    }

    public override bool IsValidForAct(ActModel act)
    {
        return act.ActNumber() == 3;
    }
}