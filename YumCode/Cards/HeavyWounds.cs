using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using Yum.YumCode.Powers;

namespace Yum.YumCode.Cards;

[Pool(typeof(RegentCardPool))]
public sealed class HeavyWounds() : CustomCardModel(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/heavy_wounds.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromCard<SovereignBlade>(), HoverTipFactory.FromPower<HeavyWoundsDebuffPower>(), .. HoverTipFactory.FromForge()];

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("DamageIncrease", 30m), new ForgeVar(2)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await ForgeCmd.Forge(DynamicVars.Forge.BaseValue, Owner, this);
        await PowerCmd.Apply<SharpBladePower>(choiceContext, Owner.Creature, DynamicVars["DamageIncrease"].BaseValue / 10m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["DamageIncrease"].UpgradeValueBy(10m);
        DynamicVars.Forge.UpgradeValueBy(1m);
    }
}
