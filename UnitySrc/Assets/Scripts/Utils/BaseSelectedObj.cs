using UnityEngine;
using System;

public class BaseSelectedObj : MonoBehaviour {

	public Color curColor;

	public virtual void OnMouseEnter()
	{
		Color color = new Color(1, 0, 0, 1);
		curColor = GetComponent<Renderer>().material.color;
		GetComponent<Renderer>().material.color = color;
	}
	
	void OnMouseExit()
	{
		GetComponent<Renderer>().material.color = curColor;
	}

	public virtual void OnMouseDown()
	{
		
	}
	
	void Update () 
	{
		
	}
}