using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField, Tooltip("true : マウスカーソルを表示する")]
    bool _isUsePointer = true;

    void Start()
    {
        Cursor.visible = _isUsePointer;
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
