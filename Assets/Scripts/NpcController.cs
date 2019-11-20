using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    public Button InteractButton;
    public GameObject InteractLocation;
    public float InteractRange;

    public Sprite DialogAvatar;
    public List<string> DialogLines = new List<string>();
    public string RandomDialogLine { get => DialogLines[Random.Range(0, DialogLines.Count)]; }

    // Start is called before the first frame update
    void Start()
    {
        InteractButton.onClick.AddListener(() => TryInteract());
    }

    private void TryInteract()
    {
        ObjectMover mover = FindObjectOfType<ObjectMover>();
        Debug.Log(Vector2.Distance(transform.position, mover.transform.position));
        if(Vector2.Distance(transform.position, mover.transform.position) > InteractRange)
            Debug.Log("Far away");
        else
        {
            DialogBoxController.DialogController.SetTalker(this);
        }

    }

    private void DisableMover(ObjectMover mover)
    {
        mover.enabled = false;
        StartCoroutine(ReenableMover(mover));
    }

    private IEnumerator ReenableMover(ObjectMover mover)
    {
        yield return new WaitForSeconds(0.2f);
        mover.enabled = true;
    }

}
