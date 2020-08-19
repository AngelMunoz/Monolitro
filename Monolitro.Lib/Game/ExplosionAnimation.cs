using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using static Monolitro.Domain.Explosions;

namespace Monolitro.Lib.Game
{
  public class ExplosionAnimation
  {
    private Point currentFrame;
    private int FrameCounter { get; set; } = 0;
    public int AnimationDelay { get; set; } = 5;
    public Point FrameSize { get; private set; } = Point.Zero;
    public Point SheetSize { get; private set; } = Point.Zero;
    public Point CurrentFrame { get => currentFrame; private set => currentFrame = value; }
    public Vector2 RenderPosition { get; private set; } = Vector2.Zero;
    public ExplosionState ExplosionState { get; set; } = ExplosionState.Exploding;
    public int Id { get; }


    public event EventHandler<int> AnimationFinished;

    public ExplosionAnimation(Point frameSize, Point sheetSize, Vector2 position, int id)
    {
      FrameSize = frameSize;
      SheetSize = sheetSize;
      Id = id;
      RenderPosition = position;
    }

    private void UpdateAnimation()
    {
      if (FrameCounter == AnimationDelay)// delay frame update if it's too fast
      {
        if (CurrentFrame.X < SheetSize.X)
        {
          ++currentFrame.X;// Move to a new frame
        }
        else
        {
          currentFrame.Y++;//Move down a row, since we've
                           //hit the end of the current one
          currentFrame.X = 1;//set the X to 1, so we start fresh
        }
        if (currentFrame.Y >= SheetSize.Y)
        {
          currentFrame.X = 0;
          currentFrame.Y = 0;//Start the animation over again
          ExplosionState = ExplosionState.Finished;
          AnimationFinished?.Invoke(this, Id);
        }

        FrameCounter = -1;//Set this to 0, so we delay it again
      }
      else
      {
        FrameCounter += 1;// add one, so we can continue when we are ready
      }
    }

    public void Draw(GameTime gameTime, SpriteBatch batch, Texture2D texture)
    {
      if (!ExplosionState.IsFinished)
      {
        UpdateAnimation();
        batch.Draw(
            texture,
            RenderPosition,
            new Rectangle(CurrentFrame.X * FrameSize.X, CurrentFrame.Y * FrameSize.Y, FrameSize.X, FrameSize.Y),
            Color.White
        );
      }
    }
  }
}