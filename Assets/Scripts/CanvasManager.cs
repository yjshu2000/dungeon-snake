using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    private GameObject canvas;
    private GameObject gameStartPanel;
    private GameObject gameInProgressPanel;
    private GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI snakeHPValueText; // Assigned
    public Slider healthBar; // Assigned

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        // get the canvas object
        canvas = GameObject.Find("Canvas");
        gameStartPanel = canvas.transform.Find("GameStartPanel").gameObject;
        gameInProgressPanel = canvas.transform.Find("GameInProgressPanel").gameObject;
        gameOverPanel = canvas.transform.Find("GameOverPanel").gameObject;

        if (healthBar == null) {
            healthBar = canvas.transform.Find("GameInProgressPanel/HealthBar")?.GetComponent<Slider>();
        }

        HideAllPanels();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void HideAllPanels() {
        gameStartPanel.SetActive(false);
        gameInProgressPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void ShowGameStartPanel() {
        HideAllPanels();
        gameStartPanel.SetActive(true);
    }

    public void ShowGameInProgressPanel() {
        HideAllPanels();
        gameInProgressPanel.SetActive(true);
    }

    public void ShowGameOverPanel() {
        HideAllPanels();
        gameOverPanel.SetActive(true);
        gameInProgressPanel.SetActive(true);
    }
    
    public void SetSnakeLength(int length) {
        // change the text of GameInProgressPanel -> LengthLabelText -> LengthValueText
        GameObject lengthLabelText = gameInProgressPanel.transform.Find("LengthLabelText").gameObject;
        GameObject lengthValueText = lengthLabelText.transform.Find("LengthValueText").gameObject;
        lengthValueText.GetComponent<TMPro.TextMeshProUGUI>().text = length.ToString();
    }

    public void SetSnakeHP(int HP) {
        // change the text of GameInProgressPanel -> SnakeHPLabelText -> SnakeHPValueText
        GameObject lifePointsLabelText = gameInProgressPanel.transform.Find("SnakeHPLabelText").gameObject;
        GameObject lifePointsValueText = lifePointsLabelText.transform.Find("SnakeHPValueText").gameObject;
        lifePointsValueText.GetComponent<TMPro.TextMeshProUGUI>().text = HP.ToString();
    }

    public void SetHealthBar(float currentHP, float maxHP) {
        if (healthBar != null) {
            healthBar.value = currentHP / maxHP; // Normalize HP (0 to 1)
        }
    }


}
