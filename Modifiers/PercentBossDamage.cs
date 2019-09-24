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

        [AutoDelegation("OnModifyHitNPC")]
        private void PercentBossDamage_OnModify(ModifierPlayer player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            setBossDamage(target, ref damage);
        }

        public void setBossDamage(NPC target, ref int damage)
        {
            if (isActive == true && target.boss == true)
            {
                damage = damage + (int)Math.Ceiling(damage * percentBossDamage);
            }
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
            ModifierPlayer.Player(player).GetEffect<PercentBossDamageEffect>().percentBossDamage += Properties.RoundedPower / 1f;
            ModifierPlayer.Player(player).GetEffect<PercentBossDamageEffect>().isActive = true;
        }
    }
}

