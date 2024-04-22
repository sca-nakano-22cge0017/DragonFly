using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] SoundController sound;
    ObjectController objectController;
    FeverController feverController;
    UIDisp uiDisp;

    public enum STATE { WAIT = 0, PLAY, GAMEOVER, }
    public STATE state = 0;

    public enum MODE { DAY = 0, EVENIG, NIGHT }
    public MODE mode = 0;

    [Header("���[�v")]
    [SerializeField, Header("���[�v���̈ړ���")] float warpDis;
    [SerializeField, Header("���[�v���̉~�`�t�F�[�h")] CircleFade warpFade;
    bool isWarp =false;
    public bool IsWarp { get { return isWarp; } set { isWarp = value; } }

    bool isFever = false;
    public bool IsFever { get { return isFever; } set { isFever = value; } }

    bool isInvincible = false;
    public bool IsInvincible { get { return isInvincible; } set { isInvincible = value; } }

    //��s����
    [SerializeField] Text distance;
    float dis;

    //�Q�[���I�[�o�[
    [SerializeField] SceneChange sceneChange;
    [SerializeField, Header("���U���g�֑J�ڂ���܂ł̎���")] float toResultWait;

    [SerializeField] DataSaver dataSaver;

    private void Start()
    {
        ScriptsSet();

        //�t�F�[�h�C��
        sceneChange.FadeIn();
        StartCoroutine(FadeEndCheck());
    }

    /// <summary>
    /// ���X�N���v�g�̎擾�E�m�F
    /// </summary>
    void ScriptsSet()
    {
        if (GetComponent<ObjectController>() is var oc)
        {
            objectController = oc;
        }

        if (GetComponent<UIDisp>() is var ud)
        {
            uiDisp = ud;
        }

        if (GetComponent<FeverController>() is var fc)
        {
            feverController = fc;
        }
    }

    void Update()
    {
        switch (state)
        {
            case STATE.WAIT:
                break;

            case STATE.PLAY:
                FlyDis();
                break;

            case STATE.GAMEOVER:
                break;
        }
    }

    /// <summary>
    /// ��s�����Ǘ�
    /// </summary>
    void FlyDis()
    {
        dis += Time.deltaTime * feverController.Fever * objectController.Speed * 2;
        
        //UI�̕\���E��\���͈�ɂ܂Ƃ߂�
        uiDisp.TextPutIn(distance, Mathf.Floor(dis).ToString() + "m");
    }

    /// <summary>
    /// ���[�v�z�[���ɐG�ꂽ�Ƃ��̏���
    /// </summary>
    public void Warp()
    {
        warpFade.IsWarpFade = true; //���[�v���o
        isWarp = true;

        StartCoroutine(WarpEnd());
    }

    const float spriteFalseTime = 0.3f;
    const float warpEndTime = 0.5f;

    /// <summary>
    /// ���[�v���o
    /// </summary>
    /// <returns></returns>
    IEnumerator WarpEnd()
    {
        yield return new WaitForSeconds(spriteFalseTime);

        SpriteRenderer pSR = player.GetComponent<SpriteRenderer>();
        pSR.enabled = false; //�v���C���[��\��

        objectController.AllRelease(); // �I�u�W�F�N�g�S�ԋp

        yield return new WaitUntil(() => !warpFade.IsWarpFade); //���o���I�������

        pSR.enabled = true; //�v���C���[�\��

        //�����𑫂�
        dis += warpDis;

        //���[�v�z�[���̏o���𐶐�
        objectController.WarpExitCreate();

        yield return new WaitForSeconds(warpEndTime);

        objectController.ObstacleCreate(); // �Đ���

        isWarp = false; //���[�v�I��
    }

    public void GameOver()
    {
        state = STATE.GAMEOVER;
        
        if(GameObject.FindObjectOfType<BGM>().GetComponent<BGM>() is var bgm)
        {
            bgm.BGMStop(); // BGM��~
        }

        sound.GameOver(); // SE

        StartCoroutine(GameEnd());
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(toResultWait);

        //��s�����ۑ�
        dataSaver.saveLatestData(dis);

        //�V�[���J��
        sceneChange.ToResult();
    }

    /// <summary>
    /// �t�F�[�h�C���������������`�F�b�N����
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeEndCheck()
    {
        yield return new WaitUntil(() => sceneChange.IsFadeInEnd);
        state = STATE.PLAY;
    }
}
