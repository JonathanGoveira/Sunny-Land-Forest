using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    private int score;
    public Text txtScore;

    public void Pontuacao( int qtdPontos)
    {

        score += qtdPontos;
        txtScore.text = score.ToString();
    }
}
