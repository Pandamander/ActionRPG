using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoEndScreen : MonoBehaviour
{
    private Image _image;
    private Fader _fader;
    private void Awake()
    {
        _image = GetComponentInChildren<Image>();
        _fader = GetComponentInChildren<Fader>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }

    public void FadeIn()
    {
        _fader.FadeIn(0.03f);
    }
}
