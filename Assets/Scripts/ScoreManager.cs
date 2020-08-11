using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static ScoreManager instace;
    public TextMeshProUGUI text;
    int score;
    void Start()
    {
        if(instace==null)
        {
            instace = this;
        }
    }

    // Update is called once per frame
    public void ChangeScore(int coinValue)
    {
        score += coinValue;
        text.text =  score.ToString();
        Debug.Log("Score " + score);
    }
}
