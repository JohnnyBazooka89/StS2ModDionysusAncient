using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace DionysusAncient.DionysusAncientCode.Relics;

[Pool(typeof(EventRelicPool))]
public class TipsyShot : DionysusAncientRelic
{
    public override RelicRarity Rarity => RelicRarity.Ancient;
}