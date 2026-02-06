using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [HideInInspector] public AudioSource source;

        [Range(0.0f, 1.0f)] 
        public float volume = 1.0f; 

        public Vector2 pitchRange = new Vector2(1.0f, 1.0f); 
        public bool loop;
        public bool SFX;
    }

    public List<Sound> sounds;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.playOnAwake = false;
        }
    }

    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            float randomPitch = Random.Range(sound.pitchRange.x, sound.pitchRange.y);
            sound.source.pitch = randomPitch;

            sound.source.volume = sound.volume;

            sound.source.loop = sound.loop; 
            sound.source.Play();
        }
    }
    
    public void Stop(string name)
    {
        Sound sound = sounds.Find(s => s.name == name);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    public void StopAllSFX()
    {
        foreach (var sound in sounds)
        {
            if(sound.SFX)
                sound.source.Stop();
        }
    }
    
    public void StopAll()
    {
        foreach (var sound in sounds)
        {
            sound.source.Stop();
        }
    }
}