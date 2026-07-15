using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
    public class ChoiceData {
        public string text;
        public string targetNodeId;
    }

    [System.Serializable]
    public class DialogueNode {
        public string id;
        public string text;
        public string image;
        public List<ChoiceData> choices;
    }

    [System.Serializable]
    public class StoryWrapper {
        public List<DialogueNode> nodes;
    }

public class TextManager : MonoBehaviour
{
    public Image imageDisplay;
    public Transform choicePanelContainer;
    public GameObject choiceButtonPrefab;
    private Dictionary<string, DialogueNode> storyMap = new Dictionary<string, DialogueNode>();
    private DialogueNode currentNode;

    public TextMeshProUGUI text;
    public float textSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private string fullText;
    private bool isTyping = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadStoryData();
        DisplayNode("1"); 
    }

    void LoadStoryData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "story.json");
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            StoryWrapper wrapper = JsonUtility.FromJson<StoryWrapper>(jsonText);
            foreach (var node in wrapper.nodes)
            {
                storyMap[node.id] = node;
            }
        }
    }

    public void DisplayNode(string nodeId)
    {
        if (!storyMap.ContainsKey(nodeId)) return;

        currentNode = storyMap[nodeId];        
        ClearChoices(); // for each run

        Sprite newBG = Resources.Load<Sprite>(currentNode.image);
        if (newBG != null)
        {
            imageDisplay.sprite = newBG;
        }

        fullText = currentNode.text;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
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
        GenerateChoiceButtons();
    }

    private void GenerateChoiceButtons()
    {
        if (currentNode.choices == null || currentNode.choices.Count == 0) return;

        foreach (var choice in currentNode.choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicePanelContainer);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text;

            Button button = buttonObj.GetComponent<Button>();
            string targetNodeId = choice.targetNodeId;
            button.onClick.AddListener(() => DisplayNode(targetNodeId));
        }
    }

    void OnChoiceSelected(string targetNodeId)
    {
        DisplayNode(targetNodeId);
    }

    void ClearChoices()
    {
        foreach (Transform child in choicePanelContainer)
        {
            Destroy(child.gameObject);
        }
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
