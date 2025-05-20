using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Poligonos
{
    public List<Vector2> vertices;
    public Vector2 posicionDeOrigen;
    public float angulo;
    public float multiplicadorDeTamanoX;
    public float multiplicadorDeTamanoY;
    //Constructores--------------------------------------------------------------
    public Poligonos
    (
        List<Vector2> vertices = null,
        float angulo = 0,
        float multiplicadorDeTamanoX = 1,
        float multiplicadorDeTamanoY = 1,
        Vector2 posicionDeOrigen = new Vector2(),
        bool colocarSegunCentroide = false,
        Vector2 centroide = new Vector2()

    )
    {
        this.angulo = angulo;
        this.multiplicadorDeTamanoX = multiplicadorDeTamanoX;
        this.multiplicadorDeTamanoY = multiplicadorDeTamanoY;
        this.posicionDeOrigen = posicionDeOrigen;
        this.vertices = new List<Vector2>();

        if (vertices == null)
        {
            List<Vector2> vector2s = new List<Vector2>()
            {
                new Vector2(0, 0),
                new Vector2(64, 0),
                new Vector2(64, 64),
                new Vector2(0, 64)

            };
            ColocarVertices(vector2s, colocarSegunCentroide, centroide);
        }
        else
        {
            ColocarVertices(vertices, colocarSegunCentroide, centroide);
        }

    }
    public Poligonos Clonar(Poligonos poligonoAClonar)
    {
        // Crear una nueva lista con copias de los Vector2
        List<Vector2> copiaDeVertices = new List<Vector2>();

        foreach (Vector2 item in poligonoAClonar.vertices)
        {
            copiaDeVertices.Add(new Vector2(item.X, item.Y));
        }

        Poligonos poligono_a_regresar = new Poligonos(
            vertices: copiaDeVertices,
            angulo: poligonoAClonar.angulo,
            multiplicadorDeTamanoX: poligonoAClonar.multiplicadorDeTamanoX,
            multiplicadorDeTamanoY: poligonoAClonar.multiplicadorDeTamanoY,
            posicionDeOrigen: poligonoAClonar.posicionDeOrigen
        );

        return poligono_a_regresar;
    }
    //Constructores--------------------------------------------------------------

    //Colocar vertices al construir----------------------------------------------
    public void ColocarVertices(List<Vector2> verticesAColocar, bool utilizaCentroide = false, Vector2 centroideAUtilizar = new Vector2())
    {
        Vector2 escala = new Vector2(multiplicadorDeTamanoX, multiplicadorDeTamanoY);
        List<Vector2> verticesTemporales = new List<Vector2>();
        Vector2 vectorTemporal = new Vector2();
        if (utilizaCentroide == false)
        {
            foreach (Vector2 item in verticesAColocar)
            {
                vectorTemporal = (item * escala) + posicionDeOrigen;
                verticesTemporales.Add(vectorTemporal);
            }
            this.vertices = verticesTemporales;
        }
        else
        {
            foreach (Vector2 item in verticesAColocar)
            {
                verticesTemporales.Add(item * escala);
            }

            Vector2 centroideTemporal = CalcularCentroide(verticesTemporales);
            Vector2 desplazamiento = (posicionDeOrigen + centroideAUtilizar) - centroideTemporal;

            for (int i = 0; i < verticesTemporales.Count; i++)
            {
                verticesTemporales[i] = verticesTemporales[i] + desplazamiento;
            }

            this.vertices = verticesTemporales;
        }
    }
    //Colocar vertices al construir----------------------------------------------

    //Actualizar Vertices--------------------------------------------------------
    public void ActualizarVertices(float anguloASeguir, Vector2 velocidad)
    {
        List<Vector2> verticesTemporales = new List<Vector2>();
        foreach (Vector2 item in vertices)
        {
            verticesTemporales.Add(item + velocidad);
        }
        vertices = verticesTemporales;
        Rotar(anguloASeguir, CalcularCentroide());
    }
    //Actualizar Vertices--------------------------------------------------------

    //Rotaciones-----------------------------------------------------------------
    public void Rotar(float anguloRadianes, Vector2 puntoDeRotacion)
    {

        float anguloAGirar = Convert.ToSingle(Math.Atan2(Math.Sin(anguloRadianes - angulo), Math.Cos(anguloRadianes - angulo)));

        float cos = (float)Math.Cos(anguloAGirar);
        float sin = (float)Math.Sin(anguloAGirar);

        angulo = angulo + anguloAGirar;

        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 v = vertices[i];

            float dx = v.X - puntoDeRotacion.X;
            float dy = v.Y - puntoDeRotacion.Y;

            float xNuevo = dx * cos - dy * sin;
            float yNuevo = dx * sin + dy * cos;

            xNuevo += puntoDeRotacion.X;
            yNuevo += puntoDeRotacion.Y;

            vertices[i] = new Vector2(xNuevo, yNuevo);
        }
    }
    public List<Vector2> Rotar(Vector2 puntoDeRotacion, List<Vector2> vectoresARotar)
    {
        float cos = (float)Math.Cos(angulo);
        float sin = (float)Math.Sin(angulo);

        List<Vector2> verticesTemporales = vectoresARotar;

        //angulo = angulo; el angulo se queda igual, este metodo solo se deberia utilizar en el metodo cambiar matrices para devolverles su rotacion
        
        for(int i = 0; i < verticesTemporales.Count; i++ )
        {
            Vector2 v = verticesTemporales[i];

            float dx = v.X - puntoDeRotacion.X;
            float dy = v.Y - puntoDeRotacion.Y;

            float xNuevo = dx * cos - dy * sin;
            float yNuevo = dx * sin + dy * cos;

            xNuevo += puntoDeRotacion.X;
            yNuevo += puntoDeRotacion.Y;

            verticesTemporales[i] = new Vector2(xNuevo, yNuevo); 
        }
        return verticesTemporales;
    }

    public void Rotar(Vector2 puntoAMirar, Vector2 puntoDeRotacion)
    {
        float anguloRadianes;
        float anguloAGirar;
        
        float pmx = puntoAMirar.X - puntoDeRotacion.X;
        float pmy = puntoAMirar.Y - puntoDeRotacion.Y;

        anguloRadianes = MathF.Atan2(pmy, pmx);

        anguloAGirar = anguloRadianes - angulo;
        
        angulo = anguloRadianes;
        

        float cos = (float)Math.Cos(anguloAGirar);
        float sin = (float)Math.Sin(anguloAGirar);

        for(int i = 0; i < vertices.Count; i++ )
        {
            Vector2 v = vertices[i];

            float dx = v.X - puntoDeRotacion.X;
            float dy = v.Y - puntoDeRotacion.Y;

            float xNuevo = dx * cos - dy * sin;
            float yNuevo = dx * sin + dy * cos;

            xNuevo = xNuevo + puntoDeRotacion.X;
            yNuevo = yNuevo + puntoDeRotacion.Y;

            vertices[i] = new Vector2(xNuevo, yNuevo); 
        }
    }
    //Rotaciones-----------------------------------------------------------------

    //Centroides-----------------------------------------------------------------
    public Vector2 CalcularCentroide()
    {
        float sumaX = 0;
        float sumaY = 0;
        foreach (Vector2 item in vertices)
        {
            sumaX = sumaX + item.X;
            sumaY = sumaY + item.Y;
        }
        return new Vector2(sumaX / vertices.Count, sumaY / vertices.Count);
    }
    public Vector2 CalcularCentroide(Poligonos poligono)
    {
        float sumaX = 0;
        float sumaY = 0;
        foreach (Vector2 item in poligono.vertices)
        {
            sumaX = sumaX + item.X;
            sumaY = sumaY + item.Y;
        }
        return new Vector2(sumaX/poligono.vertices.Count, sumaY/poligono.vertices.Count);
    }
    public Vector2 CalcularCentroide(List<Vector2> _vertices)
    {
        float sumaX = 0;
        float sumaY = 0;
        foreach (Vector2 item in _vertices)
        {
            sumaX = sumaX + item.X;
            sumaY = sumaY + item.Y;
        }
        return new Vector2(sumaX/_vertices.Count, sumaY/_vertices.Count);
    }
    public int DistanciaEntreCentroidesInt(Vector2 centroide)
    {
        float distancia = (CalcularCentroide() - centroide).Length();
        return Convert.ToInt32(distancia);
    }
    public int DistanciaEntreCentroidesInt(Poligonos poligono)
    {
        float distancia = (CalcularCentroide() - CalcularCentroide(poligono)).Length();
        return Convert.ToInt32(distancia);
    }
    public Vector2 DistanciaEntreCentroidesVector(Vector2 centroide)
    {
        Vector2 distancia = new Vector2();
        distancia = CalcularCentroide() - centroide;
        return distancia;
    }
    public Vector2 DistanciaEntreCentroidesVector(Poligonos poligono)
    {
        Vector2 distancia = new Vector2();
        distancia = CalcularCentroide() - CalcularCentroide(poligono);
        return distancia;
    }
    //Centroides-----------------------------------------------------------------

    //Colisiones-----------------------------------------------------------------
    //(Separating Axis Theorem)--------------------------------------------------
    public bool SAT(Poligonos otro)
    {
        Vector2[] lados1 = new Vector2[vertices.Count];
        Vector2[] lados2 = new Vector2[otro.vertices.Count];

        for (int i = 0; i < lados1.Length; i++)
        {
            lados1[i] = vertices[(i + 1) % vertices.Count] - vertices[i];
        }
        for (int i = 0; i < lados2.Length; i++)
        {
            lados2[i] = otro.vertices[(i + 1) % otro.vertices.Count] - otro.vertices[i];
        }

        Vector2[] normales1 = new Vector2[lados1.Length];
        Vector2[] normales2 = new Vector2[lados2.Length];

        for (int i = 0; i < normales1.Length; i++)
        {
            normales1[i] = new Vector2(-lados1[i].Y, lados1[i].X);
        }
        for (int i = 0; i < normales2.Length; i++)
        {
            normales2[i] = new Vector2(-lados2[i].Y, lados2[i].X);
        }

        //Combinar todos los ejes a verificar
        List<Vector2> ejes = new List<Vector2>();
        ejes.AddRange(normales1);
        ejes.AddRange(normales2);

        //Verificar superposición en cada eje
        foreach (Vector2 eje in ejes)
        {
            // Proyección del primer polígono
            float minA, maxA;
            ProyectarVertices(vertices, eje, out minA, out maxA);

            // Proyección del segundo polígono
            float minB, maxB;
            ProyectarVertices(otro.vertices, eje, out minB, out maxB);

            // Si no hay superposición en este eje: NO hay colisión
            if (maxA < minB || maxB < minA)
            {
                return false;
            }
        }

        //Superposición en todos los ejes: hay colisión
        return true;
    }
        // Método auxiliar para proyectar vértices en un eje
    private void ProyectarVertices(List<Vector2> vertices, Vector2 eje, out float min, out float max)
    {
        min = float.MaxValue;
        max = float.MinValue;
        foreach (Vector2 vertice in vertices)
        {
            // Producto punto para la proyección
            float proyeccion = vertice.X * eje.X + vertice.Y * eje.Y;
            if (proyeccion < min) 
            {
                min = proyeccion;
            }
            if (proyeccion > max) 
            {
                max = proyeccion;
            }
        }
    }
    //(Separating Axis Theorem)--------------------------------------------------

    //Colisiones dentro de una distancia-----------------------------------------
    public bool EstaColisionandoCon(Poligonos otro, int distancia_minima_de_comprobacion)
    {
        if(DistanciaEntreCentroidesInt(otro) < distancia_minima_de_comprobacion)
        {
            return SAT(otro);
        }
        else
        {
            return false;
        }
    }
    public bool EstaColisionandoCon(Poligonos otro, Vector2 distancia_minima_de_comprobacion)
    {
        if(DistanciaEntreCentroidesVector(otro).Length() < distancia_minima_de_comprobacion.Length())
        {
            return SAT(otro);
        }
        else
        {
            return false;
        }
    }
    //Colisiones dentro de una distancia-----------------------------------------

    //Colisiones-----------------------------------------------------------------

    //Poligono contiene x--------------------------------------------------------
    public bool ContienePunto(Vector2 punto)
    {
        bool dentro = false;
        
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector2 verticeMas1 = vertices[(i +1) % vertices.Count];

            if(Math.Abs(vertices[i].Y - verticeMas1.Y) < 0.01f) // para evitar divisiones por 0
            {
                continue;
            }

            if(punto.Y <= vertices[i].Y && punto.Y >= verticeMas1.Y || punto.Y >= vertices[i].Y && punto.Y <= verticeMas1.Y)
            {
                if (punto.X < vertices[i].X + ((punto.Y - vertices[i].Y)*(vertices[(i + 1) % vertices.Count].X - vertices[i].X) /( vertices[(i + 1) % vertices.Count].Y - vertices[i].Y)))
                {
                    dentro = !dentro;
                }
            }
        }
            

        return dentro;
    }
    //Poligono contiene x--------------------------------------------------------
    
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