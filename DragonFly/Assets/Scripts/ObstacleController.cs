using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Q���̈ړ��E����
/// </summary>
public class ObstacleController : MonoBehaviour
{
    MainGameController mainGameController;

    [SerializeField, Header("���̏�Q���������C��")] float createPosX;

    bool canCreate = true;
    //���̏�Q�������̃t���O ����ʒu�܂ŗ����玟�̏�Q���𐶐�����

    void Start()
    {
        if (GameObject.FindObjectOfType<MainGameController>() is MainGameController mg)
        {
            mainGameController = mg;
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
            mainGameController.ObstacleCreate();
            mainGameController.ItemCreate(); //�ꏏ�ɃA�C�e���̐�������
        }
    }
}
