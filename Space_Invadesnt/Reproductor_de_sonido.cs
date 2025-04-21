using System.Collections;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

public class Reproductor_de_sonido
{
    private List<Song> queue;
    private int index;
    private Dictionary<string,SoundEffect> diccionario_de_efectos_de_sonido;
    public Reproductor_de_sonido()
    {
        queue = new List<Song>();
        diccionario_de_efectos_de_sonido = new Dictionary<string, SoundEffect>();
        index = 0;
    }
    public void AñadirCancion(Song cancion)
    {
        queue.Add(cancion);
    }
    public void AñadirCancion(List<Song> canciones)
    {
        foreach (Song item in canciones)
        {
            queue.Add(item);
        }
    }
    public void Update()
    {
        
        if(MediaPlayer.State != MediaState.Playing)
        {
            index = index + 1;
            MediaPlayer.Play(queue[index - 1]);
            if(index == queue.Count)
            {
                index = 0;
            }
        }
    }

    public void AñadirEfectoDeSonido(string nombre, SoundEffect efecto_de_sonido)
    {
        diccionario_de_efectos_de_sonido.Add(nombre,efecto_de_sonido);
    }
    public void AñadirEfectoDeSonido(Dictionary<string,SoundEffect> pairs)
    {
        foreach (KeyValuePair<string,SoundEffect> item in pairs)
        {
            diccionario_de_efectos_de_sonido.Add(item.Key,item.Value);
        }
    }
    public void AñadirEfectoDeSonido(KeyValuePair<string,SoundEffect> item)
    {
        diccionario_de_efectos_de_sonido.Add(item.Key,item.Value);
    }
    public void ReproducirEfectoDeSonido(KeyValuePair<string,SoundEffect> item)
    {
        if(diccionario_de_efectos_de_sonido.ContainsKey(item.Key))
        {
            diccionario_de_efectos_de_sonido[item.Key].Play();
        }
    }
    public void ReproducirEfectoDeSonido(string item)
    {
        if(diccionario_de_efectos_de_sonido.ContainsKey(item))
        {
            diccionario_de_efectos_de_sonido[item].Play();
        }
    }
}