using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Kitchen
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BigSky : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Effect effect;

        public BigSky(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GraphicsDevice device = this.GraphicsDevice;

            effect = Game.Content.Load<Effect>("bigsky");


            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            VertexPositionColor[] vertices = new VertexPositionColor[6];

            vertices[0].Position = new Vector3(1f, 1f, 0f);
            vertices[1].Position = new Vector3(1, 0, 0f);
            vertices[2].Position = new Vector3(-1, 1, 0f);

            vertices[3].Position = new Vector3(-1, 1, 0f);
            vertices[4].Position = new Vector3(1, 0, 0f);
            vertices[5].Position = new Vector3(-1, 0, 0f);

            GraphicsDevice device = this.GraphicsDevice;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2, VertexPositionColor.VertexDeclaration);
            }

            base.Draw(gameTime);
        }
    }
}
