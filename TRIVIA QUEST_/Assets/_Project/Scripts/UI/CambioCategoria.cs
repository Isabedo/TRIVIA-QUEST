using UnityEngine;
using UnityEngine.UI;

public class CambiarColor : MonoBehaviour
{

    [Header("Referencias a las imágenes (excepto el fondo)")]
    public RawImage[] imagenesACambiar; // Las RawImages que deben cambiar de color

    [Header("Colores por categoría")]
    public Color colorAlterna = new Color(0.8f, 0.3f, 0.6f);
    public Color colorPop = new Color(0.3f, 0.6f, 1f);
    public Color colorReggueaton = new Color(1f, 0.4f, 0.2f);
    public Color colorReggae = new Color(0.1f, 0.8f, 0.2f);

    // Llamas a este método cada vez que una pregunta cambie de categoría
    public void CambiarColorCategoria(Categoria categoria)
    {
        Color nuevoColor = Color.white;

        switch (categoria)
        {
            case Categoria.Alterna:
                nuevoColor = colorAlterna;
                break;
            case Categoria.Pop:
                nuevoColor = colorPop;
                break;
            case Categoria.Reggueaton:
                nuevoColor = colorReggueaton;
                break;
            case Categoria.Reggae:
                nuevoColor = colorReggae;
                break;
        }

        foreach (RawImage img in imagenesACambiar)
        {
            if (img != null)
                img.color = nuevoColor;
        }
    }
}