using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainGameController : MonoBehaviour
{
    [SerializeField] GameObject player;
    ObjectController objectController;
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

    //��s����
    [SerializeField] Text distance;
    float dis;

    //�Q�[���I�[�o�[
    [SerializeField] SceneChange sceneChange;
    [SerializeField, Header("���U���g�֑J�ڂ���܂ł̎���")] float toResultWait;

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
        dis += Time.deltaTime * 10; //!�@�I�u�W�F�N�g�ړ����x�ɉ����Đ��l�𑫂�
        
        //UI�̕\���E��\���͈�ɂ܂Ƃ߂�
        uiDisp.TextPutIn(distance, dis.ToString("f0") + "m");
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

    /// <summary>
    /// ���[�v���o
    /// </summary>
    /// <returns></returns>
    IEnumerator WarpEnd()
    {
        yield return new WaitForSeconds(0.5f);

        SpriteRenderer pSR = player.GetComponent<SpriteRenderer>();
        pSR.enabled = false; //�v���C���[��\��

        yield return new WaitUntil(() => !warpFade.IsWarpFade); //���o���I�������

        pSR.enabled = true; //�v���C���[�\��

        //���[�v�z�[���̏o���𐶐�
        objectController.WarpExitCreate();

        //�����𑫂�
        dis += warpDis;

        yield return new WaitForSeconds(0.5f);

        isWarp = false; //���[�v�I��
    }

    public void GameOver()
    {
        state = STATE.GAMEOVER;
        StartCoroutine(GameEnd());
    }

    IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(toResultWait);

        //��s�����ۑ�
        PlayerPrefs.SetFloat("Distance", dis);

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
