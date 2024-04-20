using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//! �f�B���N�g���E�t�@�C���̑��݊m�F���܂Ƃ߂�

public class DataSaver : MonoBehaviour
{
    string rankingDataPath = "/Data/RankingData.csv";
    string latestDataPath = "/Data/LatestData.csv";

    /// <summary>
    /// �ŐV�X�R�A�@�Z�[�u
    /// </summary>
    public void saveLatestData(float score)
    {
        string directoryName = Application.persistentDataPath + "/Data";
        string fileName = Application.persistentDataPath + latestDataPath;

        StreamWriter writer;

        while (!Directory.Exists(directoryName)) //�f�B���N�g�����Ȃ�������
        {
            Directory.CreateDirectory(directoryName); //�f�B���N�g�����쐬
        }
        
        while (!File.Exists(fileName)) // �t�@�C�����Ȃ�������
        {
            FileStream fs = File.Create(fileName); // �t�@�C�����쐬
            fs.Close(); // �t�@�C�������
        }

        string s = Mathf.Floor(score).ToString();

        writer = new StreamWriter(fileName, false); // �㏑��
        writer.WriteLine(s);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// �ŐV�X�R�A ���[�h
    /// </summary>
    /// <returns></returns>
    public float loadLatestData()
    {
        string directoryName = Application.persistentDataPath + "/Data";
        string fileName = Application.persistentDataPath + latestDataPath;
        string dataStr = "";
        float score = 0;

        while (!Directory.Exists(directoryName)) //�f�B���N�g�����Ȃ�������
        {
            Directory.CreateDirectory(directoryName); //�f�B���N�g�����쐬
        }

        // �t�@�C�����Ȃ�������
        while (!File.Exists(fileName))
        {
            FileStream fs = File.Create(fileName); // �t�@�C�����쐬
            fs.Close(); // �t�@�C�������

            // ���f�[�^�ۑ�
            saveLatestData(0);
        }

        StreamReader reader = new StreamReader(fileName);
        dataStr = reader.ReadToEnd(); // �ǂݍ���
        reader.Close();

        // string��float���m�F���đ��
        if (float.TryParse(dataStr, out float s))
        {
            score = s;
        }

        return score;
    }

    /// <summary>
    /// �����L���O�f�[�^ �Z�[�u
    /// </summary>
    /// <param name="score">�Z�[�u����float�^�̔z��</param>
    public void saveRankingData(float[] score)
    {
        StreamWriter writer;
        string directoryName = Application.persistentDataPath + "/Data";
        string fileName = Application.persistentDataPath + rankingDataPath;

        string[] s = new string[score.Length];

        while (!Directory.Exists(directoryName)) //�f�B���N�g�����Ȃ�������
        {
            Directory.CreateDirectory(directoryName); //�f�B���N�g�����쐬
        }

        // �t�@�C�����Ȃ�������
        while (!File.Exists(fileName))
        {
            FileStream fs = File.Create(fileName); // �t�@�C�����쐬
            fs.Close(); // �t�@�C�������
        }

        //�X�R�A�}��
        for (int i = 0; i < score.Length; i++)
        {
            s[i] = Mathf.Floor(score[i]).ToString();
        }

        //�������","�ŋ�؂�
        string s2 = string.Join(",", s);

        writer = new StreamWriter(fileName, false); // �㏑��
        writer.WriteLine(s2);
        writer.Flush();
        writer.Close();
    }

    /// <summary>
    /// �����L���O�f�[�^ ���[�h
    /// </summary>
    /// <param name="length">�f�[�^���̔z��</param>
    /// <returns></returns>
    public float[] loadRankingData(int length)
    {
        string directoryName = Application.persistentDataPath + "/Data";
        string fileName = Application.persistentDataPath + rankingDataPath;
        string dataStr = "";
        
        float[] score = new float[length];

        while (!Directory.Exists(directoryName)) //�f�B���N�g�����Ȃ�������
        {
            Directory.CreateDirectory(directoryName); //�f�B���N�g�����쐬
        }

        //�t�@�C�����Ȃ�������
        while (!File.Exists(fileName))
        {
            FileStream fs = File.Create(fileName); // �t�@�C�����쐬
            fs.Close(); // �t�@�C�������

            // ���f�[�^�ۑ�
            DataInitialize(length);
        }

        StreamReader reader = new StreamReader(fileName);
        dataStr = reader.ReadToEnd(); // �ǂݍ���
        reader.Close();

        score = DataTrans(dataStr, length);
        
        return score;
    }

    /// <summary>
    /// �e�L�X�g�f�[�^��float�̔z��ɕϊ�
    /// </summary>
    /// <param name="data">�Ώۂ̕�����</param>
    /// <param name="length">�f�[�^���̔z��</param>
    /// <returns></returns>
    float[] DataTrans(string data, int length)
    {
        float[] score = new float[length];

        //�R���}�ŋ�؂�
        var line = data.Split(",");

        // �f�[�^��z��ɓ����
        for (int i = 0; i < length; i++)
        {
            // string��float���m�F���đ��
            if(float.TryParse(line[i], out float s))
            {
                score[i] = s;
            }
        }

        return score;
    }

    /// <summary>
    /// �f�[�^�S����
    /// </summary>
    /// <param name="length">�f�[�^���̔z��</param>
    public void DataInitialize(int length)
    {
        float[] score = new float[length];

        // 0�ɂ���
        for(int i = 0; i < score.Length; i++)
        {
            score[i] = 0;
        }

        // �ۑ�
        saveRankingData(score);
        saveLatestData(0);
    }
}
