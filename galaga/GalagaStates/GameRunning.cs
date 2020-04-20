using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;

namespace galaga.GalagaStates {
  public class GameRunning : IGameState {
    private static MainMenu instance = null;

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

    public static GameRunning GetInstance() {
      return GameRunning.instance ?? (GameRunning.instance = new GameRunning());
    }

    public void InitializeGameState() {
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
      IterateShots();
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
