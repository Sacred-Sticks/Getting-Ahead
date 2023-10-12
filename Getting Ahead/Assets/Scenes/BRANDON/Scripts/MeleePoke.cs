using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleePoke : MonoBehaviour
{
    private int liveTime = 1;
    void Awake()
    {
        StartCoroutine(CastTrigger());
    }
   

    void OnTriggerEnter(Collider other)
    {
        //Deal damage
        Debug.Log("HIT - MELEE");
    }



    private IEnumerator CastTrigger()
    {
        yield return new WaitForSeconds(liveTime);
        Destroy(gameObject);
    }
}
