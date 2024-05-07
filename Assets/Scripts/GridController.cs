using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridController : MonoBehaviour
{

	[SerializeField] int size; // quadratical.
	[SerializeField, Tooltip("if there are less openFields than his digit, the simulation starts over")] int FullFieldThreshold;

	[SerializeField] GameObject head;
	[SerializeField] GameObject path;
	[SerializeField] GameObject border;

	[SerializeField, Header("turn Probability"), Range(0f, 1f), Tooltip("1 results in turnless Pipes")]
	private float turn;
	private bool firstChangeDone = false;
	private Vector3Int lastPositionChange;

	private Vector3Int startPosition;
	private Vector3Int currentPosition;
	private Vector3Int nextPosition;


	private bool[,,] grid; // each coord has a dot, to avoid collisions
						   // false = free space, true = taken (eg. borders, other pipes)

	private void Start()
	{

		InitiateGrid(size);
		startPosition = new(Random.Range(0, size), Random.Range(0, size), Random.Range(0, size));
		//startPosition = new(19, 1, 1);
		currentPosition = startPosition;
		head.transform.position = startPosition;

		Debug.Log("startPosition: " + head.transform.position.ToString());
		Debug.Log("free Fields: " + Mathf.Pow((size), 3));
	}


	private void Update()
	{
		if (nextPosition != Vector3.zero) {
			currentPosition = nextPosition;
		}
		nextPosition = CalculateNextPosition(currentPosition);
		head.transform.position = nextPosition;
	}


	/// <summary>
	/// calculate the next move of the head, if somehow the head doesnt move anymore,the algorithm sucks
	/// </summary>
	/// <param name="currentPosition"></param>
	/// <returns></returns>
	private Vector3Int CalculateNextPosition(Vector3Int currentPosition)
	{

		Vector3Int newPosition = Vector3Int.zero;
		if (StraightForward() && firstChangeDone) {

			newPosition = MoveForward();

			if (newPosition != Vector3Int.zero) { // just move if the newPosition is valid
				lastPositionChange = currentPosition - newPosition;
				grid[newPosition.x, newPosition.y, newPosition.z] = true; // update grid
				VisualizePath(newPosition);
				return newPosition;
			}
		}


		List<Vector3Int> potentialPositions = new List<Vector3Int>();
		List<Vector3Int> actualUsablePositions = new List<Vector3Int>();

		potentialPositions.Add(Vector3Int.right);
		potentialPositions.Add(Vector3Int.left);
		potentialPositions.Add(Vector3Int.up);
		potentialPositions.Add(Vector3Int.down);
		potentialPositions.Add(Vector3Int.forward);
		potentialPositions.Add(Vector3Int.back);


		// collect possible new Positions
		for (int i = 0; i < potentialPositions.Count; i++) {
			int x = Mathf.Clamp(currentPosition.x + potentialPositions[i].x, 0, size - 1);
			int y = Mathf.Clamp(currentPosition.y + potentialPositions[i].y, 0, size - 1);
			int z = Mathf.Clamp(currentPosition.z + potentialPositions[i].z, 0, size - 1);

			if (!grid[x, y, z]) {
				actualUsablePositions.Add(potentialPositions[i]);
			}
		}

		// reaching deadend
		if (actualUsablePositions.Count == 0) {
			Debug.Log("start new Pipe");
			newPosition = StartNewPipe();
			grid[newPosition.x, newPosition.y, newPosition.z] = true; // update grid
			return newPosition;
		}

		newPosition = currentPosition + actualUsablePositions[Random.Range(0, actualUsablePositions.Count)];

		if (!firstChangeDone) {
			lastPositionChange = newPosition - currentPosition;
			firstChangeDone = true;
		}

		VisualizePath(newPosition);
		grid[newPosition.x, newPosition.y, newPosition.z] = true; // update grid
		return newPosition;

	}


	/// <summary>
	/// sets an empty grid
	/// </summary>
	/// <param name="gridsize"></param>

	void InitiateGrid(int gridsize)
	{
		grid = new bool[gridsize, gridsize, gridsize];
		for (int x = 0; x < gridsize; x++)
			for (int y = 0; y < gridsize; y++) {
				for (int z = 0; z < gridsize; z++) {
					if (x == 0 || x == gridsize - 1 || y == 0 || y == gridsize - 1 || z == 0 || z == gridsize - 1) { // borders are blocked instant
						grid[x, y, z] = true;

					//	VisualizeBorders(x, y, z);
					} else {
						grid[x, y, z] = false;
					}
					//	Debug.Log(x + ", " + y + ", " + z + ": " + grid[x, y, z]);
				}
			}
	}

	private Vector3Int StartNewPipe()
	{
		List<Vector3Int> potentialStartPositions = new List<Vector3Int>();
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				for (int z = 0; z < size; z++) {
					if (!grid[x, y, z]) {
						potentialStartPositions.Add(new Vector3Int(x, y, z));
					}

				}
			}
		}
		//if field reaches it full capacity
		if (potentialStartPositions.Count < FullFieldThreshold) {
			RestartScene();
		}


		return potentialStartPositions[Random.Range(0, potentialStartPositions.Count)];
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	/// <summary>
	/// visualize the borders  -> debugpurpose
	/// </summary>
	private void VisualizeBorders(int x, int y, int z)
	{

		Instantiate(border);
		border.transform.position = new(x, y, z);
	}
	private void VisualizePath(Vector3 pos)
	{
		GameObject ins = Instantiate(path);
		ins.transform.parent = transform;
		ins.transform.position = pos;
	}
	/// <summary>
	/// Weighted probability to run forward without any turn
	/// </summary>
	/// <returns></returns>
	private bool StraightForward()
	{
		if (Random.value < turn) {
			return true;
		}
		return false;

	}


	private Vector3Int MoveForward()
	{

		Vector3Int newPosition = currentPosition + (-1 * lastPositionChange);
		int x = Mathf.Clamp(newPosition.x, 0, size - 1);
		int y = Mathf.Clamp(newPosition.y, 0, size - 1);
		int z = Mathf.Clamp(newPosition.z, 0, size - 1);
		if (grid[x, y, z]) {
			return Vector3Int.zero;
		}

		return newPosition;
	}
}