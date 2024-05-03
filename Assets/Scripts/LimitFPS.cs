using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
[ExecuteInEditMode]
public class LimitFPS : MonoBehaviour
{
    [SerializeField] int framerate;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = framerate;

	}

}
