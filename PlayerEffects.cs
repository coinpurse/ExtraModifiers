using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
namespace ExtraModifiers
{
    class PlayerEffects : ModPlayer
    {
        public void createBloodplosion(float X, float Y, int damage)
        {
            Projectile.NewProjectile(X, Y, 0, 0, mod.ProjectileType("Bloodsplosion"), damage, 0f, Main.myPlayer);
        }
    }
}
