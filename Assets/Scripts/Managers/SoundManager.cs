using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	// Audio players components.
	private static AudioSource effectsSource;
	private static AudioSource musicSource;

	// Random pitch adjustment range.
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Singleton instance
	private static SoundManager instance = null;

    // Singleton getter
    public static SoundManager Instance
    {
        get
        {
            // If instance is null, find or create the SoundManager in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                // If not found, create a new GameObject in the current scene and attach the SoundManager to it
                if (instance == null)
                {
					// Create manager object
                    GameObject managerObject = new GameObject("SoundManager");
                    instance = managerObject.AddComponent<SoundManager>();

					// Create sound source objects
					GameObject effectsSourceObject = new GameObject("effectsSource");
                    GameObject musicSourceObject = new GameObject("musicSource");

                    // Add audio source component
                    effectsSource = effectsSourceObject.AddComponent<AudioSource>();
                    musicSource = musicSourceObject.AddComponent<AudioSource>();

                    // Set SoundManager as parent
                    effectsSourceObject.transform.SetParent(managerObject.transform);
                    musicSourceObject.transform.SetParent(managerObject.transform);
                }
            }

            // Return SoundManager
            return instance;
        }
    }

    // Initialize the singleton instance.
    private void Awake()
	{
        // Make sure the SoundManager persists between scene loads
        DontDestroyOnLoad(gameObject);
	}

	// Play a single clip through the sound effects source.
	public void Play(AudioClip clip)
	{
		effectsSource.clip = clip;
		effectsSource.Play();
	}

	// Play a single clip through the music source.
	public void PlayMusic(AudioClip clip, bool loop)
	{
		musicSource.clip = clip;
		musicSource.loop = loop;
		musicSource.Play();
	}

	// Play a random clip from an array, and randomize the pitch slightly.
	public void RandomSoundEffect(params AudioClip[] clips)
	{
		int randomIndex = Random.Range(0, clips.Length);
		float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

		effectsSource.pitch = randomPitch;
		effectsSource.clip = clips[randomIndex];
		effectsSource.Play();
	}

}
