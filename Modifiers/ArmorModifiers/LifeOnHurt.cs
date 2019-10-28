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

namespace ExtraModifiers.Modifiers.ArmorModifiers
{

    class LifeOnHurtEffect : ModifierEffect
    {
        public float percentHeal;
        public bool isActive;
        public int delay;
        public double savedDamage;

        public override void OnInitialize(ModifierPlayer player)
        {
            isActive = false;
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            percentHeal = 0f;
        }

        [AutoDelegation("OnPostHurt")]
        private void LifeOnHurt_OnPostHurt(ModifierPlayer player, bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {
            if (isActive == false)
            {
                isActive = true;
                savedDamage = damage;
                delay = 0;
            }
        }

        [AutoDelegation("OnPreUpdate")]
        private void LifeOnHurt_OnPreUpdate(ModifierPlayer player)
        {
            if(isActive == true)
            {
                delay++;
                if(delay > 180)
                {
                    isActive = false;
                    delay = 0;
                    player.player.statLife += amountToHeal();
                    if(Main.myPlayer == player.player.whoAmI)
                    {
                        player.player.HealEffect(amountToHeal(), true);
                    }
                }
            }
        }

        private int amountToHeal()
        {
            return (int)Math.Ceiling(savedDamage * percentHeal);
        }
    }

    [UsesEffect(typeof(LifeOnHurtEffect))]
    class LifeOnHurt : ArmorModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = $"3 seconds after being hit, heal for +" + Properties.RoundedPower.ToString() + "% of the damage taken", Color = Color.Lime}
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
            ModifierPlayer.Player(player).GetEffect<LifeOnHurtEffect>().percentHeal += Properties.RoundedPower / 100f;
        }
    }
}
