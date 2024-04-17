using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleFade : MonoBehaviour
{
    float power = 1.5f;
    [SerializeField] float fadeSpeed;

    bool isFade = false;

    /// <summary>
    /// true�̂Ƃ��Ƀt�F�[�h�J�n
    /// </summary>
    public bool IsWarpFade
    {
        get { return isFade; }
        set { isFade = value; isFadeOut = true; }
    }

    bool isFadeIn = false;
    bool isFadeOut = false;

    private void Awake()
    {
        power = 1.5f;
        GetComponent<Image>().material.SetFloat("_Power", power);
    }

    void Update()
    {
        if(isFade)
        {
            Fade();

            GetComponent<Image>().material.SetFloat("_Power", power);
        }
    }

    void Fade()
    {
        //�t�F�[�h�A�E�g
        if(isFadeOut)
        {
            if (power > 0)
            {
                power -= fadeSpeed * Time.deltaTime;
            }
            else
            {
                power = 0;
                isFadeOut = false;
                isFadeIn = true;
            }
        }

        //�t�F�[�h�C��
        if(isFadeIn)
        {
            if (power < 1.5f)
            {
                power += fadeSpeed * Time.deltaTime;
            }
            else
            {
                power = 1.5f;
                isFadeIn = false;
                isFade = false;
            }
        }
    }
}
