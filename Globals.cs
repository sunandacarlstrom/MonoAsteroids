using Microsoft.Xna.Framework;
namespace MonoAsteroids
{
    class Globals
    {
        //P.S. använd static här för att INTE behöva instansiera ett nytt objekt vid användning av ScreenWidth/ScreenHeight
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        //flyg ut ur skärmen men kom tillbaka på andra sidan 
        public static Rectangle GameArea
        {
            get
            {
                //här ritas spel arean ut
                return new Rectangle(-80, -80, ScreenWidth + 160, ScreenHeight + 160);
            }
        }

        //definerar en annan Rectangle som vi kallar för RespawnArea
        public static Rectangle RespawnArea
        {
            //mitten av skärmen (centrerad kring 200 pixlar i sidled till vänster och uppåt, och 200 pixlar till höger och neråt. Detta kommer att bli ett kvadratiskt område på 400x400 pixlar)
            //detta ska vi använda till RespawnArea dvs. här ska vi inte kunna placera ut meteorer! 
            get
            {
                return new Rectangle((int)CenterScreen.X - 200, (int)CenterScreen.Y - 200, 400, 400);
            }
        }
        //behöver en center screen 
        public static Vector2 CenterScreen
        {
            get
            {
                return new Vector2(ScreenWidth / 2, ScreenHeight / 2);
            }
        }
    }


}