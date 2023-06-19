using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    /// <summary>シーン名 : Title</summary>
    public const string _SCENE_NAME_TITLE = "Title";

    /// <summary>シーン名 : EasyStage</summary>
    public const string _SCENE_NAME_STAGE = "EasyLayer";

    /// <summary>シーン名 : Result</summary>
    public const string _SCENE_NAME_RESULT = "Result";



    [SerializeField, Tooltip("シーン変更時に遅延する時間")]
    float _delay = 2f;

    /// <summary>シーン変更時に遅延させるタイマー</summary>
    float _delayTimer = 0f;

    /// <summary>シーン変更の予約中のシーン名</summary>
    string _bookingChangeSceneName = null;

    /// <summary>ゲームを終了する</summary>
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>シーンを変更する</summary>
    /// <param name="sceneName">シーン名</param>
    public void ChangeScene(string sceneName)
    {
        if (_bookingChangeSceneName is not null) return;

        _bookingChangeSceneName = sceneName;
        _delayTimer = _delay;
    }



    void Update()
    {
        if(_bookingChangeSceneName is not null)
        {
            _delayTimer -= Time.unscaledDeltaTime;

            if (_delayTimer < 0f)
            {
                _delayTimer = 0f;
                string sceneName = _bookingChangeSceneName;
                _bookingChangeSceneName = null;
                GameManager.PauseMode(false);
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
        }
    }
}
