using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameConstants;

public class CanvasManager : MonoBehaviour {
    private GameObject canvas; //somehow there's no way to make this public. wtf.
    private Dictionary<string, GameObject> panels = new Dictionary<string, GameObject>();
    private Slider healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        canvas = GameObject.Find("Canvas");

        panels[GameStartPanel] = canvas.transform.Find(GameStartPanel).gameObject;
        panels[GameInProgressPanel] = canvas.transform.Find(GameInProgressPanel).gameObject;
        panels[GameOverPanel] = canvas.transform.Find(GameOverPanel).gameObject;

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
        // change the text of GameInProgressPanel -> LengthLabelText -> LengthValueText
        GameObject lengthLabelText = panels[GameInProgressPanel].transform.Find("LengthLabelText").gameObject;
        GameObject lengthValueText = lengthLabelText.transform.Find("LengthValueText").gameObject;
        lengthValueText.GetComponent<TMPro.TextMeshProUGUI>().text = length.ToString();
    }

    public void SetSnakeHP(int HP) {
        // change the text of GameInProgressPanel -> SnakeHPLabelText -> SnakeHPValueText
        GameObject lifePointsLabelText = panels[GameInProgressPanel].transform.Find("SnakeHPLabelText").gameObject;
        GameObject lifePointsValueText = lifePointsLabelText.transform.Find("SnakeHPValueText").gameObject;
        lifePointsValueText.GetComponent<TMPro.TextMeshProUGUI>().text = HP.ToString();
    }

    public void SetHealthBar(float currentHP, float maxHP) {
        if (healthBar != null) {
            healthBar.value = currentHP / maxHP; // Normalize HP (0 to 1)
        }
    }


}
