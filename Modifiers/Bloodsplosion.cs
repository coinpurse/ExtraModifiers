using ExtraModifiers.Projectiles;
using Loot;
using Loot.Core;
using Loot.Core.Attributes;
using Loot.Modifiers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

namespace ExtraModifiers.Modifiers
{
    public class BloodsplosionEffect : ModifierEffect
    {

        public float Multiplier;
        public bool isActive;
        public override void OnInitialize(ModifierPlayer player)
        {
            Multiplier = 1f;
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            Multiplier = 1f;
            isActive = false;
        }

        [AutoDelegation("OnPostUpdateEquips")]
        private void ExplosiveHolding(ModifierPlayer player)
        {
            Item checkItem = player.player.HeldItem;

            if (checkItem != null && !checkItem.IsAir)
            {
                if (ActivatedModifierItem.Item(checkItem).IsActivated)
                {
                    int c = EMMItem.GetActivePool(checkItem).Count(x => x.GetType() == typeof(Bloodsplosion));
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
        [AutoDelegation("OnOnHitNPC")]
        public void Bloodplosion_OnHitNPC(ModifierPlayer player, Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0 && isActive)
            {
                Bloodsplode((int)(Multiplier * damage), player, target.position.X, target.position.Y);
            }
        }

        [AutoDelegation("OnOnHitPVP")]
        public void Bloodplosion_OnHitPVP(ModifierPlayer player, Item item, Player target, int damage, float knockback, bool crit)
        {
            if (target.statLife <= 0 && isActive)
            {
                Bloodsplode((int)(Multiplier * damage), player, target.position.X, target.position.Y);
            }
        }

        private void Bloodsplode(int damage, ModifierPlayer player, float X, float Y)
        {
            player.player.GetModPlayer<PlayerEffects>().createBloodplosion(X, Y, damage);
        }

    }
    [UsesEffect(typeof(BloodsplosionEffect))]
    public class Bloodsplosion : WeaponModifier
    {
        public override ModifierTooltipLine[] TooltipLines => new[]
        {
            new ModifierTooltipLine {Text = $"When an enemy dies, they explode for {Properties.RoundedPower}% of your weapon's damage", Color = Color.DarkOrange}
        };

        public override ModifierProperties GetModifierProperties(Item item)
        {
            return base.GetModifierProperties(item).Set(minMagnitude: 8f, maxMagnitude: 15f, basePower: 10f, rollChance: 2f, uniqueRoll: true);
        }

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx) && ctx.Method != ModifierContextMethod.SetupStartInventory;
        }

        public override void UpdateEquip(Item item, Player player)
        {
            ModifierPlayer.Player(player).GetEffect<BloodsplosionEffect>().Multiplier += Properties.RoundedPower / 100f;
        }
    }


}
