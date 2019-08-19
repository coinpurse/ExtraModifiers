using Loot.Modifiers;
using Loot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loot;
using Terraria;
using Microsoft.Xna.Framework;
using Loot.Core.Attributes;

namespace TestMod.Modifiers
{
    class PercentBossDamageEffect : ModifierEffect
    {
        public float percentBossDamage;
        public bool isActive;

        public override void OnInitialize(ModifierPlayer player)
        {
            percentBossDamage = 0f;
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            percentBossDamage = 0f;
            isActive = false;
        }

        [AutoDelegation("OnOnHitNPC")]
        private void PercentGold_OnPostHurt(ModifierPlayer player, Item item, NPC target, int damage, float knockback, bool crit)
        {
            /* TODO
             * Add boss condition to condition statement
            if (isActive == true)
            {
                // Increase the damage dealt
            }
            */
        }


    }

    [UsesEffect(typeof(PercentBossDamageEffect))]
    class PercentBossDamage : AccessoryModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
{
            new ModifierTooltipLine {Text = $"+" + Properties.RoundedPower.ToString() + "% damage to bosses", Color = Color.White}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(minMagnitude: 2f, maxMagnitude: 15f, rollChance: 2f, uniqueRoll: false);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            ModifierPlayer.Player(player).GetEffect<PercentBossDamageEffect>().percentGold += Properties.RoundedPower / 100f;
        }
    }
}

