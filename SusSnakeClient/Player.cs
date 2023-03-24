using System.Numerics;
using Raylib_cs;

/*
    This is the player class
    The player class is used by the user playing
    Other players just use the "PlayerProperties"
*/

public class Player
{
    public PlayerProperties playerProps = new();
    Vector2 res;

    private Vector2 Pos
    {
        get
        {
            return new(playerProps.X, playerProps.Y);
        }
        set
        {
            playerProps.X = value.X;
            playerProps.Y = value.Y;
        }
    }

    // The player init, gets the resolution
    public Player()
    {
        res = new(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
    }

    // Draw the player (does not use SnakeProperties draw because it is offset for rendering other players)
    public void Draw()
    {
        // Just always draw the player in the middle of the screen
        Raylib.DrawCircle((int)(res.X / 2), (int)(res.Y / 2), playerProps.Size, Color.GREEN);
    }

    // This function is for moving and keeping the player in the playable zone
    public void Move(float xSpeed, float ySpeed)
    {
        playerProps.X += xSpeed;
        playerProps.Y += ySpeed;

        int limit = (int)(Food.SpawnRadius);

        if (playerProps.X <= -limit + playerProps.Size) playerProps.X = -limit + playerProps.Size;
        if (playerProps.X >= limit - playerProps.Size) playerProps.X = limit - playerProps.Size;

        if (playerProps.Y <= -limit + playerProps.Size) playerProps.Y = -limit + playerProps.Size;
        if (playerProps.Y >= limit - playerProps.Size) playerProps.Y = limit - playerProps.Size;

        Console.WriteLine("X: {0}, Y: {1}", playerProps.X, playerProps.Y);

    }

    // This function checks if the player is eating any food
    public List<int> Intersect(ref List<Food> food)
    {
        if (food.Count < 1) return new();

        List<int> foodIndexes = new();

        for (int i = food.Count - 1; i >= 0; i--)
        {
            if (food[i].taken) continue;
            if (Raylib.CheckCollisionCircles(Pos, playerProps.Size, new(food[i].X, food[i].Y), Food.Radius))
            {
                // Add the foods index to the list of indexes, later gets sent to the server for handling
                foodIndexes.Add(i);
                food[i].taken = true;
                playerProps.Size++;
            }
        }

        return foodIndexes;
    }
}
