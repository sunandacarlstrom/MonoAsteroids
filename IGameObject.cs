using Microsoft.Xna.Framework;

namespace MonoAsteroids;

//interface är som en slags lista (kontrakt) över alla "game objects" och vad de har för egenskaper
//P.S. allt som är i ett interface är tänkt att vara publikt och därmed behöver man inte skriva ut public för varje egenskap i själva interfacet. 
interface IGameObject
{
    bool IsDead { get; set; }
    Vector2 Position { get; set; }
    //Radius = radie som vi kommer använda till alla "game obejcts" om de koliderar eller inte 
    float Radius { get; set; }
    Vector2 Speed { get; set; }
    float Rotation { get; set; }
}
