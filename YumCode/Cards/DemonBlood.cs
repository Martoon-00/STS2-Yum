using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Yum.YumCode.Cards;

[Pool(typeof(IroncladCardPool))]
public sealed class DemonBlood() : CustomCardModel(2, CardType.Power, CardRarity.Rare, TargetType.AllAllies)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/demon_blood.png";

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<TheCallOfBlood>(IsUpgraded)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null)
        {
            return;
        }

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

        foreach (Creature creature in CombatState.GetTeammatesOf(Owner.Creature))
        {
            if (creature is not { IsAlive: true, Player: { } player })
            {
                continue;
            }

            CardModel card = CombatState.CreateCard<TheCallOfBlood>(player);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card);
            }

            await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner);
        }
    }
}
