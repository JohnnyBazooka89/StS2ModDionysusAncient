using BaseLib.Abstracts;
using BaseLib.Extensions;
using DionysusAncient.DionysusAncientCode.Extensions;

namespace DionysusAncient.DionysusAncientCode.Enchantments;

public abstract class DionysusEnchantment : CustomEnchantmentModel
{
    protected override string CustomIconPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentImagePath();
}