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

namespace galaga.GalagaStates {
  public class GameRunning : IGameState {
    private static GameRunning instance = null;

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
    private Game game;
    private StateMachine stateMachine = new StateMachine();

    public static GameRunning GetInstance() {
      return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
    }

    public void InitializeGameState() {
      game = new Game();
      win = new Window("Galaga", 500, 500);
      gameTimer = new GameTimer(60, 60);

      // Creating enemies
      enemyStrides = ImageStride.CreateStrides(4,
      Path.Combine("Assets", "Images", "BlueMonster.png"));
      enemies = game.AddEnemies();

      // Shots
      playerShots = new List<PlayerShot>();

      // Explosion
      explosionStrides = ImageStride.CreateStrides(8,
      Path.Combine("Assets", "Images", "Explosion.png"));
      explosions = new AnimationContainer(300);

      // Scoreboard
      score = new Score(new Vec2F(0.05F, -0.25F), new Vec2F(0.3F, 0.3F));
    }

    public void RenderState() {
      player.Entity.RenderEntity();
      foreach(Enemy enemy in enemies)
      {
          enemy.RenderEntity();
      }
      foreach(PlayerShot shot in playerShots)
      {
          shot.RenderEntity();
      }
      game.IterateShots();
      explosions.RenderAnimations();
      score.RenderScore();
    }

    public void HandleKeyEvent(string keyValue, string keyAction) {

    }

    public void GameLoop() {
      while(win.IsRunning())
      {
          gameTimer.MeasureTime();
          while (gameTimer.ShouldUpdate())
          {
              win.PollEvents();
              // Update game logic here
              player.Move();
              eventBus.ProcessEvents();
              stateMachine.ActiveState.RenderState();
              win.SwapBuffers();
          }

          if (gameTimer.ShouldRender())
          {
              win.Clear();
              // Render gameplay entities here

          }
          if (gameTimer.ShouldReset())
          {
              // 1 second has passed - display last captured ups and fps
              win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " + gameTimer.CapturedFrames;
          }
      }
    }

    public void UpdateGameLogic() {

    }
  }
}
