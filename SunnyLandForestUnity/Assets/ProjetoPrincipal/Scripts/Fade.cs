using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public static Fade instanceFade;
    public Image imagemFade;
    public Color corInicial;
    public Color corFinal;
    public float duracaoFade;
    public bool isFade;
    private float _tempo;
    // Start is called before the first frame update

    private void Awake()
    {
        //instanceFade = GetComponent<ControllerFade>();
        instanceFade = this;
    }

    IEnumerator InicioFade()
    {
        isFade = true;
        _tempo = 0f;
        while (_tempo <= duracaoFade)
        {
            imagemFade.color = Color.Lerp(corInicial, corFinal, _tempo / duracaoFade);
            _tempo = _tempo + Time.deltaTime;
            yield return null;
        }
        isFade = false;

    }
    void Start()
    {
        StartCoroutine(InicioFade());
    }
}
