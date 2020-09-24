using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class TareaMemoria : Tarea
{
    [SerializeField] private GameObject avatar; 
    private ControladorIK controladorIK; 
    private Vector2 puntoFiltrado; 

    protected override void Inicio()
    {    
        // referenciar el controladorIK del avatar
        controladorIK = avatar.GetComponent<ControladorIK>();
    }

    void Update()
    {
        if(controladorIK!=null)
        {            
            ActualizarBrazoVirtual();
        }
    }

   

    private void ActualizarBrazoVirtual()
    {
        // obtnemos el punto 
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        // si es valido actualizamos el brazo 
		if (gazePoint.IsValid)
		{
            // coordenadas en espacio de pantalla
			Vector2 posicionGaze = gazePoint.Screen;	
            // interpolamos y redondeamos las coordenadas
            puntoFiltrado = Vector2.Lerp(puntoFiltrado, posicionGaze, 0.5f);
			Vector2 posicionEntera = new Vector2(
                Mathf.RoundToInt(puntoFiltrado.x), 
                Mathf.RoundToInt(puntoFiltrado.y)
            );         
            MoverBrazoVirtual(posicionEntera); 
			
		} 
      

    }
    private void MoverBrazoVirtual(Vector2 posicionEnPantalla)
    {        
        Ray ray;
     	RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(posicionEnPantalla); // Input.mousePosition);	
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.name == "Mesa")
            {
                Vector3 posicionColisionPlano = hit.point;
                controladorIK.MoverObjetivo(posicionColisionPlano);
            }
		    
        }


    }
}
