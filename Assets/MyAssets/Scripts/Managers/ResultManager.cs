using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.SlowMode(false);
        GameManager.PauseMode(false);
        GameManager.CursorMode(true);

        BGMManager.Instance.BGMCallResultOnClear();
    }
}
