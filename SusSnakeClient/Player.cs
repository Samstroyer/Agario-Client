using System.Numerics;
using Raylib_cs;

public class Player
{
    public SnakeProperties playerProps = new();
    Vector2 res;

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
        Raylib.DrawCircle((int)(res.X / 2), (int)(res.Y / 2), playerProps.Size, Color.GREEN);
    }

    public void Move(float xSpeed, float ySpeed)
    {
        playerProps.X += (int)xSpeed;
        playerProps.Y += (int)ySpeed;

        int limit = (int)(Food.SpawnRadius);

        if (playerProps.X <= -limit + playerProps.Size) playerProps.X = -limit + playerProps.Size;
        if (playerProps.X >= limit - playerProps.Size) playerProps.X = limit - playerProps.Size;

        if (playerProps.Y <= -limit + playerProps.Size) playerProps.Y = -limit + playerProps.Size;
        if (playerProps.Y >= limit - playerProps.Size) playerProps.Y = limit - playerProps.Size;

        Console.WriteLine("X: {0}, Y: {1}", playerProps.X, playerProps.Y);

    }

    public List<int> Intersect(ref List<Food> food)
    {
        if (food.Count < 1) return new();

        List<int> foodIndexes = new();

        for (int i = food.Count - 1; i >= 0; i--)
        {
            if (food[i].taken) continue;
            if (Raylib.CheckCollisionCircles(Pos, playerProps.Size, new(food[i].X, food[i].Y), Food.Radius))
            {
                foodIndexes.Add(i);
                food[i].taken = true;
                playerProps.Size++;
            }
        }

        return foodIndexes;
    }
}
