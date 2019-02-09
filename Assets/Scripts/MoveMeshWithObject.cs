using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMeshWithObject : MonoBehaviour {

	public GameObject Target;
	private Vector3 TargetLastPosition;
	
	private MeshFilter ThisMeshFilter;
	private Mesh ThisMesh;
	private Vector3[] OriginalVertices;
	private float Scale;

	// Use this for initialization
	void Start () {
		ThisMeshFilter = this.GetComponent<MeshFilter>();
		ThisMesh = ThisMeshFilter.mesh;
		OriginalVertices = new Vector3[ThisMesh.vertices.Length];
		ThisMesh.vertices.CopyTo(OriginalVertices, 0);
		TargetLastPosition = Target.transform.position;
		Scale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3[] vertices = ThisMesh.vertices;
		for (int i=0; i<vertices.Length; i++) {
			vertices[i] = OriginalVertices[i] + Target.transform.position/Scale;
			vertices[i].y = 0;
		}
		ThisMesh.vertices = vertices;
	}
}
