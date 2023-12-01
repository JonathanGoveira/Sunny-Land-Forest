using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemie : MonoBehaviour
{


    public Transform enemie;
    public SpriteRenderer enemieSprite;
    public Transform[] target;
    public float speed;
    public bool isRight;

    private int idTarget;

    // Start is called before the first frame update
    void Start()
    {

        enemieSprite = enemie.gameObject.GetComponent<SpriteRenderer>();
        //enemie.position = target[0].position;
        enemie.position = new Vector3(target[0].position.x,enemie.position.y,0);
        idTarget = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (enemie != null)
        {
            enemie.position = Vector3.MoveTowards(enemie.position, target[idTarget].position, speed * Time.deltaTime);

            if (enemie.position == target[idTarget].position)
            {
                idTarget += 1;
                if(idTarget == target.Length)
                {
                    idTarget = 0;
                }
                //Debug.Log("inimigo" + enemie.position);
                //Debug.Log("target" + target[idTarget].position);
                Debug.Log("target colidiu");
                if (target[idTarget].position.x < enemie.position.x && isRight == true)
                {
                    flip();

                }
                else if (target[idTarget].position.x > enemie.position.x && isRight == false)
                {
                    flip();
                }
                
            }

            
            
        }
        
    }

    void flip()
    {
        isRight = !isRight;
        enemieSprite.flipX = !enemieSprite.flipX;

    }
}
