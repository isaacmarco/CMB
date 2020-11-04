using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))] 
public class ControladorIK : MonoBehaviour
{       
    [Header("Configuracion de la IK")]    
    public bool usarIK = true;
    public bool utilizarIzquierda; 
    [Header("Objetivos de las manos")]
    public Transform objetivoMano = null;
    public Transform objetivoManoContraria = null;
    protected Animator animator;   
    [Header("Referencias a otros huesos")] 
    public Transform huesoMano; 

    void Start () 
    {
        animator = GetComponent<Animator>();
        // establecer mano del paciente
        utilizarIzquierda = FindObjectOfType<TareaMemory>().Configuracion.pacienteActual.esZurdo;
        if(utilizarIzquierda)
            Debug.Log("Paciente es zurdo");
    }   

    // mueve el objetivo de la mano a la posicion proporcionada
    public void MoverObjetivo(Vector3 posicion)
    {
        objetivoMano.position = posicion; 
    }

    void LateUpdate()
    {
        // orientacion por defecto de la mano
        // Vector3 porDefecto = new Vector3(40f,10f, 6f);

        // inclinacion forzada de la mano para dejar mas visibilidad
        // de las tarjetas
        Vector3 inclinacion = new Vector3(37f, -15f, -30f);
        huesoMano.localEulerAngles = inclinacion;
    }

    // callback del animator    
    void OnAnimatorIK()
    {
        if(animator) {
            
            // obtener las manos del IK
            AvatarIKGoal mano = AvatarIKGoal.RightHand;
            AvatarIKGoal manoContraria = AvatarIKGoal.LeftHand;

            // si el paciente es zurdo se invierten las referencias
            if(utilizarIzquierda)
            {
                mano = AvatarIKGoal.LeftHand; 
                manoContraria = AvatarIKGoal.RightHand;
            }


            // al activar la IK posicionamos los huesos para alcanzar el objetivo
            if(usarIK) {
                
                if(objetivoMano != null) {
                    animator.SetIKPositionWeight(mano,1);
                    animator.SetIKRotationWeight(mano,1);  
                    animator.SetIKPosition(mano,objetivoMano.position);
                    animator.SetIKRotation(mano,objetivoMano.rotation);
                }    

                
                // la mano contraria debe moverse a una posicion 
                // que no moleste
                if(objetivoManoContraria != null)
                {
                    animator.SetIKPositionWeight(manoContraria,1);
                    animator.SetIKRotationWeight(manoContraria,1);  
                    animator.SetIKPosition(manoContraria,objetivoManoContraria.position);
                    animator.SetIKRotation(manoContraria,objetivoManoContraria.rotation);
                }

                
            }
                        
            // si la IK esta desactivada mantenemos la posicion original 
            else {          
                animator.SetIKPositionWeight(mano,0);
                animator.SetIKRotationWeight(mano,0); 
                animator.SetLookAtWeight(0);
            }
        }
    }    
}
