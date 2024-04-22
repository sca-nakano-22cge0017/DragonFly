using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerSet : MonoBehaviour
{
    [SerializeField] GameObject controllers;

    void Start()
    {
        controllers.SetActive(false);

        // �A���h���C�h�̏ꍇ�̓R���g���[���[��\������
        #if UNITY_ANDROID
            controllers.SetActive(true);
        #endif
    }
}
