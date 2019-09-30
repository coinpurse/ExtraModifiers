using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace ExtraModifiers
{
    class PlayerEffects : ModPlayer
    {
        private static bool isShadowPartner;

        public void createBloodplosion(float X, float Y, int damage)
        {
            Projectile.NewProjectile(X, Y, 0, 0, mod.ProjectileType("Bloodsplosion"), damage, 0f, Main.myPlayer);
        }
        public void enableShadowPartner()
        {
            isShadowPartner = true;
        }
        public void disableShadowPartner()
        {
            isShadowPartner = false;
        }

        public static readonly PlayerLayer ShadowPartnerLayer = new PlayerLayer("ExtraModifiers", "ShadowPartnerLayer", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Mod mod = ModLoader.GetMod("ExtraModifiers");
                PlayerEffects modPlayer = drawPlayer.GetModPlayer<PlayerEffects>(mod);
                if (isShadowPartner)
                {
                    Texture2D texture;
                    int drawX;
                    if (drawPlayer.direction == 1)
                    {
                        texture = mod.GetTexture("EffectResources/ShadowPartner_Inv");
                        drawX = (int)(drawInfo.position.X - Main.screenPosition.X - (drawPlayer.direction * (drawPlayer.width)) + 5f);
                    }
                    else {
                        texture = mod.GetTexture("EffectResources/ShadowPartner");
                        drawX = (int)(drawInfo.position.X - Main.screenPosition.X - (drawPlayer.direction * (drawPlayer.width)));
                    }
                    int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 3f - Main.screenPosition.Y);
                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.Black , 0f, new Vector2(texture.Width / 4f, texture.Height / 4f), 1f, SpriteEffects.None, 0);
                    Main.playerDrawData.Add(data);
                }
            });

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            ShadowPartnerLayer.visible = true;
            layers.Insert(0, ShadowPartnerLayer);
        }
    }
}
