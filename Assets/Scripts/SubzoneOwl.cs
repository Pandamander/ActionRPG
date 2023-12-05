using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class SubzoneOwl : SubzoneEnemy
{
    private Animator _animator;

    public override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
}
