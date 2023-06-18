using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ReloadAmmo : IExecutionMethod
    {
        public Status Method(ComputerParameter param, ComputerMove move)
        {
            param.UsingGun.DoReload();

            return Status.Success;
        }
    }
}
