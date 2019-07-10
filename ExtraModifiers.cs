using Terraria;
using Microsoft.Xna.Framework;
using Loot;
using Terraria.ModLoader;

namespace TestMod
{
    class TestMod : Mod
    {
        public TestMod()
        {
        }

        public override void Load()
        {
            EMMLoader.RegisterMod(this);
            EMMLoader.SetupContent(this);
        }
    }
}

