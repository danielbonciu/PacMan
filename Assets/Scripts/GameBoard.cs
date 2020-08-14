using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    private static int boardWidth = 1000;
    private static int boardHeight = 1000;

    public int totalPellets = 0;
    public int score = 0;

    public TextMeshProUGUI text;

    public GameObject[,] board = new GameObject[boardWidth, boardHeight];
    // Start is called before the first frame update
    void Start()
    {
        Object[] objects = GameObject.FindObjectsOfType(typeof(GameObject));

        foreach (GameObject o in objects)
        {
            Vector2 pos = o.transform.position;

            if (o.name != "Player" && o.name != "Nodes" && o.name != "NotNodes" && o.name != "Collidable" && o.name != "BaseLayer" && o.name != "Ghost" && o.name != "Canvas")
            {
                if(o.GetComponent<Tiles>() != null)
                {
                    if(o.GetComponent<Tiles>().isPellet || o.GetComponent<Tiles>().isSuperPellet)
                    {
                        totalPellets++;
                        Debug.Log(o.name);
                    }
                }

                board[(int)pos.x, (int)pos.y] = o;
            }
            else
            {
                Debug.Log("Found Pacman at: " + pos);
            }
        }

        Debug.Log(totalPellets);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + score.ToString() + "/"  + totalPellets.ToString();

        if (score == totalPellets)
        {
            Debug.Log("You Win!");
            return;
        }
    }
}
