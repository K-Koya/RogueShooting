using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameter : CharacterParameter
{


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _Allies.Add(this);
    }

    private void OnDestroy()
    {
        _Allies.Remove(this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        //É|Å[ÉYéûÇÕé~ÇﬂÇÈ
        if (GameManager.IsPose)
        {
            return;
        }

        base.Update();
    }
}
