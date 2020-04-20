namespace galagaTests {
  [TestFixture]
  public class StateMachineTesting {
    private StateMachine stateMachine;

    [SetUp]
    public void InitiateStateMachine() {
      DIKUArcade.Window.CreateOpenGLContext();
      // Here you should:
      // (1) Initialize a GalagaBus with proper GameEventTypes
      // (2) Instantiate the StateMachine
      // (3) Subscribe the GalagaBus to proper GameEventTypes
      // and GameEventProcessors

      GameEventBus<object> GalagaBus = GalagaBus.eventBus();
      StateMachine stateMachine = new StateMachine();

    }

    [Test]
    public void TestInitialState() {
      Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
    }

    [Test]
    public void TestEventGamePaused() {
      GalagaBus.GetBus().RegisterEvent(
        GameEventFactory<object>.CreateGameEventForAllProcessors(
          GameEventType.GameStateEvent,
          this,
          "CHANGE_STATE",
          "GAME_PAUSED", ""));

      GalagaBus.GetBus().ProcessEventsSequentially();
      Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
    }
  }
}
