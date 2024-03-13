using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plataforma_move : MonoBehaviour
{
    [SerializeField] private float velocidade;
    [SerializeField] private float quantidadeDeMovimento;

    private Vector3 posicaoInicial;
    private enum Orientation{Left,Right,Top,Bottom}
    private enum Eixo {X,Y}

    [SerializeField] private Orientation orientation;
    [SerializeField] private Eixo eixo;
    // Start is called before the first frame update
    


    void Start()
    {
        posicaoInicial = transform.position;
    }

    private void FixedUpdate()
    {
        movePlatform();
    }
    // Update is called once per frame
    void Update()
    {
       
    }

    private void movePlatform()
    {
        if (eixo == Eixo.X && orientation == Orientation.Right)
        {
            transform.position = new Vector3(Mathf.PingPong(Time.time * velocidade, quantidadeDeMovimento) + posicaoInicial.x, transform.position.y, transform.position.z);
        }
        else if (eixo == Eixo.X && orientation == Orientation.Left)
        {
            transform.position = new Vector3(-Mathf.PingPong(Time.time * velocidade, quantidadeDeMovimento) + posicaoInicial.x, transform.position.y, transform.position.z);
        }
        else if (eixo == Eixo.Y && orientation == Orientation.Top)
        {
            transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time * velocidade, quantidadeDeMovimento) + posicaoInicial.y, transform.position.z);
        }
        else if (eixo == Eixo.Y && orientation == Orientation.Bottom)
        {
            transform.position = new Vector3(transform.position.x, -Mathf.PingPong(Time.time * velocidade, quantidadeDeMovimento) + posicaoInicial.y, transform.position.z);
        }
    }
    
}
