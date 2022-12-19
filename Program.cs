namespace MonoAsteroids;
public class Program
{
    static void Main(string[] args)
    {
        using var game = new MonoAsteroids.Game1();
        game.Run();
    }
}
