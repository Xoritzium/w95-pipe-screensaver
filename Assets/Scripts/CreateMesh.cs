using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{

	Mesh mesh;
	Vector3[] vertices;
	Vector2[] uv;
	int[] triangles;

	// Start is called before the first frame update
	void Start()
	{
		mesh = new Mesh();

		vertices = new Vector3[4];
		uv = new Vector2[4];
		triangles = new int[6];

		vertices[0] = new Vector3(0, 1);
		vertices[1] = new Vector3(1, 1);
		vertices[2] = new Vector3(0, 0);
		vertices[3] = new Vector3(1, 0);

		uv[0] = new Vector2(0, 1);
		uv[1] = new Vector2(1, 1);
		uv[2] = new Vector2(0, 0);
		uv[3] = new Vector2(1, 0);


		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;

		triangles[3] = 2;
		triangles[4] = 1;
		triangles[5] = 3;

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;

		GameObject gameobject = new GameObject("MeshContainer", typeof(MeshFilter), typeof(MeshRenderer));
		gameobject.transform.localScale = Vector3.one;
		gameobject.GetComponent<MeshFilter>().mesh = mesh;
		gameobject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Standard")); // bad performance
		gameobject.GetComponent<MeshRenderer>().material.color = Color.red;
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) ) {

			vertices = new Vector3[mesh.vertices.Length + 2];
			uv = new Vector2[mesh.uv.Length];
			triangles = new int[mesh.triangles.Length];

			mesh.vertices.CopyTo(vertices, 0);
			mesh.uv.CopyTo(uv, 0);
			mesh.triangles.CopyTo(triangles, 0);

			int vIndex = vertices.Length - 4;
			int vIndex0 = vIndex;
			int vIndex1 = vIndex + 1;
			int vIndex2 = vIndex + 2;
			int vIndex3 = vIndex + 3;


			Vector3 firstnewVert = vertices[vIndex0] + Vector3.right;
			Vector3 scndnewVert = vertices[vIndex1] + Vector3.right;

			vertices[vIndex2] = firstnewVert;
			vertices[vIndex3] = scndnewVert;

			int tIndex = triangles.Length - 6;
			triangles[tIndex] = vIndex0;
			triangles[tIndex + 1] = vIndex2;
			triangles[tIndex + 2] = vIndex1;

			triangles[tIndex + 3] = vIndex1;
			triangles[tIndex + 4] = vIndex2;
			triangles[tIndex + 5] = vIndex3;
			Debug.Log("pressed");

		}
	}

}
