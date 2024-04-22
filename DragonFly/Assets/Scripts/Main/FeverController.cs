using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverController : MonoBehaviour
{
    [SerializeField] SoundController sound;
    MainGameController mainGameController;
    ObjectController objectController;
    UIDisp uiDisp;
    [SerializeField, Header("�����ꏊ")] Transform objectsParent;

    [Header("�t�B�[�o�[")]
    [SerializeField] Image[] balls;
    int ball = 0; //�l���A�C�e���̐�
    int lateBall;
    [SerializeField, Header("�ő��")] int maxBall = 7;
    public int Ball { get { return ball; } set { ball = value; } }

    [SerializeField] GameObject[] ballsImage;
    [SerializeField, Header("�Q�[�W")] GameObject guages;
    [SerializeField] Image guageInside;

    [SerializeField, Header("�p������")] float feverTime;
    float nowTimeFever = 0f; // �o�ߎ���

    [SerializeField, Header("�t�B�[�o�[��̖��G����")] float invincibleTime = 0.5f;

    [SerializeField, Header("�t�B�[�o�[���̑��x�㏸�{��")] float feverRatio;
    float _ratio = 1;

    [SerializeField, Header("�t�B�[�o�[���̋����㏸�{��")] float fever;
    float _fever = 1;
    public float Fever { get { return _fever; } }

    [SerializeField, Header("���G�t�F�N�g")] ParticleSystem windEffect;
    
    void Start()
    {
        if (GetComponent<MainGameController>() is var mgc) mainGameController = mgc;
        if (GetComponent<UIDisp>() is var ud) uiDisp = ud;
        if (GetComponent<ObjectController>() is var oc) objectController = oc;

        //�t�B�[�o�[�A�C�e����������
        ball = 0;
        lateBall = 0;
        uiDisp.UIDisplay(balls, balls.Length, false);

        //���G�t�F�N�g
        windEffect.Stop();
    }

    void Update()
    {
        if (mainGameController.state == MainGameController.STATE.PLAY)
        {
            FeverControll();
        }
    }

    /// <summary>
    /// �t�B�[�o�[�̊Ǘ�
    /// </summary>
    void FeverControll()
    {
        if (ball >= maxBall) ball = maxBall;

        //�O�t���[������l�����ɕύX������Ε\����������
        if (lateBall != ball)
        {
            //�A�C�e�������ׂď���
            uiDisp.UIDisplay(balls, balls.Length, false);

            //���݂̃A�C�e���l���������ĕ\��
            uiDisp.UIDisplay(balls, ball, true);

            lateBall = ball;
        }

        //7�l��������t�B�[�o�[�˓�
        if (ball >= maxBall)
        {
            ball = 0; //������
            mainGameController.IsFever = true;
            mainGameController.IsInvincible = true;
            _fever = fever;

            sound.WindStart(); // SE�Đ�

            objectController.AllItemRelease(); //�A�C�e������

            StartCoroutine(FeverTimeCheck());
        }

        //�t�B�[�o�[�^�C��
        if (mainGameController.IsFever)
        {
            _ratio = feverRatio;
            
            windEffect.Play(); //�G�t�F�N�g�Đ�

            //�Q�[�W�ɕ\����ς���
            for (int i = 0; i < ballsImage.Length; i++)
            {
                ballsImage[i].SetActive(false);
            }
            guages.SetActive(true);

            //�Q�[�W���l�ύX
            nowTimeFever -= Time.deltaTime;
            float c = nowTimeFever / feverTime;
            guageInside.fillAmount = c;
        }

        else
        {
            //�o�ߎ��Ԃ�������
            nowTimeFever = feverTime;

            _ratio = 1;

            windEffect.Stop(); // �G�t�F�N�g��~

            sound.WindStop(); // SE��~

            //�Q�[�W����\����߂�
            for (int i = 0; i < ballsImage.Length; i++)
            {
                ballsImage[i].SetActive(true);
            }
            guages.SetActive(false);
            guageInside.fillAmount = 1;

            _fever = 1;
        }

        SpeedChange(_ratio); //���x�ύX
    }

    IEnumerator FeverTimeCheck()
    {
        yield return new WaitForSecondsRealtime(feverTime);
        mainGameController.IsFever = false; //�t�B�[�o�[�I��

        yield return new WaitForSecondsRealtime(invincibleTime);
        mainGameController.IsInvincible = false;
    }

    void SpeedChange(float speed)
    {
        // �q�I�u�W�F�N�g�̑��x�{����ύX
        foreach (Transform child in objectsParent)
        {
            if(child.GetComponent<ObjectsMove>() is var obj)
            {
                obj.Ratio = _ratio;
            }
        }
    }
}
