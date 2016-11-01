namespace Emu12864
{
    class Game : GameBase
    {
        private string str;
        int x, y;
        private Images imgs;

        public override void Start()
        {
            str = "Hello Gensokyo!";
            x = y = 0;
            imgs = new Images();
        }

        public override void Loop()
        {
            imgs.BMP.Draw(0, 0);
            Engine.DrawString(x, y, str, 0xFFFFFF, false, true);
        }
    }
}
