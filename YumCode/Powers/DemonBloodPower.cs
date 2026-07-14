
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yum.YumCode.Powers;

public sealed class DemonBloodPower : CustomPowerModel
{
    public override string CustomPackedIconPath => $"{MainFile.ResPath}/images/powers/demon_blood_power.png";
    public override string CustomBigIconPath => $"{MainFile.ResPath}/images/powers/demon_blood_power.png";

    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new IntVar("HpLoss", 4)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    public void SetHpLoss(decimal hpLoss)
    {
        AssertMutable();
        DynamicVars["HpLoss"].BaseValue = hpLoss;
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (dealer != Owner || target.Side == Owner.Side || result.UnblockedDamage <= 0)
        {
            return;
        }

        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, 1m, Owner, null);
    }

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            VfxCmd.PlayOnCreatureCenter(Owner, "vfx/vfx_bloody_impact");
            await Cmd.CustomScaledWait(0.2f, 0.4f);
            await CreatureCmd.Damage(choiceContext, Owner, DynamicVars["HpLoss"].BaseValue, ValueProp.SkipHurtAnim, null, null);
        }
    }

}