using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using System;
using System.Collections.Generic;
using System.Linq;
using Monolitro.Lib.Game;

namespace Monolitro.Lib
{
  public class Game1 : Microsoft.Xna.Framework.Game
  {
    private GraphicsDeviceManager Graphics;
    private SpriteBatch SpriteBatch;
    private Texture2D SpriteSheet;
    private Dictionary<int, ExplosionAnimation> explosions = new Dictionary<int, ExplosionAnimation>();

    public Game1()
    {
      Graphics = new GraphicsDeviceManager(this);
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
    }

    protected override void Initialize()
    {
      // TODO: Add your initialization logic here

      base.Initialize();
    }

    protected override void LoadContent()
    {
      SpriteBatch = new SpriteBatch(GraphicsDevice);
      SpriteSheet = Content.Load<Texture2D>("explosion-4");
      // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
#if !IOS
      if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        Exit();
#endif 

      // TODO: Add your update logic here
      var tstate = TouchPanel.GetState(Window).GetState();
      var mstate = Mouse.GetState(Window);
      foreach (var touchpoint in tstate)
      {
        if (!explosions.ContainsKey(touchpoint.Id))
        {
          var offsetPos = Vector2.Subtract(touchpoint.Position, new Point(64, 128).ToVector2());
          var explosion = new ExplosionAnimation(new Point(128), new Point(12, 1), offsetPos, touchpoint.Id);
          explosion.AnimationFinished += Explosion_AnimationFinished;
          explosions.Add(touchpoint.Id, explosion);
        }
      }
      if (ButtonState.Pressed == mstate.LeftButton)
      {
        var id = explosions.Count + 1;
        if (!explosions.ContainsKey(id))
        {
          var offsetPos = Vector2.Subtract(mstate.Position.ToVector2(), new Point(64, 128).ToVector2());
          var explosion = new ExplosionAnimation(new Point(128), new Point(12, 1), offsetPos, id);
          explosion.AnimationFinished += Explosion_AnimationFinished;
          explosions.Add(id, explosion);
        }
      }
      base.Update(gameTime);
    }

    private void Explosion_AnimationFinished(object sender, int e)
    {
      var explosion = sender as ExplosionAnimation;
      explosions.Remove(e);
      explosion.AnimationFinished -= Explosion_AnimationFinished;
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.CornflowerBlue);
      // TODO: Add your drawing code here
      SpriteBatch.Begin();
      foreach (var value in explosions.Values.ToList())
      {
        value.Draw(gameTime, SpriteBatch, SpriteSheet);
      }
      SpriteBatch.End();
      base.Draw(gameTime);
    }

  }
}
