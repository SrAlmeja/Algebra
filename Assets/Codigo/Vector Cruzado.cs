using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculadorea : MonoBehaviour
{


    public enum Operacion { SUMA, RESTA, MULT, DIV }
    public Operacion operación;
    public float num1, num2, resultado;
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

    }
}

