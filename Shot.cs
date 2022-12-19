using Microsoft.Xna.Framework;

namespace MonoAsteroids;

class Shot : GameObject
{
    public Shot()
    {
        Radius = 16;
    }

    //lägger till Update för att skotten ska uppdateras av sig själva 
    public void Update(GameTime gameTime)
    {
        Position += Speed;
        Rotation += 0.08f;

        //kontrollerar om rotationen är större än ett helt varv då sätts rotatioonen till 0 igen. För att variabelerna inte ska snurra på i alla evighet då det finns risk för bugg (overflow)
        if (Rotation > MathHelper.TwoPi)
        {
            Rotation = 0;
        }
    }
}