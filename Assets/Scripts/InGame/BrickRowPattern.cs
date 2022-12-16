using System;
using UnityEngine;

namespace InGame
{
    [Serializable]
    public class BrickRowPattern
    {
        public Pattern[] bricks;
    }

    [Serializable]
    public class Pattern
    {
        public int level;
        public GameObject brick;
    }
}