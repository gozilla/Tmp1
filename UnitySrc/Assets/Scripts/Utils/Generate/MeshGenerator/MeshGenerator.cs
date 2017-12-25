#if UNITY_EDITOR
#endif
using UnityEngine;

namespace Project.Utils
{
	public class MeshDirections
	{
		public Directions.Direction NextPos;
		public Vector3 Direction;

		public MeshDirections(Directions.Direction nextPos, Vector3 direction)
		{
			NextPos = nextPos;
			Direction = direction;
		}
	}

	public class MeshDimentions
	{
		public Vector3 Length;
		public Vector3 Width;
		public Vector3 Height;
		public MeshDimentions( Vector3 length, Vector3 width, Vector3 height)
		{
			Length = length;
			Width = width;
			Height = height;
		}
	}

	public class MeshGenerator : DontDestroyMonoSingleton<MeshGenerator>
	{
		public static Mesh Triangle(Vector3 vertex0, Vector3 vertex1, Vector3 vertex2)
		{
			var normal = Vector3.Cross((vertex1 - vertex0), (vertex2 - vertex0)).normalized;
			var mesh = new Mesh
			{
				vertices = new[] { vertex0, vertex1, vertex2 },
				normals = new[] { normal, normal, normal },
				uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) },
				triangles = new[] { 2, 1, 0 }
			};
			return mesh;
		}

		public static Mesh Quad(Vector3 origin, Vector3 width, Vector3 length)
		{
			var normal = Vector3.Cross(length, width).normalized;
			var mesh = new Mesh
			{
				vertices = new[] { origin, origin + length, origin + length + width, origin + width },
				normals = new[] { normal, normal, normal, normal },
				uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) },
				triangles = new[] { 2, 1, 0, 3, 2, 0 }
			};
			return mesh;
		}

		public Mesh Cube(Vector3 pos, Vector3 width, Vector3 length, Vector3 height)
		{

			var corner0 = -width / 2 - length / 2 - height / 2 + pos;
			var corner1 = width / 2 + length / 2 + height / 2 + pos;

			var combine = new CombineInstance[6];
			combine[0].mesh = Quad(corner0, length, width);
			combine[1].mesh = Quad(corner0, width, height);
			combine[2].mesh = Quad(corner0, height, length);
			combine[3].mesh = Quad(corner1, -width, -length);
			combine[4].mesh = Quad(corner1, -height, -width);
			combine[5].mesh = Quad(corner1, -length, -height);

			var mesh = new Mesh();
			mesh.CombineMeshes(combine, true, false);
			mesh.RecalculateNormals();
			return mesh;
		}
		// ѕарралелепипед
		public Mesh Ppip(Vector3 pos, Vector3 length, Vector3 width, Vector3 height)
		{

			// Ѕлижн€€ к нам грань
			// Ћева€ нижн€€ точка с которой начинаем
			Vector3 v1 = pos;
			// ѕрава€ нижн€€ точка от нул€ вправо по длинне 
			Vector3 v2 = pos + length;
			// Ћева€ верхн€€ точка
			Vector3 v3 = pos + height;
			// ѕрава€ верхн€€
			Vector3 v4 = pos + length + height;

			// ƒальн€€ от нас грань
			// Ћева€ нижн€€ точка 
			Vector3 v1_1 = pos + width;
			// ѕрава€ нижн€€ точка от нул€ вправо по длинне 
			Vector3 v1_2 = v1_1 + length;
			// Ћева€ верхн€€ точка
			Vector3 v1_3 = v1_1 + height;
			// ѕрава€ верхн€€
			Vector3 v1_4 = v1_1 + length + height;

			var combine = new CombineInstance[12];
			//ƒл€ каждой противоположной грани точки отдаем наоборот что бы поверхность рисовалась в обратную сторону
			// Ќижн€€ поверхность
			combine[0].mesh = Triangle(v3, v2, v1);
			combine[1].mesh = Triangle(v3, v4, v2);
			// ¬ерхн€€ поверхность
			combine[2].mesh = Triangle(v1_1, v1_2, v1_3);
			combine[3].mesh = Triangle(v1_2, v1_4, v1_3);

			// ѕередн€€ поверхность
			combine[4].mesh = Triangle(v1, v2, v1_1);
			combine[5].mesh = Triangle(v2, v1_2, v1_1);
			// «адн€€ поверхность
			combine[6].mesh = Triangle(v1_3, v4, v3);
			combine[7].mesh = Triangle(v1_3, v1_4, v4);

			// Ћева€ поверхность
			combine[8].mesh = Triangle(v1, v1_1, v1_3);
			combine[9].mesh = Triangle(v1_3, v3, v1);
			// ѕрава€ поверхность
			combine[10].mesh = Triangle(v2, v4, v1_4);
			combine[11].mesh = Triangle(v1_4, v1_2, v2);

			var mesh = new Mesh();
			mesh.CombineMeshes(combine, true, false);
			mesh.RecalculateNormals();
			return mesh;
		}

		public GameObject Generate(Material mat, Vector3[] vertices, Vector3 pos = new Vector3(), string meshName = "tmp")
		{
			if (mat == null)
			{
				Debug.LogError("No material attached! Aborting");
				//return null;
			}
			GameObject go = new GameObject(meshName);
			go.transform.position = pos;
			go.AddComponent<MeshFilter>();
			go.AddComponent<MeshRenderer>();

			go.GetComponent<Renderer>().material = mat;

			Mesh mesh = new Mesh();

			
			Vector2[] UVs = new Vector2[vertices.Length];
			Vector4[] tangs = new Vector4[vertices.Length];

			Vector2 uvScale = new Vector2(1 , 1 );
			Vector3 sizeScale = new Vector3(1, 1, 1);

			int index;
			for (int i = 0; i < vertices.Length; i++)
			{
				
					Vector2 cur_uv = new Vector2(vertices[i].x, vertices[i].y);
					UVs[i] = Vector2.Scale(cur_uv, uvScale);

					Vector3 leftV = new Vector3(vertices[i].x - 1, vertices[i].y - 1, vertices[i].y);
					Vector3 rightV = new Vector3(vertices[i].x+1, vertices[i].y+1, vertices[i].z);
					Vector3 tang = Vector3.Scale(sizeScale, rightV - leftV).normalized;
					tangs[i] = new Vector4(tang.x, tang.y, tang.z, 1);
			}

			mesh.vertices = vertices;
			mesh.uv = UVs;

			index = 0;
			int[] triangles = new int[vertices.Length * 3];
			for (int i = 0; i < vertices.Length; i++)
			{
				if (i < vertices.Length - 3)
				{
					triangles[index++] = i;
					triangles[index++] = i + 1;
					triangles[index++] = i + 2;
				}
				else
				{
					triangles[index++] = i;
					triangles[index++] = i;
					triangles[index++] = i;
				}
				
			}

			mesh.triangles = triangles;
			mesh.RecalculateNormals();
			mesh.tangents = tangs;
			go.GetComponent<MeshFilter>().sharedMesh = mesh;
			return go;
		}

		public GameObject GenerateCombine(Material mat, Vector3[] vertices, Vector3 pos = new Vector3(), string meshName = "tmp")
		{
			if (mat == null)
			{
				Debug.Log("No material attached!");
				//return null;
			}
			GameObject go = new GameObject(meshName);
			go.transform.position = pos;
			go.AddComponent<MeshFilter>();
			go.AddComponent<MeshRenderer>();

			go.GetComponent<Renderer>().material = mat;

			Mesh mesh = new Mesh();


			var combine = new CombineInstance[vertices.Length - 3];

			for (int i = 0; i < vertices.Length; i++)
			{
				if (i < vertices.Length - 3)
				{
					combine[i].mesh = Triangle(vertices[i], vertices[i + 1], vertices[i + 2]);
				}
			}

			mesh.CombineMeshes(combine, true, false);

			mesh.RecalculateNormals();

			go.GetComponent<MeshFilter>().sharedMesh = mesh;
			return go;
		}

		public GameObject Generate(Material mat, Mesh mesh, Vector3 pos = new Vector3(), string meshName = "tmp")
		{
			if (mat == null)
			{
				Debug.Log("No material attached!");
				//return null;
			}
			GameObject go = new GameObject(meshName);
			go.transform.position = pos;
			go.AddComponent<MeshFilter>();
			go.AddComponent<MeshRenderer>();

			go.GetComponent<Renderer>().material = mat;
			go.GetComponent<MeshFilter>().sharedMesh = mesh;

			go.AddComponent<MeshCollider>();
			go.GetComponent<MeshCollider>().convex = true;
			return go;
		}

		public GameObject GenerateRandomObject(MeshDimentions initialDimentions, MeshDirections[] directions, Material mat, Vector3 pos = new Vector3(), string meshName = "tmp")
		{
			var combine = new CombineInstance[directions.Length ];
			Vector3 curPos = new Vector3();
			Vector3 curLength = initialDimentions.Length;
			Vector3 curWidth = initialDimentions.Width;
			Vector3 curHeight = initialDimentions.Height;
			for (int i = 0; i < directions.Length; i++)
			{
				float deltaCoef = 0.0f;
				switch (directions[i].NextPos)
				{
					case Directions.Direction.Forward:
						{
							combine[i].mesh = Ppip(curPos, curLength, curWidth, curHeight);
							curPos += curHeight + new Vector3(0, 0, deltaCoef);
							curHeight = directions[i].Direction;
						}
						break;
					case Directions.Direction.Back:
						{
							curHeight = directions[i].Direction;
							curPos -= curHeight + new Vector3(0, 0, deltaCoef);
							combine[i].mesh = Ppip(curPos, curLength, curWidth, curHeight);
						}
						break;
					case Directions.Direction.Right:
						{
							combine[i].mesh = Ppip(curPos, curLength, curWidth, curHeight);
							curPos += curLength + new Vector3(deltaCoef, 0, 0);
							curLength = directions[i].Direction;
						}
						break;
					case Directions.Direction.Left:
						{
							curLength = directions[i].Direction;
							curPos -= curLength + new Vector3(deltaCoef, 0, 0);
							combine[i].mesh = Ppip(curPos, curLength, curWidth, curHeight);
						}
						break;
					case Directions.Direction.Up:
						{
							combine[i].mesh = Ppip(curPos, curLength, curWidth, curHeight);
							curPos += curWidth + new Vector3(0, deltaCoef, 0);
							curWidth = directions[i].Direction;
						}
						break;
					case Directions.Direction.Down:
						{
							curWidth = directions[i].Direction;
							curPos -= curWidth + new Vector3(0, deltaCoef, 0);
							combine[i].mesh = Ppip(curPos, curLength, curWidth, curHeight);
						}
						break;
					default:
						break;
				}

				
			}
			var mesh = new Mesh();
			mesh.CombineMeshes(combine, true, false);
			mesh.RecalculateNormals();

			return Generate(mat, mesh, pos, meshName);
		}

		public int Cut(int value)
		{
			return Mathf.Min(value, 255);
		}
	}
}