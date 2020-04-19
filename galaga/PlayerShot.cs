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


public class PlayerShot : Entity 
{
    //CLASS CONSTRUCTOR
    public PlayerShot(DynamicShape shape, IBaseImage image) : base(shape, image)
    {
       Shape.AsDynamicShape().Direction = new Vec2F(0.0F,0.01F);
    }
}