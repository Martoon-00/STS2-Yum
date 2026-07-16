using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Yum.YumCode.Powers;

public sealed class PunishingHandPower : CustomPowerModel
{
    public override string CustomPackedIconPath => $"{MainFile.ResPath}/images/powers/punishing_hand_power.png";

    public override string CustomBigIconPath => $"{MainFile.ResPath}/images/powers/punishing_hand_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Single;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    public override async Task AfterAutoPostPlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            await PlaySovereignBladesOnTop(choiceContext, player);
        }
    }

    // AfterAutoPostPlayPhaseEntered dispatches once, before other cards in that same pass (like
    // I Am Invincible) get a chance to auto-play themselves and uncover another Sovereign Blade
    // underneath. Reacting to every card played during that same phase lets the chain resolve
    // regardless of what exposes the next Sovereign Blade. Gated to AutoPostPlay so this doesn't
    // also fire on ordinary plays during the rest of the turn.
    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Player? player = Owner.Player;
        if (player == null || player.PlayerCombatState == null)
        {
            return;
        }

        if (cardPlay.Card.Owner == player && player.PlayerCombatState.Phase == PlayerTurnPhase.AutoPostPlay)
        {
            await PlaySovereignBladesOnTop(choiceContext, player);
        }
    }

    private static async Task PlaySovereignBladesOnTop(PlayerChoiceContext choiceContext, Player player)
    {
        while (PileType.Draw.GetPile(player).Cards.FirstOrDefault() is SovereignBlade)
        {
            await CardPileCmd.AutoPlayFromDrawPile(choiceContext, player, 1, CardPilePosition.Top, forceExhaust: false);
        }
    }
}
