using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    MainGameController mainGameController;

    [SerializeField] float speed;
    public float Speed { get { return speed; }  set { speed = value; } }

    [SerializeField, Header("�����ʒu")] float destroyPosX;

    void Start()
    {
        if (GameObject.FindObjectOfType<MainGameController>() is MainGameController mg)
        {
            mainGameController = mg;
        }
    }

    void Update()
    {
        if (mainGameController.state == MainGameController.STATE.PLAY)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        //����ʒu�܂ŗ�����I�u�W�F�N�g�폜
        if (destroyPosX > transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
