using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���J�ڂ𐧌�
/// </summary>
public class SceneChange : MonoBehaviour
{
    [SerializeField] Fade fadeIn;
    bool isFadeIn;
    [SerializeField] Fade fadeOut;
    bool isFadeOut;
    [SerializeField] float fadeTime;

    string sceneName = "MainScene";

    bool isFadeInEnd = false;

    public bool IsFadeInEnd
    {
        get { return isFadeInEnd; }
    }

    private void Update()
    {
        //�t�F�[�h�C��
        if(isFadeIn)
        {
            isFadeIn = false;
            fadeIn.FadeIn(fadeTime, () => isFadeInEnd = true);
        }

        //�t�F�[�h�A�E�g
        if(isFadeOut)
        {
            isFadeOut = false;

            if (sceneName == "GameEnd")
            {
                fadeOut.FadeOut(fadeTime, () => Application.Quit()); //�Q�[���I��
            }
            else fadeOut.FadeOut(fadeTime, () => SceneManager.LoadScene(sceneName)); //�V�[���J��
        }
    }

    public void FadeIn()
    {
        isFadeIn = true;
    }

    /// <summary>
    /// ���C���Q�[����
    /// </summary>
    public void ToMain()
    {
        isFadeOut = true;
        sceneName = "MainScene";
    }

    /// <summary>
    /// �^�C�g����ʂ�
    /// </summary>
    public void ToTitle()
    {
        isFadeOut = true;
        sceneName = "TitleScene";
    }

    /// <summary>
    /// ���U���g��ʂ�
    /// </summary>
    public void ToResult()
    {
        isFadeOut = true;
        sceneName = "ResultScene";
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void GameEnd()
    {
        isFadeOut = true;
        sceneName = "GameEnd";
    }
}
