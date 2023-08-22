﻿using Terraria;
using Terraria.ModLoader;

namespace Arknights.SceneEffects
{
	internal class ArknightsDaytimeScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Sounds/Music/lifeglow");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
		public override bool IsSceneEffectActive(Player player) => Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneOverworldHeight && Main.dayTime && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach;
	}
}
