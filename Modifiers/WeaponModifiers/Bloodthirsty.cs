using Loot.Modifiers;
using Loot.Core;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loot;
using Loot.Core.Attributes;
using Microsoft.Xna.Framework;
using TestMod.NPCs;

namespace ExtraModifiers.Modifiers.WeaponModifiers
{
    class BloodthirstyEffect : ModifierEffect
    {
        public bool isActive;
        public int numOfKills;
        public int rampageDuration;
        public float spawnRateMultiplier;
        public float spawnMaxMultiplier;

        public override void OnInitialize(ModifierPlayer player)
        {
            numOfKills = 0;
            spawnRateMultiplier = 1f;
            spawnMaxMultiplier = 1f;
        }
        public override void ResetEffects(ModifierPlayer player)
        {
            isActive = false;
        }

        [AutoDelegation("OnOnHitNPC")]
        public void Bloodthirsty_OnHitNPC(ModifierPlayer player, Item item, NPC target, int damage, float knockback, bool crit)
        {
                StartRampage(target);
        }

        [AutoDelegation("OnPostUpdateEquips")]
        private void HasBloodthirsty(ModifierPlayer player)
        {
            Item checkItem = player.player.HeldItem;

            if (checkItem != null && !checkItem.IsAir)
            {
                if (ActivatedModifierItem.Item(checkItem).IsActivated)
                {
                    int c = EMMItem.GetActivePool(checkItem).Count(x => x.GetType() == typeof(Bloodthirsty));
                    if (c > 0)
                    {
                        isActive = true;
                    }
                    else
                    {
                        isActive = false;
                    }
                }
                else
                {
                    isActive = false;
                }
            }
        }

        [AutoDelegation("OnPreUpdate")]
        private void Bloodthirsty_OnPreUpdate(ModifierPlayer player)
        {
            if (isActive) { 
                rampageDuration++;
                if (rampageDuration > 900)
                {
                    rampageDuration = 0;
                    resetSpawns();
                 }
             }
            else
            {
                rampageDuration = 0;
                resetSpawns();
            }
        }

        public void resetSpawns()
        {
            spawnRateMultiplier = 1f;
            spawnMaxMultiplier = 1f;
        }

        public void StartRampage(NPC target)
        {
            if (target.life <= 0 && isActive)
            {
                numOfKills++;
                rampageDuration = 0;
                if (numOfKills >= 5)
                {
                    numOfKills = 0;
                    spawnRateMultiplier += .25f;
                    spawnMaxMultiplier += 1f;
                }
            }
        }
    }

    [UsesEffect(typeof(BloodthirstyEffect))]
    class Bloodthirsty : WeaponModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = $"Bloodthirsty", Color = Color.Red}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(rollChance: 2f, uniqueRoll: true);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx);
        }
    }
}
