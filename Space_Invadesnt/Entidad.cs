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
    public bool esta_vivo;
    public bool activa;
    public Vector2 posicion;
    public Texture2D imagen;
    private int prioridad_de_dibujado;
    public Vector2 velocidad;
    public Vector2 aceleracion;
    public float angulo;
    public float velocidad_de_rotacion;
    private Rectangle rectangulo_de_imagen;
    public Rectangle rectangulo_de_colision;
    public Vector2 origen_de_imagen;
    public Vector2 origen_de_colision;
    public float fuerza_de_aceleracion;
    public float offset_angulo;
    private bool es_jugador;
    public float tiempo_de_existencia;
    public int ID;
    public Texture2D imagen_de_colision;
    public float multiplicador;

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
        bool es_jugador = false,
        bool activa = true,
        Vector2 velocidad = new Vector2(),
        Vector2 aceleracion = new Vector2(),
        float tiempo_de_existencia = 0f,
        int vida_maxima = 5,
        int vida_actual = 5,
        bool esta_vivo = true,
        int ID = 0,
        Texture2D imagen_de_colision = null,
        float multiplicador = 1
    )
    {
        this.posicion = posicion;
        this.imagen = imagen;
        this.prioridad_de_dibujado = prioridad_de_dibujado;
        this.es_jugador = es_jugador;
        this.activa = activa;
        this.tiempo_de_existencia = tiempo_de_existencia;
        this.vida_maxima = vida_maxima;
        this.vida_actual = vida_actual;
        this.esta_vivo = esta_vivo;
        this.ID = ID;
        this.imagen_de_colision = imagen_de_colision;
        this.multiplicador = multiplicador;

        this.velocidad = velocidad;
        this.aceleracion = aceleracion;
        this.fuerza_de_aceleracion = fuerza_de_aceleracion;

        
        this.velocidad_de_rotacion = velocidad_de_rotacion;
        this.offset_angulo = offset_angulo;
        this.angulo = angulo + offset_angulo;

        if (this.imagen == null)
        {
            this.rectangulo_de_imagen = new Rectangle();
            this.rectangulo_de_colision = new Rectangle();
        }
        else
        {
            CambiarRectangulo_de_imagen();
            CambiarRectangulo_de_colision(multiplicador);
        }

        if (this.imagen == null)
        {
            this.origen_de_imagen = new Vector2(0,0);
            this.origen_de_colision = new Vector2(0,0);
        }
        else
        {
            this.origen_de_imagen = new Vector2(imagen.Width/2,imagen.Height/2);
            this.origen_de_colision = new Vector2(imagen.Width/2,imagen.Height/2);

        }
        
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


    //------CambiarRectangulo_de_colision---------------------
    public void CambiarRectangulo_de_colision(float multiplicador)
    {
        rectangulo_de_colision.Width = Convert.ToInt32(imagen.Width * multiplicador);
        rectangulo_de_colision.Height = Convert.ToInt32(imagen.Height * multiplicador);
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
    public float MirarAUnPunto(Vector2 punto, bool soobrecarga_extra)
    {
        Vector2 punto_a_mirar = DistanciaRelativa(punto);
        angulo = AnguloDeVector(punto_a_mirar);
        angulo = angulo + offset_angulo;
        return angulo;
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

    //------Colisiones--------------------------------
    public void ActualizarRectaguloDeColision()
    {
        rectangulo_de_colision.X = Convert.ToInt32(posicion.X - origen_de_colision.X);
        rectangulo_de_colision.Y = Convert.ToInt32(posicion.Y - origen_de_colision.Y);
        rectangulo_de_colision.Width = Convert.ToInt32(imagen.Width * multiplicador);
        rectangulo_de_colision.Height = Convert.ToInt32(imagen.Height * multiplicador);
    }

    public bool EstaColisionando(List<Entidad> lista_entidades, out List<Entidad> lista_entidades_colisionando)
    {
        lista_entidades_colisionando = new List<Entidad>();
        foreach (Entidad item in lista_entidades)
        {
            if(rectangulo_de_colision.Intersects(item.rectangulo_de_colision))
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
    public bool EstaColisionando(List<Entidad> lista_entidades, out Entidad entidad)
    {
        foreach (Entidad item in lista_entidades)
        {
            if(rectangulo_de_colision.Intersects(item.rectangulo_de_colision))
            {
                entidad = item;
                return true;
            }
        }
        entidad = null;
        return false;
    }
    public bool EstaColisionando(List<Entidad> lista_entidades)
    {
        foreach (Entidad item in lista_entidades)
        {
            if(rectangulo_de_colision.Intersects(item.rectangulo_de_colision))
            {
                return true;
            }
        }
        return false;
    }
    public bool EstaColisionando(Entidad otra_entidad)
    {
        if(rectangulo_de_colision.Intersects(otra_entidad.rectangulo_de_colision))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EstaColisionando(Rectangle otro_rectangulo)
    {
        if(rectangulo_de_colision.Intersects(otro_rectangulo))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EstaColisionando(Entidad otra_entidad, int ID_a_colisionar)
    {
        if(rectangulo_de_colision.Intersects(otra_entidad.rectangulo_de_colision) && otra_entidad.ID == ID_a_colisionar)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EstaColisionandoIDExcluyente(Entidad otra_entidad, int ID_a_no_colisionar)
    {
        if(rectangulo_de_colision.Intersects(otra_entidad.rectangulo_de_colision) && otra_entidad.ID != ID_a_no_colisionar)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //------Colisiones--------------------------------

    //------Updates-----------------------------------
    public void Update(KeyboardState teclado, GameTime gameTime)
    {
        
        if (es_jugador)
        {
            MovimientoTeclas(teclado);
            angulo = angulo + velocidad_de_rotacion;
                
        }
        else
        {
            velocidad = velocidad + aceleracion;
            angulo = angulo + velocidad_de_rotacion;
        }
        posicion = posicion + velocidad;
        ActualizarRectaguloDeColision();
        tiempo_de_existencia = tiempo_de_existencia + Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
    }
    public void Update(GameTime gameTime)
    {
        
        velocidad = velocidad + aceleracion;
        angulo = angulo + velocidad_de_rotacion;
        posicion = posicion + velocidad;
        ActualizarRectaguloDeColision();
        tiempo_de_existencia = tiempo_de_existencia + Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
    }
    public void Update(Vector2 punto, GameTime gameTime)
    {

        
        var velocidad_de_destino = MoverseAUnPunto(punto);
        if (velocidad_de_destino != Vector2.Zero)
        {
            angulo = angulo + velocidad_de_rotacion;
            posicion = posicion + velocidad_de_destino;
        }
        ActualizarRectaguloDeColision();
        tiempo_de_existencia = tiempo_de_existencia + Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
    }
    //------Updates-----------------------------------


    //------Daño--------------------------------------
    public void RecibirDaño(int cantidad)
    {
        vida_actual = vida_actual - cantidad;
        if(vida_actual <= 0)
        {
            esta_vivo = false;
        }
    }
    public void RecibirDaño(int cantidad, Entidad entidad)
    {
        entidad.vida_actual = entidad.vida_actual - cantidad;
        if(vida_actual <= 0)
        {
            esta_vivo = false;
        }
    }
    public void RecibirDaño(int cantidad, List<Entidad> lista_entidades)
    {
        foreach (Entidad item in lista_entidades)
        {
            item.vida_actual = item.vida_actual - cantidad;
        }
        if(vida_actual <= 0)
        {
            esta_vivo = false;
        }
    }
    //------Daño--------------------------------------

    //------Crear Entidades---------------------------
    public Entidad Clonar()
    {
        return new Entidad(
        vida_maxima: this.vida_maxima,
        vida_actual: this.vida_actual,
        esta_vivo: this.esta_vivo,
        posicion: this.posicion,
        imagen: this.imagen,
        prioridad_de_dibujado: this.prioridad_de_dibujado,
        angulo: this.angulo,
        velocidad_de_rotacion: this.velocidad_de_rotacion,
        offset_angulo: this.offset_angulo,
        fuerza_de_aceleracion: this.fuerza_de_aceleracion,
        es_jugador: this.es_jugador,
        activa: this.activa,
        velocidad: this.velocidad,
        aceleracion: this.aceleracion,
        tiempo_de_existencia: this.tiempo_de_existencia,
        ID: this.ID,
        imagen_de_colision: this.imagen_de_colision,
        multiplicador: this.multiplicador
        
        );
    }

    public Entidad Clonar(Entidad entidad_a_clonar)
    {
        return new Entidad(
        vida_maxima: entidad_a_clonar.vida_maxima,
        vida_actual: entidad_a_clonar.vida_actual,
        esta_vivo: entidad_a_clonar.esta_vivo,
        posicion: entidad_a_clonar.posicion,
        imagen: entidad_a_clonar.imagen,
        prioridad_de_dibujado: entidad_a_clonar.prioridad_de_dibujado,
        angulo: entidad_a_clonar.angulo,
        velocidad_de_rotacion: entidad_a_clonar.velocidad_de_rotacion,
        offset_angulo: entidad_a_clonar.offset_angulo,
        fuerza_de_aceleracion: entidad_a_clonar.fuerza_de_aceleracion,
        es_jugador: entidad_a_clonar.es_jugador,
        activa: entidad_a_clonar.activa,
        velocidad: entidad_a_clonar.velocidad,
        aceleracion: entidad_a_clonar.aceleracion,
        tiempo_de_existencia: entidad_a_clonar.tiempo_de_existencia,
        ID: entidad_a_clonar.ID,
        imagen_de_colision: entidad_a_clonar.imagen_de_colision,
        multiplicador: entidad_a_clonar.multiplicador
        
        );
    }
    public Entidad DispararEntidad(Entidad entidad_a_disparar, Vector2 punto_a_dispararlo, Entidad entidad_de_la_que_sale)
    {
        Entidad entidad = Clonar(entidad_a_disparar);
        entidad.posicion = entidad_de_la_que_sale.posicion;
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
    public void DrawColision(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(imagen_de_colision,posicion,rectangulo_de_imagen,Color.Red,0, origen_de_imagen, multiplicador,SpriteEffects.None,prioridad_de_dibujado);
        spriteBatch.Draw(imagen_de_colision,posicion,rectangulo_de_imagen,Color.White,0, origen_de_colision, multiplicador,SpriteEffects.None,prioridad_de_dibujado);
        spriteBatch.End();
    }
    
}