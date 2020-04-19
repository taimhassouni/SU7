namespace galaga.GalaStates
{
  public enum GameStateType {
    GameRunning,
    GamePaused,
    MainMenu
  }

  public class StateTransformer {
    public static GameStateType TransformStringToState(string state) {
      if (state == "GAME_RUNNING") {
        return GameStateType.GameRunning;
      } else if (state == "GAME_PAUSED") {
        return GameStateType.GamePaused;
      } else if (state == "MAIN_MENU") {
        return GameStateType.MainMenu;
      }
    }

    public static string TransformStateToString(GameStateType state) {
      if (state == GameStateType.GameRunning) {
        return "GAME_RUNNING";
      } else if (state == GameStateType.GamePaused) {
        return "GAME_PAUSED";
      } else if (state == GameStateType.MainMenu) {
        return "MAIN_MENU";
      }
    }
  }
}
