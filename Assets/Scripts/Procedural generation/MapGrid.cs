using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MapGrid : MonoBehaviour
{
	[Header("Map grid settings")]
	[SerializeField]
	[Range(1, 50)]
	private int sizeX = 1;

	[SerializeField]
	[Range(1, 50)]
	private int sizeY = 1;

	[SerializeField]
	[Range(0.1f, 16f)]
	private float extent = 1f;

	private List<List<Cell>> cellsList;

	public float Extent
	{
		get
		{
			return extent;
		}
	}

	private void Update()
	{
		if (ShouldUpdate())
		{
			DestroyOldCells();
			Build();
		}
	}

	private bool ShouldUpdate()
	{
		bool isEditorMode = !Application.IsPlaying(this);
		bool cellsArrayIsInvalid = cellsList == null || cellsList.Count == 0;
		bool sizePropertiesChanged = false;
		bool extentPropertyChanged = false;

		if (!cellsArrayIsInvalid)
		{
			sizePropertiesChanged = cellsList.Count != sizeY || cellsList[0].Count != sizeX;

			Vector3 currentScaleProperty = GetExtentScale();
			extentPropertyChanged = transform.localScale != currentScaleProperty;
		}

		return isEditorMode && (cellsArrayIsInvalid || sizePropertiesChanged || extentPropertyChanged);
	}

	private Vector3 GetExtentScale()
	{
		return new Vector3(extent, transform.localScale.y, extent);
	}

	private void DestroyOldCells()
	{
		if (cellsList != null) return;

		Cell[] otherCell = FindObjectsOfType<Cell>();
		for (int i = 0; i < otherCell.Length; i++)
		{
			DestroyImmediate(otherCell[i].gameObject);
		}
		Debug.Log("Old cells destroyed");
	}

	private void Build()
	{
		if (IsCellsListInvalid())
		{
			CreateCellsArray();
		}

		BuildGrid();
		UpdateExtentCells();
		Debug.Log("Grid was builded.");
	}

	private bool IsCellsListInvalid()
	{
		return cellsList == null;
	}

	private void CreateCellsArray()
	{
		cellsList = new List<List<Cell>>();
	}

	private void BuildGrid()
	{
		if (cellsList.Count < sizeY || cellsList[0].Count < sizeX)
		{
			GameObject sampleCell = CreateSampleCell();
			for (int y = 0; y < sizeY; y++)
			{
				if (cellsList.Count < sizeY)
				{
					cellsList.Add(new List<Cell>());
				}

				for (int x = cellsList[y].Count; x < sizeX; x++)
				{
					cellsList[y].Add(InstantiateNewCell(sampleCell, x, y));
					if (IsBound(y, x))
					{
						cellsList[y][x].SetLocked(true);
					}

					SetCellUnlocked(cellsList[y].Count > 1, y, x - 1);
					SetCellUnlocked(cellsList.Count > 1 && y > 0, y - 1, x);
				}
			}

			DestroyImmediate(sampleCell);
		}
		else if (cellsList.Count > sizeY || cellsList[0].Count > sizeX)
		{
			for (int y = cellsList.Count - 1; y >= 0; y--)
			{
				int currentSizeX = cellsList[y].Count;
				if (y >= sizeY)
				{
					for (int x = 0; x < currentSizeX; x++)
					{
						DestroyImmediate(cellsList[y][x].gameObject);
						cellsList[y - 1][x].SetLocked(true);
					}
					cellsList.RemoveAt(y);
				}
				else
				{
					for (int x = currentSizeX - 1; x >= sizeX; x--)
					{
						DestroyImmediate(cellsList[y][x].gameObject);
						cellsList[y].RemoveAt(x);
					}
					cellsList[y][sizeX - 1].SetLocked(true);
				}
			}
		}
	}

	private void SetCellUnlocked(bool checkState, int y, int x)
	{
		if (checkState && cellsList[y][x] != null && !IsBound(y, x))
		{
			cellsList[y][x].SetLocked(false);
		}
	}

	private GameObject CreateSampleCell()
	{
		GameObject sampleCell = GameObject.CreatePrimitive(PrimitiveType.Quad);
		sampleCell.isStatic = true;
		DestroyImmediate(sampleCell.GetComponent<Collider>());

		// Uncomment in release
		// DestroyImmediate(sampleCell.GetComponent<MeshRenderer>());

		sampleCell.transform.position = transform.position;
		sampleCell.transform.localScale *= extent;
		sampleCell.AddComponent<Cell>();

		return sampleCell;
	}

	private Cell InstantiateNewCell(GameObject sampleCell, int x, int y)
	{
		Debug.Log("New cell instantiation started.");
		Vector3 cellPosition = CalculateNewCellPosition(x, y);
		Quaternion cellRotation = CalculatenewCellRotation();
		GameObject newCell = Instantiate(sampleCell, cellPosition, cellRotation);
		AttachToParent(newCell);
		return newCell.GetComponent<Cell>();
	}

	private Vector3 CalculateNewCellPosition(int x, int y)
	{
		return new Vector3(transform.position.x + x * extent, transform.position.y, transform.position.z + y * extent);
	}

	private Quaternion CalculatenewCellRotation()
	{
		return Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + 90, 0, 0));
	}

	private void AttachToParent(GameObject gameObject)
	{
		gameObject.transform.parent = transform;
	}

	private bool IsBound(int y, int x)
	{
		return y == 0 || y == sizeY - 1 || x == 0 || x == sizeX - 1;
	}

	private void UpdateExtentCells()
	{
		transform.localScale = GetExtentScale();
	}
}
