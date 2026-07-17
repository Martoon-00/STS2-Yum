using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace Yum.YumCode.Cards;

[Pool(typeof(TokenCardPool))]
public sealed class PowerUnleash() : CustomCardModel(1, CardType.Skill, CardRarity.Token, TargetType.AllAllies)
{
    private CardModel? _selectedCard;

    public CardModel? SelectedCard
    {
        get => _selectedCard;
        set
        {
            AssertMutable();
            _selectedCard = value;
        }
    }

    public override string PortraitPath => $"{MainFile.ResPath}/images/card_portraits/power_unleash.png";

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => SelectedCard == null ? [] : [HoverTipFactory.FromCard(SelectedCard)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null || SelectedCard == null)
        {
            return;
        }

        foreach (Creature creature in CombatState.GetTeammatesOf(Owner.Creature))
        {
            if (creature is not { IsAlive: true, Player: { } player })
            {
                continue;
            }

            CardModel copy = SelectedCard.CreateCloneForPlayer(player);
            copy.EnergyCost.SetThisCombat(0);
            copy.AddKeyword(CardKeyword.Ethereal);

            await CardPileCmd.AddGeneratedCardToCombat(copy, PileType.Hand, Owner);
        }
    }

    protected override void AddExtraArgsToDescription(LocString description)
    {
        description.Add("CardName", SelectedCard?.Title ?? "");
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-2);
    }
}
