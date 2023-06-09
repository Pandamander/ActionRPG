using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterActions : MonoBehaviour
{
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
        GetComponent<SpriteRenderer>().color = Color.black;
        GetComponent<AudioSource>().Play();
        Debug.Log("You get 1000 XP!");
    }
}
