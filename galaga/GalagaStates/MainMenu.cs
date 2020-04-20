using DIKUArcade.State;
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
  }
}
