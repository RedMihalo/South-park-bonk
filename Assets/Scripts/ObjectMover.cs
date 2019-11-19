using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectMover : MonoBehaviour
{

    private Vector3 currentTarget;
    private bool bTargetReached = true;
    public float AcceptanceRadius = 1.0f;
    public float Speed = 1.0f;
    public float WaveTime = 0.5f;
    public GameObject BonesSet;

    public Tile CurrentTile = null;

    public CharacterController CharacterController;

    public Animator Animator;

    public UnityEvent OnDestinationReached = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        // ProcessTouch();

        if(!bTargetReached)
            MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if(Vector3.Distance(currentTarget, transform.position) <= AcceptanceRadius)
        {
            bTargetReached = true;
            Animator.SetBool("IsWalking", false);
            Animator.SetBool("IsWaving", true);
            OnDestinationReached.Invoke();
            return;
        }

        Vector3 positionDelta = currentTarget - transform.position;
        positionDelta.Normalize();
        positionDelta *= Time.deltaTime * Speed;

        // CharacterController.Move(positionDelta);
        transform.Translate(positionDelta);
    }

    private void StopWaving()
    {
        Animator.SetBool("IsWaving", false);
    }

    private void ProcessTouch()
    {
        if(Input.touches.Length < 1)
            return;

        Vector2 touchPosition = Input.GetTouch(0).position;
        Camera currentCamera = Camera.main;
        if(!currentCamera)
        {
            Debug.LogError("LOL");
            return;
        }
        Vector3 worldSpacePosition = currentCamera.ScreenToWorldPoint(touchPosition);

        worldSpacePosition.z = transform.position.z;

        SetCurrentTarget(worldSpacePosition);
    }

    public void SetCurrentTarget(Vector3 newTarget)
    {
        transform.Translate(0, 0.2f, 0);
        currentTarget = newTarget;
        float distance = Vector3.Distance(transform.position, newTarget);
        bTargetReached = distance <= AcceptanceRadius;
        if(bTargetReached)
            return;

        FacePosition(newTarget);
        Animator.SetBool("IsWaving", false);

        Animator.SetBool("IsWalking", true);
    }

    public void FaceObject(GameObject target)
    {
        FacePosition(target.transform.position);
    }

    public void FacePosition(Vector3 position)
    {
        BonesSet.transform.rotation = Quaternion.Euler(0, position.x > transform.position.x ? 180 : 0, 0);
    }
}
