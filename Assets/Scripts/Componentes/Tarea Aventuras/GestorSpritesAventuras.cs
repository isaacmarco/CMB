using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorSpritesAventuras : MonoBehaviour
{
    
    public Sprite[] sprites;

    public Sprite ObtenerSprite(ObjetosAventuras tipo)
    {
        return sprites[ (int) tipo];
    }
}
