using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Overlays;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;

public class Map : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject bossNodePrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform mapCanvas;

    private Node playerNode;
    private int columns = 3, rows = 5;
    public static List<List<Node>> nodeGrid;

    public static List<List<Node>> savedMap;
    public static Node savedPlayerNode;
    public static List<Vector2> savedPrevPositions = new List<Vector2>();

    public List<String> towerMaps = new List<String>
    {
        "RockyMap",
        "GrassyMap",
        "CorruptedMap"
    };

    private void Start()
    {
        AudioManager.menumusicSound();
        if (nodeGrid == null)
        {
            playerNode = new Node(-1, columns/2, Vector2.zero, playerPrefab);
            GenerateNodeGrid();
            PopulateNodeGrid();
            InstantiateNodeGrid(nodeGrid);
            savedMap = nodeGrid; //saving nodeGrid
        }
        else
        {
            InstantiateNodeGrid(savedMap);
            LoadPlayerNode(savedPlayerNode);
            LoadPrevPositions(savedPrevPositions);
        }

    }


    private void Update()
    {

    }



    private void GenerateNodeGrid() //5x8 list of Nodes filled with Null
    {
        nodeGrid = new List<List<Node>>();
        for (int i = 0; i < rows; i++)
        {
            List<Node> row = new List<Node>();
            for (int j = 0; j < columns; j++)
            {
                row.Add(null); // Add default value
            }
            nodeGrid.Add(row);
        }
    }

    private void PopulateNodeGrid()
    {
        for (int y = 0; y < rows; y++)
        {
            int nodesOnRow = UnityEngine.Random.Range(2, columns); //2-4
            while (nodesOnRow > 0)
            {
                int x = UnityEngine.Random.Range(0, columns); //0-4 eg 5
                if (nodeGrid[y][x] == null)

                {
                    nodeGrid[y][x] = new Node(y, x, Vector2.zero, nodePrefab);
                    nodesOnRow--;
                }
            }
        }
    }

    private void InstantiateNodeGrid(List<List<Node>> grid)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (grid[row][col] == null) continue;
                GameObject nodeInstance = Instantiate(grid[row][col].nodePrefab, mapCanvas); //instantises the prefab
                RectTransform rect = nodeInstance.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2((col * 200) - (columns/2 * 200), (row * 100) - (rows/2 * 100) - 50);
                Button nodeButton = nodeInstance.GetComponent<Button>();
                int r = row; int c = col;
                nodeButton.onClick.AddListener(() => clickToMove(nodeGrid[r][c]));
                grid[row][col].mapPos = nodeInstance.transform.position;
            }
        }
        GameObject bossNodeInstance = Instantiate(bossNodePrefab, mapCanvas);
        RectTransform bossRect = bossNodeInstance.GetComponent<RectTransform>();
        bossRect.anchoredPosition = new Vector2(0, (rows * 100) - (rows / 2 * 100));
        Button bossNodeButton = bossNodeInstance.GetComponent<Button>();
        bossNodeButton.onClick.AddListener(() => clickToMove(new Node(rows,0,bossNodeInstance.transform.position,bossNodePrefab)));

    }

    public void clickToMove(Node targetNode)
    {
        //Debug.Log($"playerNoderow:{playerNode.row},targetNoderow:{targetNode.row}");
        //Debug.Log(Mathf.Abs(targetNode.column - playerNode.column));
        //Debug.Log(playerNode.row + 1);
        if ((targetNode.row == playerNode.row + 1) && (3 >= Mathf.Abs(targetNode.column-playerNode.column)))
        {
            AudioManager.swordSound();
            playerNode.row = targetNode.row;
            playerNode.column = targetNode.column;
            playerNode.mapPos = targetNode.mapPos;
            Debug.Log(playerNode.mapPos);
        }
        else if (playerNode.row == rows)
        {
            AudioManager.swordSound();
            DifficultyManager.gameDifficulty += 30;
            SceneManager.LoadScene("BossMap");
        }
        else
        {
            AudioManager.deniedSound();
            Debug.Log("Cannot move here");
            return;
        }

        if (playerNode.row != rows)
        {
            GameObject playerPrefabInstance = Instantiate(playerNode.nodePrefab, mapCanvas);
            RectTransform rect = playerPrefabInstance.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2((playerNode.column * 200) - (columns / 2 * 200), (playerNode.row * 100) - (rows / 2 * 100) - 50);

            savedPrevPositions.Add(rect.anchoredPosition);
            savedPlayerNode = playerNode;

            DifficultyManager.gameDifficulty += 10; //difficulty goes up each node chosen
            SceneManager.LoadScene(towerMaps[(int)UnityEngine.Random.Range(0, towerMaps.Count)]); //random map loaded
        }
    }

    private void LoadPrevPositions(List<Vector2> savedPrevPositions)
    {
        foreach (Vector2 position in savedPrevPositions)
        {
            GameObject playerPrefabInstance = Instantiate(playerNode.nodePrefab, mapCanvas);
            RectTransform rect = playerPrefabInstance.GetComponent<RectTransform>();
            rect.anchoredPosition = position;
        }
    }

    private void LoadPlayerNode(Node savedPlayerNode)
    {
        playerNode = savedPlayerNode;
    }
}

public class Node
{
    public int row;
    public int column;
    public Vector2 mapPos;
    public GameObject nodePrefab;

    public Node(int row, int column, Vector2 mapPos, GameObject nodePrefab)
    {
        this.row = row;
        this.column = column;
        this.mapPos = mapPos;
        this.nodePrefab = nodePrefab;
    }
}
