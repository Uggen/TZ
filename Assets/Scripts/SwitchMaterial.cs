using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterial : MonoBehaviour
{
   public Material selected;
   public Material classic;
   private float LastClickTime = 0.0f;


   private void OnMouseDown()
   {
     float timeFromLastClick = Time.time - LastClickTime;
     LastClickTime = Time.time;

    gameObject.GetComponent<Renderer>().material = selected;
    if (timeFromLastClick < 0.2)
    {
        gameObject.GetComponent<Renderer>().material = classic;
    }
   }  
}
