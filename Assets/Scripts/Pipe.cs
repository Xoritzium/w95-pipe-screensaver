using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

		Vector3 prev_Me = previousPipe - me;
		Vector3 me_Next = nextPipe-me;
		angle = Vector3.Angle(prev_Me, me_Next);
		if (angle == 0 || angle == 180) {   // next and previous pipe are aligned
			Debug.Log("angle = 0");
			if (prev_Me.z != 0) {
				transform.rotation = Quaternion.Euler(0, 90, 0);
			} else if (prev_Me.y != 0) {
				transform.rotation = Quaternion.Euler(0, 0, 90);
			}
			straight.SetActive(true);
		} else {

			if (prev_Me.x == 1) { 

				if (me_Next.y == -1) {
					transform.rotation = Quaternion.Euler(90, 0, 0);
				} else if (me_Next.y == 1) {
					transform.rotation = Quaternion.Euler(-90, 0, 0);
				} else if (me_Next.z == -1) {
					transform.rotation = Quaternion.Euler(180, 0, 0);
				}
			} else if (prev_Me.x == -1) { 

				if (me_Next.y == -1) {
					transform.rotation = Quaternion.Euler(90, 180, 0);
				} else if (me_Next.y == 1) {
					transform.rotation = Quaternion.Euler(-90, 180, 0);
				} else if (me_Next.z == -1) {
					transform.rotation = Quaternion.Euler(0, 180 ,0);
				} else if (me_Next.z == 1) {
					transform.rotation = Quaternion.Euler(0, 0, 180);
				}

			} else if (prev_Me.y == -1) {
				if (me_Next.x == 1) {
					transform.rotation = Quaternion.Euler(0, 90, -90);
				} else if (me_Next.x == -1) {
					transform.rotation = Quaternion.Euler(0, -90, -90);
				} else if (me_Next.z == -1) {
					transform.rotation = Quaternion.Euler(0, 180, -90);
				} else if (me_Next.z == 1) {
					transform.rotation = Quaternion.Euler(0, 0, -90);
				}

			} else if (prev_Me.y == 1) {
				if (me_Next.x == 1) {
					transform.rotation = Quaternion.Euler(0, 90, 90);
				} else if (me_Next.x == -1) {
					transform.rotation = Quaternion.Euler(0, -90, 90);
				} else if (me_Next.z == 1) {
					transform.rotation = Quaternion.Euler(0, 0, 90);
				} else if (me_Next.z == -1) {
					transform.rotation = Quaternion.Euler(0, 180, 90);
				}

			} else if (prev_Me.z == 1) {
				if (me_Next.x == -1) {
					transform.rotation = Quaternion.Euler(0, 0, 180);
				} else if (me_Next.y == 1) {
					transform.rotation = Quaternion.Euler(0, 0, 90);
				} else if (me_Next.y == -1) {
					transform.rotation = Quaternion.Euler(0, 0, -90);
				}
			} else if (prev_Me.z == -1) {
				if (me_Next.x == -1) {
					transform.rotation = Quaternion.Euler(180, 90, 0);
				} else if (me_Next.y == 1) {
					transform.rotation = Quaternion.Euler(-90, 90, 0);
				} else if (me_Next.y == -1) {
					transform.rotation = Quaternion.Euler(90, 90, 0);
				}else if (me_Next.x == 1) {
					transform.rotation = Quaternion.Euler(180, 0, 0);
				}
			}




			corner.SetActive(true);
		}
		try {

			Material material = GetComponentInChildren<MeshRenderer>().material = new Material(Shader.Find("Standard"));
			material.color = thisColor;
		} catch {
			// nothing found
		}
	}




}
