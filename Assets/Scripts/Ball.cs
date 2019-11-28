using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ball : MonoBehaviour
{
    public ObjectMover mover;
    public UnityEvent OnTargetReached = new UnityEvent();

    private void Start()
    {
        mover.OnDestinationReached.AddListener(() => OnTargetReached.Invoke());
    }

    public void Launch(GameObject target)
    {
        mover.SetCurrentTarget(target.transform.position);
    }
}
