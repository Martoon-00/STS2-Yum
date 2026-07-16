using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using Yum.YumCode.Powers;

namespace Yum.YumCode.Cards;

[Pool(typeof(RegentCardPool))]
public sealed class PunishingHand() : CustomCardModel(2, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/punishing_hand.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    protected override IEnumerable<DynamicVar> CanonicalVars => [new ForgeVar(5)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        await ForgeCmd.Forge(DynamicVars.Forge.BaseValue, Owner, this);
        await PowerCmd.Apply<PunishingHandPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
