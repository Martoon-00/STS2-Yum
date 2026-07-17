using BaseLib.Patches.Localization;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using Yum.YumCode.Powers;

namespace Yum.YumCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "Yum"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();

        DescriptionOverrides.CustomizeDescriptionPost += (CardModel card, Creature? target, ref string description) =>
        {
            if (card is SovereignBlade && card.Owner != null && card.Owner.Creature.HasPower<SharpBladePower>())
            {
                LocString suffix = new("powers", "YUM-SHARP_BLADE_POWER.sovereignBladeSuffix");
                suffix.Add("DamageIncrease", card.Owner.Creature.GetPowerAmount<SharpBladePower>() * 10m);
                description += suffix.GetFormattedText();
            }
        };
    }
}
