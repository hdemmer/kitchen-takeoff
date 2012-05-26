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
    struct House
    {
        public Vector3 origin;
        public Vector3 rotation;
        public Vector3 scale;
    };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class World : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Model houseModel;
        House[] houses;
        Effect effect;
        Game game;

        
        double time;


        public World(Game theGame)
            : base(theGame)
        {
            game = theGame;
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        /// 
        public override void Initialize()
        {
            int num = 10000;

            Random random = new Random(num + 1);

            houses = new House[num];
            for (int i = 0; i < num; i++)
            {
                House house = new House();

                house.origin = new Vector3(random.Next(-50,50), random.Next(-20,0), random.Next(-50,50));
                house.rotation = new Vector3((float)random.NextDouble() * MathHelper.Pi, (float)random.NextDouble() / 100.0f, (float)random.NextDouble() / 100.0f);
                house.scale = new Vector3((float)random.NextDouble() + 1, 2*(float)random.NextDouble() + 1, (float)random.NextDouble() + 1);

                houses.SetValue(house, i);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = game.Content.Load<Effect>("Hades");

            houseModel = game.Content.Load<Model>("box");
            foreach (ModelMesh mesh in houseModel.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }


            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            time += gameTime.ElapsedGameTime.TotalMilliseconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector3 cameraPosition = new Vector3(0, 10+MathHelper.SmoothStep(0,1, (float)(time / 10000)), 50 - 1 * (float)(time / 1000));
            Vector3 lookAt = new Vector3(0, -0.2f, -1);
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;

            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraPosition+lookAt, Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f), aspectRatio,
                1.0f, 10000.0f);

            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            foreach (House house in houses)
            {

                Model myModel = houseModel;

                Matrix world = Matrix.Identity;
                world *= Matrix.CreateScale(house.scale);
                world *= Matrix.CreateFromYawPitchRoll(house.rotation.X, house.rotation.Y, house.rotation.Z);
                world *= Matrix.CreateTranslation(house.origin.X, house.origin.Y, house.origin.Z);

                foreach (ModelMesh mesh in myModel.Meshes)
                {
                    effect.Parameters["World"].SetValue(world);

                    // Draw the mesh, using the effects set above.
                    mesh.Draw();
                }
            }

            base.Draw(gameTime);
        }
    }
}
