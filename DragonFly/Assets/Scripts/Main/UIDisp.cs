using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI����
/// </summary>
public class UIDisp : MonoBehaviour
{
    /// <summary>
    /// UI�C���X�g�̕\���E��\��
    /// </summary>
    /// <param name="image">�Ώۂ̃C���X�g</param>
    /// <param name="num">�\���E��\���ɂ��鐔</param>
    /// <param name="isDisp">true�̂Ƃ��\���Afalse�̂Ƃ���\��</param>
    public void UIDisplay(Image[] image, int num, bool isDisp)
    {
        for (int i = 0; i < num; i++)
        {
            image[i].enabled = isDisp;
        }
    }

    /// <summary>
    /// �e�L�X�g�̑��
    /// </summary>
    /// <param name="text">�����Text</param>
    /// <param name="str">���ꂽ��������</param>
    public void TextPutIn(Text text, string str)
    {
        text.text = str;
    }
}
