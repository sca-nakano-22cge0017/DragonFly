using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// ���[�v�A�C�e�� �I�u�W�F�N�g�v�[���Ǘ�
/// </summary>
public class WarpCreate : MonoBehaviour
{
    [SerializeField, Header("�����ꏊ")] Transform parent;
    [SerializeField] ObjectsMove warpObjects;

    private ObjectPool<ObjectsMove> pool;

    Vector2 pos;
    public Vector2 PosSet { set { pos = value; } }

    void Start()
    {
        pool = new ObjectPool<ObjectsMove>(
            OnCreatePlloedObject,
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject
            );
    }

    /// <summary>
    /// �Q�[���I�u�W�F�N�g���������̊֐�
    /// </summary>
    /// <returns></returns>
    public ObjectsMove OnCreatePlloedObject()
    {
        ObjectsMove gameObject = Instantiate(warpObjects, pos, Quaternion.identity, parent);
        return gameObject;
    }

    /// <summary>
    /// �I�u�W�F�N�g�v�[������Q�[���I�u�W�F�N�g���擾����֐�
    /// </summary>
    /// <returns></returns>
    public void OnGetFromPool(ObjectsMove target)
    {
        target.gameObject.SetActive(true);
    }

    /// <summary>
    /// �Q�[���I�u�W�F�N�g���I�u�W�F�N�g�v�[���ɕԋp����֐�
    /// </summary>
    /// <returns></returns>
    public void OnReleaseToPool(ObjectsMove target)
    {
        target.gameObject.SetActive(false);
    }

    /// <summary>
    /// �Q�[���I�u�W�F�N�g���폜����֐�
    /// </summary>
    /// <param name="target"></param>
    public void OnDestroyPooledObject(ObjectsMove target)
    {
        Destroy(target.gameObject);
    }
}