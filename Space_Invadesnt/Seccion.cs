using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Seccion
{
    public List<Entidad> paredes;
    public Vector2 puntoDeAparicionJugador;
    public List<Vector2> puntosDeAparicionEnemigos;
    public List<Entidad> balasEnemigos;
    public bool elJugadorEstaAdentro;
    public Poligonos areaDeLaSeccion; //detecta si el jugador esta dentro de esta area
    public List<Entidad> enemigos;

    public Seccion
    (
        List<Entidad> paredes = null,
        Vector2 puntoDeAparicionJugador = new Vector2(),
        List<Vector2> puntosDeAparicionEnemigos = null,
        List<Entidad> balasEnemigos = null,
        Poligonos areaDeLaSeccion = null,
        List<Entidad> enemigos = null
    )
    {

        if (paredes == null)
        {
            this.paredes = new List<Entidad>();
        }
        else
        {
            this.paredes = paredes;
        }

        if (puntoDeAparicionJugador != Vector2.Zero)
        {
            this.puntoDeAparicionJugador = puntoDeAparicionJugador;
        }

        if (puntosDeAparicionEnemigos == null)
        {
            this.puntosDeAparicionEnemigos = new List<Vector2>();
        }
        else
        {
            this.puntosDeAparicionEnemigos = puntosDeAparicionEnemigos;
        }

        if (balasEnemigos == null)
        {
            this.balasEnemigos = new List<Entidad>();
        }
        else
        {
            this.balasEnemigos = balasEnemigos;
        }

        if (areaDeLaSeccion == null)
        {
            this.areaDeLaSeccion = new Poligonos();
        }
        else
        {
            this.areaDeLaSeccion = areaDeLaSeccion;
        }

        if (enemigos == null)
        {
            enemigos = new List<Entidad>();
        }
        else
        {
            this.enemigos = enemigos;
        }

        this.elJugadorEstaAdentro = false;
    }

    public void EntidadDentroDelArea(Entidad entidadARevisar)
    {
        if (areaDeLaSeccion.ContienePunto(entidadARevisar.posicion))
        {
            elJugadorEstaAdentro = true;
        }
        else
        {
            elJugadorEstaAdentro = false;
        }
    }

    public void Update(GameTime gameTime)
    {
        foreach (Entidad item in paredes)
        {
            item.Update(gameTime, true);
        }

        foreach (Entidad item in enemigos)
        {
            item.Update(gameTime, true);
        }

        foreach (Entidad item in balasEnemigos)
        {
            item.Update(gameTime, true);
        }
    }

    public void Draw(SpriteBatch _SpriteBatch, Camara camara)
    {
        foreach (Entidad item in paredes)
        {
            item.Draw(_SpriteBatch, camara);
        }

        foreach (Entidad item in enemigos)
        {
            item.Draw(_SpriteBatch, camara);
        }

        foreach (Entidad item in balasEnemigos)
        {
            item.Draw(_SpriteBatch, camara);
        }
    }
}