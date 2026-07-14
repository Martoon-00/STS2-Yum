# Localization / tooltip conventions

Rules for writing card and power text in `Yum/localization/{eng,rus}/*.json`, learned from
the base game's own localization (`spire-codex/extraction/raw/localization/`) and decompiled
code (`spire-codex/extraction/decompiled/`).

## Keyword highlighting: `[gold]...[/gold]`

Any game mechanic keyword mentioned in card/power text (Forge, Block, Exhaust, Ethereal, etc.)
is wrapped in `[gold]...[/gold]` in the description. This is what colors the word gold in-game
and visually marks it as a keyword. Example from the base game:

```json
"FURNACE.description": "At the start of your turn, [gold]Forge[/gold] {Forge:diff()}."
"HAMMER_TIME.description": "Whenever you [gold]Forge[/gold], all allies [gold]Forge[/gold] as well."
```

This applies per-occurrence — every place the keyword verb/noun appears in the sentence gets
its own tag, not just the first.

## Number highlighting: `[blue]...[/blue]`

Numeric values — both literal numbers and `{Amount}`-style placeholders — are wrapped in
`[blue]...[/blue]`:

```json
"FURNACE_POWER.description": "At the start of your turn, [gold]Forge[/gold] [blue]5[/blue]."
"FURNACE_POWER.smartDescription": "At the start of your turn, [gold]Forge[/gold] [blue]{Amount}[/blue]."
```

Note `{Damage:diff()}`/`{Cards:diff()}`-style tokens (auto-diffed stat placeholders) are generally
left untagged in the base game's own text — only plain numbers and `{Amount}` get `[blue]`.

## The tooltip box itself is wired in code, not text

The `[gold]` tag only affects text color — it does **not** by itself make the hover tooltip box
appear. That box comes from `ExtraHoverTips` on the `CustomCardModel`/`CustomPowerModel`, e.g.:

```csharp
protected override IEnumerable<IHoverTip> ExtraHoverTips => HoverTipFactory.FromForge();
```

So both pieces are required for a mechanic keyword to look and behave correctly:
1. Code: `ExtraHoverTips => HoverTipFactory.From<Keyword>()` (or equivalent) on the card/power.
2. Text: wrap every mention of the keyword in `[gold]...[/gold]` in all localization files/languages.

## Where keyword tooltip text lives

- Built-in mechanic keywords (Forge, Block, Stun, Cook, ...) are defined by the base game in its
  own `static_hover_tips.json` (`"<KEY>.title"` / `"<KEY>.description"`, e.g. `FORGE.title`,
  `FORGE.description`). Mods don't need to redefine these — `HoverTipFactory.From<Keyword>()`
  pulls the base game's text automatically.
- Per-card keywords (Exhaust, Ethereal, Retain, ...) live in the base game's `card_keywords.json`
  with the same `"<KEY>.title"` / `"<KEY>.description"` shape.
- A mod only needs its own `static_hover_tips.json` / `card_keywords.json` if it's introducing a
  brand-new keyword the base game doesn't have, or overriding/retranslating an existing one. This
  mod doesn't currently have either file since it only reuses existing base-game keywords (Forge).
