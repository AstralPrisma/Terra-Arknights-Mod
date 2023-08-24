using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Arknights.Content.Effects;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Arknights.Content.Effects
{
    public class TestScreenShaderData : ScreenShaderData
    {
        public TestScreenShaderData(string passName) : base(passName)
        {
        }
        public TestScreenShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
        {
        }
        public override void Apply()
        {
            base.Apply();
        }
    }
}