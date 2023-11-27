using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    private int score;
    public Text txtScore;

    public AudioSource fxGame;
    public AudioClip fxCenouraColetada;

    public void Pontuacao( int qtdPontos)
    {

        score += qtdPontos;
        txtScore.text = score.ToString();

        // Executa o som da coleta da cenoura
        
        fxGame.PlayOneShot(fxCenouraColetada);

    }

}
