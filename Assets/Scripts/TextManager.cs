using UnityEngine;
using System.Collections;
using TMPro;

public class TextManager : MonoBehaviour
{

    public TextMeshProUGUI text;
    public float textSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private string fullText;
    private bool isTyping = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText(string currText)
    {
        fullText = currText;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        text.text = "";
        foreach (char letter in fullText.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            text.text = fullText;
            isTyping = false;
        }
    }
}
