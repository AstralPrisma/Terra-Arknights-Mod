using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.GameContent;
using Terraria.ModLoader;
using Arknights.NPCs;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using MonoMod.Cil;
using System.Reflection;

namespace Arknights
{
	public class Arknights : Mod
	{
		//public static Arknights instance;

		public static Effect IACTSW;

		public override void Load()
		{			
			if (Main.netMode != NetmodeID.Server)//shader
			{
				//1.3屏幕效果通式：Filters.Scene["FilterName"] = new Filter(new ScreenShaderData(filterRef, "PassName"), EffectPriority.Medium);
				//Filters.Scene["IACTSW"] = new Filter(new ScreenShaderData(new Ref<Effect>(GetEffect("Effects/IACTSW")), "IACTSW"), EffectPriority.VeryHigh);
				//Filters.Scene["IACTSW"].Load();
				//1.4如下

				IACTSW = ModContent.Request<Effect>("Arknights/Effects/IACTSW", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
           		Filters.Scene["IACTSW"] = new Filter(new ScreenShaderData(new Terraria.Ref<Effect>(IACTSW), "IACTSW"), EffectPriority.VeryHigh);
				Filters.Scene["IACTSW"].Load();
			}
		}

		/*public override void UpdateMusic(ref int music, ref MusicPriority priority)//1.3音乐
        {
            if (!Main.gameMenu)
            {
                if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneOverworldHeight && Main.dayTime && !Main.player[Main.myPlayer].ZoneDesert)//白天
                {
                    music = ((Mod)this).GetSoundSlot((SoundType)51, "Sounds/Music/lifeglow");//生命流
                    priority = (MusicPriority)2;
                }
				else if (Main.player[Main.myPlayer].active && Main.player[Main.myPlayer].ZoneOverworldHeight && !Main.dayTime && !Main.player[Main.myPlayer].ZoneDesert)//夜晚
                {
                    music = ((Mod)this).GetSoundSlot((SoundType)51, "Sounds/Music/asisaw");//如我所见
                    priority = (MusicPriority)2;
                }
			}
		}*/
	}
}