using System.Collections.Generic;
using System.Numerics;

public class Mapa
{
    public List<Entidad> paredes;
    public List<Entidad> enemigos;
    public List<Entidad> balasJugador;
    public List<Entidad> balasEnemigos;
    public List<Vector2> posicionesDeAparcionJugador;
    public List<Entidad> listaDeDibujado;
    public int puntoDeAparicionActivo;
    public Mapa
    (
        List<Entidad> paredes = null,
        List<Entidad> enemigos = null,
        List<Entidad> balasJugador = null,
        List<Entidad> balasEnemigos = null,
        List<Vector2> posicionesDeAparcionJugador = null,
        int puntoDeAparicionActivo = 0

    )
    {

    }
}
