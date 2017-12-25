using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utils
{
	[Serializable]
	public class MaterialForList
	{
		public string Name;
		public Material material;
	}

	public class Directions
	{
		public enum Direction
		{
			Up,
			Down,
			Left,
			Right,
			Forward,
			Back,
			None
		}

		public static Direction RevertDirection(Direction dir)
		{
			Direction revertDir = dir;
			if ((int)dir % 2 == 1)
				revertDir--;
			else
				revertDir++;
			return revertDir;
		}
		
	}

	

	public static class MathUtil
	{
		public static float GetAngle ( Vector2 from, Vector2 to )
		{
			float angle = Vector2.Angle ( from, to );
			Vector3 cross = Vector3.Cross ( from, to );
			
			if ( cross.z > 0 )
				angle = 360 - angle;
			
			return angle;
		}

		public static float GetAngle ( float x, float y )
		{
			float angle = Mathf.Atan2 ( y, x ) * 57.295f;
			if ( angle < 0 )
				angle = 360 + angle;

			return angle;
		}

		public static Vector2 Rotate ( Vector2 vector, float angle )
		{
			float directionAngle = angle / 180.0f * Mathf.PI;
			float x = vector.x * Mathf.Cos ( directionAngle ) - vector.y * Mathf.Sin ( directionAngle );
			float y = vector.x * Mathf.Sin ( directionAngle ) + vector.y * Mathf.Cos ( directionAngle );

			return new Vector2 ( x, y );
		}

		private static bool AreClockwise ( Vector3 v1, Vector3 v2 ) 
		{
			return -v1.x * v2.z + v1.z * v2.x > 0;
		}
		
		private static bool IsWithinRadius ( Vector3 v, float radius ) 
		{
			return v.x * v.x + v.y * v.y <= radius * radius;
		}

		public static bool IsInsideSector ( Vector3 basePosition, Vector3 targetPosition, float baseAngle, float sectorAngle, float radius )
		{
			if ( sectorAngle <= 180 )
			{
				float currentAngle = ( baseAngle - sectorAngle / 2 ) * Mathf.Deg2Rad;
				Vector3 sectorStart = new Vector3 ( radius * Mathf.Sin (currentAngle), 0, radius * Mathf.Cos (currentAngle));
				
				currentAngle = (baseAngle + sectorAngle / 2) * Mathf.Deg2Rad;
				Vector3 sectorEnd = new Vector3 ( radius * Mathf.Sin (currentAngle), 0, radius * Mathf.Cos (currentAngle));
				
				return IsInsideSector ( targetPosition, basePosition, sectorStart, sectorEnd, radius );
			}
			else
			{
				baseAngle = baseAngle >= 180 ? baseAngle - 180 : baseAngle + 180;
				return !IsInsideSector ( basePosition, targetPosition, baseAngle, 360 - sectorAngle, radius );
			}
		}
		
		private static  bool IsInsideSector ( Vector3 point, Vector3 center, Vector3 sectorStart, Vector3 sectorEnd, float radius ) 
		{
			Vector3 relPoint = point - center;
			
			return AreClockwise ( sectorStart, relPoint ) &&
				!AreClockwise ( sectorEnd, relPoint ) &&
					IsWithinRadius ( relPoint, radius );
		}

		public static Material GetMatrialByName(string name, List<MaterialForList> materials)
		{
			foreach (MaterialForList item in materials)
			{
				if (item.Name == name)
				{
					return item.material;
				}
			}
			return null;
		}
	}
}

