using UnityEngine;

public class Wolf : MonoBehaviour
{
    public AudioManager audioManager;
    public float hunger = 100f;
    public float hygiene = 100f;
    public float training = 20f;
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
        hunger = 100f;
        hygiene = 100f;
        training = 20f;
    }

    public void Feed()
    {
        hunger = Mathf.Min(hunger + 20f, 100f);
        audioManager.PlaySound("feed");
    }

    public void Groom()
    {
        hygiene = Mathf.Min(hygiene + 20f, 100f);
        audioManager.PlaySound("brush");
    }

    public void Train()
    {
        training = Mathf.Min(training + 20f, 100f);
        audioManager.PlaySound("bark");
    }

    public void Pet()
    {
        audioManager.PlaySound("bark");
    }

}
