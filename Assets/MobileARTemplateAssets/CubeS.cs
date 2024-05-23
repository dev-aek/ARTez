using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime*10, Space.World);
        transform.Rotate(Vector3.left * Time.deltaTime*15, Space.World);
    }
}
