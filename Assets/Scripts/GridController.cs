using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridController : MonoBehaviour
{

	[SerializeField] int width, height, depth;
	[SerializeField, Tooltip("if there are less openFields than his digit, the simulation starts over")]
	int FullFieldThreshold;

	private GameObject head; // runs at the head of the current building pipe

	[SerializeField] GameObject pipeContainer;
	private GameObject currentPipeContainer;
	private GameObject prevPipeContainer;
	private bool firstContainer = true;

	// debug purpose
	[SerializeField] GameObject border;


	[SerializeField, Header("turn Probability"), Range(0f, 1f), Tooltip("1 results in turnless Pipes")]
	private float turnProbability;
	private bool firstChangeDone = false;
	private Vector3Int lastPositionChange;
	// calculation to ensure right rotation of the pipeParts
	private Vector3Int startPosition;
	private Vector3Int currentPosition;
	private Vector3Int nextPosition;

	private Color currentColor;


	private bool[,,] grid; // each coord has a dot, to avoid collisions
						   // false = free space, true = taken (eg. borders, other pipes)

	private void Start()
	{
		head = new GameObject("head");

		InitiateGrid();
		startPosition = new(Random.Range(0, width), Random.Range(0, height), Random.Range(0, depth));
		//startPosition = new(19, 1, 1);
		currentPosition = startPosition;
		head.transform.position = startPosition;

		Debug.Log("startPosition: " + head.transform.position.ToString());
		Debug.Log("free Fields: " + width * height * depth);
		currentColor = GetNewRandomColor();

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
	/// calculate the next move of the head
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


		List<Vector3Int> potentialPositions = new();
		List<Vector3Int> actualUsablePositions = new();

		potentialPositions.Add(Vector3Int.right);
		potentialPositions.Add(Vector3Int.left);
		potentialPositions.Add(Vector3Int.up);
		potentialPositions.Add(Vector3Int.down);
		potentialPositions.Add(Vector3Int.forward);
		potentialPositions.Add(Vector3Int.back);


		// collect possible new Positions
		for (int i = 0; i < potentialPositions.Count; i++) {
			int x = Mathf.Clamp(currentPosition.x + potentialPositions[i].x, 0, width - 1);
			int y = Mathf.Clamp(currentPosition.y + potentialPositions[i].y, 0, height - 1);
			int z = Mathf.Clamp(currentPosition.z + potentialPositions[i].z, 0, depth - 1);

			if (!grid[x, y, z]) {
				actualUsablePositions.Add(potentialPositions[i]);
			}
		}

		// reach deadend
		if (actualUsablePositions.Count == 0) {
			Debug.Log("start new Pipe");
			newPosition = StartNewPipe();
			grid[newPosition.x, newPosition.y, newPosition.z] = true; // update grid
			return newPosition;
		}

		newPosition = currentPosition + actualUsablePositions[Random.Range(0, actualUsablePositions.Count)];

		if (!firstChangeDone) { // avoid Nullpointer in first iteration
			lastPositionChange = newPosition - currentPosition;
			firstChangeDone = true;
		}

		VisualizePath(newPosition);
		grid[newPosition.x, newPosition.y, newPosition.z] = true; // update grid
		return newPosition;

	}


	/// <summary>
	/// sets the empty grid
	/// </summary>
	/// <param></param>

	void InitiateGrid()
	{
		grid = new bool[width, height, depth];
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++) {
				for (int z = 0; z < depth; z++) {
					if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || z == 0 || z == depth - 1) { // borders are blocked instant
						grid[x, y, z] = true;
						//	VisualizeBorders(x, y, z);
					} else {
						grid[x, y, z] = false;
					}
					//	Debug.Log(x + ", " + y + ", " + z + ": " + grid[x, y, z]);
				}
			}
	}
	/// <summary>
	/// Start the next Pipe
	/// </summary>
	/// <returns></returns>
	private Vector3Int StartNewPipe()
	{
		List<Vector3Int> potentialStartPositions = new();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				for (int z = 0; z < depth; z++) {
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

		currentColor = GetNewRandomColor();
		nextPosition = Vector3Int.zero;
		return potentialStartPositions[Random.Range(0, potentialStartPositions.Count)];
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	/// <summary>
	/// Probabilit to move straight insteat of a turn
	/// </summary>
	/// <returns></returns>
	private bool StraightForward()
	{
		if (Random.value < turnProbability) {
			return true;
		}
		return false;

	}

	/// <summary>
	/// Redo the last move - works not flawless
	/// </summary>
	/// <returns>the last position + 1 on the same axis</returns>
	private Vector3Int MoveForward()
	{

		Vector3Int newPosition = currentPosition + (-1 * lastPositionChange);
		int x = Mathf.Clamp(newPosition.x, 0, width - 1);
		int y = Mathf.Clamp(newPosition.y, 0, height - 1);
		int z = Mathf.Clamp(newPosition.z, 0, depth - 1);
		if (grid[x, y, z]) {
			return Vector3Int.zero;
		}

		return newPosition;
	}

	/*---------------------------------------------
	 * ------------Visualisation-------------------
	 * -------------------------------------------*/

	/// <summary>
	/// visualize the borders  -> debugpurpose
	/// </summary>
	/// 
	private void VisualizeBorders(int x, int y, int z)
	{

		Instantiate(border);
		border.transform.position = new(x, y, z);
	}
	/// <summary>
	/// Instanziates the next Pipe. 
	/// </summary>
	/// <param name="pos">pos is the next position where the pipe is placed</param>
	private void VisualizePath(Vector3Int pos)
	{
		if (prevPipeContainer != null) {
			prevPipeContainer = currentPipeContainer; // will be the one out of the last iteration.
			prevPipeContainer.GetComponent<Pipe>().SetNextPipePosition(pos);
		}

		currentPipeContainer = Instantiate(pipeContainer);
		currentPipeContainer.transform.parent = transform;
		Pipe currentPipe = currentPipeContainer.GetComponent<Pipe>();
		currentPipe.InitiatePipe(pos, currentPosition, currentColor);

		if (firstContainer) { // disables in the seconds loop
			prevPipeContainer = currentPipeContainer; // just to ensure, prevPipeCon != null
			firstContainer = false;
		}

	}


	private Color GetNewRandomColor()
	{
		return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
	}

}