using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoAsteroids;

//P.S. klassen Player ärver ifrån en redan färdig basklass DrawableGameComponent från ett bibliotek "Microsoft Xna Framework"
//Player ärver också ifrån ett interface (dvs. användargränsnittet mot den här klassen) och därmed måste du skriva ut public för varje egenskap, annars funkar inte programmet.
class Player : DrawableGameComponent, IGameObject
{
    public bool IsDead { get; set; }
    public Vector2 Position { get; set; }
    public float Radius { get; set; }
    public Vector2 Speed { get; set; }
    public float Rotation { get; set; }
    //en egenskap som signalierar om man kan avfyra skott eller inte 
    public bool CanShoot { get { return _reloadTimer == 0; } }
    private Texture2D _playerTexture;
    //skapar en timer för att begränsa antal laserskott per knapptryckning (P.S. man skulle kunna göra en klass för timer eftersom det finns på flera ställen så man samlar alla timers på ett ställe)
    private int _reloadTimer = 0;
    //skapar ett random-objekt för att kunna slumpa ett antal tal 
    private Random random = new Random();

    //skicka med ett game för att använda DrawableGameComponent genom att skapa en konstruktor
    public Player(Game game) : base(game)
    {
        //ge spelaren en startposition (mitt på skärmen)
        Position = new Vector2(Globals.ScreenWidth / 2, Globals.ScreenHeight / 2);
    }

    protected override void LoadContent()
    {
        _playerTexture = Game.Content.Load<Texture2D>("player");
    }

    //skapar en egen metod Draw eftersom vi vill ha kontroll över vad som ritas ut och att det sker i rätt ordning (ej automatiskt av DrawableGameComponent)
    public void Draw(SpriteBatch spriteBatch)
    {
        //ritar ut spelaren på skärmen
        //ändra uppritningen av spriteBatch 
        //piOver2 = roteras 90 grader. Alltså skeppet kommer att åka med framänden i rätt riktning
        spriteBatch.Draw(_playerTexture, Position, null, Color.White, Rotation + MathHelper.PiOver2,
        new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
        1.0f, SpriteEffects.None, 0f);
    }
    public override void Update(GameTime gameTime)
    {
        Position += Speed;

        //kontrollerar om reloadtimer är större än 0 då kommer den minskas till den blir 0 för att vi ska kunna skjuta laserskott på nytt. Det gör att antal skott begränsas per knapptrycking! 
        if (_reloadTimer > 0)
            _reloadTimer--;

        //spelaren kontrollerar positionen mot ytan på spel planen
        //Exempelvis om spelaren är för långt till vänster av spel planen (dvs. skeppet åker ut till höger) då hamnar skeppet till vänster i x-led
        if (Position.X < Globals.GameArea.Left)
            Position = new Vector2(Globals.GameArea.Right, Position.Y);

        if (Position.X > Globals.GameArea.Right)
            Position = new Vector2(Globals.GameArea.Left, Position.Y);

        if (Position.Y < Globals.GameArea.Top)
            Position = new Vector2(Position.X, Globals.GameArea.Bottom);

        if (Position.Y > Globals.GameArea.Bottom)
            Position = new Vector2(Position.X, Globals.GameArea.Top);

        base.Update(gameTime);
    }
    public void Accelerate()
    {
        //öka hastigheten i den riktning som skeppet pekar åt genom att använda trigonomitri
        Speed += new Vector2((float)Math.Cos(Rotation),
        (float)Math.Sin(Rotation)) * 0.10f;

        //begränsa hastigheten (maxgräns) till 5 pixlar per uppdatering
        if (Speed.LengthSquared() > 25)
        {
            //Normalisera ner hastigheten till längden 1 för att sedan multiplicera med 5 för få maxhastigheten
            Speed = Vector2.Normalize(Speed) * 5;
        }
    }
    public Shot Shoot()
    {
        //skapar en spärr dvs. om vi inte kan skjuta så retuneras null
        if (!CanShoot)
        {
            return null;
        }

        _reloadTimer = 20;

        return new Shot()
        {
            //först skapas objektet och sedan sätts egenskaperna för Position innan de läggs in i listan för Shot()
            Position = Position,
            //P.S. Math-operationer ger värde double och därför måste vi tvinga värdena till en float eftersom man i spel oftast använder float
            //Rotation = riktningen på skeppet 
            Speed = Speed + 10f * new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation)),
            //alla skotten behöver INTE börja på samma position och därför slumpar vi ett tal med hjälp av MathHelper.TwoPi i en riktning för ett helt varv 
            Rotation = random.Next() * MathHelper.TwoPi
        };
    }
}
