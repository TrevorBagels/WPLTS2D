using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Conf;
[System.Serializable]
public class Config
{
    [SerializeField]
    public PlayerConfig Player;
}
namespace Conf
{
    [System.Serializable]
    public class PlayerConfig
    {
        public Vector3 Speed = new Vector3(1.7f, 5, 0);
        public Vector3 CameraOffset = new Vector3(0, 2f);
        public float Zoom = 6;
        public float RunMultiplier = 2.5f;
        public float VaultCheckDistance = .85f;
    }
}

