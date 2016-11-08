using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFade : MonoBehaviour {

    CanvasRenderer cr;
    RawImage obj;

    void Start()
    {
        cr = GetComponent<CanvasRenderer>();
        obj = GetComponent<RawImage>();

        cr.SetAlpha(0f);
    }

    public void FadeIn()
    {
        cr = GetComponent<CanvasRenderer>();
        obj = GetComponent<RawImage>();

        cr.SetAlpha(0f);
        obj.CrossFadeAlpha(1.0f, 0.5f, false);
    }

    public void FadeOut()
    {
        obj = GetComponent<RawImage>();
        obj.CrossFadeAlpha(0f, 0.5f, false);
    }
}
