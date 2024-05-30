using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private TextMeshProUGUI[] m_StatTextElements;
    [SerializeField] public string[] m_StatTexts;
    [SerializeField] private GameObject m_GameUIPanel;
    [SerializeField] private GameObject m_StatPanel;
    [SerializeField] private RectTransform crosHair;

    private float m_TimerCount;
    private bool isRunning = false;
    private void Start()
    {
        m_StatTexts = new string[m_StatTextElements.Length];
    }
    private void Update()
    {
        if (isRunning)
        {
            m_TimerCount -= Time.deltaTime;
            DisplayTime(m_TimerCount);
            if (m_TimerCount<=0.1f)
            {
                ResetTimer();
            }
        }
    }
    public void SetTimer(float refCount)
    {
        m_TimerCount = refCount;
        StatsCalculator.Instance.ClearRefferance();
        StatsCalculator.Instance.totalModuleTime = m_TimerCount;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        m_TimerCount = 0;
        isRunning = false;

        SetStatValues();

    }

    public void SetMenuUI()
    {
        m_GameUIPanel.SetActive(true);
        m_StatPanel.SetActive(true);
        crosHair.gameObject.SetActive(false);
    }
    void DisplayTime(float timeToDisplay)
    {

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        m_TimerText.text = seconds.ToString() ;
    }

    private void SetStatTextElements()
    {
        for (int i = 0; i < m_StatTextElements.Length; i++)
        {
            if (m_StatTexts[i]!=null)
            {
                m_StatTextElements[i].text = m_StatTexts[i];
            }
            else
            {
                m_StatTextElements[i].text = "Not Calculated";
            }
        }
    }

    private void SetStatValues()
    {
        StatsCalculator.Instance.SetStatsValues();
        SetStatTextElements();
    }

}
