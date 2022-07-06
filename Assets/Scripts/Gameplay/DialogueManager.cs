using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject dialogueBox;
    [SerializeField] Text dialogueText;
    [SerializeField] int lettersPerSecond;

    public event Action onShowDialog;
    public event Action OnDialogFinished;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool IsShowing { get; private set; }

    public IEnumerator ShowDialogText(string text, bool waitForInput=true, bool autoClose=true)
    {
        onShowDialog?.Invoke();
        IsShowing = true;
        dialogueBox.SetActive(true);

        yield return TypeDialog(text);
        if (waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        if (autoClose)
        {
            CloseDialog();
        }

        OnDialogFinished?.Invoke();
    }

    public void CloseDialog()
    {
        dialogueBox.SetActive(false);
        IsShowing = false;       
    }

    public IEnumerator ShowDialog(Dialogue dialog)
    {
        yield return new WaitForEndOfFrame();

        onShowDialog?.Invoke();
        IsShowing = true;
        dialogueBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        dialogueBox.SetActive(false);
        IsShowing = false;
        OnDialogFinished?.Invoke();
    }

    public void HandleUpdate()
    {
        
    }

    public IEnumerator TypeDialog(string line)
    {
        dialogueText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
