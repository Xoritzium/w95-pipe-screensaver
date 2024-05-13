using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
[ExecuteAlways]
public class LimitFPS : MonoBehaviour
{
	[SerializeField, Tooltip("Slower framerates results into slower PipeBilding")] int framerate;
	void Start()
	{
		Application.targetFrameRate = framerate;
	}

}
