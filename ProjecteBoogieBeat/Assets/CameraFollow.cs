using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 posOffset;
    [SerializeField] private Vector3 rotOffset;
    [SerializeField] private Vector3 posVelocity;
    [SerializeField] private float rotSpeed;


    private void FixedUpdate()
    {
        HandlePosition();
        HandleRotation();

    }


    private void HandlePosition()
    {
        Vector3 targetPos = target.TransformPoint(posOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref posVelocity, Time.deltaTime);

    }

    private void HandleRotation()
    {
        Vector3 dir = ((target.position + rotOffset) - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

    }


}
