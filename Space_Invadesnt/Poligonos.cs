using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Poligonos
{
    public List<Vector2> vertices;
    public float angulo;

    public Poligonos
    (
        List<Vector2> vertices = null,
        float angulo = 0
    )
    {

        this.angulo = angulo;

        if (vertices == null)
        {
            List<Vector2> vector2s = new List<Vector2>()
            {
                new Vector2(0,0),
                new Vector2(64,0),
                new Vector2(64,64),
                new Vector2(0,64)
                
            };
            CambiarVertices(vector2s);
        }
        else
        {
            CambiarVertices(vertices);
        }
    }

    public void CambiarVertices(List<Vector2> vector2s)
    {
        this.vertices =  vector2s;
    }

    public void Rotar(float anguloRadianes, Vector2 puntoDeOrigen)
    {
        float cos = (float)Math.Cos(anguloRadianes);
        float sin = (float)Math.Sin(anguloRadianes);

        angulo = angulo + anguloRadianes;
        
        for(int i = 0; i < vertices.Count; i++ )
        {
            Vector2 v = vertices[i];

            float dx = v.X - puntoDeOrigen.X;
            float dy = v.Y - puntoDeOrigen.Y;

            float xNuevo = dx * cos - dy * sin;
            float yNuevo = dx * sin + dy * cos;

            xNuevo += puntoDeOrigen.X;
            yNuevo += puntoDeOrigen.Y;

            vertices[i] = new Vector2(xNuevo, yNuevo); 
        }
    }

    public void Rotar(Vector2 puntoAMirar, Vector2 puntoDeOrigen)
    {
        float anguloRadianes;
        float anguloAGirar;
        
        float pmx = puntoAMirar.X - puntoDeOrigen.X;
        float pmy = puntoAMirar.Y - puntoDeOrigen.Y;

        anguloRadianes = MathF.Atan2(pmy, pmx);

        anguloAGirar = anguloRadianes - angulo;
        
        angulo = anguloRadianes;
        

        float cos = (float)Math.Cos(anguloAGirar);
        float sin = (float)Math.Sin(anguloAGirar);

        for(int i = 0; i < vertices.Count; i++ )
        {
            Vector2 v = vertices[i];

            float dx = v.X - puntoDeOrigen.X;
            float dy = v.Y - puntoDeOrigen.Y;

            float xNuevo = dx * cos - dy * sin;
            float yNuevo = dx * sin + dy * cos;

            xNuevo = xNuevo + puntoDeOrigen.X;
            yNuevo = yNuevo + puntoDeOrigen.Y;

            vertices[i] = new Vector2(xNuevo, yNuevo); 
        }
    }

    public Vector2 CalcularCentroide()
    {
        float sumaX = 0;
        float sumaY = 0;
        foreach (Vector2 item in vertices)
        {
            sumaX = sumaX + item.X;
            sumaY = sumaY + item.Y;
        }
        return new Vector2(sumaX/vertices.Count,sumaY/vertices.Count);
    }

    public void Draw(SpriteBatch spriteBatch,Texture2D pixel)
    {
        spriteBatch.Begin();

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 start = vertices[i];
            Vector2 end = vertices[(i + 1) % vertices.Count];
            Vector2 edge = end - start;

            float angle = (float)Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            spriteBatch.Draw(pixel, 
            new Rectangle((int)start.X, (int)start.Y, (int)length, 1), 
            null, 
            Color.White, 
            angle, 
            Vector2.Zero, 
            SpriteEffects.None, 
            0);
        }

        
        spriteBatch.End();
    }
}