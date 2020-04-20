using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;

namespace galaga.GalagaStates {
  public class GamePaused : IGameState {
    private static GamePaused instance = null;

    private Entity backGroundImage;
    private Text[] menuButtons;
    private int activeMenuButton;
    private int maxMenuButtons;

    public static GamePaused GetInstance() {
      return GamePaused.instance ?? (GamePaused.instance = new GamePaused());
    }

    public void InitializeGameState() {

    }

    public void RenderState() {
      Image image = new Image("TitleImage.png");
      Shape shape = new Shape();
      backGroundImage = new Entity(shape, image);
      backGroundImage.RenderEntity();

      Text new_game = new Text("New Game",
                               new Vec2F(0.25f, 0.5f),
                               new Vec2F(0.5f, 0.25f));
      Text quit = new Text("Quit",
                           new Vec2F(0.25f, 0.5f),
                           new Vec2F(0.5f, 0.25f));
      menuButtons = new Text[]{new_game, quit};
      activeMenuButton = 0;
      menuButtons[activeMenuButton].RenderText();
    }

    public void HandleKeyEvent(string keyValue, string keyAction) {
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

    }

    public void UpdateGameLogic() {

    }
  }
}