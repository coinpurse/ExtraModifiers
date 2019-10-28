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
        public LinkedList<int> buffExtender;
        public LinkedList<int> buffResistor;
        public int[] buffWatcher;
        public bool initBuffs;

        public override void Initialize()
        {
            buffExtender = new LinkedList<int>();
            buffResistor = new LinkedList<int>();
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
                PlayerEffects modPlayer = drawPlayer.GetModPlayer<PlayerEffects>();
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
            // If the buffWatcher hasn't been initialized, initialize it buy adding 1 to the buffWatcher at that index if the player has a buff at that index,
            // otherwise add 0 to the index
            if (initBuffs == false)
            {
                for (int i = 0; i < player.buffType.Length; i++)
                {
                    if (player.buffType[i] != 0)
                    {
                        buffWatcher[i] = 1;
                    }
                    else
                        buffWatcher[i] = 0;
                }
                initBuffs = true;
            }
            else  // When the buffWatcher has been initialized, keep checking for new buffs to appear, handle them accordingly.
            {
                for (int i = 0; i < player.buffType.Length; i++)
                {
                    if (player.buffType[i] != 0)
                    {
                        if (buffWatcher[i] == 0)
                        {
                            if (Main.debuff[player.buffType[i]] == false)
                            { // Is a buff
                              // A new buff has been found             
                                buffWatcher[i] = 1;
                                buffExtender.AddLast(i);
                            }
                            else    // Is a debuff
                            {
                                buffWatcher[i] = 2;
                                if (player.buffType[i] != 21)   // Don't resist potion sickness
                                {
                                    buffResistor.AddLast(i);
                                }
                            }
                        }
                    }
                    else
                        buffWatcher[i] = 0;
                }
            }
        }
    }
}
