using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMarble : MonoBehaviour
{
    
     [SerializeField] Transform targetA;  
    [SerializeField] Transform targetB;  

    void Update()
    {
        if (targetA != null && targetB != null)
        {
            Vector3 averagePosition = (targetA.position + targetB.position) / 2f;
            transform.position = averagePosition; 
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }   
}
