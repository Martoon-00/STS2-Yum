
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;

namespace Yum.YumCode.Powers;

public sealed class TheSquadPower : CustomPowerModel
{
    public override string CustomPackedIconPath => $"{MainFile.ResPath}/images/powers/the_squad_power.png";
    public override string CustomBigIconPath => $"{MainFile.ResPath}/images/powers/the_squad_power.png";

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (command._sourceType != AttackCommand.SourceType.Card)
        {
            return;
        }

        var hits = command.Results.Aggregate(0, (acc, singleHitResult) => acc + singleHitResult.Count);
        IEnumerable<Player> players = CombatState.Players.Where(p => p.Creature.IsAlive);
        foreach (Player player in players)
        {
            await ForgeCmd.Forge(hits * Amount, player, this);
        }
        
    }

}