using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
    [SerializeField] GameObject player;

    public enum STATE { WAIT = 0, PLAY, GAMEOVER, }
    public STATE state = 0;

    [Header("��Q������")]
    [SerializeField, Header("��Q���𐶐�����Ƃ��̐e�I�u�W�F�N�g")] GameObject obstacles;
    [SerializeField, Header("��Q�������ʒu")] Vector3 createPos = new Vector3(10, 0, 0);
    [SerializeField, Header("��Q����Prefab")] GameObject[] obstacle;

    [Header("�A�C�e������")]
    [SerializeField, Header("�A�C�e���𐶐�����Ƃ��̐e�I�u�W�F�N�g")] GameObject items;
    [SerializeField, Header("�A�C�e�������ʒu�@X")] int itemPosX;
    [SerializeField, Header("�A�C�e�������ʒu�@Y")] int[] itemPosY;
    [SerializeField, Header("�񕜃A�C�e����Prefab")] GameObject healItem;
    [SerializeField, Header("���[�v�z�[����Prefab")] GameObject warpHole;
    [SerializeField, Header("�t�B�[�o�[�A�C�e����Prefab")] GameObject feverItem;
    [SerializeField, Header("�񕜃A�C�e���@�����m��")] float healProb;
    float _healProb = 0; //�v�Z�p
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

    [Header("HP")]
    [SerializeField] Image[] heart;
    [SerializeField, Header("����HP")] int defaultHp;
    int hp; //HP
    int lateHp; //�O�t���[����HP
    public int HP { get { return hp;}  set { hp = value; } }

    [Header("�t�B�[�o�[")]
    [SerializeField] Image[] balls;
    int ball = 0; //�l���A�C�e���̐�
    int lateBall;
    [SerializeField, Header("�ő��")] int maxBall = 7;
    public int Ball { get { return ball; } set { ball = value; } }

    [SerializeField, Header("�p������")] float feverTime;
    [SerializeField, Header("�t�B�[�o�[���̑��x�㏸�{��")] float feverRatio;
    float _ratio = 1;
    [SerializeField, Header("���G�t�F�N�g")] ParticleSystem windEffect;

    bool isFever = false;
    public bool IsFever { get { return isFever; } set { isFever = value; } }

    //��s����
    [SerializeField] Text distance;
    float dis;

    //�Q�[���I�[�o�[
    [SerializeField] SceneChange sceneChange;
    [SerializeField, Header("���U���g�֑J�ڂ���܂ł̎���")] float toResultWait;
    bool isGameover = false;

    void Awake()
    {
        //HP������
        hp = defaultHp;
        lateHp = hp;
        UIDisplay(heart, hp, true);

        //�t�B�[�o�[�A�C�e����������
        ball = 0;
        lateBall = 0;
        UIDisplay(balls, balls.Length, false);

        //�ŏ��̏�Q���𐶐�
        ObstacleCreate();

        //�����m��������
        _healProb = healProb;
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
                HpControll();
                FeverControll();
                FlyDis();
                CreateProbability();

                //��Q���̈ړ����x���グ��
                if(maxSpeed > _addSpeed) _addSpeed += addSpeed * Time.deltaTime;
                break;

            case STATE.GAMEOVER:
                break;
        }
    }

    /// <summary>
    /// ��Q������
    /// </summary>
    public void ObstacleCreate()
    {
        int num = Random.Range(0, obstacle.Length);
        var obj = Instantiate(obstacle[num], createPos, Quaternion.identity, obstacles.transform);

        if (obj)
        {
            SpeedChange(obj, objSpeed, false);
            SpeedChange(obj, _addSpeed, true);
        }
    }

    /// <summary>
    /// �A�C�e�������m���̒���
    /// </summary>
    void CreateProbability()
    {
        //HP���ő�̂Ƃ��͉񕜃A�C�e������������Ȃ��悤�ɂ��� ���A�C�e���̐����m�����グ��
        if (hp >= defaultHp)
        {
            _healProb = 0;
            _warpProb += _healProb / 4;
            _feverProb += _healProb / 2;
        }
        else if(!isFever)
        {
            //�����l�ɖ߂�
            _healProb = healProb;
            _warpProb = warpProb;
            _feverProb = feverProb;
        }

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

        //�񕜃A�C�e������
        if (num <= _healProb)
        {
            obj = Instantiate(healItem, pos, Quaternion.identity);
        }
        //���[�v�z�[������
        else if(num <= _healProb + _warpProb)
        {
            obj = Instantiate(warpHole, pos, Quaternion.identity);
        }
        //�t�B�[�o�[�A�C�e������
        else if(num <= _healProb + _warpProb + _feverProb)
        {
            obj = Instantiate(feverItem, pos, Quaternion.identity);
        }

        if (obj)
        {
            SpeedChange(obj, objSpeed, false);
            SpeedChange(obj, _addSpeed, true);
        }
    }

    /// <summary>
        /// ��Q��/�A�C�e���̑��x�ύX
        /// </summary>
        /// <param name="obj">���x��ς���I�u�W�F�N�g</param>
        /// <param name="speed">���Z/������x</param>
        /// <param name="isAdd">���Z���ǂ��� true�ŉ��Z false�ő��</param>
    void SpeedChange(GameObject obj, float speed, bool isAdd)
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

    /// <summary>
    /// HP�Ǘ�
    /// </summary>
    void HpControll()
    {
        if(hp >= defaultHp) hp = defaultHp;

        //�O�t���[������HP�ɕύX������Ε\����������
        if (lateHp != hp)
        {
            //�n�[�g�����ׂď���
            UIDisplay(heart, heart.Length, false);

            //���݂�HP�̐������ĕ\��
            UIDisplay(heart, hp, true);

            lateHp = hp;
        }

        //hp��0�ȉ��ɂȂ�����Q�[���I�[�o�[
        if(hp <= 0)
        {
            state = STATE.GAMEOVER;
            if(!isGameover)
            {
                isGameover = true;
                StartCoroutine(GameOver());
            }
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
        }
        else
        {
            _ratio = 1;
            Time.timeScale = 1;
            windEffect.Stop();
        }
    }

    IEnumerator FeverTimeCheck()
    {
        yield return new WaitForSecondsRealtime(feverTime);
        isFever = false;
    }

    /// <summary>
    /// HP�C���X�g�̕\���E��\��
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
        var obj = Instantiate(warpExit, pPos + new Vector3(2, 0, 0), Quaternion.identity, obstacles.transform);
        if (obj)
        {
            SpeedChange(obj, objSpeed, false);
            SpeedChange(obj, _addSpeed, true);
        } //�ړ����x�ύX

        dis += warpDis;

        yield return new WaitForSeconds(1f);

        isWarp = false; //���[�v�I��
    }

    IEnumerator GameOver()
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
