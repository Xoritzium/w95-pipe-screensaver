using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pipe : MonoBehaviour
{
	[SerializeField]
	GameObject straight, corner;

	[SerializeField] // {get, set;} cancels the Serialization
	private Vector3 previousPipe;
	[SerializeField]
	private Vector3 me;
	[SerializeField]
	private Vector3 nextPipe;
	[SerializeField]
	private float angle;

	private Color thisColor;

	private void Awake()
	{
		previousPipe = Vector3Int.zero;
		nextPipe = Vector3Int.zero;



	}


	public void InitiatePipe(Vector3Int position, Vector3Int prevPos, Color color)
	{

		transform.position = position;
		me = position;
		previousPipe = prevPos;
		thisColor = color;

	}

	public void CorrectRotation(Vector3 dir)
	{

		if (dir.y == 1) {
			this.transform.localRotation = Quaternion.Euler(0, 0, 90f);
		} else if (dir.y == -1) {
			this.transform.localRotation = Quaternion.Euler(0, 0, -90f);
		} else if (dir.z == 1) {
			this.transform.localRotation = Quaternion.Euler(0, -90f, 0);
		} else if (dir.z == -1) {
			this.transform.localRotation = Quaternion.Euler(0, 90f, 0);
		}
	}

	public void SetNextPipePosition(Vector3Int nextPipePosition)
	{
		nextPipe = nextPipePosition;

		Vector3 prev_Me = me - previousPipe;
		Vector3 me_Next = nextPipe - me;
		angle = Vector3.Angle(prev_Me, me_Next);

		if (angle == 0) {
			Debug.Log("angle = 0");
			if (prev_Me.z != 0) {
				transform.rotation = Quaternion.Euler(0, 90, 0);
			} else if (prev_Me.z != 0) {
				transform.rotation = Quaternion.Euler(0, 0, 90);
			}

			straight.SetActive(true);
		}

		//set correct Rotation and object!

		//	CorrectRotation(prev_Me);




		//erst in der 2. Runde, wenn alle bekannt sind
		//	straight.SetActive(true);try{
		try {

			Material material = GetComponentInChildren<MeshRenderer>().material = new Material(Shader.Find("Standard"));
			material.color = thisColor;
		} catch {
			// nothing found
		}
	}




}
