using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// CSVファイル リーダー ローダー
/// </summary>
public static class CSVIO
{
    /// <summary> 使用する文字コード </summary>
    const string _ENCODER = "UTF-8";

    /// <summary> 改行コード </summary>
    public const string _LINE_CODE = "END_ROW";

    /// <summary>リスト先頭のCSVファイル名のファイルに、以降のリストメンバを書きこむ</summary>
    /// <param name="datam"> 先頭のみ書き込み先のCSVファイル名で、以降はメンバ変数のリスト型 </param>
    public static void SaveCSV(List<string> datam)
    {
        //TODO
        try
        {
            //書き込みオブジェクト作成し書き込み処理
            using (StreamWriter writer = new StreamWriter(@datam[0], false, System.Text.Encoding.GetEncoding(_ENCODER)))
            {
                //datam先頭のCSVファイル名のみ取り除く
                datam.RemoveAt(0);

                foreach (string data in datam)
                {
                    //一つずつ書き込み
                    writer.WriteLine(data);
                }
            }

            Debug.Log("書き込みに成功しました。");
        }
        catch (System.Exception e)
        {
            Debug.LogError("書き込みに失敗しました。\n" + e);
        }
    }

    /// <summary>引数のファイル名のCSVファイルから、文字列のリストとして受け取る</summary>
    /// <param name="path"> 読みだすファイル名 </param>
    /// <returns> 読みだした文字列リストのデータ </returns>
    public static List<string[]> LoadCSV(string path)
    {
        //csvファイル用変数
        TextAsset csvFile = Resources.Load(path) as TextAsset;

        //データ格納用文字列
        List<string[]> datam = new List<string[]>();

        try
        {
            //読み込みオブジェクト作成し読み込み処理
            using (StringReader reader = new StringReader(csvFile.text))
            {
                //ファイル終わりまで読み出し
                while (reader.Peek() != -1)
                {
                    //読みだしてカンマ区切りで配列化
                    string[] readed = reader.ReadLine().Split(',');

                    //リストに格納
                    datam.Add(readed);
                }
            }

            if (datam.Count > 0) Debug.Log(@path + " からのデータの呼び出しを完了しました。\n" + datam);
            else Debug.LogWarning(@path + "からのデータを読みだせませんでした。");
        }
        catch (System.Exception e)
        {
            Debug.LogError(path + "からの読み込みに失敗しました。\n" + e);
        }

        return datam;
    }
}

/// <summary>CSV用のデータに持ちデータを相互変換させる機能を持たせるインターフェース</summary>
public interface ICSVDataConverter
{
    /// <summary> 自身のメンバ変数をstring型のListにして返す </summary>
    /// <returns> 先頭のみ書き込み先のCSVファイル名で、以降はメンバ変数のリスト型 </returns>
    public List<string> MembersToCSV();

    /// <summary> 渡されたリストからメンバ変数へ投げ込む </summary>
    /// <param name="csv"> CSVファイルから渡されたであろうデータ群 </param>
    public void CSVToMembers(List<string[]> csv);
}
