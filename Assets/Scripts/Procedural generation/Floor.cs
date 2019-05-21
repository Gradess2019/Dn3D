using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
public class Floor : MonoBehaviour
{
    
	[SerializeField]
	private float extent = 1f;

	[SerializeField]
	private Material material;

	private Vector3[] vertices;

	private int[] triangles;

	private Mesh mesh;

    void Start()
	{
		CreateMeshComponent();
		CreateVertices();
		CreateTriangles();
		CreateShape();
		UpdateCollider();
		SetMaterial();
	}

	private void CreateMeshComponent()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void CreateVertices()
	{
		vertices = new Vector3[]
		{
			new Vector3(extent, transform.position.y, extent),
			new Vector3(extent, transform.position.y, -extent),
			new Vector3(-extent, transform.position.y, -extent),
			new Vector3(-extent, transform.position.y, extent)
		};
	}

	private void CreateTriangles()
	{
		triangles = new int[]
		{
			0, 1, 2,
			2, 3, 0
		};
	}

	private void CreateShape()
	{
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
	}

	private void UpdateCollider()
	{
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	private void SetMaterial()
	{
		GetComponent<MeshRenderer>().material = material;
	}
}
