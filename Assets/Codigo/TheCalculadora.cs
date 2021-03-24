using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCalculadora : MonoBehaviour
{


    public enum Operacion { SUMA, RESTA, MULT, DIV }
    public Operacion operación;
    public float num1, num2, resultado;

    public float x;
    public float y;
    public float z;
    public float Magnitud;
    public GameObject Sphere;
    public float NormalizarX;
    public float NormalizarY;
    public float NormalizarZ;
    public float x1;
    public float y1;
    public float z1;
    public GameObject OptimizedTree;
    public float distanciaX;
    public float distanciaY;
    public float distanciaZ;
    public float DistanciaTotal;

    // Start is called before the first frame update
    void Start()
    {

        switch (operación)
        {
            case Operacion.SUMA:
                resultado = num1 + num2;
                Debug.Log("El resultado es: {0}" + resultado);
                break;

            case Operacion.RESTA:
                resultado = num1 - num2;
                Debug.Log("El resultado es: {0}" + resultado);
                break;

            case Operacion.MULT:
                resultado = num1 * num2;
                Debug.Log("El resultado es: {0}" + resultado);
                break;

            case Operacion.DIV:
                if (num2 != 0)

                    resultado = num1 / num2;
                Debug.Log("El resultado es: {0}" + resultado);
                break;
        }



    }

    // Update is called once per frame
    void Update()
    {
        x = Sphere.transform.position.x;
        y = Sphere.transform.position.y;
        z = Sphere.transform.position.z;
        Magnitud = Mathf.Sqrt((x * x) + (y * y) + (z * z));
        Debug.Log("El Resultado de la Magnitud es" + Magnitud);

        NormalizarX = x / Magnitud;
        NormalizarY = y / Magnitud;
        NormalizarZ = z / Magnitud;
        //Ya solo me falta distancia :c
        //x1-x2, resultado al cuadrado. Con yz tambien. Sumar 3 resultados

        x1 = OptimizedTree.transform.position.x;
        y1 = OptimizedTree.transform.position.y;
        z1 = OptimizedTree.transform.position.z;

        distanciaX = x - x1;
        distanciaY = y - y1;
        distanciaZ = z - z1;

        DistanciaTotal = (distanciaX * distanciaX) + (distanciaY * distanciaY) + (distanciaZ * distanciaZ);
        Debug.Log(DistanciaTotal);
    }
}

