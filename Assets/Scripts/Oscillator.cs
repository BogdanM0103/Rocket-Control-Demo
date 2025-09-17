using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // The offset from the starting position that defines how far the object moves
    [SerializeField] Vector3 movementVector;

    // Speed of oscillation (higher = faster movement)
    [SerializeField] float speed;

    // Cached positions
    Vector3 startPosition;   // Where the object begins
    Vector3 endPosition;     // Target position based on movementVector

    // Value between 0 and 1 used to interpolate between start and end
    float movementFactor;

    void Start()
    {
        // Record the initial position of the object
        startPosition = transform.position;

        // Calculate the end position by adding the movement vector
        endPosition = startPosition + movementVector;
    }

    void Update()
    {
        // Mathf.PingPong moves a value back and forth between 0 and a set limit
        // Here: it cycles between 0 and 1 over time, scaled by speed
        movementFactor = Mathf.PingPong(Time.time * speed, 1f);

        // Interpolate between startPosition and endPosition
        // When movementFactor = 0 → startPosition
        // When movementFactor = 1 → endPosition
        transform.position = Vector3.Lerp(startPosition, endPosition, movementFactor);
    }
}
