using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GestorItemsAventuras : MonoBehaviour
{
    
    public Texture[] sprites;

    public Texture ObtenerSprite(ObjetosAventuras tipo)
    {
        return sprites[ (int) tipo];
    }
}
