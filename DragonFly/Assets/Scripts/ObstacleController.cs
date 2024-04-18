using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Q���̈ړ��E����
/// </summary>
public class Obstacle : MonoBehaviour
{
    ObjectController objectController;

    [SerializeField, Header("���̏�Q���������C��")] float createPosX;

    bool canCreate = true;
    //���̏�Q�������̃t���O ����ʒu�܂ŗ����玟�̏�Q���𐶐�����

    void Start()
    {
        if (GameObject.FindObjectOfType<ObjectController>() is ObjectController oc)
        {
            objectController = oc;
        }
    }

    private void OnEnable()
    {
        canCreate = true;
    }

    void FixedUpdate()
    {
        //����ʒu�܂ŗ������x�������̏�Q���𐶐�����
        if(createPosX > transform.position.x && canCreate)
        {
            canCreate = false;
            objectController.ObstacleCreate();
            objectController.ItemCreate(); //�ꏏ�ɃA�C�e���̐�������
        }
    }
}
