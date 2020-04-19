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

namespace galaga.Squadron {
  public interface ISquadron {
  EntityContainer<Enemy> Enemies { get; }
  
  int MaxEnemies { get; }
    void CreateEnemies(List<Image> enemyStrides);
  }
}

public class Enemy : Entity
{
    //CLASS CONSTRUCTORS
    public Enemy(DynamicShape shape, IBaseImage image) : base(shape, image)
    {

    }
}
