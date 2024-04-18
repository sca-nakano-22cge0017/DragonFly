using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
    [SerializeField] GameObject player;

    public enum STATE { WAIT = 0, PLAY, GAMEOVER, }
    public STATE state = 0;

    public enum MODE { DAY = 0, EVENIG, NIGHT }
    public MODE mode = 0;
    int modeNum = 0;
    int loopNum = 0; //3��̃��[�h�����񃋁[�v������
    int lastLoopNum = 0;

    [Header("���[�h")]
    [SerializeField, Header("�e���[�h�̎���")] float modeInterval;
    [SerializeField, Header("�w�i")] Image bg;
    [SerializeField, Header("�w�i�摜")] Sprite[] bgSprite;

    [Header("��Q������")]
    [SerializeField, Header("��Q���𐶐�����Ƃ��̐e�I�u�W�F�N�g")] GameObject obstacles;
    [SerializeField, Header("��Q�������ʒu")] Vector3 createPos = new Vector3(10, 0, 0);
    [SerializeField, Header("��Q��")] GameObject[] obstacle;
    int lastObj = 3; //���߂ɐ���������Q��

    [Header("�A�C�e������")]
    [SerializeField, Header("�A�C�e���𐶐�����Ƃ��̐e�I�u�W�F�N�g")] GameObject items;
    [SerializeField, Header("�A�C�e�������ʒu�@X")] int itemPosX;
    [SerializeField, Header("�A�C�e�������ʒu�@Y")] int[] itemPosY;
    [SerializeField, Header("���[�v�z�[����Prefab")] GameObject warpHole;
    [SerializeField, Header("�t�B�[�o�[�A�C�e����Prefab")] GameObject feverItem;
    [SerializeField, Header("���[�v�z�[���@�����m��")] float warpProb;
    float _warpProb = 0; //�v�Z�p
    [SerializeField, Header("�t�B�[�o�[�A�C�e���@�����m��")] float feverProb;
    float _feverProb = 0; //�v�Z�p

    [SerializeField, Header("�������x")] float objSpeed;
    [SerializeField, Header("�ړ����x�@�㏸��")] float addSpeed;
    [SerializeField, Header("�ړ����x�@�ő�l")] float maxSpeed;
    float _addSpeed; //���ۂɌv�Z�Ɏg���l

    [Header("���[�v")]
    [SerializeField, Header("���[�v���̈ړ���")] float warpDis;
    [SerializeField, Header("���[�v���̉~�`�t�F�[�h")] CircleFade warpFade;
    bool isWarp =false;
    public bool IsWarp { get { return isWarp; } set { isWarp = value; } }
    [SerializeField, Header("���[�v�o��")] GameObject warpExit;

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
    [SerializeField, Header("�t�B�[�o�[���̑��x�㏸�{��")] float feverRatio;
    float _ratio = 1;
    [SerializeField, Header("���G�t�F�N�g")] ParticleSystem windEffect;
    public float Ratio { get { return feverRatio; } }

    bool isFever = false;
    public bool IsFever { get { return isFever; } set { isFever = value; } }

    //��s����
    [SerializeField] Text distance;
    float dis;

    //�Q�[���I�[�o�[
    [SerializeField] SceneChange sceneChange;
    [SerializeField, Header("���U���g�֑J�ڂ���܂ł̎���")] float toResultWait;

    void Awake()
    {
        //�t�B�[�o�[�A�C�e����������
        ball = 0;
        lateBall = 0;
        UIDisplay(balls, balls.Length, false);

        //�ŏ��̏�Q���𐶐�
        ObstacleCreate();

        //�����m��������
        _warpProb = warpProb;
        _feverProb = feverProb;

        //�t�F�[�h�C��
        sceneChange.FadeIn();
        StartCoroutine(FadeEndCheck());

        //���G�t�F�N�g
        windEffect.Stop();
    }

    void Update()
    {
        switch (state)
        {
            case STATE.WAIT:
                break;

            case STATE.PLAY:
                FeverControll();
                FlyDis();
                CreateProbability();
                ModeChange();
                break;

            case STATE.GAMEOVER:
                break;
        }
    }

    float nowTimeMode = 0; //�o�ߎ���
    /// <summary>
    /// ���[�h�ύX
    /// </summary>
    void ModeChange()
    {
        nowTimeMode += Time.deltaTime;

        if(nowTimeMode >= modeInterval)
        {
            nowTimeMode = 0;
            
            if(modeNum < MODE.GetNames(typeof(MODE)).Length - 1)
            {
                modeNum++;
            }
            else
            {
                modeNum = 0;
                loopNum++; //���[�v�񐔒ǉ�
            }
        }

        mode = (MODE) modeNum;
        bg.sprite = bgSprite[modeNum];

        //���[�h1��������
        if(lastLoopNum != loopNum)
        {
            //��Q���̈ړ����x���グ��
            if (maxSpeed > _addSpeed + objSpeed) _addSpeed += addSpeed;

            lastLoopNum = loopNum;
        }
    }

    /// <summary>
    /// ��Q������
    /// </summary>
    public void ObstacleCreate()
    {
        int num = 0;

        //���[�h�ɂ���Đ�������ύX
        switch (mode)
        {
            case MODE.DAY:
                num = Random.Range(0, 3); //2�}�X�󂢂Ă�I�u�W�F�N�g�̂�
                break;

            case MODE.EVENIG:
                switch(lastObj) //���߂̃I�u�W�F�N�g�ƍő�㉺1�}�X�����
                {
                    case 3:
                        num = lastObj + Random.Range(0, 2);
                        break;
                    case 4:
                    case 5:
                        num = lastObj + Random.Range(-1, 2);
                        break;
                    case 6:
                        num = lastObj + Random.Range(-1, 1);
                        break;
                }
                lastObj = num; //���������I�u�W�F�N�g�̔ԍ���ێ�
                break;

            case MODE.NIGHT:
                switch (lastObj) //���߂̃I�u�W�F�N�g�Ə㉺�ɍŏ�2�}�X�A�ő�3�}�X�����
                {
                    case 3:
                        num = lastObj + Random.Range(2, 4);
                        break;
                    case 4:
                        num = lastObj + 2;
                        break;
                    case 5:
                        num = lastObj - 2;
                        break;
                    case 6:
                        num = lastObj + Random.Range(-3, -1);
                        break;
                }
                lastObj = num; //���������I�u�W�F�N�g�̔ԍ���ێ�
                break;

            default:
                //���S�����_��
                num = Random.Range(0, obstacle.Length);
                break;
        }
        
        var obj = Instantiate(obstacle[num], createPos, Quaternion.identity, obstacles.transform);

        if (obj)
        {
            ObjSpeedChange(obj, objSpeed, false);
            ObjSpeedChange(obj, _addSpeed, true);
        }
    }

    /// <summary>
    /// �A�C�e�������m���̒���
    /// </summary>
    void CreateProbability()
    {
        //�t�B�[�o�[���̓t�B�[�o�[�A�C�e���E���[�v�A�C�e������������Ȃ��悤�ɂ���
        if (isFever) { _feverProb = 0; _warpProb = 0; }
        else { _warpProb = warpProb; _feverProb = feverProb; }
    }

    /// <summary>
    /// �A�C�e���̐���
    /// </summary>
    public void ItemCreate()
    {
        //���m���Ő�������
        int num = Random.Range(1, 101);

        //�����ʒu�������_���ɎZ�o
        int n = Random.Range(0, itemPosY.Length);
        Vector3 pos = new Vector3(itemPosX, itemPosY[n], 0);
        GameObject obj = null;

        //���[�v�z�[������
        if(num <= _warpProb)
        {
            obj = Instantiate(warpHole, pos, Quaternion.identity);
        }
        //�t�B�[�o�[�A�C�e������
        else if(num <= _warpProb + _feverProb)
        {
            obj = Instantiate(feverItem, pos, Quaternion.identity);
        }

        if (obj)
        {
            ObjSpeedChange(obj, objSpeed, false);
            ObjSpeedChange(obj, _addSpeed, true);
        }
    }

    /// <summary>
        /// ��Q��/�A�C�e���̑��x�ύX
        /// </summary>
        /// <param name="obj">���x��ς���I�u�W�F�N�g</param>
        /// <param name="speed">���Z/������x</param>
        /// <param name="isAdd">���Z���ǂ��� true�ŉ��Z false�ő��</param>
    void ObjSpeedChange(GameObject obj, float speed, bool isAdd)
    {
        if (obj.GetComponent<ObjectsMove>() is ObjectsMove om)
        {
            if(isAdd)
            {
                om.Speed += speed; //���Z
            }
            else om.Speed = speed; //���
        }
    }

    float nowTimeFever = 0f; // �o�ߎ���
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
            UIDisplay(balls, balls.Length, false);

            //���݂̃A�C�e���l���������ĕ\��
            UIDisplay(balls, ball, true);

            lateBall = ball;
        }

        //7�l��������t�B�[�o�[�˓�
        if (ball >= maxBall)
        {
            ball = 0; //������
            isFever = true;
            StartCoroutine(FeverTimeCheck());
        }

        //�t�B�[�o�[�^�C��
        if(isFever)
        {
            _ratio = feverRatio;
            Time.timeScale = feverRatio; //���x�㏸
            windEffect.Play(); //�G�t�F�N�g�Đ�

            for(int i = 0; i < ballsImage.Length; i++)
            {
                ballsImage[i].SetActive(false);
            }

            guages.SetActive(true);

            //�Q�[�W���l�ύX
            nowTimeFever -= Time.deltaTime / feverRatio;
            float c = nowTimeFever / feverTime;
            guageInside.fillAmount = c;
        }
        else
        {
            nowTimeFever = feverTime;

            _ratio = 1;
            Time.timeScale = 1;
            windEffect.Stop();

            for (int i = 0; i < ballsImage.Length; i++)
            {
                ballsImage[i].SetActive(true);
            }

            guages.SetActive(false);
            guageInside.fillAmount = 1;
        }
    }

    IEnumerator FeverTimeCheck()
    {
        yield return new WaitForSecondsRealtime(feverTime);
        isFever = false;
    }

    /// <summary>
    /// UI�C���X�g�̕\���E��\��
    /// </summary>
    /// <param name="image">�Ώۂ̃C���X�g</param>
    /// <param name="num">�\���E��\���ɂ��鐔</param>
    /// <param name="isDisp">true�̂Ƃ��\���Afalse�̂Ƃ���\��</param>
    void UIDisplay(Image[] image, int num, bool isDisp)
    {
        for (int i = 0; i < num; i++)
        {
            image[i].enabled = isDisp;
        }
    }

    /// <summary>
    /// ��s�����Ǘ�
    /// </summary>
    void FlyDis()
    {
        dis += Time.deltaTime * 10 * _ratio + _addSpeed / 100;
        distance.text = dis.ToString("f0") + "m";
    }

    /// <summary>
    /// ���[�v�z�[���ɐG�ꂽ�Ƃ��̏���
    /// </summary>
    public void Warp()
    {
        warpFade.IsWarpFade = true; //���[�v���o
        isWarp = true;

        StartCoroutine(WarpEnd());
    }

    /// <summary>
    /// ���[�v���o
    /// </summary>
    /// <returns></returns>
    IEnumerator WarpEnd()
    {
        yield return new WaitForSeconds(0.5f);

        SpriteRenderer pSR = player.GetComponent<SpriteRenderer>();
        pSR.enabled = false; //�v���C���[��\��

        yield return new WaitUntil(() => !warpFade.IsWarpFade); //���o���I�������

        pSR.enabled = true; //�v���C���[�\��

        //���[�v�z�[���̏o���𐶐�
        Vector3 pPos = player.GetComponent<Transform>().transform.position;
        Vector3 bPos = new Vector3(2, 0, 0); //�{�[�������ʒu
        var obj = Instantiate(warpExit, pPos + bPos, Quaternion.identity, obstacles.transform);
        if (obj)
        {
            ObjSpeedChange(obj, objSpeed, false);
            ObjSpeedChange(obj, _addSpeed, true);
        } //�ړ����x�ύX

        dis += warpDis;

        yield return new WaitForSeconds(0.5f);

        isWarp = false; //���[�v�I��
    }

    public void GameOver()
    {
        state = STATE.GAMEOVER;
        StartCoroutine(GameEnd());
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(toResultWait);

        //��s�����ۑ�
        PlayerPrefs.SetFloat("Distance", dis);

        //�V�[���J��
        sceneChange.ToResult();
    }

    /// <summary>
    /// �t�F�[�h�C���������������`�F�b�N����
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeEndCheck()
    {
        yield return new WaitUntil(() => sceneChange.IsFadeInEnd);
        state = STATE.PLAY;
    }
}
