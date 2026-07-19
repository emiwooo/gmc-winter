using UnityEngine;
using UnityEngine.UI;

public class Wolf : MonoBehaviour
{
    public GameManager gameManager;
    public AudioManager audioManager;
    public float hunger = 60f;
    public float affection = 0f;

    public Image imageDisplay;
    public Sprite noDog;
    public Sprite smallDog;
    public Sprite mediumDog;
    public Sprite largeDogGood;
    public Sprite largeDogEvil;
    private Sprite currentImage;

    public Slider hungerBar;
    public Slider affectionBar;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        noDog = Resources.Load<Sprite>("black");
        currentImage = noDog;
        smallDog = Resources.Load<Sprite>("smallDog");
        mediumDog = Resources.Load<Sprite>("mediumDog");
        largeDogGood = Resources.Load<Sprite>("largeDogGood");
        largeDogEvil = Resources.Load<Sprite>("largeDogEvil");
    }

    // Update is called once per frame
    void Update()
    {
        hungerBar.value = hunger / 100f;
        affectionBar.value = affection / 100f;
    }

    public void Reset()
    {
        hunger = 60f;
        affection = 0f;
        currentImage = noDog;
    }

    public void Feed()
    {
        if (gameManager.food > 0)
        {
            gameManager.food--;
            gameManager.UpdateFoodUI();
            hunger = Mathf.Min(hunger + 20f, 100f);
            audioManager.PlaySound("bark");
        }
    }

    public void Eat()
    {
        hunger = Mathf.Min(hunger + 20f, 100f);
        audioManager.PlaySound("growl");
    }

    public void Pet()
    {
        affection = Mathf.Min(affection + 2f, 100f);
        audioManager.PlaySound("bark");
    }

    public void AffectionIncrease()
    {
        affection = Mathf.Min(affection + 30f, 100f);
    }

    public void hungry()
    {
        hunger = Mathf.Max(hunger - 30f, 0f);
    }

    public void UpdateWolfImage()
    {
        currentImage = noDog;
        if ((gameManager.daysNo == 1)&&(gameManager.hasWolf == true))
        {
            currentImage = smallDog;
        }
        else if ((gameManager.daysNo == 2)&&(gameManager.hasWolf == true))
        {
            currentImage = mediumDog;
        }
        else if ((GameManager.Instance.daysNo == 3)&&(gameManager.hasWolf == true))
        {
            if (GameManager.Instance.morality <= -2)
            {
                currentImage = largeDogEvil;
            }
            else
            {
                currentImage = largeDogGood;
            }
        }
        imageDisplay.sprite = currentImage;
    }
}
