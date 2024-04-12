using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
    public enum STATE { WAIT = 0, PLAY, GAMEOVER, }
    public STATE state = 0;

    //��Q������
    [SerializeField, Header("��Q����Prefab")] GameObject[] obstacle;
    [SerializeField, Header("��Q���𐶐�����e�I�u�W�F�N�g")] GameObject obstacles;
    [SerializeField, Header("��Q�������ʒu")] Vector3 createPos = new Vector3(10, 0, 0);
    [SerializeField, Header("��Q���ړ����x �㏸��")] float addSpeed;
    float _addSpeed; //���ۂɌv�Z�Ɏg���l

    //HP
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
        switch(state)
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
        ObstacleController obs = obj.GetComponent<ObstacleController>();
        obs.ObstacleSpeed += _addSpeed;
    }

    /// <summary>
    /// HP�Ǘ�
    /// </summary>
    void HpControll()
    {
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

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(toResultWait);

        //��s�����ۑ�
        PlayerPrefs.SetFloat("Distance", dis);

        //�V�[���J��
        SceneManager.LoadScene("ResultScene");
    }
}
