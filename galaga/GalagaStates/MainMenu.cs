using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;

namespace galaga.GalagaStates {
  public class MainMenu : IGameState {
    private static MainMenu instance = null;

    private Entity backGroundImage;
    private Text[] menuButtons;
    private int activeMenuButton;
    private int maxMenuButtons;

    public static MainMenu GetInstance() {
      return MainMenu.instance ?? (MainMenu.instance = new MainMenu());
    }

    public void InitializeGameState() {

    }

    public void RenderState() {
      Image image = new Graphics.Image("TitleImage.png");
      Shape shape = new Entities.Shape();
      backGroundImage = new Entity(shape, image);
      backGroundImage.RenderEntity();

      Text new_game = new Graphics.Text("New Game",
                                        new Vec2F(0.25f, 0.5f),
                                        new Vec2F(0.5f, 0.25f));
      Text quit = new Graphics.Text("Quit",
                                    new Vec2F(0.25f, 0.5f),
                                    new Vec2F(0.5f, 0.25f));
      menuButtons = {new_game, quit};
      activeMenuButton = 0;
      menuButtons[activeMenuButton].RenderText();
    }

    public void HandleKeyEvent(string keyValue, string keyAction) {
      if (keyAction == "KEY_PRESS") {
        if (keyValue == "KEY_UP") {
          activeMenuButton = 1;
        }
        if (keyValue == "KEY_UP") {
          activeMenuButton = 0;
        }
      }
      if (keyAction == "KEY_RELEASE") {
        if (keyValue == "KEY_ENTER") {
          if (activeMenuButton == 0) {
            GameEventFactory<object>.CreateGameEventForAllProcessors(
              GameEventType.GameStateEvent,
              this,
              "CHANGE_STATE",
              "GAME_RUNNING", "");
          } else {
            GameEventFactory<object>.CreateGameEventForAllProcessors(
              GameEventType.GameStateEvent,
              this,
              "CHANGE_STATE",
              "GAME_PAUSED", "");
          }
        }
      }
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
