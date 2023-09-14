using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public float minX = -8.5f;  // Adjust this value as per your requirement
    public float maxX = 26f;   // Adjust this value as per your requirement

    private void LateUpdate()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);
        transform.position = currentPos;
    }
}
