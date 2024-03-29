﻿using Loot.Modifiers;
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

namespace ExtraModifiers.Modifiers.AccessoryModifiers
{
    class PercentGoldEffect : ModifierEffect
    {
        public float percentGold;
        public bool isActive;

        public override void OnInitialize(ModifierPlayer player)
        {
            percentGold = 0f;
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            percentGold = 0f;
            isActive = false;
        }

        [AutoDelegation("OnModifyHitNPC")]
        private void PercentGold_OnModifyHurt(ModifierPlayer player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            setPercentGold(target);
        }

        public void setPercentGold(NPC target)
        {
            if (isActive == true)
            {
                target.value = target.value + (target.value * percentGold);
            }
        }
    }

    [UsesEffect(typeof(PercentGoldEffect))]
    class PercentGold : AccessoryModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
{
            new ModifierTooltipLine {Text = $"Enemies drop +" + Properties.RoundedPower.ToString() + "% more gold", Color = Color.Lime}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(minMagnitude: 2f, maxMagnitude: 10f, rollChance: 2f, uniqueRoll: false);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            ModifierPlayer.Player(player).GetEffect<PercentGoldEffect>().percentGold += Properties.RoundedPower / 100f;
            ModifierPlayer.Player(player).GetEffect<PercentGoldEffect>().isActive = true;
        }
    }
}
