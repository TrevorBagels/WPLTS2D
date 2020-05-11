using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//The game manager. This is the gateway between C# and python.
public class GM : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //Load into main menu
        //for now we load straight into the game
        OnLoadLevel();
    }
    void OnLoadLevel()
    {
        Transform cam = Camera.main.transform;
        GameObject character = Instantiate(Resources.Load<GameObject>("Prefabs/Character"));
        character.transform.position = new Vector3(cam.position.x, cam.position.y, 0);
        PlayerMovement mvmt = character.AddComponent<PlayerMovement>();
        mvmt.Camera = cam;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
