using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public ConfiguracionScriptable configuracion; 
    public ConfiguracionScriptable Configuracion { get { return configuracion;} }
    
    [SerializeField] private RectTransform canvasRect; 

    public RectTransform CanvasRect 
    {
        get {return canvasRect;}
    }


    
}
