using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectMover : MonoBehaviour
{

    private Vector3 currentTarget;
    private bool bTargetReached = true;

    public bool AllowFreeMove = false;
    // only important for free move
    public Collider2D WalkArea;
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
        if(AllowFreeMove)
            ProcessTouch();

        if(!bTargetReached)
            MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        if(Vector3.Distance(currentTarget, transform.position) <= AcceptanceRadius)
        {
            ReachTarget();
            return;
        }

        Vector3 positionDelta = currentTarget - transform.position;
        positionDelta.Normalize();
        positionDelta *= Time.deltaTime * Speed;

        if(AllowFreeMove)
            CharacterController.Move(positionDelta);
        else
            transform.Translate(positionDelta);
    }

    public void ReachTarget()
    {
        bTargetReached = true;
        Animator.SetBool("IsWalking", false);
        Animator.SetBool("IsWaving", true);
        OnDestinationReached.Invoke();
    }

    private void StopWaving()
    {
        Animator.SetBool("IsWaving", false);
    }

    private void ProcessTouch()
    {
        if(Input.touches.Length < 1)
            return;

        if(EventSystem.current.IsPointerOverGameObject(0))
            return;

        if(Input.GetTouch(0).phase == TouchPhase.Ended)
            return;

        Vector2 touchPosition = Input.GetTouch(0).position;
        Camera currentCamera = Camera.main;
        if(!currentCamera)
        {
            Debug.LogError("LOL");
            return;
        }
        Vector3 worldSpacePosition = currentCamera.ScreenToWorldPoint(touchPosition);
        if(WalkArea != null)
            worldSpacePosition = WalkArea.bounds.ClosestPoint(worldSpacePosition);

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
