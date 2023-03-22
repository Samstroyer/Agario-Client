using Raylib_cs;

Engine e;

Setup();
Start();

void Setup()
{
    Raylib.InitWindow(800, 800, "Sussy wussy snake");
    Raylib.SetTargetFPS(60);
    e = new();
}

void Start()
{
    e.Start();
}
