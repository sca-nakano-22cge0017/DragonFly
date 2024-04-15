using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] MainGameController mainGameController;

    [SerializeField] float speed;

    //�ړ��ł���͈͂̏��/�����܂ňړ����Ă��邩�ǂ���
    bool isMax = false, isMin = false;

    //�_���[�W�A���G
    bool isDamage = false, isInvincible = false;

    [Header("�Q�[���I�[�o�[���o")]
    [SerializeField, Header("�h��̑��x")] float cycleSpeed = 10;
    [SerializeField, Header("�h�ꕝ")] float amplitude = 0.001f;

    void Start()
    {

    }

    void Update()
    {
        switch(mainGameController.state)
        {
            case MainGameController.STATE.WAIT:
                break;

            case MainGameController.STATE.PLAY:
                Move();
                Damage();
                break;

            case MainGameController.STATE.GAMEOVER:
                Gameover();
                break;
        }
    }

    /// <summary>
    /// �v���C���[�ړ�
    /// </summary>
    void Move()
    {
        if(transform.position.y >= 2) isMax = true;
        else isMax = false;

        if(transform.position.y <= -4) isMin = true;
        else isMin = false;

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !isMax)
        {
            transform.Translate(Vector3.up * speed);
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !isMin)
        {
            transform.Translate(Vector3.down * speed);
        }
    }

    //�_���[�W����
    void Damage()
    {
        if(isDamage)
        {
            isDamage = false;
            mainGameController.HP--;
            StartCoroutine(DamageDirection());
        }
    }

    Color nomalColor = new Color(1f, 1f, 1f, 1f);
    Color damageColor = new Color(1f, 1f, 1f, 0.7f);
    [SerializeField, Header("��_�����̖��ő��x")] float damageSpeed;

    //�_���[�W���o
    IEnumerator DamageDirection()
    {
        //�_��
        for (int i = 0; i < 3; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().color = damageColor;
            yield return new WaitForSeconds(damageSpeed);
            gameObject.GetComponent<SpriteRenderer>().color = nomalColor;
            yield return new WaitForSeconds(damageSpeed);
        }

        //���G�I��
        isInvincible = false;
    }

    void Gameover()
    {
        float x = Mathf.Sin(Time.time * cycleSpeed) * amplitude;
        this.transform.position -= new Vector3(x, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��Q���ɂԂ������Ƃ��@���@���G��Ԃ���Ȃ��Ƃ�
        if(collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            isDamage = true;
            isInvincible = true;
        }

        //�񕜃A�C�e���ɐG�ꂽ�Ƃ�
        if(collision.gameObject.CompareTag("Heal"))
        {
            mainGameController.HP++;

            if (collision.gameObject.GetComponent<SpriteRenderer>() is SpriteRenderer sr)
            {
                sr.enabled = false; //�A�C�e�����\���ɂ���
            }
        }

        //���[�v�z�[���ɐG�ꂽ�Ƃ�
        if(collision.gameObject.CompareTag("WarpHole"))
        {
            mainGameController.Warp();
        }
    }
}
