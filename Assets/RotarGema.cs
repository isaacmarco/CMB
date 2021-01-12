using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotarGema : MonoBehaviour
{
   

   
    void Update()
    {
        gameObject.transform.Rotate(
            Vector3.up, Time.deltaTime * 100f 
        );
    }
}
