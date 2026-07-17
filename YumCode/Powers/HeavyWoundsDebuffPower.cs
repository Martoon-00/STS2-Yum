using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yum.YumCode.Powers;

public sealed class HeavyWoundsDebuffPower : CustomPowerModel
{
    public override string CustomPackedIconPath => $"{MainFile.ResPath}/images/powers/heavy_wounds_debuff_power.png";
    public override string CustomBigIconPath => $"{MainFile.ResPath}/images/powers/heavy_wounds_debuff_power.png";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageIncrease", 0m)];

    public void SetDamageIncrease(decimal percent)
    {
        AssertMutable();
        DynamicVars["DamageIncrease"].BaseValue = percent;
    }

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource, CardPlay? cardPlay)
    {
        if (target != Owner || !props.IsPoweredAttack())
        {
            return 1m;
        }

        return 1m + DynamicVars["DamageIncrease"].BaseValue / 100m;
    }

    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (side == CombatSide.Enemy)
        {
            await PowerCmd.TickDownDuration(this);
        }
    }
}
