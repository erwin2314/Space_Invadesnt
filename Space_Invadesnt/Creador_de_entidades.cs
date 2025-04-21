using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Creador_de_entidades
{
    public Entidad entidad_base;
    public List<Entidad> lista_entidades;
    public float tiempo_transcurrido;
    public float tiempo_entre_apariciones;
    public Rectangle rectangulo_de_aparicion;
    public Vector2 posicion_del_rectangulo_de_aparicion;
    public float tiempo_maximo_de_existencia_de_entidad;
    private Random random;
    //-------Constructores----------------------------------------------------------
    public Creador_de_entidades
    (
        Entidad entidad_base,
        Rectangle rectangulo_de_aparicion,
        Vector2 posicion_del_rectangulo_de_aparicion,
        float tiempo_entre_apariciones = 5f,
        float tiempo_maximo_de_existencia_de_entidad = 10f

    )
    {
        this.entidad_base = entidad_base;
        this.lista_entidades = new List<Entidad>();
        this.tiempo_transcurrido = 0;
        this.tiempo_entre_apariciones = tiempo_entre_apariciones;
        this.rectangulo_de_aparicion = rectangulo_de_aparicion;
        this.posicion_del_rectangulo_de_aparicion = posicion_del_rectangulo_de_aparicion;
        this.random = new Random();
        this.tiempo_maximo_de_existencia_de_entidad = tiempo_maximo_de_existencia_de_entidad;
    }
    public Creador_de_entidades
    (
        Entidad entidad_base,
        float tiempo_entre_apariciones = 5f,
        float tiempo_maximo_de_existencia_de_entidad = 10f

    )
    {
        this.entidad_base = entidad_base;
        this.lista_entidades = new List<Entidad>();
        this.tiempo_transcurrido = 0;
        this.tiempo_entre_apariciones = tiempo_entre_apariciones;
        this.posicion_del_rectangulo_de_aparicion = new Vector2(1280,0);
        this.rectangulo_de_aparicion = new Rectangle(1280,0,400,720);
        this.random = new Random();
        this.tiempo_maximo_de_existencia_de_entidad = tiempo_maximo_de_existencia_de_entidad;
    }
    //-------Constructores----------------------------------------------------------

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (Entidad e in lista_entidades)
        {
            e.Draw(spriteBatch);
            //e.DrawColision(spriteBatch);
        }
    }
    

    //-------Updates------------------------------------------------------------------
    public void Update(GameTime gameTime, Vector2 punto, bool mantener_posicion_x_del_punto)
    {
        tiempo_transcurrido = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) + tiempo_transcurrido;

        if (tiempo_transcurrido >= tiempo_entre_apariciones)
        {
            Entidad entidad_nueva = entidad_base.Clonar();
            entidad_nueva.posicion = new Vector2(random.Next(rectangulo_de_aparicion.X,rectangulo_de_aparicion.Right),random.Next(rectangulo_de_aparicion.Y,rectangulo_de_aparicion.Bottom));
            lista_entidades.Add(entidad_nueva);
            tiempo_transcurrido = 0f;
        }

        lista_entidades.RemoveAll(e => e.tiempo_de_existencia >= tiempo_maximo_de_existencia_de_entidad);
        lista_entidades.RemoveAll(e => e.esta_vivo == false);
        foreach (Entidad item in lista_entidades)
        {
            
            if (mantener_posicion_x_del_punto == true)
            {
                punto.Y = item.posicion.Y;
                item.Update(punto, gameTime);
            }
            else
            {
                punto.X = item.posicion.X;
                item.Update(punto, gameTime);
            }
            
        }
        
    }
    public void Update(GameTime gameTime, Vector2 punto)
    {
        tiempo_transcurrido = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) + tiempo_transcurrido;

        if (tiempo_transcurrido >= tiempo_entre_apariciones)
        {
            Entidad entidad_nueva = entidad_base.Clonar();
            entidad_nueva.posicion = new Vector2(random.Next(rectangulo_de_aparicion.X,rectangulo_de_aparicion.Right),random.Next(rectangulo_de_aparicion.Y,rectangulo_de_aparicion.Bottom));
            lista_entidades.Add(entidad_nueva);
            tiempo_transcurrido = 0f;
        }

        lista_entidades.RemoveAll(e => e.tiempo_de_existencia >= tiempo_maximo_de_existencia_de_entidad);
        lista_entidades.RemoveAll(e => e.esta_vivo == false);
        foreach (Entidad item in lista_entidades)
        {
            item.Update(punto, gameTime);
        }
        
    }
    public void Update(GameTime gameTime)
    {
        tiempo_transcurrido = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds) + tiempo_transcurrido;

        if (tiempo_transcurrido >= tiempo_entre_apariciones)
        {
            Entidad entidad_nueva = entidad_base.Clonar();
            entidad_nueva.posicion = new Vector2(random.Next(rectangulo_de_aparicion.X,rectangulo_de_aparicion.Right),random.Next(rectangulo_de_aparicion.Y,rectangulo_de_aparicion.Bottom));
            lista_entidades.Add(entidad_nueva);
            tiempo_transcurrido = 0f;
        }

        lista_entidades.RemoveAll(e => e.tiempo_de_existencia >= tiempo_maximo_de_existencia_de_entidad);
        lista_entidades.RemoveAll(e => e.esta_vivo == false);
        foreach (Entidad item in lista_entidades)
        {

            item.Update(gameTime);
        }
        
    }
    //-------Updates------------------------------------------------------------------
}