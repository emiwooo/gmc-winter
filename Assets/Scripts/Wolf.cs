using UnityEngine;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
    public AudioManager audioManager;
    public float hunger = 80f;
    public float affection = 0f;

    public Image smallDog;
    public Image mediumDog;
    public Image largeDogEvil;
    public Image largeDogGood;

    public StatsBar hungerBar;
    public StatsBar affectionBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        hunger = 80f;
        affection = 0f;
    }

    public void Feed()
    {
        hunger = Mathf.Min(hunger + 20f, 100f);
        audioManager.PlaySound("feed");
    }

    public void Pet()
    {
        affection = Mathf.Min(affection + 1f, 100f);
        audioManager.PlaySound("bark");
    }

}
