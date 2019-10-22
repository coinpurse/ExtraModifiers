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
using ExtraModifiers.Projectiles;
using ExtraModifiers.Modifiers.ArmorModifiers;
using Loot;
using ExtraModifiers.Modifiers.AccessoryModifiers;
using Terraria.ModLoader.IO;

namespace ExtraModifiers
{
    class PlayerEffects : ModPlayer
    {
        public bool initializeShadowProjectile;
        public bool hasShadowPartner;
        public LinkedList<int> buffExtender = new LinkedList<int>();
        public int[] buffWatcher;
        public bool initBuffs;

        public override void Initialize()
        {
            buffExtender = new LinkedList<int>();
            buffWatcher = new int[player.buffType.Length];
        }
        public void CreateBloodplosion(float X, float Y, int damage)
        {
            Projectile.NewProjectile(X, Y, 0, 0, mod.ProjectileType("Bloodsplosion"), (int)Math.Ceiling(.5*damage), 0f, Main.myPlayer);
        }
        public void createShadowProjectile(Projectile projectile)
        {
            projectile.noDropItem = true;
            Projectile.NewProjectile(projectile.position, projectile.velocity, projectile.type, (int)Math.Ceiling(projectile.damage * 0.33f), projectile.knockBack * 0.3f, Main.myPlayer);
        }
        public void setHasShadowPartner(bool isActive)
        {
            hasShadowPartner = isActive;
        }

        public readonly PlayerLayer ShadowPartnerLayer = new PlayerLayer("ExtraModifiers", "ShadowPartnerLayer", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }
                Player drawPlayer = drawInfo.drawPlayer;
                Mod mod = ModLoader.GetMod("ExtraModifiers");
                PlayerEffects modPlayer = drawPlayer.GetModPlayer<PlayerEffects>(mod);
                if (modPlayer.hasShadowPartner && modPlayer.player.statLife > 0)
                {
                    Texture2D texture = mod.GetTexture("EffectResources/ShadowPartner");
                    int drawX;
                    if (drawPlayer.direction == 1)
                    {
                        drawX = (int)(drawInfo.position.X - Main.screenPosition.X - (drawPlayer.direction * (drawPlayer.width)) + 5f);
                    }
                    else {   
                        drawX = (int)(drawInfo.position.X - Main.screenPosition.X - (drawPlayer.direction * (drawPlayer.width)));
                    }
                    int drawY = (int)(drawInfo.position.Y + drawPlayer.height / 3f - Main.screenPosition.Y);
                    DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.Black , 0f, new Vector2(texture.Width / 4f, texture.Height / 4f), 1f, drawPlayer.direction==1? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
                    Main.playerDrawData.Add(data);
                }
            });
        
        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            ShadowPartnerLayer.visible = true;
            layers.Insert(0, ShadowPartnerLayer);
        }

        public override void PreUpdateBuffs()
        {
            if (initBuffs == false)
            {
                for (int i = 0; i < player.buffType.Length; i++)
                {
                    if (player.buffType[i] != 0 && Main.debuff[player.buffType[i]] == false)
                    {
                        buffWatcher[i] = 1;
                    }
                    else
                        buffWatcher[i] = 0;
                }
                initBuffs = true;
            }
            else
            {
                for (int i = 0; i < player.buffType.Length; i++)
                {
                    if (player.buffType[i] != 0 && Main.debuff[player.buffType[i]] == false)
                    {
                        if (buffWatcher[i] == 0)
                        {
                            buffWatcher[i] = 1;
                            buffExtender.AddLast(i);
                        }
                    }
                    else
                        buffWatcher[i] = 0;
                }
            }
        }
    }
}
