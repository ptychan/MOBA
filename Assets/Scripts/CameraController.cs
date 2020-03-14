using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float screenEdgeThreshold = 10.0f;

    public Vector2 minLimit = Vector2.negativeInfinity;
    public Vector2 maxLimit = Vector2.positiveInfinity;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 newPosition = transform.position;

        if (h > 0.0f || Input.mousePosition.x >= Screen.width - screenEdgeThreshold)
            newPosition.x += moveSpeed * Time.deltaTime;
        if (h < 0.0f || Input.mousePosition.x <= screenEdgeThreshold)
            newPosition.x -= moveSpeed * Time.deltaTime;
        if (v > 0.0f || Input.mousePosition.y >= Screen.height - screenEdgeThreshold)
            newPosition.z += moveSpeed * Time.deltaTime;
        if (v < 0.0f || Input.mousePosition.y <= screenEdgeThreshold)
            newPosition.z -= moveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minLimit.x, maxLimit.x);
        newPosition.z = Mathf.Clamp(newPosition.z, minLimit.y, maxLimit.y);

        transform.position = newPosition;
    }
}
