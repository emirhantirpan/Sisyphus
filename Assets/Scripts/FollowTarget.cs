using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;

    private void FixedUpdate()
    {
        var vector3 = transform.position;
        vector3.y = target.position.y - 1.02f;
        vector3.z = target.position.z - 1.467f;
        vector3.x = target.position.x;
        transform.position = vector3;
        transform.rotation = target.rotation;
    }
}
