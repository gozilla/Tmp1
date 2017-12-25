using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Project.Utils;

namespace Project.Utils
{

	public class ReSerializable
	{
		
	}

	[System.Serializable]
	public class Vector3Serializable
	{
		public float _x;
		public float _y;
		public float _z;

		public Vector3Serializable(float x, float y)
		{
			_x = x;
			_y = y;
		}
		public Vector3Serializable(float x, float y, float z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public Vector3 GetVector3()
		{
			Vector3 vector3 = new Vector3(_x, _y, _z);
			return vector3;
		}

	}

	[System.Serializable]
	public class TreeInstanceSerializable
	{
		[SerializeField]
		public float _heightScale;
		[SerializeField]
		public Vector3Serializable _position;
		[SerializeField]
		public int _prototypeIndex;
		[SerializeField]
		public float _rotation;
		[SerializeField]
		public float _widthScale;

		public TreeInstanceSerializable(float heightScale, float widthScale, Vector3Serializable position, float rotation, int prototypeIndex)
		{
			_heightScale = heightScale;
			_widthScale = widthScale;
			_position = position;
			_rotation = rotation;
			_prototypeIndex = prototypeIndex;
		}

		public TreeInstanceSerializable(TreeInstance treeInstance)
		{
			_heightScale = treeInstance.heightScale;
			_widthScale = treeInstance.widthScale;
			_position = new Vector3Serializable(treeInstance.position.x, treeInstance.position.y, treeInstance.position.z);
			_rotation = treeInstance.rotation;
			_prototypeIndex = treeInstance.prototypeIndex;
		}

		public TreeInstanceSerializable(float heightScale)
		{
			_heightScale = heightScale;
		}

		public TreeInstance GetTreeInstance()
		{
			TreeInstance treeInstance = new TreeInstance();
			treeInstance.heightScale = _heightScale;
			treeInstance.widthScale = _widthScale;
			treeInstance.position = _position.GetVector3();
			treeInstance.rotation = _rotation;
			treeInstance.prototypeIndex = _prototypeIndex;
			return treeInstance;
		}
		
	}

}
