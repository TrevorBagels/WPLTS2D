using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    bool isplayer = false;
    void Start()
    {
        if (GetComponentInParent<PlayerMovement>())
            isplayer = true;
    }
    public void FinishClimb()
    {
        if(isplayer)
        GetComponentInParent<PlayerMovement>().FinishClimb();
    }
    public void OnJump()
    {
        if(isplayer)
        GetComponentInParent<PlayerMovement>().OnJump();
    }
    public void OnLand()
    {
        if(isplayer)
        GetComponentInParent<PlayerMovement>().OnLand();
        GetComponentInParent<CharacterModelData>().OnLand();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
