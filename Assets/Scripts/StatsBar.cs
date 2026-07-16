using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] private Image statsBarFill;

    public void UpdateStatsBar(float value)
    {
        statsBarFill.fillAmount = value / 100f;
    }

}
