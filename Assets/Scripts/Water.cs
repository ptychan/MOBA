using UnityEngine;

public class Water : MonoBehaviour
{
    public Vector2 scrollSpeed;

    new Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        var offset = Time.time * scrollSpeed;
        renderer.material.mainTextureOffset = offset;
    }
}
