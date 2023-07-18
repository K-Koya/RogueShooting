using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SlowMode(false);
        GameManager.Instance.PauseMode(false);
        GameManager.Instance.CursorMode(true);

        BGMManager.Instance.BGMCallResultOnClear();
    }
}
