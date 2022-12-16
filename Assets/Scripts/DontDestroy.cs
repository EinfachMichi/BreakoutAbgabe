using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector] public string objectID;

    private void Awake()
    {
        objectID = name + transform.position + transform.eulerAngles;
    }

    private void Start()
    {
        for (int i = 0; i < FindObjectsOfType<DontDestroy>().Length; i++)
        {
            if (FindObjectsOfType<DontDestroy>()[i] != this)
            {
                if (FindObjectsOfType<DontDestroy>()[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
