using ExtraModifiers.Modifiers.WeaponModifiers;
using Loot;
using Loot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TestMod.NPCs
{
    class ExtraModifiersNPC : GlobalNPC
    {

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            BloodthirstyEffect bloodthirsty = player.GetModPlayer<ModifierPlayer>().GetEffect<BloodthirstyEffect>();
            spawnRate = (int)(spawnRate / bloodthirsty.spawnRateMultiplier);
            maxSpawns = (int)(maxSpawns * bloodthirsty.spawnMaxMultiplier);
        }
    }
}
