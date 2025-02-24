using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Brandon Callaway | 000954560
public class ClothesPickup : MonoBehaviour
{
    public TextMeshPro tmp;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshPro>();
        tmp.alpha = 0;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If player is within range of the NPC, display text
        if (collision.gameObject.tag == "Player")
        {
            tmp.alpha = 255;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // If player is within range of the NPC, display text
        if (collision.gameObject.tag == "Player")
        { 
            tmp.alpha = 0;
        }
    }
}
