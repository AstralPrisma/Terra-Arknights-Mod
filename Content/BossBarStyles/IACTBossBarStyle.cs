using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.BigProgressBar;

namespace Arknights.Content.BossBars
{
	public class IACTBossBarStyle : ModBossBarStyle
	{
		public override bool PreventDraw => true;
		public override void Draw(SpriteBatch spriteBatch, IBigProgressBar currentBar, BigProgressBarInfo info)
		{
			if (currentBar == null)
			{
				return;
			}
			if (currentBar is CommonBossBigProgressBar)
			{
				NPC npc = Main.npc[info.npcIndexToAimAt];
				float lifePercent = Utils.Clamp(npc.life / (float)npc.lifeMax, 0f, 1f);
				BigProgressBarHelper.DrawBareBonesBar(spriteBatch, lifePercent);
			}
			else
			{
				currentBar.Draw(ref info, spriteBatch);
			}
		}
	}
}