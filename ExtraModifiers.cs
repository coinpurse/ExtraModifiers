using Terraria;
using Microsoft.Xna.Framework;
using Loot;
using Terraria.ModLoader;

namespace ExtraModifiers
{
    class ExtraModifiers : Mod
    {
        public ExtraModifiers()
        {
        }

        public override void Load()
        {
            
            EMMLoader.RegisterMod(this);
            EMMLoader.SetupContent(this);
        }
    }
}

