using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridController : MonoBehaviour
{

	[SerializeField] int size; // quadratical.

	[SerializeField] GameObject head;

	private Vector3 startPosition;
	private Vector3 currentPosition;

	private Vector3 lastPosition;

	private bool[,,] dots; // each coord has a dot, to avoid collisions

	private void Start()
	{
		InitiateGrid(size);
		startPosition = new(Random.Range(0, size), Random.Range(0, size), Random.Range(0, size));
		lastPosition = new(0f, 0f, 0f);
		head.transform.position = startPosition = currentPosition;
	}


	private void Update()
	{

		lastPosition = head.transform.position;

		currentPosition = CalculateCurrentPosition(lastPosition);
		head.transform.position = currentPosition;
	}
	/// <summary>
	/// calculate the next move of the head, if somehow the head doesnt move anymore,the algorithm sucks
	/// </summary>
	/// <param name="lp"></param>
	/// <returns></returns>
	private Vector3 CalculateCurrentPosition(Vector3 lp)
	{
		switch (Random.Range(1, 6)) {
			case 1: return lastPosition + new Vector3(1f, 0f, 0f); // +1x
			case 2: return lastPosition + new Vector3(-1f, 0f, 0f); // -1x
			case 3: return lastPosition + new Vector3(0f, 1f, 0f); // +1y
			case 4: return lastPosition + new Vector3(0f, -1f, 0f); // -1y
			case 5: return lastPosition + new Vector3(0f, 0f, 1f); // +1z
			case 6: return lastPosition + new Vector3(0f, 0f, -1f); // -1z
		}
		return lp;

	}

	/// <summary>
	/// sets an empty grid
	/// </summary>
	/// <param name="gridsize"></param>
	void InitiateGrid(int gridsize)
	{
		dots = new bool[gridsize, gridsize, gridsize];
		for (int x = 0; x < gridsize; x++) {
			for (int y = 0; y < gridsize; y++) {
				for (int z = 0; z < gridsize; z++) {
					dots[x, y, z] = false;
				}
			}
		}
	}
}
