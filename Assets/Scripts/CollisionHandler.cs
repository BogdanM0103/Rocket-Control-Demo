using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Delay (in seconds) before changing/reloading levels after a crash or success
    [SerializeField] float levelLoadDelay = 2f;

    // Audio assets for feedback
    [SerializeField] AudioClip successSFX;
    [SerializeField] AudioClip crashSFX;

    // Particle effects for feedback
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    // Cached components
    AudioSource audioSource;

    // State flags to avoid double-handling collisions and input after outcome
    bool isControllable = true;  // When false, disables player control after an outcome
    bool isCollidable = true;    // Debug toggle to ignore collisions

    private void Start()
    {
        // Grab the AudioSource on the same GameObject
        audioSource = GetComponent<AudioSource>();
        // (Optional) You could also cache Movement here to avoid repeated GetComponent calls
        // movement = GetComponent<Movement>();
    }

    private void Update()
    {
        // Only for quick testing in the Editor / Development builds
        RespondToDebugKeys();
    }

    // Handle debug keyboard shortcuts
    void RespondToDebugKeys()
    {
        // Press 'L' → skip to the next level
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        // Press 'C' → toggle collision handling on/off (useful when stuck)
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }

    // Unity collision callback
    private void OnCollisionEnter(Collision other)
    {
        // Ignore collisions if we've already triggered success/crash or collisions are disabled
        if (!isControllable || !isCollidable) { return; }

        // Decide what to do based on the collided object's tag
        // NOTE: CompareTag is slightly faster/safer than string equality on tag
        if (other.gameObject.CompareTag("Friendly"))
        {
            // Harmless surface (e.g., launchpad, wall you can touch)
            Debug.Log("Everything is looking good!");
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            // Reached the landing zone / finish pad
            StartSuccessSequence();
        }
        else
        {
            // Any other collision counts as a crash
            StartCrashSequence();
        }
    }

    // Triggered when reaching the finish
    void StartSuccessSequence()
    {
        isControllable = false;           // Lock out movement & further collisions
        audioSource.Stop();               // Stop any engine loop/oneshots
        audioSource.PlayOneShot(successSFX);
        successParticles.Play();          // Fire success VFX

        // Disable player movement script so the rocket stops responding
        GetComponent<Movement>().enabled = false;

        // Schedule level advance after a short delay (for SFX/VFX to play)
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    // Triggered when crashing into non-friendly geometry
    void StartCrashSequence()
    {
        isControllable = false;           // Lock out input & collisions
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticles.Play();            // Fire crash VFX

        // Disable movement so the rocket no longer responds
        GetComponent<Movement>().enabled = false;

        // Schedule a reload after delay
        Invoke(nameof(ReloadLevel), levelLoadDelay);
    }

    // Loads the next scene in Build Settings; wraps to 0 if we're at the end
    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;

        // If we've passed the last scene, wrap to the first
        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }

        SceneManager.LoadScene(nextScene);
    }

    // Reloads the current scene (useful after a crash)
    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
