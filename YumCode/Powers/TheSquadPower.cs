
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
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();


    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        var stacks = Amount;
        var hits = command.Results.Count();
        IEnumerable<Player> players = CombatState.Players.Where((Player p) => p.Creature.IsAlive);
        foreach (Player player in players)
        {
            await ForgeCmd.Forge(hits * stacks, player, this);
        }
        
    }

}