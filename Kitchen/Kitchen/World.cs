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
        public int meshIndex;
    };

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class World : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Model houseModel;
        House[] houses;
        Effect effect;
      
        BigSky sky;
        
        double time;

        float worldSize = 10000.0f;


        public World(Game game)
            : base(game)
        {
          
            sky = new BigSky(game);
            game.Components.Add(sky);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        /// 
        public override void Initialize()
        {
            int num = 2000;

            Random random = new Random(num + 1);

            houses = new House[num];
            for (int i = 0; i < num; i++)
            {
                House house = new House();

                house.origin = new Vector3(random.Next(-(int)worldSize / 2, (int)worldSize / 2), random.Next(-50, 0), random.Next(-(int)worldSize / 2, (int)worldSize / 2));
                house.rotation = new Vector3((float)random.NextDouble() , (float)random.NextDouble() / 100.0f, (float)random.NextDouble() / 100.0f);
                house.scale = (new Vector3((float)random.NextDouble()*3 + 1, 2*(float)random.NextDouble() + 1, (float)random.NextDouble()*6 + 1));

                house.meshIndex = random.Next(0, 15);

                houses.SetValue(house, i);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = Game.Content.Load<Effect>("Hades");

            houseModel = Game.Content.Load<Model>("greebles");
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
            Vector3 cameraPosition = new Vector3(0, 80 + MathHelper.SmoothStep(0, 200, (float)(time / worldSize)+0.11f), worldSize / 2 - 0.9f* (float)(time));
            Vector3 lookAt = new Vector3(0, -0.2f, -1);
            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;

            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraPosition+lookAt, Vector3.Up);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f), aspectRatio,
                1.0f, 10000.0f);

            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            // draw base

            effect.Parameters["World"].SetValue(Matrix.Identity);

            VertexPositionColor[] vertices = new VertexPositionColor[3];

            vertices[0].Position = new Vector3(cameraPosition.X + worldSize, 0, cameraPosition.Z - worldSize);
            vertices[1].Position = new Vector3(cameraPosition.X, 0, cameraPosition.Z);
            vertices[2].Position = new Vector3(cameraPosition.X - worldSize, 0, cameraPosition.Z - worldSize);

            GraphicsDevice device = this.GraphicsDevice;

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 1, VertexPositionColor.VertexDeclaration);
            }

            // draw houses
            foreach (House house in houses)
            {
                float lag = MathHelper.Max(0, house.origin.Z - cameraPosition.Z - 1000);

                int lagSteps = (int)Math.Ceiling(lag / worldSize);

                Model myModel = houseModel;

                Matrix world = Matrix.Identity;
                world *= Matrix.CreateScale(house.scale);
                world *= Matrix.CreateFromYawPitchRoll(house.rotation.X, house.rotation.Y, house.rotation.Z);
                world *= Matrix.CreateTranslation(house.origin.X, house.origin.Y, house.origin.Z - lagSteps * worldSize);

                effect.Parameters["World"].SetValue(world);

                myModel.Meshes[house.meshIndex].Draw();
            }

            base.Draw(gameTime);
        }
    }
}
