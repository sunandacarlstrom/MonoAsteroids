using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoAsteroids;

public class Game1 : Game
{
    //skapar fields (variabler)
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _backgroundTexture;
    private Player _player;
    private KeyboardState _previousKbState;
    private List<Shot> _shots = new List<Shot>();
    private Texture2D _laserTexture;
    private List<Meteor> _meteors = new List<Meteor>();
    private Texture2D _meteorBigTexture;
    private Random random = new Random();


    //skapar konstruktor
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        //ändrar standardupplösningen av skärmen med egna inställningar i klassen Globals
        _graphics.PreferredBackBufferWidth = Globals.ScreenWidth;
        _graphics.PreferredBackBufferHeight = Globals.ScreenHeight;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        //skapar spelaren måste också skicka med ett spel (this = spelet som vi befinner oss i)
        _player = new Player(this);
        //och lägger till en spelare bland mina komponenter. Detta gör att den automatiskt anropar metoderna Update & Draw 
        Components.Add(_player);

        //TODO: skapa 10 st meteorer och ska slumpas i vilken riktning de finns och INTE slumpas ut där spelaren befinner sig 
        //anropar ResetMeteors 
        ResetMeteors();

        base.Initialize();
    }

    public void ResetMeteors()
    {
        //kör sålänge antal meteorer är under 10
        while (_meteors.Count < 10)
        {
            //slumpar ut en vinkel med hjälp av Random-objektet, på samma sätt som vi slumpar ut vinkeln på skotten
            var angle = random.Next() * MathHelper.TwoPi;
            //här skapar vi ett nytt objekt (en ny meteor) med meteortypen Big
            var m = new Meteor(MeteorType.Big)
            {
                Position = new Vector2(Globals.GameArea.Left + (float)random.NextDouble() * Globals.GameArea.Width,
                Globals.GameArea.Top + (float)random.NextDouble() * Globals.GameArea.Height),
                Rotation = angle,
                Speed = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * random.Next(20, 60) / 30.0f
            };

            //avslutar loopen (ovan) genom att kontrollera om vi ligger utanför RespawnArea så ska vi läsa detta med vår position och läggaer in meteoren och fortsätter tills vi har som max 10 meteorer
            if (!Globals.RespawnArea.Contains(m.Position))
            {
                _meteors.Add(m);
            }
        }
    }

    protected override void LoadContent()
    {
        //skapar en ny SpriteBatch som kan användas för att rita texturer
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //laddar in bakgruden
        _backgroundTexture = Content.Load<Texture2D>("background");

        //laddar in laserskotten för att kunna använda i metoden Shot()
        _laserTexture = Content.Load<Texture2D>("laser");

        //laddar in en meteorer för att kunna använda i klassen Meteor 
        _meteorBigTexture = Content.Load<Texture2D>("meteorBrown_big4");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //vilka knappar som är nedtryckta 
        KeyboardState state = Keyboard.GetState();

        //kontrollerar om vi trycker ner ex. piltangent UPP så utförs funktionen
        if (state.IsKeyDown(Keys.Up))
            _player.Accelerate();

        //kontrollerar rotationen av skeppet om vi trycker ner ex. piltangent HÖGER så utförs funktionen 
        if (state.IsKeyDown(Keys.Left))
            _player.Rotation -= 0.05f;
        else if (state.IsKeyDown(Keys.Right))
            _player.Rotation += 0.05f;

        if (state.IsKeyDown(Keys.Space))
        {
            //när vi trycker på Space-knappen så får vi ett skott av player som anropar metoden Shoot
            Shot s = _player.Shoot();
            if (s != null)
                _shots.Add(s);
        }

        //gör en ny loop för varje skott 
        foreach (Shot shot in _shots)
        {
            shot.Update(gameTime);
            //för varje skott 
            //i listan av meteorer så tar den första eller standardvärdet (P.S. kan även vara null om jag inte får en träff) där jag kontrollerar om för varje meteor m i listan kolliderar med Shot (skott) dvs. första som den hittar 
            Meteor meteor = _meteors.FirstOrDefault(m => m.CollidesWith(shot));

            //om jag har en kollision så ska detta utföras 
            if (meteor != null)
            {
                _meteors.Remove(meteor);
                shot.IsDead = true;
            }
        }


        foreach (Meteor meteor in _meteors)
        {
            meteor.Update(gameTime);
        }

        //VIKTIGT att städa bort saker som ligger utanför skärmen och "skräpar" för att inte spelet ska riskera att segas ner
        //ta bort alla skjutna skott eller skott som svävar utanför skärmen 
        //skapa en loop som retunerar true om alla skott s är död 
        _shots.RemoveAll(s => s.IsDead || !Globals.GameArea.Contains(s.Position));

        //det finns redan en update i arvet DrawableGameComponent (här använder vi inte vår egen metod Update)
        _player.Update(gameTime);
        _previousKbState = state;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        //vill fortfarande visa hela bakgrunden trots att det är en större spelplan genom att...
        //för varje loop ökar både höjden och bredden på texturen tills den överlappar bakgrunden 
        for (int y = 0; y < Globals.ScreenHeight; y += _backgroundTexture.Width)
        {
            for (int x = 0; x < Globals.ScreenWidth; x += _backgroundTexture.Width)
            {

                //P.S. use white in order to use the orignal colours of the sprite
                //_spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
                //ritar över texturen så att den överlappaar bakgrunden så många ggr det behövs för att täcka bredden
                _spriteBatch.Draw(_backgroundTexture, new Vector2(x, y), Color.White);
            }
        }

        //ritar upp laserskotten på skärmen FÖRE själva spelaren för att skotten ska se ut att komma från skeppets framände och inte från mitten av skeppet då skotten istället kommer under skeppet
        foreach (Shot s in _shots)
        {
            _spriteBatch.Draw(_laserTexture, s.Position, null, Color.White, s.Rotation,
            new Vector2(_laserTexture.Width / 2, _laserTexture.Height / 2),
            1.0f, SpriteEffects.None, 0f);
        }

        //ritar upp alla meteorer 
        foreach (Meteor meteor in _meteors)
        {
            _spriteBatch.Draw(_meteorBigTexture, meteor.Position, null, Color.White, meteor.Rotation,
            new Vector2(_meteorBigTexture.Width / 2, _meteorBigTexture.Height / 2),
            1.0f, SpriteEffects.None, 0f);
        }

        _player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
