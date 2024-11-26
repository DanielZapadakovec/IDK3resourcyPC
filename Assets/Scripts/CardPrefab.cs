using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour
{
    public string lawTitle;
    public string lawProps;
    public int scienceEffectAccept;
    public int economyEffectAccept;
    public int popularityEffectAccept;
    public int stabilityEffectAccept;

    public int scienceEffectDecline;
    public int economyEffectDecline;
    public int popularityEffectDecline;
    public int stabilityEffectDecline;

    public bool isRoadRepair;
    public bool isDamCheck;

    public Text lawText;
    public Text lawPropsText;

    public bool isReusable;


    public void Update()
    {
        lawText.text = lawTitle;
        lawPropsText.text = lawProps; 

    }
}