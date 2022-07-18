using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneAction
{
    [SerializeField] string _name;

    public virtual IEnumerator Play()
    {
        yield break;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }
}
