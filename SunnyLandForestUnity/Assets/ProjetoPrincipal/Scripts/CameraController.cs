using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraController;

public class CameraController : MonoBehaviour
{

    public float offsetX = 0f;
    public float offsetY = 0f;

    public float smooth = 0.1f;

    public float limitedUp = 2f;
    public float limitedDown = 0f;
    public float limitedLeft = 0f;
    public float limitedRight = 100f;
    // colocar um outro limite para a camera abaixar mais do que o limitedDown
    // Pegar o objeto GroundCheck e verificar se ele está abaixo do chão (ou se ele é menor que o limitedDown), e com isso diminuir o offsetY da camera para ela descer
    //private Transform player;
    //private Vector2 player_position;

    public struct Player
    {
        public Transform transform;
        public Vector2 position;

    }

    Player player = new();
    // Start is called before the first frame update
    void Start()
    {
       
        player.transform = FindAnyObjectByType<PlayerController>().transform;
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(player.transform != null) 
        {
            player.position.x = Mathf.Clamp(player.transform.position.x + offsetX, limitedLeft, limitedRight);
            //player.position.y = Mathf.Clamp(player.transform.position.y + offsetY, limitedDown, limitedUp);
            player.position.y = player.transform.position.y;
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), smooth);

        }
    }
}
