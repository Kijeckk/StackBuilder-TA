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

        // ✅ ikutin X
        pos.x = Mathf.Lerp(pos.x, target.position.x, Time.deltaTime * 5);

        // ✅ ikutin Y
        pos.y = Mathf.Lerp(pos.y, target.position.y + 5, Time.deltaTime * 2);

        // Z tetap
        pos.z = -15;

        transform.position = pos;
    }
}