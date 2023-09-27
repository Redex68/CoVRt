using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeVR : MonoBehaviour
{
    [SerializeField] float fadeTime;
    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Fade()
    {
        StartCoroutine(DoFade());
    }

    private IEnumerator DoFade()
    {
        float alpha = 0.0f;
        while (alpha < 1.0f)
        {
            alpha += Mathf.Min(1.0f, (fadeTime <= 0.0f) ? 1.0f : 1.0f / fadeTime * Time.deltaTime);
            Color c = mat.color;
            c.a = alpha;
            mat.color = c;
            yield return new WaitForEndOfFrameUnit();
        }
    }
}
