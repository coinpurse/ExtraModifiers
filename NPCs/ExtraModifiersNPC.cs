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
        public static float spawnMultiplier = 1f;

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            spawnRate = (int)(spawnRate / spawnMultiplier);
            maxSpawns = (int)(maxSpawns * spawnMultiplier);
        }

        public static void resetSpawnRate()
        {
            spawnMultiplier = 1f;
        }
    }
}
