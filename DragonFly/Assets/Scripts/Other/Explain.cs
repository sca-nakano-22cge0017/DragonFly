using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explain : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] GameObject[] text;
    [SerializeField] GameObject forAndroidText;
    int page = 0;

    private void Start()
    {
        // �A���h���C�h�̏ꍇ�͕\���e�L�X�g��ς���
#if UNITY_ANDROID
        text[0].SetActive(false);
        text[0] = forAndroidText;
#endif

        for (int i = 0; i < text.Length; i++)
        {
            if (i == page) text[i].SetActive(true);
            else text[i].SetActive(false);
        }
    }

    public void Display()
    {
        window.SetActive(true);
    }

    public void UnDisplay()
    {
        window.SetActive(false);
    }

    /// <summary>
    /// �O�̃y�[�W��
    /// </summary>
    public void LastPage()
    {
        page--;
        if(page < 0) page = text.Length - 1;

        TextUpdate();
    }

    /// <summary>
    /// ���̃y�[�W��
    /// </summary>
    public void NextPage()
    {
        page++;
        if(page > text.Length - 1) page = 0;

        TextUpdate();
    }

    /// <summary>
    /// �X�V
    /// </summary>
    void TextUpdate()
    {
        for (int i = 0; i < text.Length; i++)
        {
            if (i == page) text[i].SetActive(true);
            else text[i].SetActive(false);
        }
    }
}
