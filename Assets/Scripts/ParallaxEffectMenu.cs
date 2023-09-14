using UnityEngine;

public class ParallaxEffectMenu : MonoBehaviour
{
    public Transform[] backgrounds;    // Array of all backgrounds to be parallaxed
    public float[] parallaxScales;     // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;       // How smooth the parallax will be (set above 0)

    private Vector2 previousMousePos;
    private Vector2 currentMousePos;

    void Awake()
    {
        currentMousePos = Input.mousePosition;
    }

    void Start()
    {
        previousMousePos = currentMousePos;
    }

    void Update()
    {
        // Update the currentMousePos with the current mouse position
        currentMousePos = Input.mousePosition;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector2 parallax = (previousMousePos - currentMousePos) * parallaxScales[i];

            // Target position for the background
            Vector3 backgroundTargetPos = backgrounds[i].position + new Vector3(parallax.x, 0, 0);

            // Move to target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousMousePos = currentMousePos;
    }
}
