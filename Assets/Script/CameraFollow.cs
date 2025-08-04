using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speedFollow = 2f;
    [SerializeField] private Transform target;
    void Update()
    {
        if (target == null) return;

        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, speedFollow * Time.deltaTime);
    }
}
