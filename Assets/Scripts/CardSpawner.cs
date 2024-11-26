using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public List<GameObject> cardPrefabs;
    public GameObject specialEventPrefab;
    public Transform spawnPoint;
    public ResourceManager resourceManager;

    private List<GameObject> activeCardList; // Akt�vny zoznam kariet (pracujeme s k�piou)
    private GameObject currentCard;
    private int lastCardIndex = -1;
    public Canvas canvas;

    private bool isSpecialEventActive = false;

    void Start()
    {
        // Vytvor�me k�piu zoznamu kariet
        activeCardList = new List<GameObject>(cardPrefabs);
        SpawnCard();
    }

    public void SpawnCard()
    {
        if (currentCard != null)
        {
            Destroy(currentCard);
        }

        if (isSpecialEventActive || activeCardList.Count == 0)
        {
            return;
        }

        int newCardIndex;
        do
        {
            newCardIndex = Random.Range(0, activeCardList.Count);
        } while (newCardIndex == lastCardIndex && activeCardList.Count > 1); // Aby nebolo rovnak� dvakr�t po sebe

        lastCardIndex = newCardIndex;

        GameObject selectedCardPrefab = activeCardList[newCardIndex];
        currentCard = Instantiate(selectedCardPrefab, spawnPoint.position, Quaternion.identity);
        currentCard.transform.SetParent(canvas.transform, false);
        currentCard.GetComponent<RectTransform>().anchoredPosition = spawnPoint.GetComponent<RectTransform>().anchoredPosition;
    }

    public void AcceptLaw()
    {
        var cardScript = currentCard.GetComponent<CardPrefab>();
        resourceManager.ApplyCardEffects(
            cardScript.scienceEffectAccept,
            cardScript.economyEffectAccept,
            cardScript.popularityEffectAccept,
            cardScript.stabilityEffectAccept
        );

        if (cardScript.isRoadRepair)
            resourceManager.ignoredRoadRepairs = 0;
        if (cardScript.isDamCheck)
            resourceManager.ignoredDamChecks = 0;

        HandleCardRemoval(cardScript); // Rie�enie odstr�nenia/uchovania karty
        SpawnCard();
    }

    public void DeclineLaw()
    {
        var cardScript = currentCard.GetComponent<CardPrefab>();
        resourceManager.ApplyCardEffects(
            cardScript.scienceEffectDecline,
            cardScript.economyEffectDecline,
            cardScript.popularityEffectDecline,
            cardScript.stabilityEffectDecline
        );

        if (cardScript.isRoadRepair)
            resourceManager.ignoredRoadRepairs++;
        if (cardScript.isDamCheck)
            resourceManager.ignoredDamChecks++;

        HandleCardRemoval(cardScript); // Rie�enie odstr�nenia/uchovania karty
        CheckSpecialEvents();
        SpawnCard();
    }

    private void HandleCardRemoval(CardPrefab cardScript)
    {
        // Ak karta nie je ozna�en� ako reusable, odstr�ni ju z akt�vneho zoznamu
        if (!cardScript.isReusable)
        {
            activeCardList.Remove(currentCard);
        }
    }

    private void CheckSpecialEvents()
    {
        if (resourceManager.ignoredRoadRepairs >= 3)
        {
            Debug.Log("�peci�lna udalos�: Povode� sp�sobila po�kodenie ciest!");
            TriggerSpecialEvent("Flood Damage!", -50, -20, -30, -10);
            resourceManager.ignoredRoadRepairs = 0;
        }

        if (resourceManager.ignoredDamChecks >= 3)
        {
            Debug.Log("�peci�lna udalos�: Priehrada praskla a sp�sobila povode�!");
            TriggerSpecialEvent("Dam Collapse!", -70, -30, -40, -20);
            resourceManager.ignoredDamChecks = 0;
        }
    }

    private void TriggerSpecialEvent(string title, int economyPenalty, int popularityPenalty, int stabilityPenalty, int sciencePenalty)
    {
        // Nastav flag, �e je akt�vna �peci�lna udalos�
        isSpecialEventActive = true;

        // Vytvor �peci�lnu udalos� ako kartu
        if (currentCard != null)
        {
            Destroy(currentCard);
        }

        currentCard = Instantiate(specialEventPrefab, spawnPoint.position, Quaternion.identity);
        currentCard.transform.SetParent(canvas.transform, false);
        currentCard.GetComponent<RectTransform>().anchoredPosition = spawnPoint.GetComponent<RectTransform>().anchoredPosition;

        var cardScript = currentCard.GetComponent<CardPrefab>();
        cardScript.lawTitle = title;
        cardScript.scienceEffectAccept = sciencePenalty;
        cardScript.economyEffectAccept = economyPenalty;
        cardScript.popularityEffectAccept = popularityPenalty;
        cardScript.stabilityEffectAccept = stabilityPenalty;

        cardScript.scienceEffectDecline = 0; // Game Over na odmietnutie
        cardScript.economyEffectDecline = 0;
        cardScript.popularityEffectDecline = 0;
        cardScript.stabilityEffectDecline = 0;
    }

    public void AcceptSpecialEvent()
    {
        var cardScript = currentCard.GetComponent<CardPrefab>();
        resourceManager.ApplyCardEffects(
            cardScript.scienceEffectAccept,
            cardScript.economyEffectAccept,
            cardScript.popularityEffectAccept,
            cardScript.stabilityEffectAccept
        );

        isSpecialEventActive = false; // Reset flagu
        SpawnCard();
    }

    public void DeclineSpecialEvent()
    {
        resourceManager.GameOver("The population is dissatisfied, you were deposed from the government.");
    }
}