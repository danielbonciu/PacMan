using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiles : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isPellet;
    public bool isSuperPellet;
    public bool isPortal;
    public bool didConsume;

    public GameObject portalReceiver;
    private void Start()
    {
        didConsume = false;
    }
}
