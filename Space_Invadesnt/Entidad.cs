using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Entidad
{
    public int vida_maxima;
    public int vida_actual;
    public bool activa;
    public Vector2 posicion;
    public Texture2D imagen;
    private int prioridad_de_dibujado;
    public Vector2 velocidad;
    public Vector2 aceleracion;
    public float angulo;
    public float velocidad_de_rotacion;
    private Rectangle rectangulo_de_imagen;
    public Vector2 origen_de_imagen;//centro de la imagen
    public Vector2 origen_de_poligono_colision;//posicion en la que sa va a colocar el primer vertice del poligono
    public float fuerza_de_aceleracion;
    public float offset_angulo;
    public float tiempo_de_existencia;
    public int ID;
    public Poligonos poligono_de_colision;

    //---------Constructores----------------------------------------
    public Entidad
    (
        Vector2 posicion = new Vector2(),
        Texture2D imagen = null,
        int prioridad_de_dibujado = 0,
        float angulo = 0,
        float velocidad_de_rotacion = 0,
        float offset_angulo = 0,
        float fuerza_de_aceleracion = 0,
        bool activa = true,
        Vector2 velocidad = new Vector2(),
        Vector2 aceleracion = new Vector2(),
        float tiempo_de_existencia = 0f,
        int vida_maxima = 5,
        int vida_actual = 5,
        int ID = 0,
        float multiplicador_de_colision_x = 1,
        float multiplicador_de_colision_y = 1,
        Vector2 origen_de_poligono_colision = new Vector2()
    )
    {
        this.posicion = posicion;
        this.imagen = imagen;
        this.prioridad_de_dibujado = prioridad_de_dibujado;
        this.activa = activa;
        this.tiempo_de_existencia = tiempo_de_existencia;
        this.vida_maxima = vida_maxima;
        this.vida_actual = vida_actual;
        this.ID = ID;

        this.velocidad = velocidad;
        this.aceleracion = aceleracion;
        this.fuerza_de_aceleracion = fuerza_de_aceleracion;

        
        this.velocidad_de_rotacion = velocidad_de_rotacion;
        this.offset_angulo = offset_angulo;
        this.angulo = angulo + offset_angulo;

        if(origen_de_poligono_colision == Vector2.Zero)
        {
            this.origen_de_poligono_colision = posicion;
        }
        else
        {
            this.origen_de_poligono_colision = origen_de_poligono_colision;
        }
        this.poligono_de_colision = new Poligonos();
        poligono_de_colision.multiplicadorDeTamanoX = multiplicador_de_colision_x;
        poligono_de_colision.multiplicadorDeTamanoY = multiplicador_de_colision_y;
        this.poligono_de_colision.ColocarVertices(poligono_de_colision.vertices, true, posicion);
        

        if (this.imagen == null)
        {
            this.rectangulo_de_imagen = new Rectangle();
        }
        else
        {
            CambiarRectangulo_de_imagen();
        }

        if (this.imagen == null)
        {
            this.origen_de_imagen = new Vector2(0,0);
        }
        else
        {
            this.origen_de_imagen = new Vector2(imagen.Width/2,imagen.Height/2);
        }
        
    }

    public Entidad Clonar()
    {
        Entidad entidad_a_regresar = new Entidad(
            vida_maxima: this.vida_maxima,
            vida_actual: this.vida_actual,
            activa: this.activa,
            posicion: this.posicion,
            imagen: this.imagen,
            prioridad_de_dibujado: this.prioridad_de_dibujado,
            velocidad: this.velocidad,
            aceleracion: this.aceleracion,
            angulo: this.angulo - this.offset_angulo, // para que el constructor le sume el offset después
            velocidad_de_rotacion: this.velocidad_de_rotacion,
            origen_de_poligono_colision: this.origen_de_poligono_colision,
            fuerza_de_aceleracion: this.fuerza_de_aceleracion,
            offset_angulo: this.offset_angulo,
            tiempo_de_existencia: this.tiempo_de_existencia,
            ID: this.ID,
            multiplicador_de_colision_x: this.poligono_de_colision.multiplicadorDeTamanoX,
            multiplicador_de_colision_y: this.poligono_de_colision.multiplicadorDeTamanoY
        );

        entidad_a_regresar.poligono_de_colision = this.poligono_de_colision.Clonar(this.poligono_de_colision);

        // Copiar manualmente los valores si ya estaban modificados
        entidad_a_regresar.origen_de_imagen = this.origen_de_imagen;
        entidad_a_regresar.rectangulo_de_imagen = this.rectangulo_de_imagen;

        return entidad_a_regresar;
    }

    //---------Constructores----------------------------------------


    //------CambiarOrigenDeImagen---------------------
    //es lo que se considera el centro de la imagen
    //la posicion es con respecto a la posicion de la entidad
    
    //Cambia el origen de la imagen con un punto en forma de vector
    public void CambiarOrigenDeImagen(Vector2 punto)
    {
        origen_de_imagen = punto;
    }

    //Cambia el origen de la imagen con los valores de x,y directamente
    public void CambiarOrigenDeImagen(float x, float y)
    {
        origen_de_imagen = new Vector2(x, y);
    }
    //------CambiarOrigenDeImagen---------------------


    //------CambiarRectangulo_de_imagen---------------------
    //El rectangulo de la imagen es el espacio o el "canvas" en el que se pinta la textura2d

    //cambia el rectangulo de la imagen directamente por otro rectangulo
    public void CambiarRectangulo_de_imagen(Rectangle rectangle)
    {
        rectangulo_de_imagen = rectangle;
    }

    //cambia el rectangulo de la imagen creando otro rectangulo pidiendo (posicion x, posicion y, ancho, alto)
    public void CambiarRectangulo_de_imagen(int x1, int y1, int ancho, int alto)
    {
        rectangulo_de_imagen = new Rectangle(x1,y1,ancho,alto);
    }

    //cambia el alto y ancho del rectangulo de la imagen por el alto y ancho de la textura2d de la entidad
    public void CambiarRectangulo_de_imagen()
    {
        rectangulo_de_imagen.Height = imagen.Height;
        rectangulo_de_imagen.Width = imagen.Width;
    }
    //------CambiarRectangulo_de_imagen---------------------

    //------Cambiar origen_de_poligono_colision---------------------
    public void CambiarOrigenDeColision(Vector2 vector2)
    {
        origen_de_poligono_colision = vector2;
    }
    public void CambiarOrigenDeColision(float x, float y)
    {
        origen_de_poligono_colision = new Vector2(x, y);
    }
    //------Cambiar origen_de_poligono_colision---------------------


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
    public Vector2 DireccionRelativa(Vector2 punto)
    {
        Vector2 direccion = DistanciaRelativa(punto);
        direccion.Normalize();
        return direccion;
    }
    public float AnguloDeVector(Vector2 vector2)
    {
        float angulo;
        angulo = MathF.Atan2(vector2.Y,vector2.X);
        return angulo;
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

    public Vector2 MoverseAUnPunto(Vector2 punto)
    {
        Vector2 direccion = DistanciaRelativa(punto);
        float distancia = TeoremaDePitagoras(direccion);
        float tolerancia = 1.0f;

        if (distancia > tolerancia)
        {
            direccion.Normalize();
            this.velocidad = this.velocidad.Length() * direccion;
            
            return velocidad;
        }
        return new Vector2(0,0);
    }


    //------Movimiento--------------------------------

    //------Colisiones y poligonos--------------------------------
    public void RecolocarPoligonoDeColison()
    {
        this.poligono_de_colision.ColocarVertices(poligono_de_colision.vertices, true, posicion);
    }
    public void RecolocarPoligonoDeColison(List<Vector2> verticesARecolocar)
    {
        poligono_de_colision.ColocarVertices(verticesARecolocar, true, posicion);
    }
    public void RecolocarPoligonoDeColison(List<Vector2> verticesARecolocar, Vector2 centroideARecolocar)
    {
        poligono_de_colision.ColocarVertices(verticesARecolocar, true, centroideARecolocar);
    }
    public void RecolocarPoligonoDeColison(Vector2 centroideARecolocar)
    {
        poligono_de_colision.ColocarVertices(poligono_de_colision.vertices, true, centroideARecolocar);
    }
    public void ActualizarPoligonoDeColision(Vector2 vector2)
    {
        poligono_de_colision.ActualizarVertices(angulo, velocidad);
    }

    public bool EstaColisionando(List<Entidad> lista_entidades, out List<Entidad> lista_entidades_colisionando, int distancia_minima = 100)
    {
        lista_entidades_colisionando = new List<Entidad>();
        foreach (Entidad item in lista_entidades)
        {
            if(poligono_de_colision.EstaColisionandoCon(item.poligono_de_colision, distancia_minima))
            {
                lista_entidades_colisionando.Add(item);
            }
            
        }
        if(lista_entidades_colisionando.Count > 0)
        {
            return true;
        }

        return false;
    }
    public bool EstaColisionando(List<Entidad> lista_entidades, out Entidad entidad_colisionando, int distancia_minima = 100) //obtiene la primera entidad con la que colisiona
    {

        foreach (Entidad item in lista_entidades)
        {
            if(poligono_de_colision.EstaColisionandoCon(item.poligono_de_colision, distancia_minima))
            {
                entidad_colisionando = item;
                return true;
            }
            
        }

        entidad_colisionando = null;
        return false;
    }
    public bool EstaColisionando(List<Entidad> lista_entidades, int distancia_minima = 100)
    {
        foreach (Entidad item in lista_entidades)
        {
            if(poligono_de_colision.EstaColisionandoCon(item.poligono_de_colision, distancia_minima))
            {
                return true;
            }
        }
        return false;
    }
    public bool EstaColisionando(Entidad otra_entidad, int distancia_minima = 100)
    {
        if(poligono_de_colision.EstaColisionandoCon(otra_entidad.poligono_de_colision, distancia_minima))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EstaColisionando(Entidad otra_entidad, int ID_a_colisionar , int distancia_minima = 100)
    {
        if(poligono_de_colision.EstaColisionandoCon(otra_entidad.poligono_de_colision, distancia_minima) && otra_entidad.ID == ID_a_colisionar)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EstaColisionandoIDExcluyente(Entidad otra_entidad, int ID_a_no_colisionar, int distancia_minima = 100)
    {
        if(poligono_de_colision.EstaColisionandoCon(otra_entidad.poligono_de_colision, distancia_minima) && otra_entidad.ID != ID_a_no_colisionar)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //------Colisiones y poligonos--------------------------------

    //------Updates-----------------------------------
    public void Update(KeyboardState teclado, GameTime gameTime, bool importaSiEstaVivo = true)
    {
        if(importaSiEstaVivo == true && vida_actual <= 0)
        {
            
        }
        else
        {
            MovimientoTeclas(teclado);
            angulo = angulo + velocidad_de_rotacion;
            

            posicion = posicion + velocidad;
            ActualizarPoligonoDeColision(posicion);
            tiempo_de_existencia = tiempo_de_existencia + Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
    public void Update(GameTime gameTime, bool importaSiEstaVivo = true)
    {
        
        velocidad = velocidad + aceleracion;
        angulo = angulo + velocidad_de_rotacion;
        posicion = posicion + velocidad;
        ActualizarPoligonoDeColision(posicion);
        tiempo_de_existencia = tiempo_de_existencia + Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

    }
    public void Update(Vector2 punto, GameTime gameTime, bool importaSiEstaVivo = true)
    {

        
        var velocidad_de_destino = MoverseAUnPunto(punto);
        if (velocidad_de_destino != Vector2.Zero)
        {
            angulo = angulo + velocidad_de_rotacion;
            posicion = posicion + velocidad_de_destino;
        }
        ActualizarPoligonoDeColision(posicion);
        tiempo_de_existencia = tiempo_de_existencia + Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

    }
    //------Updates-----------------------------------


    //------Daño--------------------------------------
    public void RecibirDaño(int cantidad)
    {
        vida_actual = vida_actual - cantidad;
    }
    public void RecibirDaño(int cantidad, Entidad entidad)
    {
        entidad.vida_actual = entidad.vida_actual - cantidad;
    }
    public void RecibirDaño(int cantidad, List<Entidad> lista_entidades)
    {
        foreach (Entidad item in lista_entidades)
        {
            item.vida_actual = item.vida_actual - cantidad;
        }
    }
    //------Daño--------------------------------------

    //------Crear Entidades---------------------------
    public Entidad DispararEntidad(Entidad entidad_a_disparar, Vector2 punto_a_dispararlo, Entidad entidad_de_la_que_sale)
    {
        Entidad entidad = entidad_a_disparar.Clonar();
        entidad.posicion = entidad_de_la_que_sale.posicion;
        entidad.RecolocarPoligonoDeColison();
        entidad.velocidad = entidad.MoverseAUnPunto(punto_a_dispararlo);
        entidad.velocidad = entidad_de_la_que_sale.velocidad + entidad.velocidad;
        entidad.angulo = entidad_de_la_que_sale.angulo  - entidad_de_la_que_sale.offset_angulo;
        
        return entidad;
    }
    //------Crear Entidades---------------------------
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(imagen, posicion, rectangulo_de_imagen, Color.White, angulo, origen_de_imagen, 1.0f, SpriteEffects.None, prioridad_de_dibujado);
        spriteBatch.End();
    }
    public void DrawColision(SpriteBatch spriteBatch, Texture2D pixel)
    {
        poligono_de_colision.Draw(spriteBatch, pixel);
    }
    
}