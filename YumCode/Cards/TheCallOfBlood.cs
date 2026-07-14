using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using Yum.YumCode.Powers;

namespace Yum.YumCode.Cards;

[Pool(typeof(TokenCardPool))]
public sealed class TheCallOfBlood() : CustomCardModel(1, CardType.Power, CardRarity.Token, TargetType.Self)
{
    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/the_call_of_blood.png";

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new IntVar("HpLoss", 4),
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "PowerUp", Owner.Character.PowerUpAnimDelay);
        (await PowerCmd.Apply<DemonBloodPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this))
            ?.SetHpLoss(DynamicVars["HpLoss"].BaseValue);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["HpLoss"].UpgradeValueBy(-1m);
    }
}
