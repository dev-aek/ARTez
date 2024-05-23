using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TimerText;
    [SerializeField] private GameObject m_GameUIPanel;

    private float m_TimerCount;
    private bool isRunning = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isRunning)
        {
            m_TimerCount -= Time.deltaTime;
            DisplayTime(m_TimerCount);
            if (m_TimerCount<=0)
            {
                ResetTimer();
            }
        }
    }
    public void SetTimer(float refCount)
    {
        m_TimerCount = refCount;
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
    }

    public void SetMenuUI()
    {
        m_GameUIPanel.SetActive(true);
    }
    void DisplayTime(float timeToDisplay)
    {

        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        m_TimerText.text = seconds.ToString() ;
    }
}
