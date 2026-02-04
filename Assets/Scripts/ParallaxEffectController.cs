using UnityEngine;

public class ParallaxEffectController : MonoBehaviour
{
    private float startPos, length;
    [SerializeField] private GameObject camera;
    [SerializeField] private float parallaxEffect; //speed at which the background should move relative to camera.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = camera.transform.position.x * parallaxEffect; // 0 = move with cam / 1 = not moving / 0.5 = half
        float movement = camera.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if(movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
