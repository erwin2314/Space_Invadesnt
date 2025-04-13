using System;
using System.Security.AccessControl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Entidad
{
    public Vector2 posicion;
    public Texture2D imagen;
    private int prioridad_de_dibujado;
    public Vector2 velocidad;
    public Vector2 aceleracion;
    public float angulo;
    public float velocidad_de_rotacion;
    private Rectangle rectangulo_de_imagen;
    private Rectangle rectangulo_de_colision;
    private Vector2 origen_de_imagen;
    private Vector2 origen_de_colision;
    public float fuerza_de_aceleracion;
    public float offset_angulo;
    private bool es_jugador;
    public Entidad
    (
        Vector2 posicion,
        Texture2D imagen = null,
        int prioridad_de_dibujado = 0,
        float angulo = 0,
        float velocidad_de_rotacion = 0,
        float offset_angulo = 0,
        float fuerza_de_aceleracion = 0,
        bool es_jugador = false
    )
    {
        this.posicion = posicion;
        this.imagen = imagen;
        this.prioridad_de_dibujado = prioridad_de_dibujado;
        this.es_jugador = es_jugador;

        this.velocidad = new Vector2(0,0);
        this.aceleracion = new Vector2(0,0);
        this.fuerza_de_aceleracion = fuerza_de_aceleracion;

        this.angulo = angulo;
        this.velocidad_de_rotacion = velocidad_de_rotacion;
        this.offset_angulo = offset_angulo;

        if (this.imagen == null)
        {
            this.rectangulo_de_imagen = new Rectangle();
            this.rectangulo_de_colision = rectangulo_de_imagen;
        }
        else
        {
            CambiarRectangulo_de_imagen();
            this.rectangulo_de_colision = rectangulo_de_imagen;
        }

        if (this.imagen == null)
        {
            this.origen_de_imagen = new Vector2(0,0);
        }
        else
        {
            this.origen_de_imagen = new Vector2(32,32);

        }
        this.origen_de_colision = new Vector2(0,0);
    }


    //------CambiarOrigenDeImagen---------------------
    public void CambiarOrigenDeImagen(Vector2 vector2)
    {
        origen_de_imagen = vector2;
    }
    public void CambiarOrigenDeImagen(float x, float y)
    {
        origen_de_imagen = new Vector2(x, y);
    }
    //------CambiarOrigenDeImagen---------------------


    //------CambiarRectangulo_de_imagen---------------------
    public void CambiarRectangulo_de_imagen(Rectangle rectangle)
    {
        rectangulo_de_imagen = rectangle;
    }
    public void CambiarRectangulo_de_imagen(int x1, int y1, int ancho, int alto)
    {
        rectangulo_de_imagen = new Rectangle(x1,y1,ancho,alto);
    }
    public void CambiarRectangulo_de_imagen()
    {
        rectangulo_de_imagen.Height = imagen.Height;
        rectangulo_de_imagen.Width = imagen.Width;
    }
    //------CambiarRectangulo_de_imagen---------------------


    //------CambiarRectangulo_de_colision---------------------
    public void CambiarRectangulo_de_colision()
    {
        rectangulo_de_colision.Width = imagen.Width;
        rectangulo_de_colision.Height = imagen.Height;
    }
    public void CambiarRectangulo_de_colision(Rectangle rectangle)
    {
        rectangulo_de_colision = rectangle;
    }
    public void CambiarRectangulo_de_colision(int x, int y, int ancho, int alto)
    {
        rectangulo_de_colision = new Rectangle(x,y,ancho,alto);
    }
    //------CambiarRectangulo_de_colision---------------------


    //------CambiarOrigenDeColision---------------------
    public void CambiarOrigenDeColision(Vector2 vector2)
    {
        origen_de_colision = vector2;
    }
    public void CambiarOrigenDeColision(float x, float y)
    {
        origen_de_colision = new Vector2(x, y);
    }
    //------CambiarOrigenDeImagen---------------------


    //------Rotacion y Distancia----------------------
    public Vector2 DistanciaRelativa(Vector2 vector_a_comparar)
    {
        Vector2 distancia = new Vector2(vector_a_comparar.X - posicion.X, vector_a_comparar.Y - posicion.Y);
        return distancia;
    }
    public Vector2 DistanciaRelativa(int x, int y)
    {
        Vector2 vector_a_comparar = new Vector2(x,y);
        Vector2 distancia = new Vector2(vector_a_comparar.X - posicion.X, vector_a_comparar.Y - posicion.Y);
        return distancia;
    }

    public float TeoremaDePitagoras(Vector2 distancia_relativa)
    {
        double distancia = Convert.ToSingle(Math.Pow(Convert.ToDouble(distancia_relativa.X), Convert.ToDouble(2)) + Math.Pow(Convert.ToDouble(distancia_relativa.Y), Convert.ToDouble(2)));
        distancia = Math.Sqrt(distancia);
        float distancia_final = Convert.ToSingle(distancia);
        return distancia_final;
    }

    public void MirarAUnPunto(Vector2 punto)
    {
        Vector2 punto_a_mirar = DistanciaRelativa(punto);
        angulo = MathF.Atan2(punto_a_mirar.Y, punto_a_mirar.X);
        angulo = angulo + offset_angulo;
    }
    //------Rotacion y Distancia----------------------


    //------Movimiento--------------------------------
    public void MovimientoTeclas(KeyboardState teclado)
    {
        teclado = Keyboard.GetState();
        

        if (teclado.IsKeyDown(Keys.W))
        {
            aceleracion.Y = aceleracion.Y - fuerza_de_aceleracion;
            velocidad = velocidad + aceleracion;
            aceleracion = new Vector2(0,0);
        }
        if (teclado.IsKeyDown(Keys.S))
        {
            aceleracion.Y = aceleracion.Y + fuerza_de_aceleracion;
            velocidad = velocidad + aceleracion;
            aceleracion = new Vector2(0,0);
        }
        if (teclado.IsKeyDown(Keys.D))
        {
            aceleracion.X = aceleracion.X + fuerza_de_aceleracion;
            velocidad = velocidad + aceleracion;
            aceleracion = new Vector2(0,0);
        }
        if (teclado.IsKeyDown(Keys.A))
        {
            aceleracion.X = aceleracion.X - fuerza_de_aceleracion;
            velocidad = velocidad + aceleracion;
            aceleracion = new Vector2(0,0);
        }
    }
    //------Movimiento--------------------------------
    public void Update(KeyboardState teclado)
    {
        if (es_jugador)
        {
            MovimientoTeclas(teclado);
            angulo = angulo + velocidad_de_rotacion;
            
        }
        else
        {
            velocidad = velocidad + aceleracion;
            angulo = (angulo + offset_angulo) + velocidad_de_rotacion;
        }
        posicion = posicion + velocidad;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(imagen, posicion, rectangulo_de_imagen, Color.White, angulo, origen_de_imagen, 1.0f, SpriteEffects.None, prioridad_de_dibujado);
        spriteBatch.End();
    }

    
}