using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultController : MonoBehaviour
{
    [SerializeField] Text distance;
    float dis = 0;
    float d = 0;
    float lastDis = 0;
    [SerializeField] Text newScoreText;

    void Start()
    {
        dis = PlayerPrefs.GetFloat("Distance", 0);
        lastDis = PlayerPrefs.GetFloat("LastDistance", 0);
        newScoreText.enabled = false;
    }

    void Update()
    {
        if (d < dis)
        {
            d++;

            //�X�y�[�X/�G���^�[����������X�R�A�̃J�E���g�A�b�v���X�L�b�v����
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                d = dis;
            }

            distance.text = d.ToString("f0") + "m";
        }
        else if (d >= dis)
        {
            //�O��̃X�R�A��荂��������
            if (lastDis < dis)
            {
                //�V�L�^�̕\��
                newScoreText.enabled = true;

                //�X�R�A�X�V
                lastDis = dis;
                PlayerPrefs.SetFloat("lastScore", lastDis);
            }
        }

        //�f�[�^�폜�R�}���h
        if(Input.GetKey(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
