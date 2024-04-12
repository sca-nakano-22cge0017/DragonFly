using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �{�^���ɂ��V�[���J�ڂ𐧌�
/// </summary>
public class SceneChange : MonoBehaviour
{
    /// <summary>
    /// ���C���Q�[����
    /// </summary>
    public void ToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// �^�C�g����ʂ�
    /// </summary>
    public void ToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void GameEnd()
    {
        Application.Quit();
    }
}
