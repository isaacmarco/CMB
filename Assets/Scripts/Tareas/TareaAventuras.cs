using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaAventuras : Tarea
{
    [Header("Inventario")]
    public ItemInventario[] inventario; 
    [Header("Interfaz")]
    public GameObject[] corazones; 

    public NivelAventurasScriptable Nivel { 
        get { return (NivelAventurasScriptable) Configuracion.nivelActual;} 
    }

    // vida del jugador
    private int vida = 3; 
    private int vidaMaxima = 3; 
    private IsometricPlayerMovementController jugador; 

    protected override void Actualizacion()
    {

    }

   
    public void RecibirImpacto()
    {     
        
        PerderVida();
        // parpadeo del jugador 
        jugador.RecibirImpacto();
    }

    private void PerderPartida()
    {

    }

    private void PerderVida()
    {
        vida--;
        if(vida < 0)
        {
            vida = 0; 
            PerderPartida();
        }        
        ActualizarMarcadorVida();
    }

    private void GanarVida()
    {        
        vida++;
        if(vida > vidaMaxima)
            vida = vidaMaxima; 
        ActualizarMarcadorVida();
    }

    private void ActualizarMarcadorVida()
    {
        // actualizar marcador de corazones
        for(int i=0; i<corazones.Length; i++)
            corazones[i].SetActive(vida >= i + 1);
    }

    protected override void Inicio()
    {
        tareaBloqueada = false; 
        vida = 3; 
        ActualizarMarcadorVida();
        jugador = FindObjectOfType<IsometricPlayerMovementController>();
    }

    // devuelve verdadero si hay espacio en el inventario
    private bool HayEspacioInventario()
    {        
        foreach(ItemInventario espacioInventario in inventario)
            if(espacioInventario.Libre)
                return true; 
        return false;
    }

    // devuelve un epacio libre en el inventario
    private ItemInventario ObtenerEspacioInventarioLibre()
    {
        // se debe comprobar que existe espacio libre antes
        // de usar esta funcion 
        foreach(ItemInventario espacioInventario in inventario)
        {
            // devolvemos el primer espacio libre
            if(espacioInventario.Libre)
                return espacioInventario; 
        }
        // No deberia
        return null; 
    }

    private void AgregarInventario(ObjetosAventuras item)
    {        
        Debug.Log(item + " recogido");
        // buscar un hueco libre        
        ItemInventario espacioLibre = ObtenerEspacioInventarioLibre();
        // añadirlo al inventario
        espacioLibre.Agregar(item);
    }

    public void ConsumirItem(ObjetosAventuras item)
    {
        Debug.Log("Consumiendo " + item);

        // items que no van al inventario
        switch(item)
        {
            case ObjetosAventuras.Corazon:
                // vida directa
                GanarVida();
            break;
            case ObjetosAventuras.Cofre:
                // no se agrega, se suman puntos
            break;
        }
    }

    public bool RecogerItem(ObjetosAventuras item)
    {        
        // comprobamos si hay espacio
        if(!HayEspacioInventario())
            return false; 
        
        // logica        
        switch(item)
        {           
            case ObjetosAventuras.Llave:
                
            break;
        }

        AgregarInventario(item);

        // devolvemos true para que el objeto sea destruido
        return true; 
    }


            
    public void UsarItem(ObjetosAventuras item)
    {
        Debug.Log("Usando " + item);

        

        switch(item)
        {
            case ObjetosAventuras.PocimaSalud:
                GanarVida();
            break;
        }
    }



    
    public override string ObtenerNombreTarea()
    {
        return "Tarea aventuras";
    }
    
    protected override string ObtenerCabeceraTarea()
    {
        string cabecera = string.Empty;
        // datos de la tarea
        cabecera += "Tarea de evaluacion\n";
        cabecera += "Numero de bloques de evaluacion: " + Configuracion.numberoDeBloquesDeEvaluacion + "\n";
        cabecera += "Leyenda: tiempo; estimulo fijacion visible; numero bloque actual; mirando x; mirando y; estimulo objetivo x; estimulo objetivo y";
        return "Cabecera por decidir";
    }
    
    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {        
        return new RegirstroPosicionOcultarTareaAventuras(
            tiempo, x, y
        );
    } 
    
}
