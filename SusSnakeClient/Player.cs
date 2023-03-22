using System.Numerics;
using Raylib_cs;

public class Player
{
    public SnakeProperties playerProps = new();
    Vector2 res;

    private int playerRadius = 20;
    private Vector2 Pos
    {
        get
        {
            return new(playerProps.X, playerProps.Y);
        }
        set
        {
            playerProps.X = (int)value.X;
            playerProps.Y = (int)value.Y;
        }
    }

    public Player()
    {
        res = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }

    public void Draw()
    {

        Raylib.DrawCircle((int)(res.X / 2), (int)(res.Y / 2), playerRadius, Color.GREEN);

        if (playerProps.Body.Count < 1) return;

        // List<int> points = playerProps.PreviousPointsList;
        // for (int i = 0; i < points.Count; i++)
        // {
        //     Raylib.DrawCircle(points[i].)
        // }
    }

    public void Move(float xSpeed, float ySpeed)
    {
        playerProps.X += xSpeed;
        playerProps.Y += ySpeed;

        int limit = (int)(Food.SpawnRadius);

        if (playerProps.X <= -limit) playerProps.X = -limit;
        if (playerProps.X >= limit) playerProps.X = limit;

        if (playerProps.Y <= -limit) playerProps.Y = -limit;
        if (playerProps.Y >= limit) playerProps.Y = limit;

        Console.WriteLine("X: {0}, Y: {1}", playerProps.X, playerProps.Y);
    }

    public List<int> Intersect(ref List<Food> food)
    {
        if (food.Count < 1) return new();

        List<int> foodIndexes = new();

        for (int i = food.Count - 1; i >= 0; i--)
        {
            if (food[i].taken) continue;
            if (Raylib.CheckCollisionCircles(Pos, playerRadius, new(food[i].X, food[i].Y), Food.Radius))
            {
                foodIndexes.Add(i);
                food[i].taken = true;
            }
        }

        return foodIndexes;
    }
}
