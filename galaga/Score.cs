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

public class Score
{
    // CLASS FIELDS
    private int score;
    private Text display;

    //CLASS CONSTRUCTOR
    public Score(Vec2F position, Vec2F extent)
    {
        score = 0;
        display = new Text(score.ToString(), position, extent);
    }
    //Adds points to the current score
    public void AddPoint(int point) 
    {
        {
            score = score + point;
        }
    }
    //Renders the score into the game window
    public void RenderScore()
    {
        display.SetText(string.Format("Score: {0}", score.ToString()));
        display.SetColor(new Vec3I(255, 0, 0));
        display.RenderText();
    }
}