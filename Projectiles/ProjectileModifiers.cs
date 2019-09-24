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
        
        //ModifyHit Overrides
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            SatansAce(ref damage);
            PercentGold(target);
            PercentBossDamage(target, ref damage);
            base.ModifyHitNPC(projectile, target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            SatansAce(ref damage);
            base.ModifyHitPvp(projectile, target, ref damage, ref crit);
        }

        //OnHit overrides
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

        //################################################################################################################

        //PercentBossDamage call - PvE
        public void PercentBossDamage(NPC target, ref int damage)
        {
            mplr.GetEffect<PercentBossDamageEffect>().setBossDamage(target, ref damage);
        }

        //PercentGold call - PvE
        public void PercentGold(NPC target)
        {
            mplr.GetEffect<PercentGoldEffect>().setPercentGold(target);
        }

        //Satan's Ace call - PvE & PvP
        public void SatansAce(ref int damage)
        {
            mplr.GetEffect<SatansAceEffect>().AceEffect(mplr, ref damage);
        }

        //Bloodthirsty call - PvE
        public void Bloodthirsty(NPC target)
        {
                mplr.GetEffect<BloodthirstyEffect>().StartRampage(target);
        }

        //Bloodplosion call - PvE
        public void Bloodsplode(NPC target, int damage)
        {
            mplr.GetEffect<BloodsplosionEffect>().Bloodsplode(damage, mplr, target);
        }

        //Bloodplosion call - PvP
        public void Bloodsplode(Player target, int damage)
        {
            mplr.GetEffect<BloodsplosionEffect>().Bloodsplode(damage, mplr, target);
        }

    }
}
