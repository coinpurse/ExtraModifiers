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

namespace TestMod.Modifiers
{
    class RampageEffect : ModifierEffect
    {
        public bool isActive;
        public int numOfKills;
        public int rampageDuration;

        public override void OnInitialize(ModifierPlayer player)
        {
            numOfKills = 0;
        }
        public override void ResetEffects(ModifierPlayer player)
        {
            isActive = false;
        }

        [AutoDelegation("OnOnHitNPC")]
        public void Rampage_OnHitNPC(ModifierPlayer player, Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && isActive)
            {
                StartRampage();
            }
        }

        [AutoDelegation("OnPostUpdateEquips")]
        private void HasRampage(ModifierPlayer player)
        {
            Item checkItem = player.player.HeldItem;

            if (checkItem != null && !checkItem.IsAir)
            {
                if (ActivatedModifierItem.Item(checkItem).IsActivated)
                {
                    int c = EMMItem.GetActivePool(checkItem).Count(x => x.GetType() == typeof(Rampage));
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
        private void Rampage_OnPreUpdate(ModifierPlayer player)
        {
            if (isActive) { 
                rampageDuration++;
                if (rampageDuration > 900)
                {
                    rampageDuration = 0;
                    NPCs.ExtraModifiersNPC.resetSpawnRate();
                 }
             }
            else
            {
                rampageDuration = 0;
                NPCs.ExtraModifiersNPC.resetSpawnRate();
            }
        }

        public void StartRampage()
        {
            numOfKills++;
            rampageDuration = 0;
            if (numOfKills >= 5)
            {
                numOfKills = 0;
               NPCs.ExtraModifiersNPC.spawnMultiplier += .25f;
            }
        }
    }

    [UsesEffect(typeof(RampageEffect))]
    class Rampage : WeaponModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = $"Rampage", Color = Color.Red}
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
