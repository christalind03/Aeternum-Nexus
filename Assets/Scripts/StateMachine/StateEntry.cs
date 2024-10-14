using System;
using UnityEngine;

[System.Serializable]
public struct StateEntry<EState> where EState : Enum
{
    public EState Key;
    public BaseState<EState> Value;
}
