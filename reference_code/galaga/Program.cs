using System;
using System.IO;
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

namespace galaga
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            
            game.GameLoop();
            
        }
    }
}
