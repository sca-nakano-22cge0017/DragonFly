using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�̈ړ�
/// </summary>
public class ObjectsMove : MonoBehaviour
{
    MainGameController mainGameController;

    float defaultSpeed = 5;
    float speed = 0;
    float destroyPosX = -10;

    float ratio = 1;

    /// <summary>
    /// �ړ����x
    /// </summary>
    public float Speed
    {
        get { return defaultSpeed; }
        set { defaultSpeed = value;}
    }

    /// <summary>
    /// ���x�㏸�{��
    /// </summary>
    public float Ratio
    {
        get { return ratio; }
        set { ratio = value; }
    }

    void Start()
    {
        if (GameObject.FindObjectOfType<MainGameController>() is MainGameController mg)
        {
            mainGameController = mg;
        }

        speed = defaultSpeed;
    }

    void FixedUpdate()
    {
        if (mainGameController.state == MainGameController.STATE.PLAY)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime * ratio);
        }

        //����ʒu�܂ŗ�����I�u�W�F�N�g�폜
        if (destroyPosX > transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
