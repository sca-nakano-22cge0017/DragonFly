using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
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
    [SerializeField, Header("�񕜃A�C�e���@�����m��")] float healProb;
    [SerializeField, Header("���[�v�z�[���@�����m��")] float warpProb;
    [SerializeField, Header("���[�v���̈ړ���")] float warpDis;
    List<GameObject> itemList = new List<GameObject>();

    [SerializeField, Header("��Q��/�A�C�e���������x")] float objSpeed;
    [SerializeField, Header("��Q��/�A�C�e���ړ����x �㏸��")] float addSpeed;
    float _addSpeed; //���ۂɌv�Z�Ɏg���l

    [Header("HP")]
    [SerializeField] Image[] heart;
    [SerializeField, Header("����HP")] int defaultHp;
    int hp; //HP
    int lateHp; //�O�t���[����HP
    public int HP { get { return hp;}  set { hp = value; } }

    //��s����
    [SerializeField] Text distance;
    float dis;

    //�Q�[���I�[�o�[
    [SerializeField, Header("���U���g�֑J�ڂ���܂ł̎���")] float toResultWait;
    bool isGameover = false;

    void Start()
    {
        //HP������
        hp = defaultHp;
        lateHp = hp;
        HpDisplay(heart, hp, true);

        //�ŏ��̏�Q���𐶐�
        ObstacleCreate();
    }

    void Update()
    {
        switch (state)
        {
            case STATE.WAIT:
                break;

            case STATE.PLAY:
                HpControll();
                FlyDis();

                //��Q���̈ړ����x���グ��
                _addSpeed += addSpeed * Time.deltaTime;
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
        if (num <= healProb)
        {
            obj = Instantiate(healItem, pos, Quaternion.identity);
        }
        //���[�v�z�[������
        else if(num <= healProb + warpProb)
        {
            obj = Instantiate(warpHole, pos, Quaternion.identity);
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
            HpDisplay(heart, heart.Length, false);

            //���݂�HP�̐������ĕ\��
            HpDisplay(heart, hp, true);

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
    /// HP�C���X�g�̕\���E��\��
    /// </summary>
    /// <param name="image">�Ώۂ̃C���X�g</param>
    /// <param name="num">�\���E��\���ɂ��鐔</param>
    /// <param name="isDisp">true�̂Ƃ��\���Afalse�̂Ƃ���\��</param>
    void HpDisplay(Image[] image, int num, bool isDisp)
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
        dis += Time.deltaTime * 10 + _addSpeed / 100;
        distance.text = dis.ToString("f0") + "m";
    }

    /// <summary>
    /// ���[�v�z�[���ɐG�ꂽ�Ƃ��̏���
    /// </summary>
    public void Warp()
    {
        dis += warpDis;
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(toResultWait);

        //��s�����ۑ�
        PlayerPrefs.SetFloat("Distance", dis);

        //�V�[���J��
        SceneManager.LoadScene("ResultScene");
    }
}
