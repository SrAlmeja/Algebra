using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arreglandocalculadora : MonoBehaviour
{




    // Start is called before the first frame update
    void Start()
    {
        int sum = suma(5, 8);
        print("suma" + sum);

        //Move();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.A))
        {

        }
    }


    public int suma(int a, int b)
    {
        int c = a + b;
        return c;
    }


}
