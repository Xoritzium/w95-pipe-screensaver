using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawMesh : MonoBehaviour
{

	private Vector3 lastMousePosition;

	private Vector3 GetMousePosition()
	{
		Vector3 pos = Input.mousePosition;
		return pos;
	}

	private void Update()
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[4];
		Vector2[] uv = new Vector2[4];
		int[] triangles = new int[6];


		if (Input.GetMouseButtonDown(0)) {

			vertices[0] = new Vector3(-1, 1);
			vertices[1] = new Vector3(-1, -1);
			vertices[2] = new Vector3(1, -1);
			vertices[3] = new Vector3(1, 1);

			uv[0] = Vector2.zero;
			uv[1] = Vector2.zero;
			uv[2] = Vector2.zero;
			uv[3] = Vector2.zero;

			triangles[0] = 0;
			triangles[1] = 3;
			triangles[2] = 1;

			triangles[3] = 1;
			triangles[4] = 3;
			triangles[5] = 2;


			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
			mesh.MarkDynamic(); // improve performance

			GetComponent<MeshFilter>().mesh = mesh;
			transform.position = GetMousePosition();

			lastMousePosition = GetMousePosition();
		}
		if (Input.GetMouseButton(0)) {
			vertices = new Vector3[mesh.vertices.Length +2];
			uv = new Vector2[mesh.uv.Length];
			triangles = new int[mesh.triangles.Length];

			mesh.vertices.CopyTo(vertices, 0);
			mesh.uv.CopyTo(uv, 0);
			mesh.triangles.CopyTo(triangles, 0);

			int vIndex = vertices.Length-4;
			int vIndex0 = vIndex + 0;		// access the last 2 vertices
			int vIndex1 = vIndex + 1;		
			int vIndex2 = vIndex + 2;       // for the two who will be added
			int vIndex3 = vIndex + 3;


			// soll durch automatisches Dings ersetzt werden.
			Vector3 mouseForwardVector = GetMousePosition() - lastMousePosition.normalized;
			Vector3 normal2D = new(0, 0, -1);
			Vector3 newVertexUp = GetMousePosition() + Vector3.Cross(mouseForwardVector, normal2D) *1f ; // 1 = line thickness // vector between these two are always orthogonal to the last quad
			Vector3 newVertexDown = GetMousePosition() + Vector3.Cross(mouseForwardVector, normal2D *-1) *1f ; // 1 = line thicknessvertices
		
			vertices[vIndex2] = newVertexUp;
			vertices[vIndex3] = newVertexDown;

			uv[vIndex2] = Vector2.zero;
			uv[vIndex3] = Vector2.zero;


		}
	}

}
