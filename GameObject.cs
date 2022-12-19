using Microsoft.Xna.Framework;

namespace MonoAsteroids;

//skapar en fungerande basklass som ärver ifrån IGameObject (P.S. detta för att slippa realisera / implementera allt från IGameObject varje gång jag skapar en klass som nu istället ingår i GameObject)
//sätter abstract här eftersom GameObject inte är ett "färdig" objekt, snarare en slags förlaga
abstract class GameObject : IGameObject
{
    public bool IsDead { get; set; }
    public Vector2 Position { get; set; }
    public float Radius { get; set; }
    public Vector2 Speed { get; set; }
    public float Rotation { get; set; }

    //kontrollera ett GameObject mot ett annat GameObject ifall de kolliderar
    public bool CollidesWith(IGameObject other)
    {
        //gör en beräkning beroende på skillnaden på Positionsläge (P.S. vi ser varje meteor som cirklar och därför satt Radius främst för kollisionen)
        //så när meteorerna börjar kollidera då har längden mellan dem understigit den sammanlagda radien och överlappar varandra.  
        return (this.Position - other.Position).LengthSquared() < (Radius + other.Radius) * (Radius + other.Radius);
    }

}
