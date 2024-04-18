using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! ToDo �I�u�W�F�N�g�v�[���̎���

/// <summary>
/// �I�u�W�F�N�g�̐����E���x�ύX
/// </summary>
public class ObjectController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] MainGameController mainGameController;

    [SerializeField, Header("�����ꏊ")] GameObject parent;

    [Header("��Q������")]
    [SerializeField, Header("�����ʒu")] Vector3 createPos = new Vector3(10, 0, 0);
    [SerializeField, Header("��Q��")] GameObject[] obstacle;
    int lastObj = 3; //���߂ɐ���������Q��

    [Header("�A�C�e������")]
    [SerializeField, Header("�A�C�e�������ʒu�@X")] int itemPosX;
    [SerializeField, Header("�A�C�e�������ʒu�@Y")] int[] itemPosY;
    [SerializeField, Header("���[�v�z�[����Prefab")] GameObject warpHole;
    [SerializeField, Header("�t�B�[�o�[�A�C�e����Prefab")] GameObject feverItem;
    [SerializeField, Header("���[�v�z�[���@�����m��")] float warpProb;
    float _warpProb = 0; //�v�Z�p
    [SerializeField, Header("�t�B�[�o�[�A�C�e���@�����m��")] float feverProb;
    float _feverProb = 0; //�v�Z�p

    [Header("���[�v")]
    [SerializeField, Header("���[�v�o��")] GameObject warpExit;

    [SerializeField, Header("�������x")] float objSpeed;
    [SerializeField, Header("�ړ����x�@�㏸��")] float addSpeed;
    [SerializeField, Header("�ړ����x�@�ő�l")] float maxSpeed;
    float _addSpeed; //���ۂɌv�Z�Ɏg���l

    void Awake()
    {
        //�����m��������
        _warpProb = warpProb;
        _feverProb = feverProb;

        //�ŏ��̏�Q���𐶐�
        ObstacleCreate();
    }

    void Update()
    {
        if (mainGameController.state == MainGameController.STATE.PLAY)
        {
            CreateProbability();
        }
    }

    /// <summary>
    /// ��Q������
    /// </summary>
    public void ObstacleCreate()
    {
        int num = 0;

        //���[�h�ɂ���Đ�������ύX
        switch (mainGameController.mode)
        {
            case MainGameController.MODE.DAY:
                num = Random.Range(0, 3); //2�}�X�󂢂Ă�I�u�W�F�N�g�̂�
                break;

            case MainGameController.MODE.EVENIG:
                switch (lastObj) //���߂̃I�u�W�F�N�g�ƍő�㉺1�}�X�����
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

            case MainGameController.MODE.NIGHT:
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

        var obj = Instantiate(obstacle[num], createPos, Quaternion.identity, parent.transform);

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
        if (mainGameController.IsFever) { _feverProb = 0; _warpProb = 0; }
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
        if (num <= _warpProb)
        {
            obj = Instantiate(warpHole, pos, Quaternion.identity);
        }
        //�t�B�[�o�[�A�C�e������
        else if (num <= _warpProb + _feverProb)
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
    /// ���[�v�̏o������
    /// </summary>
    public void WarpExitCreate()
    {
        Vector3 pPos = player.GetComponent<Transform>().transform.position;
        Vector3 bPos = new Vector3(2, 0, 0); //�{�[�������ʒu
        var obj = Instantiate(warpExit, pPos + bPos, Quaternion.identity, parent.transform);
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
            if (isAdd)
            {
                om.Speed += speed; //���Z
            }
            else om.Speed = speed; //���
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g �ړ����x�㏸
    /// </summary>
    public void SpeedUp()
    {
        //��Q���̈ړ����x���グ��
        if (maxSpeed > _addSpeed + objSpeed) _addSpeed += addSpeed;
    }
}
