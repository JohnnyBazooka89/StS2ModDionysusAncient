using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace DionysusAncient.DionysusAncientCode.Hooks;

public interface IModifyDamageToFinalValue
{
    decimal ModifyDamageToFinalValue(Creature? target, decimal amount, ValueProp props, Creature? dealer,
        CardModel? cardSource, CardPreviewMode previewMode);
}