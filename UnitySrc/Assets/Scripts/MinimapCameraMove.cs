using UnityEngine;
using System.Collections;

public class MinimapCameraMove : MonoBehaviour {
	
	public GameObject heroObj;
	
	void Start () {
	
	}
	
	
	void Update () 
	{
		transform.localPosition = new Vector3(heroObj.transform.position.x, 10, heroObj.transform.position.z);
	}
}
