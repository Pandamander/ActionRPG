using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueAction")]
public class DialogueScriptActions : ScriptableObject //MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArbitraryFunction()
    {
        Debug.Log("WHOA IT WORRRRRRKED!");
    }
}