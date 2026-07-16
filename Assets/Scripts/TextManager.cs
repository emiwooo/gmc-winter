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
        public string location;
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
    public ScrollRect scrollRect;
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
    private List<string> history = new List<string>();
    private string currentLocation = "";


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
        StartCoroutine(ClearChoicesNextFrame());

        Sprite newBG = Resources.Load<Sprite>(currentNode.image);
        if (newBG != null)
        {
            imageDisplay.sprite = newBG;
        }

        if (currentNode.location != currentLocation)
        {
            history.Clear();
            RefreshHistory(); 
            currentLocation = currentNode.location;
        }

        fullText = currentNode.text;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TypeText()
    {
        isTyping = true;
        string typed = "";
        foreach(char c in currentNode.text)
        {
            typed += c;
            text.text = string.Join("\n", history);
            if (history.Count > 0)
            {
                text.text += "\n";
            } 
            text.text += typed;

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        
            yield return new WaitForSeconds(textSpeed);    
        }
        history.Add(currentNode.text);
        RefreshHistory();

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
            string chosenText = choice.text;

            button.onClick.AddListener(() =>   
            {
                history.Add($"<color=#808080><b>> {chosenText}</b></color>");
                RefreshHistory();
                DisplayNode(targetNodeId);
            });
        }
    }

    void OnChoiceSelected(string targetNodeId)
    {
        DisplayNode(targetNodeId);
    }

    private IEnumerator ClearChoicesNextFrame()
    {
        yield return null;

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
            isTyping = false;
            history.Add(currentNode.text);
            RefreshHistory();
            GenerateChoiceButtons();
        }
    }

    private void RefreshHistory()
    {
        text.text = string.Join("\n", history);
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            text.rectTransform
        );
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0;
    }
}
