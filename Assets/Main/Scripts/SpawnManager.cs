using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : MonoBehaviour
{
    public enum SpawnMode
    {
        Target,
        Enemy,
    }

    [System.Serializable]
    public struct SpawnSettings
    {
        public SpawnMode spawnMode;
        public GameObject[] prefabs;
        public float modeTime;
        public float targetDurationTime;
    }
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GunSwitcher gunSwitcher;
    public SpawnSettings[] spawnSettings;
    private SpawnMode spawnMode;
    private SpawnSettings currentSpawnSetting;
    Vector3 positionRef1;
    Vector3 positionRef2;
    /*public void SpawnRandomPrefab(SpawnMode spawnMode)
    {
        GameObject[] selectedPrefabs = null;


        switch (spawnMode)
        {
            case SpawnMode.Target:
                selectedPrefabs = targetPrefabs;
                break;
            case SpawnMode.Enemy:
                selectedPrefabs = targetPrefabs;
                break;
        }

        if (selectedPrefabs != null && selectedPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, selectedPrefabs.Length);
            GameObject selectedPrefab = selectedPrefabs[randomIndex];

            Instantiate(selectedPrefab, position, Quaternion.identity);
        }
    }*/

    public void SetPositionRefs(Vector3 ref1, Vector3 ref2)
    {
        positionRef1 = ref1;
        positionRef2 = ref2;
    }

    public void StartSpawnMode(int i)
    {
        currentSpawnSetting = spawnSettings[i];
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        uiManager.SetTimer(5);
        yield return new WaitForSeconds(5f);
        gunSwitcher.currentGun.StartReload();
        float cTime = currentSpawnSetting.modeTime;
        uiManager.SetTimer(cTime);
        while (cTime>0)
        {
            //cTime -= Time.deltaTime;
                if (Random.Range(0f, 1f) < 0.7f) // 70% chance of spawning this type
                {
                    GameObject prefabToSpawn = currentSpawnSetting.prefabs[Random.Range(0, currentSpawnSetting.prefabs.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(positionRef1.x+1, positionRef2.x-1), positionRef1.y, Random.Range(positionRef1.z  + Mathf.Abs(positionRef2.z - positionRef1.z)/15, positionRef2.z));
                    var prfb = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                StatsCalculator.Instance.totalTarget += 1;
                
                StatsCalculator.Instance.totalDistanceMeter += Mathf.Abs(prfb.transform.position.z - Camera.main.transform.position.z);
                }

            yield return new WaitForSeconds(currentSpawnSetting.targetDurationTime);
            cTime -= currentSpawnSetting.targetDurationTime;
        }
        uiManager.SetMenuUI();
    }
}