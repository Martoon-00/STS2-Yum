using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yum.YumCode.Cards;

[Pool(typeof(IroncladCardPool))]
public sealed class Kill() : CustomCardModel(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
{
    protected override bool ShouldGlowGoldInternal => true;

    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/kill.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(99m, ValueProp.Move),
        new IntVar("Burn", 9),
        new BoolVar("WithDoom"),
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        var hits = DynamicVars["Burn"].IntValue;
        await CommonActions.CardAttack(this, cardPlay, hitCount: hits, vfx: "vfx/vfx_attack_slash")
            .Execute(choiceContext);
        await CreatureCmd.Damage(choiceContext, Owner.Creature, 10m, ValueProp.SkipHurtAnim, this, null);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(900m);
        DynamicVars["Burn"].UpgradeValueBy(90m);
        DynamicVars["WithDoom"].UpgradeValueBy(1m);
    }

    public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature? dealer, DamageResult result, ValueProp props, Creature target, CardModel? cardSource)
    {
        if (DynamicVars["WithDoom"].IntValue != 0 && target.IsEnemy) {
            await PowerCmd.Apply<DoomPower>(choiceContext, CombatState?.HittableEnemies, 999m, Owner.Creature, this);
        }
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        await CreatureCmd.Heal(Owner.Creature, 100m);
    }

}
