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
        public List<ConditionalTarget> conditionalTargets; // first match wins, if none match, use default targetNodeId
        public string visibleIf;
    }

    [System.Serializable]
    public class ConditionalTarget {
        public string condition;
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
    public AudioManager audioManager;
    public GameManager gameManager;
    public Wolf wolf;
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
        DisplayNode("INTRO-A");
    }

    void LoadStoryData()
    {
        TextAsset jsonAsset = Resources.Load<TextAsset>("story");
        if (jsonAsset == null)
        {
            Debug.LogError("Could not find story.json in Resources folder!");
            return;
        }

        StoryWrapper wrapper = JsonUtility.FromJson<StoryWrapper>(jsonAsset.text);
        foreach (var node in wrapper.nodes)
        {
            storyMap[node.id] = node;
        }
    }

    public void DisplayNode(string nodeId)
    {
        Debug.Log($"Displaying node: {nodeId}");
        if (!storyMap.ContainsKey(nodeId)) return;

        // variable updates based on nodeId
        if (nodeId == "INTRO-A")
        {
            gameManager.ResetRun();
            wolf.UpdateWolfImage();
        } 
        else if (nodeId == "B-START")
        {
            gameManager.hasWolf = true;
            wolf.UpdateWolfImage();
        } else if ((nodeId == "B-CAMP-MORN1")||(nodeId == "B-MORN2"))
        {
            gameManager.daysNo += 1;
            wolf.hungry();
            wolf.UpdateWolfImage();
        } else if ((nodeId == "A-G-TALK")||(nodeId == "B-G-CHAT"))
        {
            gameManager.hasCharm = true;
        } else if ((nodeId == "B-WM-EAT")||(nodeId == "B-G-EAT"))
        {
            gameManager.morality -= 1;
            wolf.AffectionIncrease();
            wolf.Eat();
        } else if (nodeId == "B-CAMP2-PLAY")
        {
            if (wolf != null)
            {
                wolf.AffectionIncrease();
            }
        } else if (nodeId == "B-WM-PROTECT")
        {
            if (wolf != null)
            {
                wolf.AffectionIncrease();
            }
        } else if ((nodeId == "B-CAMP1-SEARCH")||(nodeId == "B-WM-CHAT") || (nodeId == "B-SEARCH"))
        {
            gameManager.food += 1;
            gameManager.UpdateFoodUI();
        } else if (nodeId == "BAD-END9")
        {
            audioManager.PlaySound("whine");
        }

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
            audioManager.PlayTypingSound();
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
            if (!string.IsNullOrEmpty(choice.visibleIf) && !EvaluateCondition(choice.visibleIf))
            {
                continue; // hide this choice entirely
            }

            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicePanelContainer);
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = choice.text;
            Button button = buttonObj.GetComponent<Button>();

            ChoiceData capturedChoice = choice; 
            string chosenText = choice.text;

            button.onClick.AddListener(() =>   
            {
                history.Add($"<color=#808080><b>> {chosenText}</b></color>");
                RefreshHistory();
                if (wolf != null && wolf.hunger <= 0f)
                {
                    DisplayNode("BAD-END12");
                }
                else
                {
                    DisplayNode(ResolveTarget(capturedChoice));
                }
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

    public void Reset()
    {
        history.Clear();
        currentLocation = "";
        RefreshHistory();
    }

    private string ResolveTarget(ChoiceData choice)
    {
        if (choice.conditionalTargets != null)
        {
            foreach (var ct in choice.conditionalTargets)
            {
                if (EvaluateCondition(ct.condition))
                    return ct.targetNodeId;
            }
        }
        return choice.targetNodeId;
    }

    private bool EvaluateCondition(string condition)
    {
        if (string.IsNullOrEmpty(condition)) return true;

        string[] ops = { ">=", "<=", "==", "!=", ">", "<" };
        foreach (var op in ops)
        {
            int idx = condition.IndexOf(op);
            if (idx > 0)
            {
                string varName = condition.Substring(0, idx).Trim();
                string valueStr = condition.Substring(idx + op.Length).Trim();
                if (!float.TryParse(valueStr, out float value)) return false;

                float varValue = GetVariableValue(varName);
                switch (op)
                {
                    case ">=": return varValue >= value;
                    case "<=": return varValue <= value;
                    case "==": return varValue == value;
                    case "!=": return varValue != value;
                    case ">":  return varValue > value;
                    case "<":  return varValue < value;
                }
            }
        }

        // no operator -> it's a bool
        bool negate = condition.StartsWith("!");
        string flagName = negate ? condition.Substring(1) : condition;
        bool flagValue = GetVariableValue(flagName) != 0;
        return negate ? !flagValue : flagValue;
    }

    private float GetVariableValue(string varName)
    {
        switch (varName)
        {
            case "morality":  return gameManager.morality;
            case "food":      return gameManager.food;
            case "daysNo":    return gameManager.daysNo;
            case "hunger":    return wolf.hunger;
            case "affection": return wolf.affection;
            case "hasWolf":   return gameManager.hasWolf ? 1 : 0;
            case "hasCharm":  return gameManager.hasCharm ? 1 : 0;
            default:
                Debug.LogWarning($"Unknown condition variable: {varName}");
                return 0;
        }
    }
}
