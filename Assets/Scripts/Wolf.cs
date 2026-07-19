using UnityEngine;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
    public AudioManager audioManager;
    public float hunger = 80f;
    public float affection = 0f;

    public Image imageDisplay;
    public Sprite smallDog;
    public Sprite mediumDog;
    public Sprite largeDogGood;
    public Sprite largeDogEvil;
    private Sprite currentImage;

    public StatsBar hungerBar;
    public StatsBar affectionBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentImage = smallDog;
        smallDog = Resources.Load<Sprite>("smallDog");
        mediumDog = Resources.Load<Sprite>("mediumDog");
        largeDogGood = Resources.Load<Sprite>("largeDogGood");
        largeDogEvil = Resources.Load<Sprite>("largeDogEvil");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        hunger = 80f;
        affection = 0f;
        currentImage = smallDog;
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

    public void UpdateWolfImage()
    {
        if (GameManager.Instance.daysNo == 1)
        {
            currentImage = smallDog;
        }
        else if (GameManager.Instance.daysNo == 2)
        {
            currentImage = mediumDog;
        }
        else if (GameManager.Instance.daysNo == 3)
        {
            if (GameManager.Instance.morality == -2)
            {
                currentImage = largeDogEvil;
            }
            else
            {
                currentImage = largeDogGood;
            }
        }
    }
}
