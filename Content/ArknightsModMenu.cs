using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.ID;

namespace Arknights.Content
{
	public class ArknightsModMenu : ModMenu
	{
		private const string menuAssetPath = "Arknights/Content/Menu";

		public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>($"{menuAssetPath}/TerraArk");

        public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/mrfz");

		public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>($"{menuAssetPath}/Sami2");

		public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/Music/arkopen");

		public override string DisplayName => "泰拉方舟 TerraArk";

		public override void OnSelected()
		{
			SoundEngine.PlaySound(new SoundStyle("Arknights/Content/Sounds/AmiyaArknights"));
		}
	}
}
