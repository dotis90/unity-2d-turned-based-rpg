using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueAction : CutsceneAction
{
    [SerializeField] Dialogue dialog;

    public override IEnumerator Play()
    {
        yield return DialogueManager.Instance.ShowDialog(dialog);
    }

}
