using System.Linq;
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Yum.YumCode.Cards;

[Pool(typeof(NecrobinderCardPool))]
public sealed class DeathIntention() : CustomCardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/death_intention.png";

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null)
        {
            return;
        }

        CardModel? selectedCard = (await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), c => c.Type == CardType.Power, this)).FirstOrDefault();
        if (selectedCard == null)
        {
            return;
        }

        CardModel token = CombatState.CreateCard<PowerUnleash>(Owner);
        ((PowerUnleash)token).SelectedCard = selectedCard;
        token.EnergyCost.SetCustomBaseCost(selectedCard.EnergyCost.GetWithModifiers(CostModifiers.Local) + 2);
        if (IsUpgraded)
        {
            CardCmd.Upgrade(token);
        }

        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await CardCmd.Transform(selectedCard, token);
    }

    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
