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

public class Player : IGameEventProcessor<object>
{
    //CLASS FIELDS
    public Entity Entity {get; private set;}
    private Game game;

    private Player player;

    //CLASS CONTRUCTOR
    public Player(DynamicShape shape, IBaseImage image, Game game)
    {
        Entity = new Entity(shape, image);
        this.game = game;
    
    }

    //CLASS METHODS
    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent)
    {
        if (eventType == GameEventType.PlayerEvent) 
        {
            switch (gameEvent.Message) 
            {
                case "MOVE_LEFT":
                    Direction(new Vec2F(-0.01f, 0.00f ));
                    break;
                case "MOVE_RIGHT":
                    Direction(new Vec2F(0.01f, 0.00f));
                    break;
                case "SHOOT":
                    AddShot();
                    break;    
                default:
                    break;
            }
        }

    }
    //Decides the direction of the vec
    public void Direction(Vec2F Dir)
    {
        Entity.Shape.AsDynamicShape().Direction = Dir;
    }
    //Locks the player within the game window
    public void Move()
    {
        if(Entity.Shape.AsDynamicShape().Position.X < 0.0F)
        {
            Entity.Shape.AsDynamicShape().Position.X = 0.0F;
        }
        else if(Entity.Shape.AsDynamicShape().Position.X > 0.9F)
        {
            Entity.Shape.AsDynamicShape().Position.X = 0.9F;
        }
        else
        {
            Entity.Shape.AsDynamicShape().Move();
        }
    }

    //Adds shots to a list
    public void AddShot()
    {
        game.playerShots.Add(new PlayerShot(
                new DynamicShape(new Vec2F(Entity.Shape.AsDynamicShape().Position.X+0.047F, 0.19F), 
                new Vec2F(0.008F, 0.027F)), 
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png"))));
    }



}