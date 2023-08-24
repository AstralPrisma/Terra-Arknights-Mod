﻿using Terraria;
using Terraria.ModLoader;

namespace Arknights.Content.SceneEffects
{
	internal class ArknightsOceanNighttimeScene : ModSceneEffect
	{
		public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/seawonder");
		public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
        public override bool IsSceneEffectActive(Player player) => Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneOverworldHeight && !Main.dayTime && Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneCorrupt && !Main.player[Main.myPlayer].ZoneCrimson;
    }
}
