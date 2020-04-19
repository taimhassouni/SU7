using System.IO;
using System;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Timers;
using DIKUArcade.Utilities;
using System.Collections.Generic;

public class Game : IGameEventProcessor<object>
{
    //CLASS FIELDS
    public List<Image> enemyStrides;
    private List<Image> explosionStrides;
    private AnimationContainer explosions;
    private int explosionLength = 500;
    public List<Enemy> enemies;
    public List<PlayerShot> playerShots;
    private GameEventBus<object> eventBus;
    private Window win;
    private Player player;
    private DIKUArcade.Timers.GameTimer gameTimer;
    private Score score;

    //CLASS CONSTRUCTOR
    public Game()
    {
        win = new Window("Galaga", 500, 500);
        gameTimer = new GameTimer(60, 60);
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")),this);


        eventBus = new GameEventBus<object>(); //3.3
        eventBus.InitializeEventBus(new List<GameEventType>() {
            GameEventType.InputEvent, // key press / key release
            GameEventType.WindowEvent, // messages to the window 
            GameEventType.PlayerEvent,
            });
        win.RegisterEventBus(eventBus);
        eventBus.Subscribe(GameEventType.InputEvent, this);
        eventBus.Subscribe(GameEventType.WindowEvent, this);
        eventBus.Subscribe(GameEventType.PlayerEvent, this.player);

        // Creating enemies
        enemyStrides = ImageStride.CreateStrides(4,
        Path.Combine("Assets", "Images", "BlueMonster.png"));
        enemies = AddEnemies();

        // Shots
        playerShots = new List<PlayerShot>();

        // Explosion
        explosionStrides = ImageStride.CreateStrides(8,
        Path.Combine("Assets", "Images", "Explosion.png"));
        explosions = new AnimationContainer(300);

        // Scoreboard
        score = new Score(new Vec2F(0.05F, -0.25F), new Vec2F(0.3F, 0.3F));
    }
    //CLASS METHODS

    public List<Enemy> AddEnemies()
    {
        List<Enemy> temp = new List<Enemy>();
        for (float x = 0.10F; x <= 0.9; x = x + 0.10F)
        {
            Enemy enemy = new Enemy(
                new DynamicShape(new Vec2F(x, 0.9F), new Vec2F(0.1F, 0.1F)), 
                new ImageStride(80, enemyStrides));

            temp.Add(enemy);
        }
        return temp;
    }
    public void GameLoop()
    {
        while(win.IsRunning())
        {
            gameTimer.MeasureTime();
            while (gameTimer.ShouldUpdate())
            {
                win.PollEvents();
                // Update game logic here
                player.Move();
                eventBus.ProcessEvents();
                
            }

            if (gameTimer.ShouldRender())
            {
                win.Clear();
                // Render gameplay entities here
                player.Entity.RenderEntity();
                foreach(Enemy enemy in enemies)
                {
                    enemy.RenderEntity();
                }
                foreach(PlayerShot shot in playerShots)
                {
                    shot.RenderEntity();
                }
                IterateShots();
                explosions.RenderAnimations();
                score.RenderScore();
                win.SwapBuffers();
            }
            if (gameTimer.ShouldReset())
            {
                // 1 second has passed - display last captured ups and fps
                win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
            }
        }
    }

    private void KeyPress(string key) 
    {
        switch(key) 
        {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                    
                break;
            
            case "KEY_LEFT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this.player, "MOVE_LEFT", "", ""));         
                break;

            case "KEY_RIGHT":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this.player, "MOVE_RIGHT", "", ""));  
                break;

            case "KEY_SPACE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                    GameEventType.PlayerEvent, this.player, "SHOOT", "", "")); 
                break;
            default:
                break;
        }
    }
    public void KeyRelease(string key)
    {
        switch(key) 
        { 
            case "KEY_LEFT":
                player.Direction(new Vec2F(0.00f, 0.00f ));
                break;

            case "KEY_RIGHT":
                player.Direction(new Vec2F(0.00f, 0.00f ));
                break;
        }
    }
    public void ProcessEvent(GameEventType eventType,GameEvent<object> gameEvent){

        
        if (eventType == GameEventType.WindowEvent) 
        {
            switch (gameEvent.Message) 
            {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                default:
                    break;
            }
        } 
        else if (eventType == GameEventType.InputEvent){
            switch (gameEvent.Parameter1){
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
            }
        }

        }
    public void IterateShots()
    {
        List<PlayerShot> newShots = new List<PlayerShot>();
        List<Enemy> newEnemies = new List<Enemy>();

        foreach (PlayerShot shot in playerShots)
        {
            shot.Shape.Move();
            if (shot.Shape.Position.Y > 1.0f)
            {
                shot.DeleteEntity();    
            } 
            else 
            {
                foreach (Enemy enemy in enemies) 
                {
                    if(CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), enemy.Shape.AsStationaryShape()).Collision == true)
                    {
                        shot.DeleteEntity();
                        enemy.DeleteEntity();
                        AddExplosion(shot.Shape.Position.X, shot.Shape.Position.Y, 0.1F, 0.1F);
                        score.AddPoint(25);
                    }
                }
            }
        }
        // Check if shots are marked and delete them
        foreach(PlayerShot shot in playerShots)
        {
            if(!shot.IsDeleted())
            {
                newShots.Add(shot);
            }
        }
        playerShots = newShots;

        // Check if enemies are marked and delete them
        foreach(Enemy enemy in enemies) 
        {                        
            if (!enemy.IsDeleted())
            {
                newEnemies.Add(enemy);
            }
        }
        enemies = newEnemies;
    }
    public void AddExplosion(float posX, float posY,float extentX, float extentY)
    {
        explosions.AddAnimation(
        new StationaryShape(posX, posY, extentX, extentY), explosionLength,
        new ImageStride(explosionLength / 8, explosionStrides));
    }
}