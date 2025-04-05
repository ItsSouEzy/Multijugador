using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [Header("Level Settings")]
    public int width = 50; // Ancho del nivel
    public int length = 50; // Largo del nivel
    public int maxHeight = 3; // Altura m�xima de los obst�culos
    public float roomSize = 1f; // Tama�o de cada celda del nivel

    [Header("Prefabs")]
    public GameObject floorPrefab; // Prefab del suelo
    public GameObject wallPrefab; // Prefab de las paredes
    public GameObject ceilingPrefab; // Prefab del techo
    public GameObject[] obstaclePrefabs; // Prefabs de los obst�culos

    [Header("Generation Settings")]
    [Range(0, 100)]
    public int obstaclePercentage = 20; // Porcentaje de generaci�n de obst�culos
    public bool generateCeiling = true; // Indica si se debe generar el techo


    [Header("Items Settings")]
    [Range(0, 100)]
    public int itemSpawnPercentage = 10; // Porcentaje de generaci�n de �tems
    public GameObject[] itemPrefabs; // Prefabs de los �tems
    public float minItemSpacing = 3f; // Espacio m�nimo entre �tems

    [Header("Enemy Settings")]
    [Range(0, 100)]
    public int enemySpawnPercentage = 15; // Porcentaje de generaci�n de enemigos
    public GameObject[] enemyPrefabs; // Prefabs de los enemigos
    public float minEnemySpacing = 5f; // Espacio m�nimo entre enemigos
    private List<Vector2> occupiedPositions = new List<Vector2>(); // Lista de posiciones ocupadas

    // Sistema de grid para rastrear objetos
    private Dictionary<Vector2Int, List<GameObject>> gridObjects = new Dictionary<Vector2Int, List<GameObject>>();
    private Dictionary<Vector2Int, BlockType> gridBlockTypes = new Dictionary<Vector2Int, BlockType>();

    public enum BlockType
    {
        Empty, // Bloque vac�o
        Floor, // Bloque de suelo
        Obstacle, // Bloque de obst�culo
        Wall, // Bloque de pared
        Item, // Bloque de �tem
        Enemy // Bloque de enemigo
    }



    private void Start()
    {
        GenerateLevel();// Generar el nivel al iniciar
    }

    
        void GenerateLevel()
        {
            occupiedPositions.Clear();
            gridObjects.Clear();
            gridBlockTypes.Clear();
            GameObject levelContainer = new GameObject("GeneratedLevel");
            levelContainer.transform.parent = transform;

            // Generar suelo y objetos
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    Vector3 position = new Vector3(x * roomSize, 0, z * roomSize);
                    GameObject floor = Instantiate(floorPrefab, position, Quaternion.identity);
                    floor.transform.parent = levelContainer.transform;

                    AddToGrid(new Vector2Int(x, z), floor, BlockType.Floor);

                    // Generar obst�culos
                    if (Random.Range(0, 100) < obstaclePercentage)
                    {
                        GenerateObstacle(x, z, levelContainer);
                        occupiedPositions.Add(new Vector2(x, z));
                    }
                    // Intentar generar item si no hay obst�culo
                    else if (Random.Range(0, 100) < itemSpawnPercentage)
                    {
                        TrySpawnItem(x, z, levelContainer);
                    }
                    // Intentar generar enemigo
                    else if (Random.Range(0, 100) < enemySpawnPercentage)
                    {
                        TrySpawnEnemy(x, z, levelContainer);
                    }
                }
            }

            // Generar paredes exteriores
            GenerateWalls(levelContainer);

            // Generar techo si est� activado
            if (generateCeiling)
            {
                GenerateCeiling(levelContainer);
            }
        }
    


    private void GenerateObstacle(int x, int z, GameObject container)
    {
        float height = Random.Range(1, maxHeight);
        for (int y = 1; y <= height; y++)
        {
            Vector3 obstaclePos = new Vector3(x * roomSize, y * roomSize, z * roomSize);
            GameObject obstacle = Instantiate(
                obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
                obstaclePos,
                Quaternion.identity
            );
            obstacle.transform.parent = container.transform;
            AddToGrid(new Vector2Int(x, z), obstacle, BlockType.Obstacle);
        }
    }


    private bool TrySpawnItem(int x, int z, GameObject container)
    {
        // Verificar si hay espacio suficiente
        if (IsPositionValid(x, z, minItemSpacing))
        {
            Vector3 itemPosition = new Vector3(x * roomSize, roomSize * 1.8f, z * roomSize);
            GameObject item = Instantiate(
                itemPrefabs[Random.Range(0, itemPrefabs.Length)],
                itemPosition,
                Quaternion.identity
            );
            item.transform.parent = container.transform;
            occupiedPositions.Add(new Vector2(x, z));
            AddToGrid(new Vector2Int(x, z), item, BlockType.Item);
            return true;
        }
        return false;
    }

    private bool TrySpawnEnemy(int x, int z, GameObject container)
    {
        if (IsPositionValid(x, z, minEnemySpacing))
        {
            // Ajustamos la altura a 0.1f para que est� justo sobre el suelo
            Vector3 enemyPosition = new Vector3(x * roomSize, 1.5f, z * roomSize);
            GameObject enemy = Instantiate(
                enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                enemyPosition,
                Quaternion.identity
            );
            enemy.transform.parent = container.transform;
            occupiedPositions.Add(new Vector2(x, z));
            AddToGrid(new Vector2Int(x, z), enemy, BlockType.Enemy);
            return true;
        }
        return false;
    }



    private void AddToGrid(Vector2Int gridPosition, GameObject obj, BlockType type)
    {
        if (!gridObjects.ContainsKey(gridPosition))
        {
            gridObjects[gridPosition] = new List<GameObject>();
        }
        gridObjects[gridPosition].Add(obj);
        gridBlockTypes[gridPosition] = type;
    }


    // M�todo para obtener objetos en una posici�n espec�fica
    public List<GameObject> GetObjectsAt(Vector2Int position)
    {
        return gridObjects.ContainsKey(position) ? gridObjects[position] : new List<GameObject>();
    }


    // M�todo para obtener el tipo de bloque en una posici�n
    public BlockType GetBlockTypeAt(Vector2Int position)
    {
        return gridBlockTypes.ContainsKey(position) ? gridBlockTypes[position] : BlockType.Empty;
    }

    // M�todo para obtener objetos en un radio espec�fico
    public Dictionary<Vector2Int, List<GameObject>> GetObjectsInRadius(Vector2Int center, int radius)
    {
        Dictionary<Vector2Int, List<GameObject>> result = new Dictionary<Vector2Int, List<GameObject>>();

        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                Vector2Int checkPos = new Vector2Int(center.x + x, center.y + z);
                if (gridObjects.ContainsKey(checkPos))
                {
                    result[checkPos] = gridObjects[checkPos];
                }
            }
        }

        return result;
    }

    private bool IsPositionValid(int x, int z, float minSpacing)
    {
        foreach (Vector2 pos in occupiedPositions)
        {
            float distance = Vector2.Distance(new Vector2(x, z), pos);
            if (distance < minSpacing)
            {
                return false;
            }
        }
        return true;
    }



    void GenerateWalls(GameObject container)
    {
        // Paredes en X
        for (int x = -1; x <= width; x++)
        {
            for (int y = 0; y < maxHeight; y++)
            {
                Vector3 pos1 = new Vector3(x * roomSize, y * roomSize, -1 * roomSize);
                Vector3 pos2 = new Vector3(x * roomSize, y * roomSize, length * roomSize);

                Instantiate(wallPrefab, pos1, Quaternion.identity).transform.parent = container.transform;
                Instantiate(wallPrefab, pos2, Quaternion.identity).transform.parent = container.transform;
                AddToGrid(new Vector2Int(x, -1), wallPrefab, BlockType.Wall);
                AddToGrid(new Vector2Int(x, length), wallPrefab, BlockType.Wall);
            }
        }

        // Paredes en Z
        for (int z = -1; z <= length; z++)
        {
            for (int y = 0; y < maxHeight; y++)
            {
                Vector3 pos1 = new Vector3(-1 * roomSize, y * roomSize, z * roomSize);
                Vector3 pos2 = new Vector3(width * roomSize, y * roomSize, z * roomSize);

                Instantiate(wallPrefab, pos1, Quaternion.identity).transform.parent = container.transform;
                Instantiate(wallPrefab, pos2, Quaternion.identity).transform.parent = container.transform;
                AddToGrid(new Vector2Int(-1, z), wallPrefab, BlockType.Wall);
                AddToGrid(new Vector2Int(width, z), wallPrefab, BlockType.Wall);
            }
        }
    }

    void GenerateCeiling(GameObject container)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                Vector3 position = new Vector3(x * roomSize, maxHeight * roomSize, z * roomSize);
                GameObject ceiling = Instantiate(ceilingPrefab, position, Quaternion.identity);
                ceiling.transform.parent = container.transform;
                AddToGrid(new Vector2Int(x, z), ceiling, BlockType.Empty);
            }
        }
    }
}
