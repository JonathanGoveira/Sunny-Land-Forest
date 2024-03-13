using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlatform : MonoBehaviour
{

    public Transform plataforma, pontoA, pontoB;
    public float velocidadePlataforma;

    public Vector3 pontoDestino;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        plataforma.position = pontoA.position;
        pontoDestino = pontoB.position;
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (plataforma.position == pontoA.position)
        {
            pontoDestino = pontoB.position;
        }
        else if (plataforma.position == pontoB.position)
        {
            pontoDestino = pontoA.position;
        }

        plataforma.position = Vector3.MoveTowards(plataforma.position, pontoDestino, velocidadePlataforma);
    }
}
