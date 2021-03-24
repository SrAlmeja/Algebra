using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matriz : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public GameObject box;

    public Vector3 deltaVar;

    void Start()
    {
        
    }


    void Update()
    {
        Movement();
    }

    public void Movement ()
    {
        if(Input.GetKeyDown(KeyCode.A))
        box.transform.position = Translate(box.transform.position, deltaVar);
    }

    public Vector3 Translate(Vector3 pos, Vector3 delta)
    {
        float x = (pos.x * 1) + (pos.y * 0) + (pos.z * 0) + (1 * delta.x);
        float y = (pos.x * 0) + (pos.y * 1) + (pos.z * 0) + (1 * delta.y);
        float z = (pos.x * 0) + (pos.y * 0) + (pos.z * 1) + (1 * delta.z);

        return new Vector3(x, y, z);
    }
}
