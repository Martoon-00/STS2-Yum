
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;

namespace Yum.YumCode.Powers;

public sealed class TheSquadPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();


    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        var amount = command.Results.Count();
        IEnumerable<Player> players = CombatState.Players.Where((Player p) => p.Creature.IsAlive);
        foreach (Player player in players)
        {
            await ForgeCmd.Forge(amount, player, this);
        }
    }

}