using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCalculator : MonoBehaviour
{
    public static StatsCalculator Instance { get; private set; }

    [SerializeField] private UIManager m_UIManager;

    public int usedBulletCount = 0;
    public int hitCount = 0;
    public int killCount = 0;
    public int totalTarget=0;
    public int fatalHitCount = 0;
    public float totalResponseTime = 0;
    public float totalModuleTime = 0;
    public float totalDistanceMeter = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this){Destroy(this);}else{Instance = this;}
    }

    public void ClearRefferance()
    {
        usedBulletCount = 0;
        hitCount = 0;
        totalTarget = 0;
        fatalHitCount = 0;
        totalResponseTime = 0;
        totalModuleTime = 0;
        totalDistanceMeter = 0;
    }

    public void SetStatsValues()
    {
        string[] calculatedValues = new string[m_UIManager.m_StatTexts.Length];
        int succesRate = 0;
        float targetDistanceAvarage = 0;
        float responseAvarage = 0;
        int riskPercent = 0;
        // hit / used bullet
        // fatal point hit
        //total module time
        if (totalTarget != 0)
        {
            // succesRate = Mathf.FloorToInt((hitCount / totalTarget) * 60 + (hitCount / usedBulletCount) * 40);
             succesRate = Mathf.FloorToInt((killCount * 100 / totalTarget) );
             targetDistanceAvarage = totalDistanceMeter / totalTarget;
        }
        if (hitCount!=0)
        {
             responseAvarage = totalResponseTime / hitCount; //ToString("F2")
             riskPercent = Mathf.FloorToInt(((responseAvarage - 1) / 5) * 100);
        }

        calculatedValues[0] = string.Format("{0}%", succesRate);
        calculatedValues[1] = string.Format("{0} / {1}", hitCount, usedBulletCount);
        calculatedValues[2] = string.Format("{0}", fatalHitCount);
        calculatedValues[3] = string.Format("{0}s", responseAvarage.ToString("F2"));
        calculatedValues[4] = string.Format("{0}%", riskPercent);
        calculatedValues[5] = string.Format("{0} meter", targetDistanceAvarage.ToString("F2"));
        calculatedValues[6] = string.Format("{0}s", totalModuleTime);

        m_UIManager.m_StatTexts = calculatedValues;
        ClearRefferance();
    }
}
