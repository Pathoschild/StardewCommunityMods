using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace TimeSpeed
{
    public static class ClickableExtensions
    {
        private abstract class DrawEventHack: IClickableMenu
        {
            private readonly Action<SpriteBatch> _callback;

            protected DrawEventHack(Action<SpriteBatch> callback)
            {
                if(callback == null) throw new ArgumentNullException(nameof(callback));
                _callback = callback;
            }

            public override void receiveRightClick(int x, int y, bool playSound = true)
            {
                throw new NotImplementedException();
            }

            public override void draw(SpriteBatch b)
            {
                _callback(b);
            }

            public abstract void Subscribe(IClickableMenu menu);
        }

        private sealed class AfterDrawHack : DrawEventHack
        {
            public AfterDrawHack(Action<SpriteBatch> callback) : base(callback)
            {}

            public override void Subscribe(IClickableMenu menu)
            {
                var index = Game1.onScreenMenus.FindIndex(m => ReferenceEquals(m, menu));
                Game1.onScreenMenus.Insert(index + 1, this);
            }
        }

        private sealed class BeforeDrawHack : DrawEventHack
        {
            public BeforeDrawHack(Action<SpriteBatch> callback) : base(callback)
            {}

            public override void Subscribe(IClickableMenu menu)
            {
                var index = Game1.onScreenMenus.FindIndex(m => ReferenceEquals(m, menu));
                Game1.onScreenMenus.Insert(index, this);
            }
        }

        public static void AfterDraw(this IClickableMenu menu, Action<SpriteBatch> afterDraw)
        {
            new AfterDrawHack(afterDraw).Subscribe(menu);
        }

        public static void BeforeDraw(this IClickableMenu menu, Action<SpriteBatch> beforeDraw)
        {
            new BeforeDrawHack(beforeDraw).Subscribe(menu);
        }
    }
}
