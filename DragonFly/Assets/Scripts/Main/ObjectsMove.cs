using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �I�u�W�F�N�g�̈ړ�
/// </summary>
public class ObjectsMove : MonoBehaviour
{
    MainGameController mainGameController;

    [SerializeField] float speed = 0;
    /// <summary>
    /// �ړ����x
    /// </summary>
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    const float destroyPosX = -10;

    float ratio = 1;
    /// <summary>
    /// ���x�㏸�{��
    /// </summary>
    public float Ratio
    {
        get { return ratio; }
        set { ratio = value; }
    }

    [SerializeField] int num = -1; //�Ǘ��ԍ�
    public int Num { get { return num; } }

    void Start()
    {
        if (GameObject.FindObjectOfType<MainGameController>() is MainGameController mg)
        {
            mainGameController = mg;
        }
    }

    void FixedUpdate()
    {
        if (mainGameController.state == MainGameController.STATE.PLAY)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime * ratio);
        }

        //����ʒu�܂ŗ�����I�u�W�F�N�g��\���@�I�u�W�F�N�g�v�[���ɕԋp
        if (destroyPosX > transform.position.x)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}