using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Space_Invadesnt;

public class Space_Invadesnt : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    public MouseState mouseState;
    public Vector2 posicion_mouse;
    public KeyboardState keyboardState;
    public Entidad jugador;
    public Entidad enemigo;
    public Texture2D cuadrado;
    public double puntuacion;
    public double temporizador;
    public double temporizador2;
    public SoundEffect efecto_de_sonido_disparo;
    public SoundEffect efecto_de_sonido_explosion;
    public SoundEffect efecto_de_sonido_muerte;
    public Entidad disparo;
    public Creador_de_entidades creador_De_Entidades2;
    public Creador_de_entidades creador_De_Entidades3;
    public Song cancion_fondo1;
    public Song cancion_fondo2;
    public Texture2D fondo;
    public SpriteFont arial;
    public Creador_de_entidades creador_De_Entidades;
    public List<Entidad> balas;
    public int veces_muerto;
    public Reproductor_de_sonido reproductor_De_Sonido;
    public Texture2D pixel;
    public Space_Invadesnt()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferHeight = 720;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        temporizador = 0;
        jugador = new Entidad(new Vector2(0,0), Content.Load<Texture2D>("Imagenes/Jugador/nave_ncpu"), offset_angulo: Convert.ToSingle(Math.PI/2), fuerza_de_aceleracion: 0.1f, vida_actual: 1, multiplicador_de_colision_x: 0.4f, multiplicador_de_colision_y: 0.8f);
        enemigo = new Entidad(imagen: Content.Load<Texture2D>("Imagenes/Enemigos/asteroide"), velocidad: new Vector2(0,1), ID: 1, vida_actual: 1, multiplicador_de_colision_x: 0.9f, multiplicador_de_colision_y: 0.9f);
        disparo = new Entidad(new Vector2(100,100), Content.Load<Texture2D>("Imagenes/Balas/disparo1"), vida_actual: 1, velocidad: new Vector2(20,20), multiplicador_de_colision_x: 0.5f, multiplicador_de_colision_y: 0.5f);
        balas = new List<Entidad>();

        veces_muerto = 0;
        arial = Content.Load<SpriteFont>("Tipografias/File");
        puntuacion = 0;

        creador_De_Entidades = new Creador_de_entidades(enemigo, 5f, 30.0f);
        creador_De_Entidades2 = new Creador_de_entidades(enemigo, 5f, 20f);
        creador_De_Entidades3 = new Creador_de_entidades(enemigo, 5f, 20f);
        creador_De_Entidades2.rectangulo_de_aparicion = new Rectangle(0,-100,1280,100);
        creador_De_Entidades3.rectangulo_de_aparicion = new Rectangle(0,720,1280,100);

        reproductor_De_Sonido = new Reproductor_de_sonido();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        fondo = Content.Load<Texture2D>("Imagenes/Fondos/fondo2");
        efecto_de_sonido_disparo = Content.Load<SoundEffect>("Sonido/Efectos de sonido/disparo1");
        efecto_de_sonido_explosion = Content.Load<SoundEffect>("Sonido/Efectos de sonido/explosion1");
        efecto_de_sonido_muerte = Content.Load<SoundEffect>("Sonido/Efectos de sonido/muerte1");
        cancion_fondo1 = Content.Load<Song>("Sonido/Canciones/musica_fondo1");
        cancion_fondo2 = Content.Load<Song>("Sonido/Canciones/musica_fondo2");

        reproductor_De_Sonido.AñadirCancion(cancion_fondo2);
        reproductor_De_Sonido.AñadirCancion(cancion_fondo1);
        reproductor_De_Sonido.AñadirEfectoDeSonido("disparo",efecto_de_sonido_disparo);
        reproductor_De_Sonido.AñadirEfectoDeSonido("muerte", efecto_de_sonido_muerte);
        reproductor_De_Sonido.AñadirEfectoDeSonido("explosion", efecto_de_sonido_explosion);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        

        mouseState = Mouse.GetState();
        if ((mouseState.LeftButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Space)) & temporizador2 > 1 & jugador.vida_actual > 0)
        {
            balas.Add(jugador.DispararEntidad(disparo, posicion_mouse, jugador));
            temporizador2 = 0;
            reproductor_De_Sonido.ReproducirEfectoDeSonido("disparo");
        }
        else
        {
            temporizador2 = gameTime.ElapsedGameTime.TotalSeconds + temporizador2;
        }

        posicion_mouse = new Vector2(mouseState.X, mouseState.Y);

        creador_De_Entidades.Update(gameTime, new Vector2(-100,0), true);
        creador_De_Entidades2.Update(gameTime, new Vector2(0,820), false);
        creador_De_Entidades3.Update(gameTime, new Vector2(0,-100), false);


        if (jugador.vida_actual > 0)
        {
            puntuacion = puntuacion + gameTime.ElapsedGameTime.TotalSeconds;
            jugador.MirarAUnPunto(posicion_mouse);
            jugador.poligono_de_colision.Rotar(posicion_mouse, jugador.poligono_de_colision.CalcularCentroide());
            jugador.Update(keyboardState, gameTime);

            if (jugador.EstaColisionando(creador_De_Entidades.lista_entidades) || jugador.EstaColisionando(creador_De_Entidades2.lista_entidades) || jugador.EstaColisionando(creador_De_Entidades3.lista_entidades))
            {
                jugador.RecibirDaño(1);
            }

        }
        else if (veces_muerto == 0)
        {
            reproductor_De_Sonido.ReproducirEfectoDeSonido("explosion");
            reproductor_De_Sonido.ReproducirEfectoDeSonido("muerte");
            veces_muerto = 1;
        }

        balas.RemoveAll(e => e.vida_actual <= 0 == true);
        foreach (Entidad item in balas)
        {
            item.Update(gameTime);
            if (item.EstaColisionando(creador_De_Entidades.lista_entidades, out Entidad entidad))
            {
                Console.WriteLine("true");
                entidad.RecibirDaño(10);
                item.RecibirDaño(10);
                reproductor_De_Sonido.ReproducirEfectoDeSonido("explosion");
            }
            else if (item.EstaColisionando(creador_De_Entidades2.lista_entidades, out Entidad entidad2))
            {
                Console.WriteLine("true");
                entidad2.RecibirDaño(10);
                item.RecibirDaño(10);
                reproductor_De_Sonido.ReproducirEfectoDeSonido("explosion");
            }
            else if (item.EstaColisionando(creador_De_Entidades3.lista_entidades, out Entidad entidad3))
            {
                Console.WriteLine("true");
                entidad3.RecibirDaño(10);
                item.RecibirDaño(10);
                reproductor_De_Sonido.ReproducirEfectoDeSonido("explosion");
            }
        }

        if(creador_De_Entidades.tiempo_entre_apariciones > 0.33 & temporizador > 3)
        {
            temporizador = 0;
            creador_De_Entidades.tiempo_entre_apariciones = creador_De_Entidades.tiempo_entre_apariciones - 0.15f;
            creador_De_Entidades2.tiempo_entre_apariciones = creador_De_Entidades2.tiempo_entre_apariciones - 0.15f;
            creador_De_Entidades3.tiempo_entre_apariciones = creador_De_Entidades3.tiempo_entre_apariciones - 0.15f;
        }
        else
        {
            temporizador = temporizador + gameTime.ElapsedGameTime.TotalSeconds;
        }

        reproductor_De_Sonido.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
        _spriteBatch.Draw(fondo,new Rectangle(0,0,1280,720),Color.White);
        _spriteBatch.DrawString(arial, Convert.ToString(Convert.ToInt32(puntuacion)),new Vector2(0,0),Color.White);
        _spriteBatch.End();
        creador_De_Entidades.Draw(_spriteBatch, pixel);
        creador_De_Entidades2.Draw(_spriteBatch, pixel);
        creador_De_Entidades3.Draw(_spriteBatch, pixel);
        foreach (Entidad item in balas)
        {
            item.Draw(_spriteBatch);
            item.DrawColision(_spriteBatch, pixel);
        }


        if(jugador.vida_actual > 0)
        {
            jugador.Draw(_spriteBatch);
            jugador.DrawColision(_spriteBatch, pixel);
        }
        base.Draw(gameTime);
    }
}
