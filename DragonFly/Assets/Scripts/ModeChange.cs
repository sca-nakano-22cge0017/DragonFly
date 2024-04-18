using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���[�h�ύX
/// </summary>
public class ModeChange : MonoBehaviour
{
    MainGameController mainGameController;
    ObjectController objectController;
    BGCrossFade crossFade;

    int modeNum = 0;
    int lastModeNum = 0;
    int loopNum = 0; //3��̃��[�h�����񃋁[�v������
    int lastLoopNum = 0;

    [SerializeField, Header("�e���[�h�̎���")] float modeInterval;
    float nowTimeMode = 0; //�o�ߎ���

    void Start()
    {
        if (GetComponent<MainGameController>() is var mgc) mainGameController = mgc;
        if (GetComponent<ObjectController>() is var oc) objectController = oc;
        if (GetComponent<BGCrossFade>() is var cf) crossFade = cf;
    }

    void Update()
    {
        if(mainGameController.state == MainGameController.STATE.PLAY)
        {
            Change();
        }
    }

    /// <summary>
    /// ���[�h�ύX
    /// </summary>
    void Change()
    {
        nowTimeMode += Time.deltaTime;

        if (nowTimeMode >= modeInterval)
        {
            nowTimeMode = 0;

            if (modeNum < MainGameController.MODE.GetNames(typeof(MainGameController.MODE)).Length - 1)
            {
                modeNum++;
            }
            else
            {
                modeNum = 0;
                loopNum++; //���[�v�񐔒ǉ�
            }

            mainGameController.mode = (MainGameController.MODE)modeNum;
            
            crossFade.CrossFade(lastModeNum, modeNum);
            lastModeNum = modeNum;
        }

        //���[�h1��������
        if (lastLoopNum != loopNum)
        {
            objectController.SpeedUp();

            lastLoopNum = loopNum;
        }
    }
}
