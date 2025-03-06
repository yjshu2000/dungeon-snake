using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;

public class CanvasManager : MonoBehaviour {
    private GameObject canvas;
    private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
    private TMPro.TextMeshProUGUI lengthValueText;
    private TMPro.TextMeshProUGUI HPValueText;
    private Slider healthBar;

    public GameObject snakeObject;
    private SnakeMovement snakeMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //snakeObject = GameObject.Find("SnakeHead");
        snakeMovement = snakeObject.GetComponent<SnakeMovement>();
        snakeMovement.OnHPChanged += UpdateHealthUI;

        canvas = GameObject.Find("Canvas");
        panels[GameStartPanel] = canvas.transform.Find(GameStartPanel).gameObject;
        panels[GameInProgressPanel] = canvas.transform.Find(GameInProgressPanel).gameObject;
        panels[GameOverPanel] = canvas.transform.Find(GameOverPanel).gameObject;
        lengthValueText = panels[GameInProgressPanel].transform.Find("LengthLabelText/LengthValueText").GetComponent<TMPro.TextMeshProUGUI>();
        HPValueText = panels[GameInProgressPanel].transform.Find("SnakeHPLabelText/SnakeHPValueText").GetComponent<TMPro.TextMeshProUGUI>();
        healthBar = panels[GameInProgressPanel].transform.Find("HealthBar")?.GetComponent<Slider>();
        HideAllPanels();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void HideAllPanels() {
        foreach (var panel in panels.Values) {
            panel.SetActive(false);
        }
    }

    public void ShowPanel(string panelName) {
        if (panels.ContainsKey(panelName)) {
            HideAllPanels();
            panels[panelName].SetActive(true);
            if (panelName == GameOverPanel) {
                panels[GameInProgressPanel].SetActive(true);
            }
        }
    }
    
    public void SetSnakeLength(int length) {
        lengthValueText.text = length.ToString();
    }

    void UpdateHealthUI(float newHP) {
        SetSnakeHP(newHP);
        SetHealthBar(newHP, SnakeMaxHP);
    }

    void OnDestroy() {
        if (snakeMovement != null) {
            snakeMovement.OnHPChanged -= UpdateHealthUI;
        }
    }

    public void SetSnakeHP(float HP) {
        HPValueText.text = HP.ToString();
    }

    public void SetHealthBar(float currentHP, float maxHP) {
        if (healthBar != null) {
            healthBar.value = currentHP / maxHP; // Normalize HP (0 to 1)
        }
    }


}
