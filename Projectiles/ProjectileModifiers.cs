using Loot;
using TestMod.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Loot.Modifiers.WeaponModifiers;

namespace TestMod.Projectiles
{
    class ProjectileModifiers : Terraria.ModLoader.GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool bloodsplosion_isActive = false;
        public float bloodsplosion_multiplier = 1f;
        public bool satansAce_isActive = false;


        public bool NeedsClear;
        public bool FirstTick;
        
        ModifierPlayer mplr;
       
        public ProjectileModifiers Info(Projectile projectile, Mod mod = null)
            => mod == null
                ? projectile.GetGlobalProjectile<ProjectileModifiers>()
                : projectile.GetGlobalProjectile<ProjectileModifiers>(mod);
                
        
        public override bool PreAI(Projectile projectile)
        {
            var mproj = Info(projectile);
            if (!mproj.FirstTick)
            {
                mproj.FirstTick = true;

                // Snapshot current player values
                if (projectile.owner != 255 && projectile.friendly && projectile.owner == Main.myPlayer)
                {
                    mplr = Main.LocalPlayer.GetModPlayer<ModifierPlayer>();
                    mproj.bloodsplosion_multiplier = mplr.GetEffect<BloodsplosionEffect>().Multiplier;
                    mproj.bloodsplosion_isActive = mplr.GetEffect<BloodsplosionEffect>().isActive;
                    mproj.satansAce_isActive = mplr.GetEffect<SatansAceEffect>().isActive;
                }

            }
            if (NeedsClear)
            {
                mproj.bloodsplosion_multiplier = 1f;
                mproj.bloodsplosion_isActive = false;
                mproj.satansAce_isActive = false;
            }
            return base.PreAI(projectile);
        }
        
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            SatansAce(projectile, ref damage, target);
            base.ModifyHitNPC(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            SatansAce(projectile, ref damage, target);
            base.ModifyHitPvp(projectile, target, ref damage, ref crit);
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            Bloodsplode(target, damage);
            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            Bloodsplode(target, damage);
            base.OnHitPlayer(projectile, target, damage, crit);
        }

        public void Bloodsplode(NPC target, int damage)
        {
            if (target.life <= 0 && bloodsplosion_isActive)
            {
                mplr.player.GetModPlayer<PlayerEffects>().createBloodplosion(target.position.X, target.position.Y, (int)(damage * bloodsplosion_multiplier));
            }
        }

        public void Bloodsplode(Player target, int damage)
        {
            if (target.statLife <= 0 && bloodsplosion_isActive)
            {
                mplr.player.GetModPlayer<PlayerEffects>().createBloodplosion(target.position.X, target.position.Y, (int)(damage * bloodsplosion_multiplier));
            }
        }
        public void SatansAce(Projectile proj, ref int damage, NPC target)
        {
            if (satansAce_isActive)
            {
                int i = mplr.GetEffect<SatansAceEffect>().rand.Next(100);
                if (i < 6)
                {
                    mplr.player.statLife -= (int)(mplr.player.statLife * 0.5);
                    damage = damage * 5;
                }
            }
        }

        public void SatansAce(Projectile proj, ref int damage, Player target)
        {
            if (satansAce_isActive)
            {
                int i = mplr.GetEffect<SatansAceEffect>().rand.Next(100);
                if (i < 6)
                {
                    mplr.player.statLife -= (int)(mplr.player.statLife * 0.5);
                    damage = damage * 5;
                }
            }
        }
    }
}
