using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } //to get music in other group
    private AudioSource source;
    [SerializeField] private AudioSource animalSource;
    
    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip _sound, bool is_animalSound = false)
    {
        if (is_animalSound)
        {
            animalSource.PlayOneShot(_sound);
            return;
        }
        source.PlayOneShot(_sound);
    }
}
