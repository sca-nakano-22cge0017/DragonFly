using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// �I�u�W�F�N�g�̐����E���x�ύX
/// </summary>
public class ObjectController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] MainGameController mainGameController;

    [Header("��Q������")]
    [SerializeField, Header("�����ʒu")] Vector3 createPos = new Vector3(10, 0, 0);
    [SerializeField, Header("��Q��")] GameObject[] obstacle;
    int lastObj = 3; // ���߂ɐ���������Q��
    [SerializeField, Header("��Q�������ꏊ")] Transform obstacleParent;

    [Header("�A�C�e������")]
    [SerializeField, Header("�A�C�e�������ʒu�@X")] int itemPosX;
    [SerializeField, Header("�A�C�e�������ʒu�@Y")] int[] itemPosY;

    [SerializeField, Header("���[�v�z�[����Prefab")] GameObject warpHole;
    [SerializeField, Header("�t�B�[�o�[�A�C�e����Prefab")] GameObject feverItem;

    [SerializeField, Header("���[�v�z�[���@�����m��")] float warpProb;
    float _warpProb = 0; // �v�Z�p
    [SerializeField, Header("�t�B�[�o�[�A�C�e���@�����m��")] float feverProb;
    float _feverProb = 0; // �v�Z�p

    [SerializeField, Header("���[�v�z�[�������ꏊ")] Transform warpParent;
    [SerializeField, Header("�t�B�[�o�[�A�C�e�������ꏊ")] Transform feverParent;

    [Header("���[�v")]
    [SerializeField, Header("���[�v��̈ʒu")] Vector3 warpAjustPos;
    [SerializeField, Header("���[�v�o��")] GameObject warpExit;
    [SerializeField, Header("���[�v�z�[���o�������ꏊ")] Transform warpExitParent;

    [SerializeField, Header("�������x")] float objSpeed;
    [SerializeField, Header("�ړ����x�@�㏸��")] float addSpeed;
    [SerializeField, Header("�ړ����x�@�ő�l")] float maxSpeed;
    float _addSpeed; // ���ۂɌv�Z�Ɏg���l

    [SerializeField, Header("�e�I�u�W�F�N�g�̐����ꏊ")] Transform[] parents; // �S�Ă̐e�I�u�W�F�N�g

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
            //�����m������
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

        //����
        InstObstacle(obstacle[num], obstacleParent, num);
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

        //���[�v�z�[������
        if (num <= _warpProb)
        {
            InstItem(warpHole, pos, warpParent);
        }
        //�t�B�[�o�[�A�C�e������
        else if (num <= _warpProb + _feverProb)
        {
            InstItem(feverItem, pos, feverParent);
        }
    }

    /// <summary>
    /// ���[�v�̏o������
    /// </summary>
    public void WarpExitCreate()
    {
        Vector3 pPos = player.GetComponent<Transform>().transform.position;
        Vector3 ePos = new Vector3(2, 0, 0); //�z�[�������ʒu

        InstItem(warpExit, pPos + ePos, warpExitParent);
    }

    /// <summary>
    /// �A�C�e�������E�v�[��������o��
    /// </summary>
    /// <param name="target">�����I�u�W�F�N�g</param>
    /// <param name="pos">�����ʒu</param>
    /// <param name="parent">�e�I�u�W�F�N�g</param>
    void InstItem(GameObject target, Vector3 pos, Transform parent)
    {
        foreach(Transform t in parent)
        {
            if(!t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(true); // �\��
                t.gameObject.transform.position = pos; // �ʒu�ύX

                t.GetComponent<ObjectsMove>().Speed = objSpeed + _addSpeed; // �ړ����x�ύX

                return;
            }
        }

        var obj = Instantiate(target, pos, Quaternion.identity, parent);
        obj.GetComponent<ObjectsMove>().Speed = objSpeed + _addSpeed; // �ړ����x�ύX
        return;
    }

    void InstObstacle(GameObject target, Transform parent, int num)
    {
        foreach (Transform t in parent)
        {
            var om = t.GetComponent<ObjectsMove>();

            if(num != om.Num) // �Ώۂ̃I�u�W�F�N�g����Ȃ���Ύ��̃I�u�W�F�N�g�ɍs��
            {
                continue;
            }

            if (!t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(true); // �\��
                t.gameObject.transform.position = createPos; // �ʒu�ύX

                om.Speed = objSpeed + _addSpeed; // �ړ����x�ύX

                return;
            }
        }

        var obj = Instantiate(target, createPos, Quaternion.identity, parent);
        obj.GetComponent<ObjectsMove>().Speed = objSpeed + _addSpeed;
        return;
    }

    /// <summary>
    /// �I�u�W�F�N�g �ړ����x�㏸
    /// </summary>
    public void SpeedUp()
    {
        //��Q���̈ړ����x���グ��
        if (maxSpeed > _addSpeed + objSpeed) _addSpeed += addSpeed;
    }

    /// <summary>
    /// �I�u�W�F�N�g��S�ăv�[���ɕԋp
    /// </summary>
    public void AllRelease()
    {
        foreach(var parent in parents)
        {
            foreach(Transform p in parent)
            {
                if(!p.gameObject.activeSelf)
                {
                    continue;
                }

                p.gameObject.SetActive(false); // ������
            }
        }
    }
}
