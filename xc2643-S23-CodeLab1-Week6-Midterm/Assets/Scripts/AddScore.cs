using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddScore : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            ASCIILevelLoadScript.Instance.ChangeScore(1);
            Destroy(gameObject);
        }
    }
}
