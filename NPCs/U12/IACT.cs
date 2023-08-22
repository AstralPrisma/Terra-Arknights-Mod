using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;    
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Arknights.NPCs;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Arknights.BossBars;

namespace Arknights.NPCs.U12
{
	[AutoloadBossHead]
	public class IACT : ModNPC
	{
		private const string ChainTextPath="Arknights/NPCs/U12/IACT";
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperial Artillery Core Targeteer");
            DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"帝国炮火中枢先兆者");//翻译
			Main.npcFrameCount[NPC.type] = 1;//贴图帧数
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 16000;
			NPC.damage = 40;
			NPC.defense = 80;
			NPC.knockBackResist = 0f;//击退抗性，0f为最高，1f为最低
			NPC.width = 164;
			NPC.height = 72;
			NPC.noGravity = true;//无引力
			NPC.noTileCollide = true;//不与物块相撞
            NPC.lavaImmune = true;//免疫岩浆
			NPC.aiStyle = -1;
            NPC.boss = true;//BOSS
			NPC.HitSound = SoundID.NPCHit4;//金属声
			NPC.DeathSound = SoundID.NPCDeath14;//爆炸声
			NPC.buffImmune[BuffID.Confused] = true;//免疫混乱
			NPC.buffImmune[BuffID.Poisoned] = true;//免疫中毒
			NPC.buffImmune[BuffID.OnFire] = true;//免疫着火
			NPC.buffImmune[BuffID.Venom] = true;//免疫剧毒
			NPC.buffImmune[BuffID.Frostburn] = true;//免疫霜火
			NPC.buffImmune[BuffID.Frozen] = true;//免疫冰冻
			NPC.buffImmune[BuffID.Electrified] = true;//免疫带电
			NPC.buffImmune[BuffID.ShadowFlame] = true;//免疫暗影炎
			NPC.buffImmune[BuffID.Daybreak] = true;//免疫破晓
			NPC.buffImmune[BuffID.Ichor] = true;//免疫灵液
			NPC.buffImmune[BuffID.CursedInferno] = true;//免疫诅咒火
		}

		//NPC专家模式|大师模式血量倍率（普通模式血量*倍率*2|血量*倍率*3）
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * 0.75f * bossLifeScale);//16000|24000|36000
			NPC.damage = (int)(NPC.damage * 0.8f);//40|64|96
		}

		private float expertHealthFrac = Main.expertMode ? 0.9f : 0.75f;//进入第二阶段生命值百分比（12000/16000；21600/24000）
		
		//视觉效果部分
		#region 
		private int OAOScaleX;
		private int OAOScaleY;
		private int OARScaleX;
		private int IAORScaleX;
		private int IAORScaleY;
		private int IARScaleX;

		private int LightScale;
		private int LightScale2;
		private int LightScale3;

		private float OAOScaleEnter;
		private float OAOScale2Enter;
		private float OARScaleEnter;
		private float OAOScale;
		private float OAOScale2;
		private float IAORScale;
		private float OARScale;
		private float OAOScaleZero;
		private float OAOScale2Zero;
		private float IAORScaleZero;
		private float OARScaleZero;

		private float IACTLightScale0Enter;
		private float IACTLightScale2Enter;
		private float IACTLightScale0;
		private float IACTLightScale1;
		private float IACTLightScale2;
		private float IACTLightScale3;
		private float IACTLightScale4;
		private float IACTLightScale5;
		private float IACTLightScale0Zero;
		private float IACTLightScale1Zero;
		private float IACTLightScale2Zero;
		private float IACTLightScale3Zero;
		private float IACTLightScale4Zero;
		private float IACTLightScale5Zero;

		private float IAFRScale0Enter;
		private float IAFRScale2Enter;
		private float IAFRScale10Enter;
		private float IAFRScale0;
		private float IAFRScale2;
		private float IAFRScale4;
		private float IAFRScale6;
		private float IAFRScale8;
		private float IAFRScale10;
		private float IAFRScale0Zero;
		private float IAFRScale2Zero;
		private float IAFRScale4Zero;
		private float IAFRScale6Zero;
		private float IAFRScale8Zero;
		private float IAFRScale10Zero;

		private float IARSScale0Enter;
		private float IARSScale2Enter;
		private float IARSScale8Enter;
		private float IARSScale10Enter;
		private float IARSScale0;
		private float IARSScale2;
		private float IARSScale4;
		private float IARSScale6;
		private float IARSScale8;
		private float IARSScale10;
		private float IARSScale0Zero;
		private float IARSScale2Zero;
		private float IARSScale4Zero;
		private float IARSScale6Zero;
		private float IARSScale8Zero;
		private float IARSScale10Zero;

		private float IARScale1Enter;
		private float IARScale3Enter;
		private float IARScale5Enter;
		private float IARScale7Enter;
		private float IARScale1;
		private float IARScale3;
		private float IARScale5;
		private float IARScale7;
		private float IARScale1Zero;
		private float IARScale3Zero;
		private float IARScale5Zero;
		private float IARScale7Zero;

		private int stage1to2timer1;
		private int stage1to2timer2;
		private int stage2to3timer1;
		private int stage2to3timer2;
		private int stage2to3timer3;
		private int stage2to3timer4;
		private int stage3timer1;
		private int stage3timer2;
		private int stage3timer3;
		private int stage3timer4;
		private int stage3timer5;
		private int stage3timer6;
		private int stage3timer7;

		//NPC贴图黑暗高亮发光效果+光环效果
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)//贴图，位置，大小区域，光亮颜色，转动角度，中心点坐标，缩放倍率，特殊效果(翻转)，图层
		{
			if(crushed != true)//锁血后高亮消失
			{
				Texture2D maskTexture = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IACTMask").Value;//常驻高亮部分
				Main.EntitySpriteDraw(maskTexture, NPC.Center - Main.screenPosition + new Vector2(0,3), new Rectangle(0, 0, maskTexture.Width, maskTexture.Height), Color.White, NPC.rotation, new Vector2(maskTexture.Width / 2, maskTexture.Height / 2), 1f, SpriteEffects.None, 0);
			}
			
			//光环部分
			if(stage == 1 || stage == 1.5f)//一阶段+一转二阶段
            {
				OAOScaleX++;
				LightScale++;
				if(OAOScaleX >= 120)
				{
					OAOScaleX = 0;
				}//周期2秒
				if(LightScale >= 180)
				{
					LightScale = 0;
				}//周期3秒

				Texture2D OutringTexture = ModContent.Request<Texture2D>("Arknights/NPCs/U12/OAO").Value;
				Texture2D InringTexture = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IAOR").Value;
				Texture2D waveTexture = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IACTLightwave").Value;

				IAORScale = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f - Math.PI/2f)+0.475f);//内环缩放,周期4秒，在0~0.95倍之间变化
				if(timer1<=60)//刚进入一阶段的第一秒，光环从0增大
				{
					OAOScaleEnter = (float)(Math.Sin(Math.PI*OAOScaleX/120f));//引入过程，实现平滑过渡
					Main.EntitySpriteDraw(OutringTexture, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture.Width, OutringTexture.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture.Width / 2, OutringTexture.Height / 2), OAOScaleEnter, SpriteEffects.None, 0);
				}
				else//确实进入了一阶段（1秒后）
				{
					if(truestage1to2 == 0)//一阶段
					{
						OAOScale = (float)(0.0625f*Math.Sin(Math.PI*OAOScaleX/60f - Math.PI/2f)+0.9375f);//外环缩放,周期2秒，在1~0.875倍之间变化
						Main.EntitySpriteDraw(OutringTexture, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture.Width, OutringTexture.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture.Width / 2, OutringTexture.Height / 2), OAOScale, SpriteEffects.None, 0);
					}
					else//一转二
					{
						stage1to2timer1++;
						if(stage1to2timer1 >= 60)
						{
							stage1to2timer1 = 60;
						}
						OAOScaleZero = OAOScale-OAOScale*stage1to2timer1/60;
						IAORScaleZero = IAORScale-IAORScale*stage1to2timer1/60;
						Main.EntitySpriteDraw(OutringTexture, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture.Width, OutringTexture.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture.Width / 2, OutringTexture.Height / 2), OAOScaleZero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture.Width, InringTexture.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture.Width / 2, InringTexture.Height / 2), IAORScaleZero, SpriteEffects.None, 0);
					}
				}

				if(timer1<=90)//刚进入一阶段的第1.5秒，探照灯从0增大
				{
					IACTLightScale0Enter = (float)(Math.Sin(Math.PI*LightScale/180f));//引入过程，实现平滑过渡
					Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale0Enter , SpriteEffects.None, 0);
				}		
				else//确实进入了一阶段（1.5秒后）
				{
					if(truestage1to2 == 0)//一阶段的探照灯
					{
						IACTLightScale0 = (float)(0.15f*Math.Sin(Math.PI*LightScale/90f - Math.PI/2f)+0.85f);//灯光晕影大小变化,相位0（180）
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale0 , SpriteEffects.None, 0);
					}
					else//一转二的探照灯缩小回去
					{
						stage1to2timer2++;
						if(stage1to2timer2 >= 60)
						{
							stage1to2timer2 = 60;
						}
						IACTLightScale0Zero = IACTLightScale0-IACTLightScale0*stage1to2timer2/60;
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale0Zero , SpriteEffects.None, 0);
					}
				}
			}
			else if(stage == 2 || stage == 2.5f)//二阶段+二转三阶段
            {
				OAOScaleY++;
				IAORScaleX++;
				LightScale2++;
				if(OAOScaleY >= 120)
				{
					OAOScaleY = 0;
				}//周期2秒
				if(IAORScaleX >= 240)
				{
					IAORScaleX = 0;
				}//周期4秒
				if(LightScale2 >= 180)
				{
					LightScale2 = 0;
				}//周期3秒

				Texture2D OutringTexture2 = ModContent.Request<Texture2D>("Arknights/NPCs/U12/OAF").Value;
				Texture2D InringTexture2 = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IAFR").Value;
				Texture2D waveTexture = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IACTLightwave").Value;
				
				if(stage2timer<=20)//刚进入二阶段的第1/3秒，内圈的2、8应用于此处
				{
					IAFRScale2Enter = (float)(0.95*Math.Sin(Math.PI*IAORScaleX/40f));//引入过程，实现平滑过渡
					Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale2Enter, SpriteEffects.None, 0);
				}
				else//确实进入了二阶段（1/3秒后）
				{
					if(truestage2to3 == 0)//二阶段
					{
						IAFRScale2 = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f + Math.PI/3f)+0.475f);//多内环缩放,周期4秒，在0~0.95倍之间变化
						IAFRScale8 = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f + 4 * Math.PI/3f)+0.475f);//多内环缩放,周期4秒，在0~0.95倍之间变化
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale2, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale8, SpriteEffects.None, 0);
					}
					else//二转三的光环缩小回去
					{
						stage2to3timer3++;
						if(stage2to3timer3 >= 60)
						{
							stage2to3timer3 = 60;
						}
						IAFRScale2Zero = IAFRScale2-IAFRScale2*stage2to3timer3/60;
						IAFRScale8Zero = IAFRScale8-IAFRScale8*stage2to3timer3/60;
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale2Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale8Zero, SpriteEffects.None, 0);
					}
				}
				
				if(stage2timer<=60)//刚进入二阶段的第一秒，光环从0增大,内圈的0、6、10应用于此处
				{
					OAOScale2Enter = (float)(Math.Sin(Math.PI*OAOScaleY/120f));//引入过程，实现平滑过渡
					IAFRScale0Enter = (float)(0.95*Math.Sin(Math.PI*IAORScaleX/120f));
					IAFRScale10Enter = (float)(IAORScaleX/84.21f);
					Main.EntitySpriteDraw(OutringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture2.Width, OutringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture2.Width / 2, OutringTexture2.Height / 2), OAOScale2Enter, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale0Enter, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale10Enter, SpriteEffects.None, 0);
				}
				else//确实进入了二阶段（1秒后）
				{
					if(truestage2to3 == 0)//二阶段
					{
						OAOScale2 = (float)(0.0625f*Math.Sin(Math.PI*OAOScaleY/60f - Math.PI/2f)+0.9375f);//外环缩放,周期2秒，在1~0.875倍之间变化
						IAFRScale0 = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f)+0.475f);//多内环缩放,周期4秒，在0~0.95倍之间变化
						IAFRScale6 = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f + Math.PI)+0.475f);//多内环缩放,周期4秒，在0~0.95倍之间变化
						IAFRScale10 = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f + 5 * Math.PI/3f)+0.475f);//多内环缩放,周期4秒，在0~0.95倍之间变化
						Main.EntitySpriteDraw(OutringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture2.Width, OutringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture2.Width / 2, OutringTexture2.Height / 2), OAOScale2, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale0, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale6, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale10, SpriteEffects.None, 0);					
					}
					else//二转三的光环缩小回去
					{
						stage2to3timer1++;
						if(stage2to3timer1 >= 60)
						{
							stage2to3timer1 = 60;
						}
						OAOScale2Zero = OAOScale2-OAOScale2*stage2to3timer1/60;
						IAFRScale0Zero = IAFRScale0-IAFRScale0*stage2to3timer1/60;
						IAFRScale6Zero = IAFRScale6-IAFRScale6*stage2to3timer1/60;
						IAFRScale10Zero = IAFRScale10-IAFRScale10*stage2to3timer1/60;
						Main.EntitySpriteDraw(OutringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture2.Width, OutringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture2.Width / 2, OutringTexture2.Height / 2), OAOScale2Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale0Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale6Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale10Zero, SpriteEffects.None, 0);					
					}
				}

				IACTLightScale1 = (float)(0.5f*Math.Sin(Math.PI*LightScale2/90f - Math.PI/2f)+0.5f);//灯光晕影大小变化,相位0（180）
				if(stage2timer<=90)//刚进入二阶段的第1.5秒，探照灯从0增大
				{
					IACTLightScale2Enter = 0;//引入过程，实现平滑过渡
					Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale1 , SpriteEffects.None, 0);
					Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale2Enter , SpriteEffects.None, 0);
				}		
				else//确实进入了二阶段（1.5秒后）
				{
					if(truestage2to3 == 0)//二阶段的探照灯
					{
						IACTLightScale2 = (float)(0.5f*Math.Sin(Math.PI*LightScale2/90f + Math.PI/2f)+0.5f);//灯光晕影大小变化,相位90
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale1 , SpriteEffects.None, 0);
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale2, SpriteEffects.None, 0);
					}
					else//二转三的探照灯缩小回去
					{
						stage2to3timer2++;
						if(stage2to3timer2 >= 60)
						{
							stage2to3timer2 = 60;
						}
						IACTLightScale1Zero = IACTLightScale1-IACTLightScale1*stage2to3timer2/60;
						IACTLightScale2Zero = IACTLightScale2-IACTLightScale2*stage2to3timer2/60;
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale1Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale2Zero, SpriteEffects.None, 0);
					}
				}

				if(stage2timer>=100)//确实进入了二阶段（5/3秒后）
				{
					if(truestage2to3 == 0)//二阶段
					{
						IAFRScale4 = (float)(0.475f*Math.Sin(Math.PI*IAORScaleX/120f + 2 * Math.PI/3f)+0.475f);//多内环缩放,周期4秒，在0~0.95倍之间变化
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale4, SpriteEffects.None, 0);
					}
					else//二转三的光环缩小回去
					{
						stage2to3timer4++;
						if(stage2to3timer4 >= 60)
						{
							stage2to3timer4 = 60;
						}
						IAFRScale4Zero = IAFRScale4-IAFRScale4*stage2to3timer4/60;
						Main.EntitySpriteDraw(InringTexture2, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture2.Width, InringTexture2.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture2.Width / 2, InringTexture2.Height / 2), IAFRScale4Zero, SpriteEffects.None, 0);
					}
				}			
			}
			else if(stage >= 3)//三阶段
            {
				OARScaleX++;
				IAORScaleY++;
				IARScaleX++;
				LightScale3++;
				if(OARScaleX >= 120)
				{
					OARScaleX = 0;
				}//周期2秒
				if(IAORScaleY >= 240)
				{
					IAORScaleY = 0;
				}//周期4秒
				if(LightScale3 >= 180)
				{
					LightScale3 = 0;
				}//周期3秒
				if(IARScaleX >= 480)
				{
					IARScaleX = 0;
				}//周期8秒

				Texture2D OutringTexture3 = ModContent.Request<Texture2D>("Arknights/NPCs/U12/OARS").Value;
				Texture2D OutringTexture4 = ModContent.Request<Texture2D>("Arknights/NPCs/U12/OAR").Value;
				Texture2D InringTexture3 = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IARS").Value;
				Texture2D InringTexture4 = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IAR").Value;
				Texture2D waveTexture = ModContent.Request<Texture2D>("Arknights/NPCs/U12/IACTLightwave").Value;

				if(crushed != true)//探照灯之一
				{
					IACTLightScale3 = (float)(0.5f*Math.Sin(Math.PI*LightScale3/90f - Math.PI/2f)+0.5f);//灯光晕影大小变化,相位0（180）
					Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale3, SpriteEffects.None, 0);
				}
				else//坠毁时光环缩小回去
				{
					stage3timer1++;
					if(stage3timer1 >= 60)
					{
						stage3timer1 = 60;
					}
					IACTLightScale3Zero = IACTLightScale3-IACTLightScale3*stage3timer1/60;
					Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale3Zero, SpriteEffects.None, 0);
				}

				if(stage3timer<=20)//刚进入三阶段的第1/3秒，内圈的8应用于此处
				{
					IARSScale8Enter = 0;
					Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale8Enter, SpriteEffects.None, 0);   
				}
				else//确实进入了三阶段（1/3秒后）
				{
					if(crushed != true)//坠毁之前
					{
						IARSScale8 = (float)(0.25f*Math.Sin(Math.PI*IAORScaleY/120f + 4 * Math.PI/3f)+0.25f);
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale8, SpriteEffects.None, 0);   
					}
					else//坠毁时光环缩小回去
					{
						stage3timer2++;
						if(stage3timer2 >= 60)
						{
							stage3timer2 = 60;
						}
						IARSScale8Zero = IARSScale8-IARSScale8*stage3timer2/60;
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale8Zero, SpriteEffects.None, 0);   
					}
				}
				
				if(stage3timer<=60)//刚进入三阶段的第一秒，光环从0增大,内圈的0、6、10应用于此处
				{
					OARScaleEnter = (float)(0.875*Math.Sin(Math.PI*OARScaleX/120f));//引入过程，实现平滑过渡
					IARSScale0Enter = (float)(0.5*Math.Sin(Math.PI*IAORScaleY/120f));
					IARSScale10Enter = (float)(1/160*IAORScaleY);
					Main.EntitySpriteDraw(OutringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture3.Width, OutringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture3.Width / 2, OutringTexture3.Height / 2), OARScaleEnter, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(OutringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture4.Width, OutringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture4.Width / 2, OutringTexture4.Height / 2), OARScaleEnter, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale0Enter, SpriteEffects.None, 0);   
					Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale10Enter, SpriteEffects.None, 0);		
				}
				else//确实进入了三阶段（1秒后）
				{
					if(crushed != true)//坠毁前
					{
						OARScale = (float)(0.0625f*Math.Sin(Math.PI*OARScaleX/60f + Math.PI/2f)+0.9375f);//外环缩放,周期2秒，在1~0.875倍之间变化
						IARSScale0 = (float)(0.25f*Math.Sin(Math.PI*IAORScaleY/120f)+0.25f);//多内环缩放,周期4秒，在0~0.5倍之间变化
						IARSScale6 = (float)(0.25f*Math.Sin(Math.PI*IAORScaleY/120f + Math.PI)+0.25f);
						IARSScale10 = (float)(0.25f*Math.Sin(Math.PI*IAORScaleY/120f + 5 * Math.PI/3f)+0.25f);
						IACTLightScale4 = (float)(0.5f*Math.Sin(Math.PI*LightScale3/90f + 5*Math.PI/6f)+0.5f);//灯光晕影大小变化,相位60
						Main.EntitySpriteDraw(OutringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture3.Width, OutringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture3.Width / 2, OutringTexture3.Height / 2), OARScale, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(OutringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture4.Width, OutringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture4.Width / 2, OutringTexture4.Height / 2), OARScale, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale0, SpriteEffects.None, 0);   
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale6, SpriteEffects.None, 0);   
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale10, SpriteEffects.None, 0);		
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale4, SpriteEffects.None, 0);
					}
					else//坠毁时光环缩小回去
					{
						stage3timer3++;
						if(stage3timer3 >= 60)
						{
							stage3timer3 = 60;
						}
						OARScaleZero = OARScale-OARScale*stage3timer3/60;
						IARSScale0Zero = IARSScale0-IARSScale0*stage3timer3/60;
						IARSScale6Zero = IARSScale6-IARSScale6*stage3timer3/60;
						IARSScale10Zero = IARSScale10-IARSScale10*stage3timer3/60;
						IACTLightScale4Zero = IACTLightScale4-IACTLightScale4*stage3timer3/60;
						Main.EntitySpriteDraw(OutringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture3.Width, OutringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture3.Width / 2, OutringTexture3.Height / 2), OARScaleZero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(OutringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, OutringTexture4.Width, OutringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(OutringTexture4.Width / 2, OutringTexture4.Height / 2), OARScaleZero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale0Zero, SpriteEffects.None, 0);   
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale6Zero, SpriteEffects.None, 0);   
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale10Zero, SpriteEffects.None, 0);		
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale4Zero, SpriteEffects.None, 0);
					}
				}

				if(stage3timer >=100)
				{
					if(crushed != true)
					{
						IARSScale4 = (float)(0.25f*Math.Sin(Math.PI*IAORScaleY/120f + 2 * Math.PI/3f)+0.25f);
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale4, SpriteEffects.None, 0);		
					}
					else//坠毁时光环缩小回去
					{
						stage3timer4++;
						if(stage3timer4 >= 60)
						{
							stage3timer4 = 60;
						}
						IARSScale4Zero = IARSScale4-IARSScale4*stage3timer4/60;
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale4Zero, SpriteEffects.None, 0);		
					}
				}

				if(stage3timer<=120)//刚进入三阶段的前二秒，光环从0增大,内圈的1、5应用于此处
				{
					IARScale1Enter = (float)(0.73f*Math.Sin(Math.PI*IARScaleX/240f));
					IARScale5Enter = (float)(0.47f*Math.Sin(Math.PI*IARScaleX/240f));
					Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale1Enter, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale5Enter, SpriteEffects.None, 0);
				}
				else//确实进入了三阶段（2秒后）
				{
					if(crushed != true)//坠毁前
					{
						IARScale1 = (float)(0.13f*Math.Sin(Math.PI*IARScaleX/240f)+0.6f);
						IARScale5 = (float)(0.13f*Math.Sin(Math.PI*IARScaleX/240f + Math.PI)+0.6f);
						IACTLightScale5 = (float)(0.5f*Math.Sin(Math.PI*LightScale3/90f + Math.PI/6f)+0.5f);//灯光晕影大小变化,相位120
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale1, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale5, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale5, SpriteEffects.None, 0);			
					}
					else//坠毁时光环缩小回去
					{
						stage3timer5++;
						if(stage3timer5 >= 60)
						{
							stage3timer5 = 60;
						}
						IARScale1Zero = IARScale1-IARScale1*stage3timer5/60;
						IARScale5Zero = IARScale5-IARScale5*stage3timer5/60;
						IACTLightScale5Zero = IACTLightScale5-IACTLightScale5*stage3timer5/60;
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale1Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale5Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(waveTexture, NPC.Center - Main.screenPosition + new Vector2(0,25), new Rectangle(0, 0, waveTexture.Width, waveTexture.Height), Color.Red, NPC.rotation, new Vector2(waveTexture.Width / 2, 0), IACTLightScale5Zero, SpriteEffects.None, 0);
					}
				}

				if(stage3timer<=140)//刚进入三阶段的第7/3秒，内圈的2应用于此处
				{
					//IARSScale2Enter = (float)(0.5*Math.Sin(Math.PI*IAORScaleY/40f));//引入过程，实现平滑过渡
					IARSScale2Enter = 0;
					Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale2Enter, SpriteEffects.None, 0);   
				}
				else//确实进入了三阶段（7/3秒后）
				{
					if(crushed != true)//坠毁之前
					{
						IARSScale2 = (float)(0.25f*Math.Sin(Math.PI*IAORScaleY/120f + Math.PI/3f)+0.25f);
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale2, SpriteEffects.None, 0);   
					}
					else//坠毁时光环缩小回去
					{
						stage3timer6++;
						if(stage3timer6 >= 60)
						{
							stage3timer6 = 60;
						}
						IARSScale2Zero = IARSScale2-IARSScale2*stage3timer6/60;
						Main.EntitySpriteDraw(InringTexture3, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture3.Width, InringTexture3.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture3.Width / 2, InringTexture3.Height / 2), IARSScale2Zero, SpriteEffects.None, 0);   
					}
				}

				if(stage3timer<=240)//刚进入三阶段的前四秒，光环从0增大,内圈的3、7应用于此处
				{
					IARScale3Enter = (float)(0.47f*Math.Sin(Math.PI*IARScaleX/480f));
					IARScale7Enter = (float)(0.73f*Math.Sin(Math.PI*IARScaleX/480f));
					Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale3Enter, SpriteEffects.None, 0);
					Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale7Enter, SpriteEffects.None, 0);
				}
				else//确实进入了三阶段（4秒后）
				{
					if(crushed != true)//坠毁前
					{
						IARScale3 = (float)(0.13f*Math.Sin(Math.PI*IARScaleX/240f + Math.PI/2f)+0.6f);
						IARScale7 = (float)(0.13f*Math.Sin(Math.PI*IARScaleX/240f + 3 * Math.PI/2f)+0.6f);
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale3, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale7, SpriteEffects.None, 0);
					}
					else//坠毁时光环缩小回去
					{
						stage3timer7++;
						if(stage3timer7 >= 60)
						{
							stage3timer7 = 60;
						}
						IARScale3Zero = IARScale3-IARScale3*stage3timer7/60;
						IARScale7Zero = IARScale7-IARScale7*stage3timer7/60;
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale3Zero, SpriteEffects.None, 0);
						Main.EntitySpriteDraw(InringTexture4, NPC.Center - Main.screenPosition + new Vector2(0, 3), new Rectangle(0, 0, InringTexture4.Width, InringTexture4.Height), Color.White, (float)(0 * NPC.rotation), new Vector2(InringTexture4.Width / 2, InringTexture4.Height / 2), IARScale7Zero, SpriteEffects.None, 0);
					}
				}
			}
        }

		private float MISSChance = 0f;

        public override bool? CanBeHitByProjectile(Projectile Projectile)//不被敌方弹幕和无来源弹幕攻击&闪避
        {
			if(Projectile.hostile == true)
			{
				return false;
			}
			else if(Projectile.friendly == true)
			{
				MISSChance = Main.rand.NextFloat(1);
				if(MISSChance>0.9f)
				{
				    //CombatText.NewText(new Rectangle((int)NPC.Center.X, (int)NPC.Center.Y, 10, 10), new Color(250,180,0) , "MISS", false, false);//好像会一直弹
					MISSChance = 0f;
					return false;
				}
				else
				{
					return true;
				}
			}
			else
			{
				return false;
			}	
        }

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)//击落后隐藏下方血条
        {
			scale = 1.5f;
			if(crushed == true)
			{
				position += new Vector2(0, 10000);
				return true;
			}
			else
			{
				return true;
			}
        }

		public override void BossHeadRotation(ref float rotation)//boss地图图标旋转
		{
			BossHeadRotate++;
			rotation = BossHeadRotate / 10;
		}

		public static int HeadSlot = -1;
		public override void Load()
		{
			string texture = BossHeadTexture;
			HeadSlot = Mod.AddBossHeadTexture(texture, -1);
		}

		public override void BossHeadSlot(ref int index)//击落后不显示图标和特殊血条
        {
			int slot = HeadSlot;
			index = slot;
        }

		public override void HitEffect(int hitDirection, double damage)//击中效果
		{
			Dust.NewDust(NPC.Center, 10, 10, 219, hitDirection, 0, -1, new Color(255,255,255), 1f);
		}
        #endregion

        //AI
        #region
        private bool ontransform = false;//转阶段判定器
		private float BossHeadRotate;
		private int truestage1to2;//进入第二阶段开始检测器（即一转二开始检测器）
		private int truestage2;//进入第二阶段锁血解除检测器（即一转二结束检测器）
		private int truestage2to3;//二转三开始检测器
		private int truestage3;
		private int timer1;//玩家位置发射计时器
		private int timer2;//固定判定点发射计时器
		private int stage2timer;//第二阶段开始计时
		private int stage3timer;
		private float stage;//弹出文本计次器兼阶段标记
		private int deathtimer;//伪死亡计时
		private int deathcheck;//checkdead触发检测
		private float prefire;//预判速度倍率
		private int playerprefire;//预判攻击间隔
		private float downAcceleration;//死亡坠落加速度
		private float timer1to2;//一转二开始计时
		private float timer2to3;//二转三开始计时
		private float targetX;//转换周期时的瞄准点坐标
		private float targetY;//转换周期时的瞄准点坐标
		private float vx;//横向速度
		private float vy;//纵向速度
		private float stage1to2r;//一阶段转二阶段绕圈半径
		private float stage1to2atkspeed;//一阶段转二阶段绕圈攻击间隔
		private float stage1to2locktime;//一阶段转二阶段锁血时间
		private float stage2to3r;
		private float stage2to3atkspeed;
		private float stage2to3locktime;
		private bool crushed = false;//是否坠毁？
		private float endtimer;
		private float endexplodespeed;
		
		public override void AI()
		{
			//动态转角
			NPC.rotation = 0.01f*(float)NPC.velocity.X;

			if(Main.masterMode)//自定义血条
			{
				NPC.BossBar = ModContent.GetInstance<IACTBossBarMT>();//大师模式血条,16200|32400/36000
			}
			else if(Main.expertMode)
			{
				NPC.BossBar = ModContent.GetInstance<IACTBossBarEX>();//专家模式血条,10800|21600/24000
			}
				else
			{
				NPC.BossBar = ModContent.GetInstance<IACTBossBarNM>();//普通模式血条,06000|12000/16000
			}

			if(stage <= 1)
			{
            	Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/IACTBoss1");//音乐一
			}
			else if(timer1to2 > 0 && timer1to2 <= 120)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Arknights/Sounds/null");//一转二阶段
			}
			else if(timer2to3 > 0 && timer2to3 <= 240)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Arknights/Sounds/null");//二转三阶段
			}
			else if(stage == 2)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/IACTBoss2");//音乐二
			}
			else if(stage >= 3)
			{
				Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/IACTBoss3");//音乐三
			}

			var newSource = NPC.GetSource_FromThis();
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), 0.5f, 0f, 0.25f);//发光

			Player Player = Main.player[NPC.target];//仇恨判定和死亡判定
			if (!Player.active || Player.dead)
			{
				NPC.TargetClosest(false);
				Player = Main.player[NPC.target];
				if (Player.dead)
				{
					if (NPC.timeLeft > 15)
					{
						NPC.timeLeft = 15;
					}
					if (NPC.velocity.Y > -8)
					{
						NPC.velocity.Y -= 0.5f;
					}
					return;
				}
			}

			if(crushed != true)//拖尾特效，坠毁时就没有了
			{
				if(ontransform != true)//常态
				{
					Projectile.NewProjectile(newSource,NPC.position.X + 85, NPC.position.Y + 47, 0, 0, 90, (int)(NPC.damage * 0), 0f, 0, 0);//蓝色
				}
				else//转阶段
				{
					Dust taildust = Terraria.Dust.NewDustPerfect(NPC.position + new Vector2(85,47), 130, new Vector2(0f, 0f), 0, new Color(255,255,255), 1.4f);//红色
					taildust.noGravity = true;
				}
			}

			timer1++;
			timer2++;
			//阶段判定区
			if(stage == 0)
			{
				Main.NewText("发现可疑目标，予以抹除.", 240, 0, 0);//出场提示语
				stage = 1;//出场时文本计数=1，也即第一阶段
			}
			if(stage == 1 && NPC.life < NPC.lifeMax * expertHealthFrac)//第二阶段提示语（12000/16000；21600/24000；32400/36000）
			{
				Main.NewText("目标强度高于预测强度，调用高阶权限实施打击.", 240, 0, 0);
				truestage1to2 = 1;//开始一转二
				stage = 1.5f;//文本计数=1.5，也即一转二阶段
			}
			if(stage == 2 && NPC.life < NPC.lifeMax * expertHealthFrac / 2)//第三阶段提示语（6000/16000；10800/24000；16200/36000）
			{
				Main.NewText("目标强度过高，调用最终权限实施打击.", 240, 0, 0);
				truestage2to3 = 1;//开始二转三
				stage = 2.5f;//文本计数=2.5，也即二转三阶段
			}

			if(NPC.life <= NPC.lifeMax * expertHealthFrac)//三个阶段更改伤害值和护盾
			{
				if(NPC.life <= NPC.lifeMax * expertHealthFrac/2)//三阶段
				{
					NPC.defense = 30;
					NPC.damage = 96;
				}
				else//二阶段
				{
					NPC.defense = 40;
					NPC.damage = 64;
				}
			}
			else//一阶段
			{
				NPC.defense = 50;
				NPC.damage = 40;
			}

			//预判机制
			if(Main.masterMode)
			{
				playerprefire = 120;//攻击频率
				prefire = 72;//72倍预判单位，等于准星
			}
			else if(Main.expertMode)
			{
				playerprefire = 150;
				prefire = 60;
			}
			else
			{
				playerprefire = 180;
				prefire = 54;
			}

			//主体AI
			if(NPC.life > 1 && NPC.dontTakeDamage != true && stage != 3)//一二阶段
			{
				//2阶段之后的玩家判定区
				if(truestage2 == 1 && timer1 >= playerprefire)
				{
					Projectile.NewProjectile(newSource,Player.Center.X+prefire*Player.velocity.X,Player.Center.Y+prefire*Player.velocity.Y, 0, 0, ModContent.ProjectileType<HitboxblueCore>(),0, 0, 0 , 0,0);
					Projectile.NewProjectile(newSource,Player.Center.X+prefire*Player.velocity.X,Player.Center.Y+prefire*Player.velocity.Y, 0, 0, ModContent.ProjectileType<HitboxblueFrame>(), 0 , 0 , 0 , 0,0);
					timer1 = 0;
				}

				//固定判定区，3秒一个
				if(timer2 >= 180)
				{	
					timer2 = 0;
					if(NPC.life < NPC.lifeMax * expertHealthFrac)//第二阶段
					{
						Projectile.NewProjectile(newSource,NPC.position.X + 82, NPC.position.Y + 43, 0, 0, ModContent.ProjectileType<AuraredPro>(),(int)(NPC.damage * 0.33), 0f, 0, 0);
						Projectile.NewProjectile(newSource,NPC.position.X + 82, NPC.position.Y + 43, 0, 0, ModContent.ProjectileType<HitboxgreenPro>(),NPC.damage, 0f, 0, 0);
					}
					else//第一阶段
					{
						SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/AlertPro") with { Volume = 1f, Pitch = 0f }, NPC.Center);
						Projectile.NewProjectile(newSource,NPC.position.X + 82, NPC.position.Y + 43, 0, 0, ModContent.ProjectileType<AuragreenPro>(),(int)(NPC.damage * 0.1), 0f, 0,0);
						Projectile.NewProjectile(newSource,NPC.position.X + 82, NPC.position.Y + 43, 0, 0, ModContent.ProjectileType<Hitboxgreen>(),NPC.damage, 0f, 0, 0);
					}
				}

				//移动方式
				Vector2 velDiff = NPC.velocity - Player.velocity;
				float ax = 0.3f;
				float ay = 0.2f;
				int haltDirectionX = velDiff.X > 0 ? 1 : -1;
				int haltDirectionY = velDiff.Y > 0 ? 1 : -1;
				float haltPointX = NPC.Center.X + haltDirectionX * (velDiff.X * velDiff.X) / (2 * ax);	
				float haltPointY = NPC.Center.Y + haltDirectionY * (velDiff.Y * velDiff.Y) / (2 * ay);
				//基础移动速度设置
				if(Main.masterMode)
				{
					if(stage == 2)
					{
						vx = 8f;vy = 8f;
					}
					if(stage == 1)
					{
						vx = 6f;vy = 6f;
					}
				}
				else if(Main.expertMode)
				{
					if(stage == 2)
					{
						vx = 7f;vy = 7f;
					}
					if(stage == 1)
					{
						vx = 6f;vy = 5f;
					}
				}
				else
				{
					if(stage == 2)
					{
						vx = 6f;vy = 6f;
					}
					if(stage == 1)
					{
						vx = 5f;vy = 4f;
					}
				}

				if (Player.Center.X > haltPointX)
				{
					NPC.velocity.X += ax;
				}
				else
				{
					NPC.velocity.X -= ax;
				}
				NPC.velocity.X = Math.Min(vx, Math.Max(-vx, NPC.velocity.X));

				if (Player.Center.Y > haltPointY)
				{
					NPC.velocity.Y += ay;
				}
				else
				{
					NPC.velocity.Y -= ay;
				}
				NPC.velocity.Y = Math.Min(vy, Math.Max(-vy, NPC.velocity.Y));
			}

			if(truestage1to2 == 1)//一二阶段转阶段锁血以及攻击
			{
				if(timer1to2>=120)
				{
					ontransform = true;
				}

				NPC.life = (int)(NPC.lifeMax * expertHealthFrac);//先卡在线上

				if(Main.masterMode)
				{
					stage1to2locktime = 420;
					stage1to2r = 300;
					stage1to2atkspeed = 20;
				}
				else if(Main.expertMode)
				{
					stage1to2locktime = 480;
					stage1to2r = 400;
					stage1to2atkspeed = 30;
				}
				else
				{
					stage1to2locktime = 480;
					stage1to2r = 500;
					stage1to2atkspeed = 45;
				}

			 	if(timer1to2 <= stage1to2locktime)
				{
					timer1to2++;
					NPC.dontTakeDamage = true;
					if(timer1to2 < 120)
					{
						NPC.velocity = Vector2.Lerp(NPC.velocity,Vector2.Zero,0.05f);
					}
					else if(timer1to2 >= 120 && timer1to2 < 180)
					{
						if((int)timer1to2 == 120)
						{
							SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/IACTStageChangeTip") with { Volume = 1f, Pitch = 0f }, Player.Center);
						}
						NPC.life = (int)(NPC.lifeMax * expertHealthFrac - 1);
						stage = 2;
					}
					else if(timer1to2 >= 180 && timer1to2 < 240)
					{
						Vector2 targetPosition = Player.Center + new Vector2(targetX, -stage1to2r);
						Vector2 targetv = 20f*(targetPosition - NPC.Center).SafeNormalize(Vector2.One);
						NPC.velocity = Vector2.Lerp(NPC.velocity,targetv,0.05f);
					}
					else if(timer1to2 >= 240 && timer1to2 < 300)
					{
						Vector2 targetPosition = Player.Center + new Vector2(targetX, -stage1to2r);
						Vector2 targetv = 40f*(targetPosition - NPC.Center).SafeNormalize(Vector2.One);
						NPC.velocity = Vector2.Lerp(NPC.velocity,targetv,0.05f);
					}
					else if(timer1to2 >= 300)
					{
						if((int)timer1to2 == 300)
						{
							SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/iactstage1to2") with { Volume = 1f, Pitch = 0f }, Player.Center);
						}
						targetX = (float)(stage1to2r*Math.Sin(2*(timer1to2-300)*Math.PI/(stage1to2locktime-300)));
						targetY = (float)(-stage1to2r*Math.Cos(2*(timer1to2-300)*Math.PI/(stage1to2locktime-300)));
						NPC.velocity = new Vector2((float)Player.Center.X, (float)Player.Center.Y) + new Vector2(targetX, targetY) - NPC.Center;//期间的位置变动
						if ((int)timer1to2 % stage1to2atkspeed == 0)
						{
							Projectile.NewProjectile(newSource,NPC.Center.X, NPC.Center.Y , 0, 0, ModContent.ProjectileType<Hitboxred>(),(int)(NPC.damage), 0f,  0, 0);
						}
					}
				}
				else
				{
					Main.NewText("估算目标运动趋势，实施精确打击.", 240, 0, 0);
					truestage1to2 = 2;
					truestage2 = 1;
					NPC.dontTakeDamage = false;
					ontransform = false;
				}
			}

			if(stage == 2)
			{
				stage2timer++;
			}
			//二三阶段转阶段锁血以及攻击
			if(truestage2to3 == 1)
			{
				if(timer2to3>=240)
				{
					ontransform = true;
				}

				NPC.life = (int)(NPC.lifeMax * expertHealthFrac/2);//先卡在线上

				if(Main.masterMode)
				{
					stage2to3locktime = 600;
					stage2to3r = 300;
					//stage2to3atkspeed = (int)(Main.rand.NextFloat(10,20));
					stage2to3atkspeed = 15;
				}
				else if(Main.expertMode)
				{
					stage2to3locktime = 660;
					stage2to3r = 400;
					//stage2to3atkspeed =(int)(Main.rand.NextFloat(20,30));
					stage2to3atkspeed = 20;
				}
				else
				{
					stage2to3locktime = 660;
					stage2to3r = 500;
					//stage2to3atkspeed = (int)(Main.rand.NextFloat(30,45));
					stage2to3atkspeed = 30;
				}

			 	if(timer2to3 <= stage2to3locktime)
				{
					timer2to3++;
					NPC.dontTakeDamage = true;
					if(timer2to3 < 240)
					{
						NPC.velocity = Vector2.Lerp(NPC.velocity,Vector2.Zero,0.01f);
						if((int)timer2to3 == 235)
						{
							Projectile.NewProjectile(newSource,NPC.Center.X, NPC.Center.Y , 0, 0, ModContent.ProjectileType<IACTScreenWave>(),0, 0f,  0, 0);
						}
						if((int)timer2to3 == 238)
						{
							Player.GetModPlayer<ArknightsPlayer>().screenShakeOnlyOnY = true;//纵向震动
							Player.GetModPlayer<ArknightsPlayer>().screenShakeTime = 15;//屏幕抖动
						}
					}
					else if(timer2to3 >= 240 && timer2to3 < 360)
					{
						if((int)timer2to3 == 240)
						{
							SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/IACTStageChangeTremor") with { Volume = 1f, Pitch = 0f }, Player.Center);
						}
						NPC.life = (int)(NPC.lifeMax * expertHealthFrac/2 - 1);
						stage = 3;
					}
					else if(timer2to3 >= 360 && timer2to3 < 420)
					{
						Vector2 targetPosition = Player.Center + new Vector2(targetX, -stage2to3r);
						Vector2 targetv = 20f*(targetPosition - NPC.Center).SafeNormalize(Vector2.One);
						NPC.velocity = Vector2.Lerp(NPC.velocity,targetv,0.05f);
					}
					else if(timer2to3 >= 420 && timer2to3 < 480)
					{
						Vector2 targetPosition = Player.Center + new Vector2(targetX, -stage2to3r);
						Vector2 targetv = 40f*(targetPosition - NPC.Center).SafeNormalize(Vector2.One);
						NPC.velocity = Vector2.Lerp(NPC.velocity,targetv,0.05f);
					}
					else if(timer2to3 >= 480)
					{
						if((int)timer2to3 == 480)
						{
							SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/iactstage1to2") with { Volume = 1f, Pitch = 0f }, Player.Center);
						}
						targetX = (float)(stage2to3r*Math.Sin(2*(timer2to3-480)*Math.PI/(stage2to3locktime-480)));
						targetY = (float)(-stage2to3r*Math.Cos(2*(timer2to3-480)*Math.PI/(stage2to3locktime-480)));
						NPC.velocity = new Vector2((float)Player.Center.X, (float)Player.Center.Y) + new Vector2(targetX, targetY) - NPC.Center;//期间的位置变动
						if ((int)timer2to3 % stage2to3atkspeed == 0)
						{
							Projectile.NewProjectile(newSource,NPC.Center.X, NPC.Center.Y , 0, 0, ModContent.ProjectileType<HitboxredPro>(),(int)(NPC.damage), 0f,  0, 0);
						}
					}
				}
				else
				{
					Main.NewText("已获得最高权限，启动多功能辅助系统.", 240, 0, 0);
					truestage2to3 = 2;
					truestage3 = 1;
					NPC.dontTakeDamage = false;
					ontransform = false;
				}
			}

			if(stage == 3)
			{
				NPC.velocity = Vector2.Lerp(NPC.velocity,Vector2.Zero,0.01f);
				stage3timer++;
			}

			if(NPC.life <= 1)//坠毁及其保护机制
			{
				if(deathcheck == 1)//触发checkdead之后
				{
					endtimer++;
					if(endtimer < 180)//先照例悬停
					{
                        NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Zero, 0.05f);

                    }
					else if(endtimer >= 180 && endtimer < 240)
					{
                        if (stage == 3)
                        {
                            Main.NewText("检测到主结构严重损毁，启动自毁模式......", 240, 0, 0);
                            ontransform = true;
							stage += 1;
                        }
                        Vector2 targetPosition = Player.Center + new Vector2(targetX, -360);
                        Vector2 targetv = 20f * (targetPosition - NPC.Center).SafeNormalize(Vector2.One);
                        NPC.velocity = Vector2.Lerp(NPC.velocity, targetv, 0.05f);
                    }
					else if(endtimer >= 240 && endtimer < 360)
					{
                        Vector2 targetPosition = Player.Center + new Vector2(targetX, -360);
                        Vector2 targetv = 40f * (targetPosition - NPC.Center).SafeNormalize(Vector2.One);
                        NPC.velocity = Vector2.Lerp(NPC.velocity, targetv, 0.05f);
                    }
                    else if (endtimer >= 360 && endtimer < 480)
                    {
                        if ((int)endtimer == 360)
                        {
                            SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/iactstage1to2") with { Volume = 1f, Pitch = 0f }, Player.Center);
                        }
                        targetX = (float)(360 * Math.Sin(2 * (endtimer - 360) * Math.PI / 180));
                        targetY = (float)(-360 * Math.Cos(2 * (endtimer - 360) * Math.PI / 180));
                        NPC.velocity = new Vector2((float)Player.Center.X, (float)Player.Center.Y) + new Vector2(targetX, targetY) - NPC.Center;//期间的位置变动
                        endexplodespeed = (int)(Main.rand.NextFloat(0, 60));
                        if ((int)endexplodespeed >= 57)
                        {
                            Projectile.NewProjectile(newSource, NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<HitboxredPro>(), (int)(NPC.damage), 0f, 0, 0);
                        }
                    }
					else if (endtimer >= 480)
					{
						crushed = true;
						NPC.noTileCollide = false;//与物块相撞	
						deathtimer++;
						if(deathtimer >= 180)//下坠3秒后爆炸并触发爆炸粒子
						{
							deathcheck = 2;
							Projectile.NewProjectile(newSource,NPC.Center.X, NPC.Center.Y,0,0,ModContent.ProjectileType<Deathdust>(),0,0f,0,0);//爆炸粒子
						}
						NPC.velocity.X = NPC.velocity.X * (360 - deathtimer)/360;
						downAcceleration = deathtimer * 0.01f;
						if(downAcceleration > 0.5f)
						{
							downAcceleration = 0.5f;
						}
						NPC.velocity.Y += downAcceleration;
						Vector2 dustPos = NPC.Center + new Vector2(Main.rand.NextFloat(12), 0).RotatedByRandom(MathHelper.TwoPi);
						for(int i = 0 ; i < 2 ; i++)
						{
							Dust dust8 = Dust.NewDustPerfect(dustPos, 219, Velocity: Vector2.Zero, Scale: 1.35f);
							dust8.velocity = (3*dustPos - 3*NPC.Center);
						}
					}
				}
				if(deathcheck == 2)//赐死
				{
					NPC.dontTakeDamage = false;
					NPC.life = 0;
					NPC.checkDead();
				}
			}
		}

		public override bool CheckDead()//锁血及锁血解除
		{
			if(deathcheck == 2)//死亡
			{
				if (!Main.dedServ)//Gore
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)Main.rand.Next(-5, 5) * 0.6f, (float)Main.rand.Next(-40, -20) * 0.6f), Mod.Find<ModGore>("IACT Gore 1").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)Main.rand.Next(-30, 31) * 0.6f, (float)Main.rand.Next(-30, 31) * 0.6f), Mod.Find<ModGore>("IACT Gore 2").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)Main.rand.Next(-30, 31) * 0.6f, (float)Main.rand.Next(-30, 31) * 0.6f), Mod.Find<ModGore>("IACT Gore 3").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, new Vector2((float)Main.rand.Next(-30, 31) * 0.6f, (float)Main.rand.Next(-30, 31) * 0.6f), Mod.Find<ModGore>("IACT Gore 4").Type, 1f);
                }
				SoundEngine.PlaySound(new SoundStyle("Terraria/Sounds/NPC_Killed_14") with { Volume = 3f, Pitch = 0f }, NPC.Center);//死亡音效
				Main.NewText("来自帝国的轰炸无人机已被击毁...", 138, 0, 18);
				return true;
			}
			else//锁血
			{
				NPC.active = true;
				NPC.dontTakeDamage = true;
				NPC.life = 1;
				deathcheck = 1;
				return false;
			}
		}
        #endregion
    }

    public class Deathdust : ModProjectile//死亡粒子效果触发器
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Deathdust";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("DeathDust");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"死亡粒子效果触发器");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 0;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}	

		public override void AI()
		{
			Player Player = Main.player[Main.myPlayer];
			Player.GetModPlayer<ArknightsPlayer>().screenShakeOnlyOnY = true;//只在纵向震动
			Player.GetModPlayer<ArknightsPlayer>().screenShakeTime = 20;//屏幕抖动1秒
			Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(10), 0).RotatedByRandom(Math.PI);
			for(int i = 0 ; i < 8 ; i++)
			{
				Dust dust7 = Dust.NewDustPerfect(dustPos, 219, Velocity: Vector2.Zero, Scale: 2f);
				dust7.velocity = (5*dustPos - 5*Projectile.Center);
			}
		}
	}

	public class AuragreenPro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/AuragreenPro";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Aura");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"绿色瞄准区");
		}
		public override void SetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.alpha = 50;
			Projectile.damage = 15;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=true;
		}

		public override void AI()
		{
			Projectile.alpha = (int)(0.032 * Projectile.timeLeft * Projectile.timeLeft - 5.76 * Projectile.timeLeft + 259.2);
			Projectile.scale = (float)(- 0.000125 * Projectile.timeLeft * Projectile.timeLeft + 0.0225 * Projectile.timeLeft - 0.0125);
		}
	}

	public class AuraredPro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/AuraredPro";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Aura");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"红色瞄准区");
		}
		public override void SetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=true;
		}

		public override void AI()
		{
			Projectile.alpha = (int)(0.032 * Projectile.timeLeft * Projectile.timeLeft - 5.76 * Projectile.timeLeft + 259.2);
			Projectile.scale = (float)(- 0.000125 * Projectile.timeLeft * Projectile.timeLeft + 0.0225 * Projectile.timeLeft - 0.0125);
		}
	}

	public class Hitboxgreen : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitboxgreen";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Hitbox");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"绿色标记点");
		}
		public override void SetDefaults()
		{
			Projectile.width = 250;
			Projectile.height = 250;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}

		public override void AI()
		{
			Projectile.alpha = (int)(4 * Projectile.timeLeft);
			Projectile.scale = (float)(1 - Projectile.timeLeft * 0.01666666666666666);
		}

		public override void Kill(int timeLeft)//下一阶段：召唤普通橘色判定箱和普通绿色伤害箱
		{
			var newSource = Projectile.GetSource_FromThis();
			SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/Alert") with { Volume = 1f, Pitch = 0f }, Projectile.Center);
			Projectile.NewProjectile(newSource,Projectile.Center.X , Projectile.Center.Y , 0, 0, ModContent.ProjectileType<Hitboxorange>(),Projectile.damage, 0f,  0, 0);
			Projectile.NewProjectile(newSource,Projectile.Center.X , Projectile.Center.Y , 0, 0, ModContent.ProjectileType<Hitblockgreen>(),Projectile.damage, 0f,  0, 0);
		}
	}

	public class HitboxgreenPro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitboxgreen";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Hitbox Pro");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"二阶绿色标记点");
		}
		public override void SetDefaults()
		{
			Projectile.width = 250;
			Projectile.height = 250;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}

		public override void AI()
		{
			Projectile.alpha = (int)(4 * Projectile.timeLeft);
			Projectile.scale = (float)(1 - Projectile.timeLeft * 0.01666666666666666);
		}

		public override void Kill(int timeLeft)//下一阶段：召唤高阶橘色判定箱和普通绿色伤害箱
		{
			var newSource = Projectile.GetSource_FromThis();
			Projectile.NewProjectile(newSource,Projectile.Center.X , Projectile.Center.Y , 0, 0, ModContent.ProjectileType<HitboxorangePro>(),Projectile.damage, 0f,  0, 0);
			Projectile.NewProjectile(newSource,Projectile.Center.X , Projectile.Center.Y , 0, 0, ModContent.ProjectileType<HitblockgreenPro>(),Projectile.damage, 0f,  0, 0);
		}
	}

	public class Hitboxorange : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitboxorange";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orange Hitbox");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"橙色标记点");
		}
		public override void SetDefaults()
		{
			Projectile.width = 270;
			Projectile.height = 270;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.alpha = 10;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}

		private float scalesize;

		public override void AI()
		{
			scalesize = (float)(0.015*Projectile.timeLeft-1.6);
			if(scalesize <= 1)
			{
				scalesize = 1;
			}
			Projectile.scale = scalesize;
		}

		public override void Kill(int timeLeft)//下一阶段：召唤普通红色伤害箱
		{
			var newSource = Projectile.GetSource_FromThis();
			SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/Explode") with { Volume = 1f, Pitch = 0f }, Projectile.Center);
			Projectile.NewProjectile(newSource,Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<Hitboxred>(),(int)(Projectile.damage), 0f,  0, 0);
		}
	}

	public class HitboxorangePro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitboxorange";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orange Hitbox Pro");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"二阶橙色标记点");
		}
		public override void SetDefaults()
		{
			Projectile.width = 270;
			Projectile.height = 270;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 180;
			Projectile.alpha = 10;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}

		private float scalesize;

		public override void AI()//ai
		{
			scalesize = (float)(0.015*Projectile.timeLeft-0.7);
			if(scalesize <= 1)
			{
				scalesize = 1;
			}
			Projectile.scale = scalesize;
		}

		public override void Kill(int timeLeft)//下一阶段：召唤高伤红色伤害箱
		{
			var newSource = Projectile.GetSource_FromThis();
			SoundEngine.PlaySound(new SoundStyle("Arknights/Sounds/Explode") with { Volume = 1f, Pitch = 0f }, Projectile.Center);
			Projectile.NewProjectile(newSource,Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<HitboxredPro>(),(int)(Projectile.damage), 0f,  0, 0);
		}
	}

	public class Hitboxred : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitboxred";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Hitbox");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"红色标记点");
		}
		public override void SetDefaults()
		{
			Projectile.width = 500;
			Projectile.height = 500;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 10;
			Projectile.damage = 60;
			Projectile.light = 0.6f;
			Projectile.friendly = false;
            Projectile.hostile = false;
			Projectile.scale = 1f;
		}

		private int alphasize;
		private float randomx;
		private bool missled = false;

        public override void AI()
		{
            var newSource = Projectile.GetSource_FromThis();
            if(missled != true)
			{
				randomx = Main.rand.NextFloat(-300, 300);
                Projectile.NewProjectile(newSource, Projectile.Center.X+randomx, Projectile.Center.Y - 1800, -randomx/60 , 0 , ModContent.ProjectileType<missle>(), 15, 0f, 0, 0);
				missled = true;
			}
            alphasize = (int)(95*Math.Sin(0.4*Projectile.timeLeft-1)+127.5);
			if(alphasize <= 0)
			{
				alphasize = 0;
			}
			Projectile.alpha = alphasize;
		}

		//public override void Kill(int timeLeft)//下一阶段：召唤普通红色伤害箱
		//{
		//	var newSource = Projectile.GetSource_FromThis();
		//	Projectile.NewProjectile(newSource,Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExplodeArea>(),(int)Projectile.damage, 0f,  0, 0);
		//}
	}

	public class HitboxredPro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/HitboxredPro";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Hitbox Pro");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"二阶红色标记点");
		}
		public override void SetDefaults()
		{
			Projectile.width = 500;
			Projectile.height = 500;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 10;
			Projectile.damage = 120;
			Projectile.light = 0.6f;
			Projectile.friendly = false;
            Projectile.hostile = false;
			Projectile.scale = 1f;
		}

		private int alphasize;
        private float randomx;
        private bool missled = false;

        public override void AI()
        {
            var newSource = Projectile.GetSource_FromThis();
            if (missled != true)
            {
                randomx = Main.rand.NextFloat(-300, 300);
                Projectile.NewProjectile(newSource, Projectile.Center.X + randomx, Projectile.Center.Y - 1800, -randomx / 60, 0, ModContent.ProjectileType<misslepro>(), 30, 0f, 0, 0);
                missled = true;
            }
            alphasize = (int)(95*Math.Sin(0.4*Projectile.timeLeft-1)+127.5);
			if(alphasize <= 0)
			{
				alphasize = 0;
			}
			Projectile.alpha = alphasize;
		}

		//public override void Kill(int timeLeft)//下一阶段：召唤高阶红色伤害箱
		//{
		//	var newSource = Projectile.GetSource_FromThis();
		//	Projectile.NewProjectile(newSource,Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExplodeAreaPro>(),(int)(Projectile.damage), 0f,  0, 0);
		//}
	}

	public class ExplodeArea : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitboxred";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BIG EXPLODE");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"爆炸");
		}
		public override void SetDefaults()
		{
			Projectile.width = 500;
			Projectile.height = 500;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 255;
			Projectile.damage = 60;
			Projectile.light = 0.6f;
			Projectile.friendly = false;
            Projectile.hostile = true;
			Projectile.scale = 1f;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(16), 0).RotatedByRandom(MathHelper.TwoPi);
			Dust dust = Dust.NewDustPerfect(dustPos, 55, Velocity: Vector2.Zero, Scale: 1.5f);
			dust.noGravity = true;
			dust.velocity = (4*dustPos - 4*Projectile.Center);
			Dust dust2 = Dust.NewDustPerfect(dustPos, 6, Velocity: Vector2.Zero, Scale: 4f);
			dust2.noGravity = true;
			dust2.velocity = (4*dustPos - 4*Projectile.Center);
			for (int i = 0; i < 2; i++)
			{
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, Scale: 1.5f)].noGravity = true;
			}
			for (int j = 0; j < 4; j++)
			{
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Scale: 4f)].noGravity = true;
			}
		}

	}

	public class ExplodeAreaPro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/HitboxredPro";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("HUGE EXPLODE");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"二阶爆炸");
		}
		public override void SetDefaults()
		{
			Projectile.width = 500;
			Projectile.height = 500;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.alpha = 255;
			Projectile.damage = 120;
			Projectile.light = 0.6f;
			Projectile.friendly = false;
            Projectile.hostile = true;
			Projectile.scale = 1f;
			Projectile.hide = true;
		}

		public override void AI()
		{
			Vector2 dustPos = Projectile.Center + new Vector2(Main.rand.NextFloat(16), 0).RotatedByRandom(MathHelper.TwoPi);
			Dust dust = Dust.NewDustPerfect(dustPos, 55, Velocity: Vector2.Zero, Scale: 1.5f);
			dust.noGravity = true;
			dust.velocity = (4*dustPos - 4*Projectile.Center);
			Dust dust2 = Dust.NewDustPerfect(dustPos, 6, Velocity: Vector2.Zero, Scale: 4f);
			dust2.noGravity = true;
			dust2.velocity = (4*dustPos - 4*Projectile.Center);
			
			for (int i = 0; i < 3; i++)
			{
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 55, Scale: 1.5f)].noGravity = true;
			}
			for (int j = 0; j < 6; j++)
			{
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 6, Scale: 4f)].noGravity = true;
			}
		}
	}

	public class Hitblockgreen : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitblockgreen";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Hitblock");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"绿色伤害区");
		}
		public override void SetDefaults()
		{
			Projectile.width = 250;
			Projectile.height = 250;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}
		public override void AI()//ai
		{
			Projectile.alpha = (int)(4 * Projectile.timeLeft);
			Projectile.scale = (float)(1 - Projectile.timeLeft * 0.01666666666666666);
		}

		public override void Kill(int timeLeft)//下一阶段：召唤橘色伤害箱
		{
			var newSource = Projectile.GetSource_FromThis();
			Projectile.NewProjectile(newSource,Projectile.Center.X , Projectile.Center.Y , 0, 0, ModContent.ProjectileType<Hitblockorange>(),(int)(Projectile.damage*0.3), 0f,  0, 0);
		}
	}

	public class HitblockgreenPro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitblockgreen";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Green Hitblock Pro");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"二阶绿色伤害区");
		}
		public override void SetDefaults()
		{
			Projectile.width = 250;
			Projectile.height = 250;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}
		public override void AI()//ai
		{
			Projectile.alpha = (int)(4 * Projectile.timeLeft);
			Projectile.scale = (float)(1 - Projectile.timeLeft * 0.01666666666666666);
		}

		public override void Kill(int timeLeft)//下一阶段：召唤橘色伤害箱
		{
			var newSource = Projectile.GetSource_FromThis();
			Projectile.NewProjectile(newSource,Projectile.Center.X , Projectile.Center.Y , 0, 0, ModContent.ProjectileType<HitblockorangePro>(),(int)(Projectile.damage*0.3), 0,  0, 0);
		}
	}

	public class Hitblockorange : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitblockorange";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orange Hitblock");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"橙色伤害区");
		}
		public override void SetDefaults()
		{
			Projectile.width = 270;
			Projectile.height = 270;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.alpha = 50;
			Projectile.damage = 20;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=true;
		}

		private int alphasize;
		private float scalesize;

		public override void AI()//ai
		{
			var newSource = Projectile.GetSource_FromThis();
			scalesize = (float)(-0.00003*Projectile.timeLeft*Projectile.timeLeft+1.8);
			if(scalesize >= 1)
			{
				scalesize = 1;
			}
			alphasize = (int)(-0.025*Projectile.timeLeft*Projectile.timeLeft-6*Projectile.timeLeft+255);
			if(alphasize <= 0)
			{
				alphasize = 0;
			}
			Projectile.alpha = alphasize;
			Projectile.scale = scalesize;
		}
	}

	public class HitblockorangePro : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/Hitblockorange";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orange Hitblock Pro");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"二阶橙色伤害区");
		}
		public override void SetDefaults()
		{
			Projectile.width = 270;
			Projectile.height = 270;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 240;
			Projectile.alpha = 50;
			Projectile.damage = 40;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=true;
		}

		private int alphasize;
		private float scalesize;

		public override void AI()//ai
		{
			var newSource = Projectile.GetSource_FromThis();
			scalesize = (float)(-0.00003*Projectile.timeLeft*Projectile.timeLeft+1.8);
			if(scalesize >= 1)
			{
				scalesize = 1;
			}
			alphasize = (int)(-0.025*Projectile.timeLeft*Projectile.timeLeft-6*Projectile.timeLeft+255);
			if(alphasize <= 0)
			{
				alphasize = 0;
			}
			Projectile.alpha = alphasize;
			Projectile.scale = scalesize;
		}
	}

	public class HitboxblueCore : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/HitboxblueCore";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Player Hitbox");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"预判核心");
		}
		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 120;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}

		private float click = 0;
		private float prefire;

		public override void AI()//ai
		{
			var newSource = Projectile.GetSource_FromThis();
			Player Player = Main.player[Main.myPlayer];
			click++;

			if(Main.masterMode)
			{
				prefire = 48;
			}
			else if(Main.expertMode)
			{
				prefire = 42;
			}
			else
			{
				prefire = 36;
			}

			if(Projectile.scale > 1f)
			{
				Projectile.scale = 1f;
			}
			if(Projectile.scale < 0f)
			{
				Projectile.scale = 0f;
			}
			if(Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			if(Projectile.alpha > 255)
			{
				Projectile.alpha = 255;
			}

			Projectile.Center = Player.Center+prefire*Player.velocity;
			Projectile.scale = (float)(3.3f/(click/24+0.5f)/(click/24-5.5f)+1.4f);
			Projectile.alpha = (int)(0.1*click*click-12*click+255);
		}
	}

	public class HitboxblueFrame : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/HitboxblueFrame";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Player Hitbox");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"预判框架");
		}
		public override void SetDefaults()
		{
			Projectile.width = 200;
			Projectile.height = 200;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 120;
			Projectile.alpha = 50;
			Projectile.damage = 0;
			Projectile.light = 0.6f;
			Projectile.friendly=false;
            Projectile.hostile=false;
		}

		private float atktimer = 0;
		private int skilltimer = 0;
		private float prefire;

		public override void AI()//ai
		{
			var newSource = Projectile.GetSource_FromThis();
			Player Player = Main.player[Main.myPlayer];
			atktimer++;
			skilltimer++;

			if(Main.masterMode)
			{
				prefire = 48;
			}
			else if(Main.expertMode)
			{
				prefire = 42;
			}
			else
			{
				prefire = 36;
			}

			if(Projectile.scale > 1f)
			{
				Projectile.scale = 1f;
			}
			if(Projectile.scale < 0f)
			{
				Projectile.scale = 0f;
			}
			if(Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			if(Projectile.alpha > 255)
			{
				Projectile.alpha = 255;
			}

			Projectile.Center = Player.Center+prefire*Player.velocity;

			Projectile.scale = (float)(3.3f/(atktimer/24+0.5f)/(atktimer/24-5.5f)+1.4f);
			Projectile.alpha = (int)(0.1*atktimer*atktimer-12*atktimer+255);
			if(skilltimer == 120)
			{
				if(Player.velocity.X*Player.velocity.X + Player.velocity.Y*Player.velocity.Y >= 100)
				{
					Projectile.NewProjectile(newSource,Projectile.Center.X+0.25f*prefire*Player.velocity.X, Projectile.Center.Y+0.25f*prefire*Player.velocity.Y, 0, 0, ModContent.ProjectileType<HitboxredPro>(), 0 , 0 , 0 , 0);
				}
				else
				{
					Projectile.NewProjectile(newSource,Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<HitboxredPro>(), 0 , 0 , 0 , 0);
				}
				skilltimer = 0;
			}
		}
	}

	public class IACTScreenWave : ModProjectile
	{
		private const string ChainTextPath="Arknights/NPCs/U12/null";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("IACTScreenWave");
			DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese),"地坪弧光");
		}
		public override void SetDefaults()
		{
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 75;
			Projectile.alpha = 255;
			Projectile.damage = 0;
			Projectile.light = 1f;
			Projectile.scale = 0f;
			Projectile.friendly = false;
            Projectile.hostile = false;
		}

		private int shadertimer = 0;//充当utime
		//private int shadercheck = 0;
		private int ammount = 50;//波纹数量
		private int scalesize = 1;//波纹大小
		private int wavevelocity;//波纹速度
		private float distortStrength = 100f;//强度

		public override void AI()
		{
			shadertimer++;
			if(shadertimer > 75)
			{
				shadertimer = 75;
			}
			wavevelocity = (int)(0.0015*shadertimer*shadertimer-0.225*shadertimer+8.4375);
			if(wavevelocity < 0)
			{
				wavevelocity = 0;
			}

			if (shadertimer <= 60)
			{
				if (Projectile.ai[0] == 0)
        		{
            		Projectile.ai[0] = 1;
					if (Main.netMode != NetmodeID.Server && !Filters.Scene["IACTSW"].IsActive())
					{
						Filters.Scene.Activate("IACTSW", Projectile.Center).GetShader().UseColor(ammount, scalesize, wavevelocity).UseTargetPosition(Projectile.Center);
					}
				}

				if (Main.netMode != NetmodeID.Server && Filters.Scene["IACTSW"].IsActive())
				{
					float progress = (shadertimer) / 60f;
					Filters.Scene["IACTSW"].GetShader().UseProgress(3 * progress).UseOpacity(distortStrength * (1 - progress / 1f));
				}
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.netMode != NetmodeID.Server && Filters.Scene["IACTSW"].IsActive())
			{
				Filters.Scene["IACTSW"].Deactivate();
			}
		}
	}

	public class missle : ModProjectile
    {
        private const string ChainTextPath = "Arknights/NPCs/U12/missle";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("missle");
            DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "导弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 52;
            Projectile.aiStyle = 0;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.alpha = 10;
			Projectile.damage = 60;
            Projectile.light = 0.6f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;
        }

        private float speedy;

        public override void AI()
        {
            Dust dust = Terraria.Dust.NewDustPerfect(Projectile.Center + new Vector2(0, -10), 204, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 2.5f);
            Projectile.ai[0]++;
            Projectile.rotation = Projectile.velocity.X * 0.01f;
            speedy = 20f + Projectile.ai[0] / 3f;
			Projectile.velocity.Y = speedy;
        }

        public override void Kill(int timeLeft)//途中撞上人或者到时间就爆炸
        {
            var newSource = Projectile.GetSource_FromThis();
            Projectile.NewProjectile(newSource, Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExplodeArea>(), 10 , 0f, 0, 0);
        }
    }

    public class misslepro : ModProjectile
    {
        private const string ChainTextPath = "Arknights/NPCs/U12/missle";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("misslepro");
            DisplayName.AddTranslation(GameCulture.FromCultureName(GameCulture.CultureName.Chinese), "高阶导弹");
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 52;
            Projectile.aiStyle = 0;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;
            Projectile.alpha = 10;
            Projectile.damage = 120;
            Projectile.light = 0.6f;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.scale = 1f;
        }

        private float speedy;

        public override void AI()
        {
			Projectile.ai[0]++;
			Projectile.rotation = Projectile.velocity.X * 0.01f;
            speedy = 20f + Projectile.ai[0] / 3f;
            Projectile.velocity.Y = speedy;
            Dust dust = Terraria.Dust.NewDustPerfect(Projectile.Center + new Vector2(0,-10), 204, new Vector2(0f, 0f), 0, new Color(255, 255, 255), 2.5f);

        }

        public override void Kill(int timeLeft)//途中撞上人或者到时间就爆炸
        {
            var newSource = Projectile.GetSource_FromThis();
            Projectile.NewProjectile(newSource, Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<ExplodeAreaPro>(), 20, 0f, 0, 0);
        }
    }
}