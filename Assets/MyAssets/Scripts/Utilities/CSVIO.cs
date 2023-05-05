using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// CSV�t�@�C�� ���[�_�[ ���[�_�[
/// </summary>
public static class CSVIO
{
    /// <summary> �g�p���镶���R�[�h </summary>
    const string _ENCODER = "UTF-8";

    /// <summary> ���s�R�[�h </summary>
    public const string _LINE_CODE = "END_ROW";

    /// <summary>���X�g�擪��CSV�t�@�C�����̃t�@�C���ɁA�ȍ~�̃��X�g�����o����������</summary>
    /// <param name="datam"> �擪�̂ݏ������ݐ��CSV�t�@�C�����ŁA�ȍ~�̓����o�ϐ��̃��X�g�^ </param>
    public static void SaveCSV(List<string> datam)
    {
        //TODO
        try
        {
            //�������݃I�u�W�F�N�g�쐬���������ݏ���
            using (StreamWriter writer = new StreamWriter(@datam[0], false, System.Text.Encoding.GetEncoding(_ENCODER)))
            {
                //datam�擪��CSV�t�@�C�����̂ݎ�菜��
                datam.RemoveAt(0);

                foreach (string data in datam)
                {
                    //�����������
                    writer.WriteLine(data);
                }
            }

            Debug.Log("�������݂ɐ������܂����B");
        }
        catch (System.Exception e)
        {
            Debug.LogError("�������݂Ɏ��s���܂����B\n" + e);
        }
    }

    /// <summary>�����̃t�@�C������CSV�t�@�C������A������̃��X�g�Ƃ��Ď󂯎��</summary>
    /// <param name="path"> �ǂ݂����t�@�C���� </param>
    /// <returns> �ǂ݂����������񃊃X�g�̃f�[�^ </returns>
    public static List<string[]> LoadCSV(string path)
    {
        //csv�t�@�C���p�ϐ�
        TextAsset csvFile = Resources.Load(path) as TextAsset;

        //�f�[�^�i�[�p������
        List<string[]> datam = new List<string[]>();

        try
        {
            //�ǂݍ��݃I�u�W�F�N�g�쐬���ǂݍ��ݏ���
            using (StringReader reader = new StringReader(csvFile.text))
            {
                //�t�@�C���I���܂œǂݏo��
                while (reader.Peek() != -1)
                {
                    //�ǂ݂����ăJ���}��؂�Ŕz��
                    string[] readed = reader.ReadLine().Split(',');

                    //���X�g�Ɋi�[
                    datam.Add(readed);
                }
            }

            if (datam.Count > 0) Debug.Log(@path + " ����̃f�[�^�̌Ăяo�����������܂����B\n" + datam);
            else Debug.LogWarning(@path + "����̃f�[�^��ǂ݂����܂���ł����B");
        }
        catch (System.Exception e)
        {
            Debug.LogError(path + "����̓ǂݍ��݂Ɏ��s���܂����B\n" + e);
        }

        return datam;
    }
}

/// <summary>CSV�p�̃f�[�^�Ɏ����f�[�^�𑊌ݕϊ�������@�\����������C���^�[�t�F�[�X</summary>
public interface ICSVDataConverter
{
    /// <summary> ���g�̃����o�ϐ���string�^��List�ɂ��ĕԂ� </summary>
    /// <returns> �擪�̂ݏ������ݐ��CSV�t�@�C�����ŁA�ȍ~�̓����o�ϐ��̃��X�g�^ </returns>
    public List<string> MembersToCSV();

    /// <summary> �n���ꂽ���X�g���烁���o�ϐ��֓������� </summary>
    /// <param name="csv"> CSV�t�@�C������n���ꂽ�ł��낤�f�[�^�Q </param>
    public void CSVToMembers(List<string[]> csv);
}
