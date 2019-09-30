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

namespace ExtraModifiers.Modifiers.ArmorModifiers
{
    class ShadowPartnerEffect : ModifierEffect
    {
        public int Stack;

        public override void OnInitialize(ModifierPlayer player)
        {
            Stack = 0;
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            Stack = 0;
        }

        [AutoDelegation("OnPreUpdate")]
        private void ShadowPartner_OnPreUpdate(ModifierPlayer player)
        {
            if (Stack == 3)
            {
                player.player.GetModPlayer<PlayerEffects>().enableShadowPartner();
            }
            else
                player.player.GetModPlayer<PlayerEffects>().disableShadowPartner();
        }

    }

    [UsesEffect(typeof(ShadowPartnerEffect))]
    class ShadowPartner : ArmorModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = $"Shadow Partner", Color = Color.DarkViolet}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(rollChance: 2f, uniqueRoll: true);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            ModifierPlayer.Player(player).GetEffect<ShadowPartnerEffect>().Stack += 1;
        }
    }
}
