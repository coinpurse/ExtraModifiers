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
    class BuffDurationEffect : ModifierEffect
    {
        public float percentBuffDuration;

        public override void OnInitialize(ModifierPlayer player)
        {
        }
        public override void ResetEffects(ModifierPlayer player)
        {
            percentBuffDuration = 0f;
        }
        [AutoDelegation("OnPreUpdate")]
        private void LifeOnHurt_OnPreUpdate(ModifierPlayer player)
        {
            int index;
            ModifierPlayer mplr = player.player.GetModPlayer<ModifierPlayer>();
            PlayerEffects mplrEffects = player.player.GetModPlayer<PlayerEffects>();
            while (mplrEffects.buffExtender.Count > 0 && mplr.GetEffect<BuffDurationEffect>().percentBuffDuration > 0f)
            {
                index = mplrEffects.buffExtender.First.Value;
                player.player.buffTime[index] = (int)Math.Ceiling(player.player.buffTime[index] + (player.player.buffTime[index] * mplr.GetEffect<BuffDurationEffect>().percentBuffDuration));
                mplrEffects.buffExtender.RemoveFirst();
            }
        }


    }

    [UsesEffect(typeof(BuffDurationEffect))]
    class BuffDuration : AccessoryModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
{
            new ModifierTooltipLine {Text = Properties.RoundedPower.ToString() + "% buff duration", Color = Color.White}
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
            ModifierPlayer.Player(player).GetEffect<BuffDurationEffect>().percentBuffDuration += Properties.RoundedPower / 100f;
        }
    }
}
