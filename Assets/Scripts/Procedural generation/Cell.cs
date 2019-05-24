using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	[SerializeField]
    private bool isLocked;

	private void Awake() 
	{
		isLocked = false;
		Debug.Log("Cell was created.");
	}

	public void SetLocked(bool isLocked)
	{
		this.isLocked = isLocked;
		
		SetMaterial();
		Debug.Log("Set object to cell");
	}

	private void SetMaterial()
	{
		Color materialColor = isLocked ? Color.red : Color.white;
		Renderer renderer = GetComponent<Renderer>();
		Material material = new Material(renderer.sharedMaterial);
		material.color = materialColor;
		renderer.sharedMaterial = material;
	}

	public bool IsLocked() 
	{
		return isLocked;
	}

}
