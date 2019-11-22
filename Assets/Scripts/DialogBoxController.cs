using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{
    public Image Background;
    public Image DialogAvatar;
    public Text DialogText;
    Coroutine closeRoutine;

    public static DialogBoxController DialogController;

    private void Start()
    {
        if(DialogController != null)
            Debug.LogError("Something went wrong, only one blahblah should be spawned");
        DialogController = this;
        HideDialog();
    }

    public void SetTalker(NpcController npc)
    {
        DialogAvatar.sprite = npc.DialogAvatar;
        DialogText.text = npc.RandomDialogLine;
        ShowDialog();
        if(closeRoutine != null)
        {
            StopCoroutine(closeRoutine);
            closeRoutine = null;
        }
        closeRoutine = StartCoroutine(HideDialogDelayed());
    }

    private void ShowDialog()
    {
        Background.enabled = true;
        DialogAvatar.enabled = true;
        DialogText.enabled = true;
    }

    private IEnumerator HideDialogDelayed()
    {
        yield return new WaitForSeconds(2);
        HideDialog();
    }

    private void HideDialog()
    {
        Background.enabled = false;
        DialogAvatar.enabled = false;
        DialogText.enabled = false;
    }
}
