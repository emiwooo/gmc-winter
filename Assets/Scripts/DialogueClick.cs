using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueClick : MonoBehaviour, IPointerClickHandler
{
    public TextManager textManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        textManager.SkipTyping();
    }
}