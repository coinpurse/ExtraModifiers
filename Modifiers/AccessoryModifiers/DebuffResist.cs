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

namespace ExtraModifiers.Modifiers.AccessoryModifiers
{
   class DebuffResistEffect : ModifierEffect
    {
        public float percentResist;
        public override void OnInitialize(ModifierPlayer player)
        {
        }
        public override void ResetEffects(ModifierPlayer player)
        {
            percentResist = 0f;
        }
        public bool attemptResist()
        {
            int i;
            if(percentResist == 0)
            {
                return false;
            }
            else
            {
                if (Main.rand.Next(100) < percentResist)
                {
                    return true;
                }
                else
                    return false;
            }
           
        }

    }
    
    [UsesEffect(typeof(DebuffResistEffect))]
    class DebuffResist : AccessoryModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = Properties.RoundedPower.ToString() + "% chance to resist debuffs", Color = Color.White}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(minMagnitude: 1f, maxMagnitude: 10f, rollChance: 2f, uniqueRoll: false);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            ModifierPlayer.Player(player).GetEffect<DebuffResistEffect>().percentResist += Properties.RoundedPower;
        }
    }
}
