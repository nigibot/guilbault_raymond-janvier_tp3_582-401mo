using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudioPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool playOnce = false;
    
    [Header("Collision Settings")]
    [SerializeField] private string targetTag = "Ground";
    [SerializeField] private float minCollisionForce = 0.1f;
    
    private AudioSource audioSource;
    private bool hasPlayed = false;
    
    private void Start()
    {
        // Add AudioSource component if it doesn't exist
        audioSource = GetComponent<AudioSource>();
        // Configure AudioSource
        audioSource.playOnAwake = false;
        audioSource.clip = collisionSound;
        audioSource.volume = volume;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we should play only once and have already played
        if (playOnce && hasPlayed)
            return;
            
        // Check if we collided with the target object
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Check if collision force is strong enough
            if (collision.relativeVelocity.magnitude >= minCollisionForce)
            {
                audioSource.Play();
                hasPlayed = true;
            }
        }
    }
    
    // Method to reset the play state if needed
    public void ResetPlayState()
    {
        hasPlayed = false;
    }
}
