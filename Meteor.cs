using Microsoft.Xna.Framework;

namespace MonoAsteroids;

//definerar tre sorters meteoriter
enum MeteorType
{
    Big,
    Medium,
    Small
}

//denna är per automatik internal och betyder att man endast kan komma åt det inom projektet (assembly)
class Meteor : GameObject
{
    public MeteorType Type { get; private set; }

    //när vi skapar en Meteor kräver vi en MetorType 
    public Meteor(MeteorType type)
    {
        Type = type;
        //gör olika saker beroende på vilken meteor som vi vill skapa
        switch (Type)
        {
            case MeteorType.Big:
                Radius = 42;
                break;
            case MeteorType.Medium:
                Radius = 24;
                break;
            case MeteorType.Small:
                Radius = 16;
                break;
        }
    }
    public void Update(GameTime gameTime)
    {
        //positionen flyttas fram med hastigheten  
        Position += Speed;

        //meteoren kontrollerar positionen mot ytan på spel planen
        //Exempelvis om meteoren är för långt till vänster av spel planen (dvs. meteoren åker ut till höger) då hamnar meteoren till vänster i x-led
        if (Position.X < Globals.GameArea.Left)
            Position = new Vector2(Globals.GameArea.Right, Position.Y);

        if (Position.X > Globals.GameArea.Right)
            Position = new Vector2(Globals.GameArea.Left, Position.Y);

        if (Position.Y < Globals.GameArea.Top)
            Position = new Vector2(Position.X, Globals.GameArea.Bottom);

        if (Position.Y > Globals.GameArea.Bottom)
            Position = new Vector2(Position.X, Globals.GameArea.Top);

        //Meteorerna kommer att rotera långsamt 
        Rotation += 0.04f;

        if (Rotation > MathHelper.TwoPi)
        {
            Rotation = 0;
        }

    }
}