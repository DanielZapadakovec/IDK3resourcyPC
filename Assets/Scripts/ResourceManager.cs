using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResourceManager : MonoBehaviour
{
    public int science = 50;
    public int economy = 50;
    public int popularity = 50;
    public int stability = 50;

    public Image scienceFill, economyFill, popularityFill, stabilityFill;
    public Text scienceText, economyText, popularityText, stabilityText;
    public Text monthText;

    private int currentMonth = 1;
    private const int maxMonths = 48;

    public GameObject EndScreen;
    public Text EndScreentext;

    public int ignoredRoadRepairs = 0;
    public int ignoredDamChecks = 0;

    void Start()
    {
        EndScreen.SetActive(false);
        UpdateUI();
    }

    public void ApplyCardEffects(int scienceDelta, int economyDelta, int popularityDelta, int stabilityDelta)
    {
        science += scienceDelta;
        economy += economyDelta;
        popularity += popularityDelta;
        stability += stabilityDelta;

        CheckGameOver();

        currentMonth++;
        if (currentMonth > maxMonths)
        {
            Victory();
        }

        UpdateUI();
    }

    private void CheckGameOver()
    {
        if (science <= 0)
        {
            GameOver("Science has declined, the country is lagging behind, and the government has fallen.");
        }
        else if (economy <= 0)
        {
            GameOver("The economy is in debt. You were ousted from government.");
        }
        else if (popularity <= 0)
        {
            GameOver("The people revolted and deposed you.");
        }
        else if (stability <= 0)
        {
            GameOver("The country collapsed, the government lost control.");
        }
    }

    private void UpdateUI()
    {
        scienceFill.fillAmount = science / 100f;
        economyFill.fillAmount = economy / 100f;
        popularityFill.fillAmount = popularity / 100f;
        stabilityFill.fillAmount = stability / 100f;

        scienceText.text = science.ToString();
        economyText.text = economy.ToString();
        popularityText.text = popularity.ToString();
        stabilityText.text = stability.ToString();

        monthText.text = $"Month: {currentMonth}/{maxMonths}";
    }

    public void GameOver(string message)
    {
        EndScreen.SetActive(true);
        EndScreentext.text = message;
    }
    public void Reset()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void Victory()
    {
        EndScreen.SetActive(true);
        EndScreentext.text ="Victory! You survived 4 years as president.";
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}