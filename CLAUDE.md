# Read access

Reading (not writing) is pre-authorized, without asking first, across:
- this repo (`mod1`)
- `../spire-codex` (decompiled/extracted base-game source and localization — the reference for game mechanics, keyword/tooltip conventions, etc.)
- `../STS2-Buu` (sibling mod, useful for comparing conventions)
- `~/.nuget/packages` (NuGet cache, incl. `alchyr.sts2.baselib`/`alchyr.sts2.modanalyzers`)

Writes should still stay scoped to `mod1` unless explicitly asked otherwise.
