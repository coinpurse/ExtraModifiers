using Loot.Core;
using Loot.Core.Attributes;
using Loot.Modifiers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Loot;

namespace TestMod.Modifiers
{
    public class ReactiveArmorEffect : ModifierEffect
    {
        public int StackCount;
        public int MaxStack;
        public int ArmorDuration;

        public override void OnInitialize(ModifierPlayer player)
        {
            MaxStack = 0;
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            MaxStack = 0;
        }
         
        [AutoDelegation("OnPostHurt")]
        private void ReactiveArmor_OnPostHurt(ModifierPlayer player, bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (StackCount < MaxStack) {
                StackCount++;
                calculateDefense(player);
            }
            ArmorDuration = 0;
        }

        [AutoDelegation("OnPreUpdate")]
        private void ReactiveArmor_OnPreUpdate(ModifierPlayer player)
        {
            ArmorDuration++;
            if(ArmorDuration > 300)
            { 
                ArmorDuration = 0;
                StackCount = 0;
                calculateDefense(player);
            }
        }
        
        public void calculateDefense(ModifierPlayer player)
        {
            player.player.statDefense = (int)Math.Ceiling(player.player.statDefense + (player.player.statDefense * (.05 * StackCount)));
        }
        
        [AutoDelegation("OnPostUpdateEquips")]
        //[DelegationPrioritization(DelegationPrioritization.Late, 999)]
        private void Defense_PostUpdate(ModifierPlayer player)
        {
            calculateDefense(player);
        }
    }
    [UsesEffect(typeof(ReactiveArmorEffect))]
    class ReactiveArmor : ArmorModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = $"Reactive Armor", Color = Color.AntiqueWhite}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(minMagnitude: 5f, maxMagnitude: 15f, rollChance: 2f, uniqueRoll: true);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            ModifierPlayer.Player(player).GetEffect<ReactiveArmorEffect>().MaxStack += 3;
        }
    }
}
