using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgrounds;    // Array of all backgrounds to be parallaxed
    public float[] parallaxScales;     // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;       // How smooth the parallax will be (set above 0)

    private Transform cam;
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector3 parallax = (previousCamPos - cam.position) * parallaxScales[i];

            // Target position for the background
            Vector3 backgroundTargetPos = backgrounds[i].position + new Vector3(parallax.x, parallax.y, 0);

            // Move to target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = cam.position;
    }
}
