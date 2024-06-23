using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor;
    [SerializeField] private float _flashTime;
    [SerializeField] private float _flashAlpha;
    [SerializeField] private int _flashCount;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _material = _spriteRenderer.material;
    }

    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        for (int i = 0; i < _flashCount; i++)
        {
            _material.SetColor("_FlashColor", _flashColor);
            _material.SetFloat("_FlashAmount", 1f);
            yield return new WaitForSeconds(_flashTime);
            _material.SetFloat("_FlashAmount", 0f);
            yield return new WaitForSeconds(_flashTime);
        }
    }
}
