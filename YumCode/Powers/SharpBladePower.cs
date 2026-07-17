using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yum.YumCode.Powers;

public sealed class SharpBladePower : CustomPowerModel
{
    public override string CustomPackedIconPath => $"{MainFile.ResPath}/images/powers/sharp_blade.png";
    public override string CustomBigIconPath => $"{MainFile.ResPath}/images/powers/sharp_blade.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<SovereignBlade>(), HoverTipFactory.FromPower<HeavyWoundsDebuffPower>()];

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || cardSource is not SovereignBlade || target.Side == Owner.Side)
        {
            return;
        }

        (await PowerCmd.Apply<HeavyWoundsDebuffPower>(choiceContext, target, 2m, Owner, cardSource))
            ?.SetDamageIncrease(Amount * 10m);
    }
}
