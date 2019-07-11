using Loot;
using ExtraModifiers.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Loot.Modifiers.WeaponModifiers;
using TestMod.Modifiers;

namespace ExtraModifiers.Projectiles
{
    class ProjectileModifiers : Terraria.ModLoader.GlobalProjectile
    {
        public override bool InstancePerEntity => true;



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

                // Get effect values
                if (projectile.owner != 255 && projectile.friendly && projectile.owner == Main.myPlayer)
                {
                    mplr = Main.LocalPlayer.GetModPlayer<ModifierPlayer>();
                }

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
            Bloodthirsty(target);
            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }

        public override void OnHitPlayer(Projectile projectile, Player target, int damage, bool crit)
        {
            Bloodsplode(target, damage);
            base.OnHitPlayer(projectile, target, damage, crit);
        }

        public void Bloodthirsty(NPC target)
        {
            if (target.life <= 0 && mplr.GetEffect<BloodthirstyEffect>().isActive)
            {
                mplr.GetEffect<BloodthirstyEffect>().StartRampage();
            }
        }

        public void Bloodsplode(NPC target, int damage)
        {
            if (target.life <= 0 && mplr.GetEffect<BloodsplosionEffect>().isActive)
            {
                mplr.player.GetModPlayer<PlayerEffects>().createBloodplosion(target.position.X, target.position.Y, (int)(damage * mplr.GetEffect<BloodsplosionEffect>().Multiplier));
            }
        }

        public void Bloodsplode(Player target, int damage)
        {
            if (target.statLife <= 0 && mplr.GetEffect<BloodsplosionEffect>().isActive)
            {
                mplr.player.GetModPlayer<PlayerEffects>().createBloodplosion(target.position.X, target.position.Y, (int)(damage * mplr.GetEffect<BloodsplosionEffect>().Multiplier));
            }
        }
        public void SatansAce(Projectile proj, ref int damage, NPC target)
        {
            if (mplr.GetEffect<SatansAceEffect>().isActive)
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
            if (mplr.GetEffect<SatansAceEffect>().isActive)
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
