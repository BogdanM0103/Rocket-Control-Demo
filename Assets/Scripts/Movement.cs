using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    // Player input actions (defined in the Input System)
    [SerializeField] InputAction thrust;       // Controls upward thrust
    [SerializeField] InputAction rotation;     // Controls left/right rotation

    // Movement forces
    [SerializeField] float thrustStrength = 100f;     // Strength of upward thrust
    [SerializeField] float rotationStrength = 100f;   // Speed/strength of rotation

    // Engine effects
    [SerializeField] AudioClip mainEngineSFX;         // Sound for main engine
    [SerializeField] ParticleSystem mainEngineParticles;   // Particles when thrusting
    [SerializeField] ParticleSystem rightThrustParticles;  // Particles when rotating left
    [SerializeField] ParticleSystem leftThrustParticles;   // Particles when rotating right

    // Cached references
    Rigidbody rb;          // Physics component for movement
    AudioSource audioSource; // Plays engine sound effects

    private void Start()
    {
        // Get components attached to this GameObject
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        // Enable input actions when object becomes active
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        // Handle movement in physics update
        ProcessThrust();
        ProcessRotation();
    }

    // ------------------- THRUST -------------------
    private void ProcessThrust()
    {
        // If thrust button is held → apply thrust
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        // Apply force upwards relative to the object's orientation
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);

        // Play engine sound if not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }

        // Play engine particles if not already playing
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    private void StopThrusting()
    {
        // Stop sound & particles when not thrusting
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    // ------------------- ROTATION -------------------
    private void ProcessRotation()
    {
        // Get rotation input (positive = left, negative = right)
        float rotationInput = rotation.ReadValue<float>();

        if (rotationInput < 0)
        {
            RotateRight();
        }
        else if (rotationInput > 0)
        {
            RotateLeft();
        }
        else
        {
            StopRotating();
        }
    }

    private void RotateRight()
    {
        // Apply rotation to the right (positive)
        ApplyRotation(rotationStrength);

        // Show right thruster particles (turning left visually)
        if (!rightThrustParticles.isPlaying)
        {
            leftThrustParticles.Stop();
            rightThrustParticles.Play();
        }
    }

    private void RotateLeft()
    {
        // Apply rotation to the left (negative)
        ApplyRotation(-rotationStrength);

        // Show left thruster particles (turning right visually)
        if (!leftThrustParticles.isPlaying)
        {
            rightThrustParticles.Stop();
            leftThrustParticles.Play();
        }
    }

    private void StopRotating()
    {
        // Stop both side-thruster particles when not rotating
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        // Temporarily disable physics rotation to avoid conflict
        rb.freezeRotation = true;

        // Rotate manually (local Z-axis = forward vector in Unity 3D)
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.fixedDeltaTime);

        // Re-enable physics rotation so collisions still work
        rb.freezeRotation = false;
    }
}
