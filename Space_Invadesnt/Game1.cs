using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    public Texture2D fondo;
    public Creador_de_entidades creador_De_Entidades;
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
        jugador = new Entidad(new Vector2(124,124), Content.Load<Texture2D>("Imagenes/Jugador/nave_ncpu"), offset_angulo: Convert.ToSingle(Math.PI/2), fuerza_de_aceleracion: 0.1f, es_jugador: true);
        enemigo = new Entidad(imagen: Content.Load<Texture2D>("Imagenes/Enemigos/asteroide"), velocidad: new Vector2(0,2));
        enemigo.CambiarOrigenDeImagen(32,32);
        creador_De_Entidades = new Creador_de_entidades(enemigo, 1.0f);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        fondo = Content.Load<Texture2D>("Imagenes/Fondos/fondo2");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        mouseState = Mouse.GetState();

        posicion_mouse = new Vector2(mouseState.X, mouseState.Y);

        creador_De_Entidades.Update(gameTime,_spriteBatch, jugador.posicion);
        Console.WriteLine(creador_De_Entidades.tiempo_transcurrido);
        jugador.MirarAUnPunto(posicion_mouse);
        jugador.Update(keyboardState);  

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
        _spriteBatch.Draw(fondo,new Rectangle(0,0,1280,720),Color.White);
        _spriteBatch.End();
        creador_De_Entidades.Draw(_spriteBatch);
        jugador.Draw(_spriteBatch);
        base.Draw(gameTime);
    }
}
