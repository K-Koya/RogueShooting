using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerParameter : CharacterParameter, IReticleFocused
{
    [SerializeField, Tooltip("やられた動きをするラグドールのプレハブ")]
    GameObject _defeatedRagdollPref = null;

    public ReticleFocusedData GetData()
    {
        ReticleFocusedData data = new ReticleFocusedData();
        data._maxLife = _maxLife;
        data._currentLife = _currentLife;
        data._name = _character.ToString();

        return data;
    }

    public override void GaveDamage(short damage, float impact, Vector3 impactDirection)
    {
        base.GaveDamage(damage, impact, impactDirection);

        if (_state.Kind is MotionState.StateKind.Defeat)
        {
            GameObject ragdoll = Instantiate(_defeatedRagdollPref);
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;
            ragdoll.GetComponent<DefeatedRagdoll>().BlowAway(impactDirection * impact);

            Destroy(gameObject, 0.1f);
        }
    }    
}
