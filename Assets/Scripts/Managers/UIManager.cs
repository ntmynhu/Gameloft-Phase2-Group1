using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image healthBlockPrefab;
    [SerializeField] private GameObject healthBarPanel;
    [SerializeField] private TMP_Text nextPhaseTimeText;
    [SerializeField] private TMP_Text totalTimeText;

    private List<Image> healthBar;
    private float playerMaxHealth;
    private float goodThreshold;
    private float normalThreshold;

    private float totalTime;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = new List<Image>();
        CreateHealthBar(playerMaxHealth, goodThreshold, normalThreshold);
    }

    // Update is called once per frame
    void Update()
    {
        SetTotalTime();
    }

    private void CreateHealthBar(float playerMaxHealth, float goodThreshold, float normalThreshold)
    {
        Vector3 position = healthBarPanel.transform.position;

        for (int i = 1; i <= playerMaxHealth; i++)
        {
            Image healthBlockImage = Instantiate(healthBlockPrefab, position, Quaternion.identity, healthBarPanel.transform);
            if (i >= 1 && i < normalThreshold)
                healthBlockImage.color = Color.red;
            else if (i >= normalThreshold && i < goodThreshold)
                healthBlockImage.color = Color.yellow;
            else if (i >= goodThreshold)
                healthBlockImage.color = Color.green;
            healthBar.Add(healthBlockImage);

            position += new Vector3(20, 0, 0);
        }
    }

    public void SetHealthBar(float currentHealth)
    {
        int i = 1;
        foreach (Image healthbarImage in healthBar)
        {
            if(i <= (int) currentHealth)
                healthbarImage.gameObject.SetActive(true);
            else healthbarImage.gameObject.SetActive(false);
            i++;
        }
    }

    public void GetPlayerMaxHealth(float _playerMaxHealth)
    {
        playerMaxHealth = _playerMaxHealth;
    }
    public void GetGoodThreshold(float _goodThreshold)
    {
        goodThreshold = _goodThreshold;
    }

    public void GetNormalThreshold(float _normalThreshold)
    {
        normalThreshold = _normalThreshold;
    }

    public void SetNextPhaseTime(float _nextPhaseTime)
    {
        int minutes = Mathf.FloorToInt(_nextPhaseTime / 60);
        int seconds = Mathf.FloorToInt(_nextPhaseTime % 60);
        nextPhaseTimeText.text = "Next Phase " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void SetTotalTime()
    {
        totalTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);
        totalTimeText.text = "Total Time " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
