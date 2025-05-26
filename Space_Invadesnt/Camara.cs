using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
public class Camara
{
    public Vector2 posicion;
    public Vector2 velocidad;
    public float constante; // define que tan rapido la camara sigue al objetivo
    public Viewport viewport;
    public Camara
    (
        Viewport viewport,
        Vector2 posicion = new Vector2(),
        Vector2 velocidad = new Vector2(),
        float constante = 4f
        
    )
    {
        this.posicion = posicion;
        this.velocidad = velocidad;
        this.constante = constante;
        this.viewport = viewport;

    }

    // Devuelve la matriz de transformación para usar en SpriteBatch.Begin
    public Matrix ObtenerTransformacion()
    {
        return Matrix.CreateTranslation(new Vector3(-posicion + new Vector2(viewport.Width / 2f, viewport.Height / 2f), 0));
    }

    // Llama a este método en Update, pasándole la posición del jugador
    public void Update(Vector2 objetivo, float deltaTime)
    {
        posicion = Vector2.Lerp(posicion, objetivo, constante * deltaTime);
    }
}