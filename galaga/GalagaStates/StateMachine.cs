using DIKUArcade.EventBus;
using DIKUArcade.State;
using System;

namespace galaga.GalagaStates {
  public class StateMachine : IGameEventProcessor<object> {
    public IGameState ActiveState { get; private set; }

    public StateMachine() {
      GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
      GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
      ActiveState = MainMenu.GetInstance();
    }

    private void SwitchState(GalagaStates.GameStateType stateType) {
      switch (stateType) {
        case GameStateType.GameRunning:
          ActiveState = GameRunning.GetInstance();
          break;
        case GameStateType.GamePaused:
          ActiveState = GamePaused.GetInstance();
          break;
        case GameStateType.MainMenu:
          ActiveState = MainMenu.GetInstance();
          break;
      }
    }

    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
      System.Console.WriteLine(eventType);
      System.Console.WriteLine(gameEvent);
    }

    public void GameLoop() {

    }
  }
}
