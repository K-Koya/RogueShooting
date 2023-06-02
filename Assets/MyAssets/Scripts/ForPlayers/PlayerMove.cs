using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        ShotProcess();
    }

    void ShotProcess()
    {
        if (InputUtility.GetFire)
        {
            _param.UsingGun.DoShot();
        }

        if (InputUtility.GetDownReload)
        {
            _param.UsingGun.DoReload();
        }
    }
}
