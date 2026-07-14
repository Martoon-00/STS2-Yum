
using BaseLib.Abstracts;
using BaseLib.Hooks;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yum.YumCode.Powers;

public sealed class NecroficationPower : CustomPowerModel
{
    public Color HealthBarForecastSegmentColor => new Color(0xD000FF);

    public override string CustomPackedIconPath => $"{MainFile.ResPath}/images/powers/necrofication_power.png";
    public override string CustomBigIconPath => $"{MainFile.ResPath}/images/powers/necrofication_power.png";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.None;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [];  // TODO

    // Mirror what BufferPower does
    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (target != Owner)
        {
            return amount;
        }

        return 0m;
    }

    public override IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
    {
        return [new HealthBarForecastSegment(Amount, HealthBarForecastSegmentColor, HealthBarForecastDirection.FromRight, -999, null, null)];
    }

}