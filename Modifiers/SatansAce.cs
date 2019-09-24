using Loot;
using Loot.Core;
using Loot.Core.Attributes;
using Loot.Modifiers;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;

namespace ExtraModifiers.Modifiers
{
    public class SatansAceEffect : ModifierEffect
    {
        public Random rand;
        public bool isActive;

        public override void OnInitialize(ModifierPlayer player)
        {
            rand = new Random();
        }

        public override void ResetEffects(ModifierPlayer player)
        {
            rand = new Random();
            isActive = false;
        }

        [AutoDelegation("OnPostUpdateEquips")]
        private void AceHolding(ModifierPlayer player)
        {
            Item checkItem = player.player.HeldItem;

            if (checkItem != null && !checkItem.IsAir)
            {
                if (ActivatedModifierItem.Item(checkItem).IsActivated)
                {
                    int c = EMMItem.GetActivePool(checkItem).Count(x => x.GetType() == typeof(SatansAce ));
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

        [AutoDelegation("OnModifyHitNPC")]
        //[DelegationPrioritization(DelegationPrioritization.Late, 998)]
        public void Ace_ModifyHitNPC(ModifierPlayer player, Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            //Main.NewText("Hello", 155, 155, 155);
            AceEffect(player, ref damage);
        }

        [AutoDelegation("OnModifyHitPVP")]
        //[DelegationPrioritization(DelegationPrioritization.Late, 998)]
        public void Ace_ModifyHitPVP(ModifierPlayer player, Item item, Player target, ref int damage, ref float knockback, ref bool crit)
        {
            AceEffect(player, ref damage);
        }

        public void AceEffect(ModifierPlayer player, ref int damage)
        {
            if (isActive) { 
               int i = rand.Next(100);
               if (i < 6)
               {
                    player.player.statLife -= (int)(player.player.statLife * 0.5);
                    damage = damage * 5;
               }
            }
        }
    }
	[UsesEffect(typeof(SatansAceEffect))]
	public class SatansAce : WeaponModifier
	{
		public override ModifierTooltipLine[] TooltipLines => new[]
		{
			new ModifierTooltipLine {Text = $"Satan's Ace", Color = Color.DarkRed}
		};

		public override ModifierProperties GetModifierProperties(Item item)
		{
			return base.GetModifierProperties(item).Set(minMagnitude: 5f, maxMagnitude: 15f, rollChance: 2f, uniqueRoll: true);
		}

        public override bool CanRoll(ModifierContext ctx)
        {
            return base.CanRoll(ctx) && ctx.Method != ModifierContextMethod.SetupStartInventory;
        }
    }
}

