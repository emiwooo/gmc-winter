using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    // all the sounds!!!!!!
    [Header("UI Sounds")]
    [SerializeField] AudioClip typing1;
    [SerializeField] AudioClip typing2;
    [SerializeField] AudioClip typing3;
    [SerializeField] AudioClip click;

    [Header("Wolf Sounds")]
    [SerializeField] AudioClip bark;
    [SerializeField] AudioClip growl;
    [SerializeField] AudioClip whine;

    [Header("Music")]
    [SerializeField] AudioClip bgm1;

    [SerializeField] AudioSource audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        PlayBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTypingSound()
    {
        int randomIndex = Random.Range(1, 4); // Generates a random number between 1 and 3
        AudioClip selectedClip = null;

        switch (randomIndex)
        {
            case 1:
                selectedClip = typing1;
                break;
            case 2:
                selectedClip = typing2;
                break;
            case 3:
                selectedClip = typing3;
                break;
        }

        if (selectedClip != null)
        {
            audioSource.PlayOneShot(selectedClip);
        }
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(click);
    }

    public void PlayBGM()
    {
        audioSource.clip = bgm1;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlaySound(string soundName)
    {
        AudioClip clipToPlay = null;

        switch (soundName)
        {
            case "bark":
                clipToPlay = bark;
                break;
            case "growl":
                clipToPlay = growl;
                break;
            case "whine":
                clipToPlay = whine;
                break;
            default:
                Debug.LogWarning("Sound not found: " + soundName);
                return;
        }

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}
