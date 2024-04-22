using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] MainGameController mainGameController;
    [SerializeField] FeverController feverController;
    [SerializeField] SoundController sound;
    PlayerInput playerInput;

    [SerializeField] float speed;

    //�ړ��ł���͈͂̏��/�����܂ňړ����Ă��邩�ǂ���
    bool isMax = false, isMin = false;

    [Header("�Q�[���I�[�o�[���o")]
    [SerializeField, Header("�h��̑��x")] float cycleSpeed = 10;
    [SerializeField, Header("�h�ꕝ")] float amplitude = 0.001f;

    SpriteRenderer sr;
    Collider2D col;

    // �L����
    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
    }

    void Start()
    {
        if(GetComponent<SpriteRenderer>() is var s)
        {
            sr = s;
        }

        if(GetComponent<Collider2D>() is var c)
        {
            col = c;
        }
    }

    void Update()
    {
        switch(mainGameController.state)
        {
            case MainGameController.STATE.WAIT:
                break;

            case MainGameController.STATE.PLAY:
                if(!mainGameController.IsWarp) //���[�v���͓������Ȃ�
                {
                    Move();
                }
                break;

            case MainGameController.STATE.GAMEOVER:
                Gameover();
                break;
        }

        //���[�v������Ȃ��Ƃ�
        if(!mainGameController.IsWarp)
        {
            col.enabled = true; //�����蔻��
            sr.maskInteraction = SpriteMaskInteraction.None; //�}�X�N����
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

        // �w��L�[��������Ă���ԁ@���@�ړ��������Ȃ�������
        if (playerInput.Player.Up.IsPressed() && !isMax)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

        if (playerInput.Player.Down.IsPressed() && !isMin)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }

    void Gameover()
    {
        float x = Mathf.Sin(Time.time * cycleSpeed) * amplitude;
        this.transform.position -= new Vector3(x, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��Q���ɂԂ������Ƃ��@���@���G��Ԃ���Ȃ��Ƃ�
        if(collision.gameObject.CompareTag("Obstacle") && !mainGameController.IsInvincible && 
            mainGameController.state != MainGameController.STATE.GAMEOVER)
        {
            mainGameController.GameOver();
        }

        //�t�B�[�o�[�^�C���˓��A�C�e��
        if(collision.gameObject.CompareTag("Ball"))
        {
            feverController.Ball++;
            sound.ItemCatch(); // SE
            Destroy(collision.gameObject); //�A�C�e��������
        }

        //���[�v�z�[���ɐG�ꂽ�Ƃ�
        if(collision.gameObject.CompareTag("WarpHole"))
        {
            sr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask; //�}�X�N����
            col.enabled = false; //�����蔻�������

            mainGameController.Warp();
            sound.Warp(); // SE
        }
    }
}
