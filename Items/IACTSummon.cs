using Arknights.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria.Localization;
using Terraria.DataStructures;

namespace Arknights.Items
{
	public class IACTSummon : ModItem
	{
		public override void SetStaticDefaults() 
        {
			DisplayName.SetDefault("Suspicious Remote Control");
            DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"奇怪的信号发射器");
			Tooltip.SetDefault("Maybe only need a tiny signal , it can call something [c/FF0000:dangerous].\n"+"Call [c/FF0000:the lost military nuke weapon]\n"+"[c/FF0000:Only during NIGHT]");
            Tooltip.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"似乎只要一个电讯号，这个玩意就会发出[c/FF0000:奇怪]的波段\n"+"召唤[c/FF0000:遗落的军用打击系统]\n"+"[c/FF0000:只能在晚上使用]");
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 12;
		}

		public override void SetDefaults()
        {
			Item.width = 38;
			Item.height = 40;
			Item.maxStack = 1;
			Item.rare = 6;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = 4;
			Item.consumable = false;
            Item.noUseGraphic = true;
            Item.scale = 0.01f;
            Item.UseSound = new SoundStyle($"{nameof(Arknights)}/Sounds/airstrike");
		}

        public override bool CanUseItem(Player player)
        {
            if (!Main.dayTime && !NPC.AnyNPCs(ModContent.NPCType<NPCs.U12.IACT>()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool? UseItem(Player player) 
        {
            int IACTboss = NPC.NewNPC(Terraria.Entity.GetSource_TownSpawn(),(int)player.Center.X, (int)player.Center.Y - 800, NPCType<NPCs.U12.IACT>());
            Main.npc[IACTboss].netUpdate = true;
            Main.NewText("你感受到了来自帝国的威胁...", 138, 0, 18);
			return true;
		}

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 25);
            recipe.AddIngredient(ItemID.Wire, 100);
            recipe.AddIngredient(ItemID.ExplosivePowder, 50);
            recipe.AddIngredient(ItemID.SoulofLight, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 20);
            recipe.AddIngredient(ItemID.SoulofFlight, 20);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddIngredient(ItemID.SoulofSight, 10);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
	}
}