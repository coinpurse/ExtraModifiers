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

        [AutoDelegation("OnOnHitNPC")]
        private void PercentGold_OnPostHurt(ModifierPlayer player, Item item, NPC target, int damage, float knockback, bool crit)
        {
            if(target.statLife <= 0 && isActive == true)
            {
                // Make the NPC drop more gold
            }
        }


    }

    [UsesEffect(typeof(PercentGoldEffect))]
    class PercentGold : AccessoryModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
{
            new ModifierTooltipLine {Text = $"Enemies drop +" + Properties.RoundedPower.ToString() + "% more gold", Color = Color.Gold}
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
            ModifierPlayer.Player(player).GetEffect<PercentGoldEffect>().percentGold += Properties.RoundedPower / 100f;
        }
    }
}
