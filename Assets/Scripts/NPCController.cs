using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Brandon Callaway | 000954560
public class NPCController : MonoBehaviour
{
    [SerializeField]
    public BoxCollider2D interactableArea;

    [SerializeField]
    public GameObject deliverable;

    public TextMeshPro tmpHolder;
    public SpriteRenderer sr;
    public Animator anim;

    public string[] dialogues;
    int currentDialogue = 0;
    bool canDisplayText = false;
    public bool isAttackable = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get components
        tmpHolder = GetComponentInChildren<TextMeshPro>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        // Set current dialogue to first index, and disable the alpha channel
        tmpHolder.text = dialogues[0];
        tmpHolder.alpha = 0;

        // If no deliverable is set by default, create one to prevent errors
        // Name of gameobject acts as a warning
        if (deliverable == null)
        {
            deliverable = new GameObject("NPC DELIVERABLE NOT SET");
            deliverable.SetActive(false);
        }
        else
        {
            deliverable.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update alpha of the dialogue with respect to the canDisplayText bool
        if (canDisplayText)
        {
            tmpHolder.alpha = 255;
        }
        else
        {
            tmpHolder.alpha = 0;
        }

        // Set dialogue to current dialogue
        tmpHolder.text = dialogues[currentDialogue];
    }

    // Handle NPC collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player is within range of the NPC, display text
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.transform.position.x < transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }

            canDisplayText = true;
        }

        // If the player attacks an attackable NPC, update the dialogue
        // On attack 3, enable the key
        if (collision.gameObject.tag == "PlayerAttackArea" && isAttackable)
        {
            if (currentDialogue < dialogues.Length - 1)
            {
                currentDialogue++;
            }

            if (currentDialogue == dialogues.Length - 1)
            {
                deliverable.SetActive(true);
            }

            // Play damaged NPC animation
            anim.SetBool("hasBeenHit", true);
        }

        // If player interacts with an non-attackable NPC, 
        if (!isAttackable)
        {
            if (collision.gameObject.tag == "PlayerInteractArea")
            {
                if (currentDialogue < dialogues.Length - 1)
                {
                    currentDialogue++;
                }

                if (currentDialogue == dialogues.Length - 1)
                {
                    deliverable.SetActive(true);
                }
            }
        }
    }

    // Disable text if player is out of range, and stop the damage animation after the player attacks
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canDisplayText = false;
        }

        if (collision.gameObject.tag == "PlayerAttackArea")
        {
            anim.SetBool("hasBeenHit", false);
        }
    }
}
