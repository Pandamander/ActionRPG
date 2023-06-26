using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterActions : MonoBehaviour
{
    // New material
    public Material newMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NPCScholar_GiveAmulet()
    {
        // Assign the SpriteRenderer's material to newMaterial
        GetComponent<SpriteRenderer>().material = newMaterial;
        GetComponent<AudioSource>().Play();
        Debug.Log("You get 1000 XP!");
    }
}
