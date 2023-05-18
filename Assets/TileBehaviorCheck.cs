using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviorCheck : MonoBehaviour
{
    public TileMapManager tileMapManager;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tileMapManager.GetBehavoirForCurrentTile(transform.position).IsShaded == true)
        {
            spriteRenderer.color = Color.black;
        } else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
