using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Q���̈ړ��E����
/// </summary>
public class ObstacleController : MonoBehaviour
{
    MainGameController mainGameController;

    [SerializeField, Header("��Q���ړ����x")] float obstacleSpeed;
    [SerializeField, Header("���̏�Q���������C��")] float createPosX;
    [SerializeField, Header("�����ʒu")] float destroyPosX;

    bool canCreate = true;
    //���̏�Q�������̃t���O ����ʒu�܂ŗ����玟�̏�Q���𐶐�����

    public float ObstacleSpeed
    {
        get { return obstacleSpeed; }
        set { obstacleSpeed = value; }
    }

    void Start()
    {
        mainGameController = GameObject.FindObjectOfType<MainGameController>();
    }

    void FixedUpdate()
    {
        if (mainGameController.state == MainGameController.STATE.PLAY)
        {
            transform.Translate(Vector3.left * obstacleSpeed * Time.deltaTime);
        }

        //����ʒu�܂ŗ������x�������̏�Q���𐶐�����
        if(createPosX > transform.position.x && canCreate)
        {
            canCreate = false;
            mainGameController.ObstacleCreate();
        }

        //����ʒu�܂ŗ�����I�u�W�F�N�g�폜
        if(destroyPosX > transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
