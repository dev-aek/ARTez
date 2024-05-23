using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class PlaceManager : MonoBehaviour
{
    
    private PlaceIndicator placeIndicator;
    public GameObject objectToPlace;
    public List<GameObject> refObjects;
    public GameObject creatablePrefab;
    public GameObject creatableWallPrefab;
    public GameObject creatableTargetPrefab;
    private GameObject createdObject;

    private GameObject newPlacedObject;
    [SerializeField] private float wallHight = 3;
    [SerializeField] private SpawnManager spawnManager;
    void Start()
    {
        placeIndicator = FindObjectOfType<PlaceIndicator>();
        //refObjects = new GameObject[3];
        Debug.Log(refObjects.Count);
    }

    public void ClickToPlace()
    {
        if (refObjects.Count<3)
        {
            newPlacedObject = Instantiate(objectToPlace, placeIndicator.transform.position, placeIndicator.transform.rotation);
            refObjects.Add(newPlacedObject);
            Debug.Log(refObjects.Count);

            
        }

    }

    public void ScanPlace()
    {
        placeIndicator.gameObject.SetActive(true);
    }
    public void SetPlace()
    {
        if (refObjects.Count == 3)
        {
            spawnManager.SetPositionRefs(refObjects[0].transform.position, new Vector3(refObjects[1].transform.position.x, refObjects[0].transform.position.y, refObjects[2].transform.position.z));
            placeIndicator.gameObject.SetActive(false);
            var trPoObjectValue = new Vector3(
                (refObjects[0].transform.position.x + Mathf.Abs(refObjects[1].transform.position.x - refObjects[0].transform.position.x) / 2),
                (refObjects[0].transform.position.y - (refObjects[0].transform.localScale.y / 2)),
                (refObjects[0].transform.position.z + Mathf.Abs(refObjects[2].transform.position.z - refObjects[0].transform.position.z) / 2));

            createdObject = Instantiate(creatablePrefab, trPoObjectValue, Quaternion.identity);

            var trScObjectValue = new Vector3(
                Mathf.Abs(refObjects[1].transform.position.x - refObjects[0].transform.position.x),
                createdObject.transform.localScale.y,
                Mathf.Abs(refObjects[2].transform.position.z - refObjects[0].transform.position.z));

            createdObject.transform.DOScale(trScObjectValue, 1f);
            for (int i = 0; i < 3; i++)
            {
                SetWalls(i);
            }
        }
    }

    public void SetWalls(int wallId)
    {
        Vector3 wallPosition;
        Vector3 wallScale;
        GameObject createdWall;
        if (wallId == 2)
        {
            wallPosition = new Vector3(
                (refObjects[0].transform.position.x + Mathf.Abs(refObjects[1].transform.position.x - refObjects[0].transform.position.x) / 2),
                (refObjects[0].transform.position.y - (refObjects[0].transform.localScale.y / 2))+(wallHight / 2),
                (refObjects[2].transform.position.z));

            wallScale = new Vector3(
                Mathf.Abs(refObjects[1].transform.position.x - refObjects[0].transform.position.x),
                wallHight,
                0.05f);


        }
        else
        {
            wallPosition = new Vector3(
                (refObjects[wallId].transform.position.x),
                (refObjects[0].transform.position.y - (refObjects[0].transform.localScale.y / 2)) + (wallHight / 2),
                (refObjects[0].transform.position.z + Mathf.Abs(refObjects[2].transform.position.z - refObjects[0].transform.position.z) / 2));

            wallScale = new Vector3(
                0.05f,
                wallHight,
                Mathf.Abs(refObjects[2].transform.position.z - refObjects[0].transform.position.z));
        }
        createdWall = Instantiate(creatableWallPrefab, wallPosition, Quaternion.identity);
        createdWall.transform.DOScale(wallScale, 1f);



    }

    public void CreateTarget()
    {

        
        for (int i = 0; i < 5; i++)
        {
            float refX = Random.Range(refObjects[0].transform.position.x, refObjects[1].transform.position.x);
            float refZ = Random.Range(refObjects[0].transform.position.z + Mathf.Abs(refObjects[2].transform.position.z - refObjects[0].transform.position.z) / 4, refObjects[2].transform.position.x);
            float refY = refObjects[0].transform.position.y;
            var target = Instantiate(creatableTargetPrefab, new Vector3(refX, refY, refZ), Quaternion.Euler(90,0,0));

            target.transform.DORotate(Vector3.zero, 2f,RotateMode.Fast);
        }
    }
    private void Update()
    {
     
    }

    /*
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) ClickToPlace();
    }*/
}
