using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{
    /// <summary>�V�[���� : Title</summary>
    public const string _SCENE_NAME_TITLE = "Title";

    /// <summary>�V�[���� : EasyStage</summary>
    public const string _SCENE_NAME_STAGE = "EasyLayer";

    /// <summary>�V�[���� : Result</summary>
    public const string _SCENE_NAME_RESULT = "Result";



    [SerializeField, Tooltip("�V�[���ύX���ɒx�����鎞��")]
    float _delay = 2f;

    /// <summary>�V�[���ύX���ɒx��������^�C�}�[</summary>
    float _delayTimer = 0f;

    /// <summary>�V�[���ύX�̗\�񒆂̃V�[����</summary>
    string _bookingChangeSceneName = null;

    /// <summary>�Q�[�����I������</summary>
    public void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    /// <summary>�V�[����ύX����</summary>
    /// <param name="sceneName">�V�[����</param>
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
