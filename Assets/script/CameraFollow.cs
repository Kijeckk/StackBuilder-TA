using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, target.position.y + 5, Time.deltaTime * 2);
        pos.z = -10;
        transform.position = pos;
    }
}