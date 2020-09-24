using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))] 
public class ControladorIK : MonoBehaviour
{       
    [Header("Configuracion de la IK")]    
    public bool usarIK = true;
    public bool utilizarIzquierda; 
    [Header("Objetivo de la mano")]
    public Transform objetivoMano = null;
    protected Animator animator;    

    void Start () 
    {
        animator = GetComponent<Animator>();
    }   

    // mueve el objetivo de la mana a la posicion proporcionada
    public void MoverObjetivo(Vector3 posicion)
    {
        objetivoMano.position = posicion; 
    }

    // callback del animator    
    void OnAnimatorIK()
    {
        if(animator) {
            
            // seleccionar la mano del IK
            AvatarIKGoal mano = AvatarIKGoal.RightHand;
            if(utilizarIzquierda)
                mano = AvatarIKGoal.LeftHand; 

            // al activar la IK posicionamos los huesos para alcanzar el objetivo
            if(usarIK) {
                
                if(objetivoMano != null) {
                    animator.SetIKPositionWeight(mano,1);
                    animator.SetIKRotationWeight(mano,1);  
                    animator.SetIKPosition(mano,objetivoMano.position);
                    animator.SetIKRotation(mano,objetivoMano.rotation);
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
