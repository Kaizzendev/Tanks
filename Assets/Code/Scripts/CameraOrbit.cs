using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Target and Distance")]
    private Transform target; 
    [SerializeField]
    private float distance = 100f; 
    [SerializeField]
    public float minDistance = 20f;
    [SerializeField]
    public float maxDistance = 200f;

    [Header("Sensitivity")] 
    [SerializeField]
    private float xSpeed = 120f;
    [SerializeField]
    private float ySpeed = 120f;
    [SerializeField]
    private float scrollSensivity = 80f;

    [Header("Y Rotation Limits")]
    [SerializeField]
    private float yMinLimit = -20f;
    [SerializeField]
    private float yMaxLimit = 80f;

    private float x = 0f;
    private float y = 0f;

    private void Start()
    {
        if (target == null)
        {
            GameObject go = new GameObject("CameraTarget");
            go.transform.position = Vector3.zero;
            target = go.transform;
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButton(0)) 
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
        }

        distance -= Input.GetAxis("Mouse ScrollWheel") * scrollSensivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0f, 0f, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
