using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LayOutRooms : MonoBehaviour
{
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject doorwayPrefab;
    [SerializeField] private float wallHeightOffset;
    [Space]
    [Min(1)]
    [SerializeField] private int numRoomsOnFloor = 1;
    [Space]
    [SerializeField] private Vector2Int numRoomsPerDirection;
    [SerializeField] private Vector2 roomSizes;

    private readonly List<(int x, int z)> potentialRooms = new List<(int x, int z)>();
    private readonly Dictionary<GameObject, int> roomIndices = new Dictionary<GameObject, int>();
    private readonly Dictionary<(int, int), GameObject> rooms = new Dictionary<(int, int), GameObject>();
    private readonly List<Vector3> usedWallPositions = new List<Vector3>();

    private bool[,] roomLayout;
    private int roomIndex;
    private UnionFind unionFind;

    private GameObject roomParent;
    private GameObject wallsParent;

    private enum Direction
    {
        North,
        South,
        East,
        West,
    }

    public void InitializeLayout()
    {
        CreateRoomPlacements();
        roomParent = new GameObject("Rooms");
        wallsParent = new GameObject("Wall");
        unionFind = new UnionFind(numRoomsOnFloor);

        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < roomLayout.GetLength(1); j++)
            {
                if (!roomLayout[i, j])
                    continue;
                PlaceRoom(i, j);
            }
        }
        for (int i = 0; i < roomLayout.GetLength(0); i++)
        {
            for (int j = 0; j < roomLayout.GetLength(1); j++)
            {
                if (!roomLayout[i, j])
                    continue;
                PlaceWalls(i, j);
            }
        }
    }

    private void CreateRoomPlacements()
    {
        roomLayout = new bool[numRoomsPerDirection.x, numRoomsPerDirection.y];

        (int x, int z) roomCoordinates = (Random.Range(0, numRoomsPerDirection.x - 1), Random.Range(0, numRoomsPerDirection.y - 1));

        roomLayout[roomCoordinates.x, roomCoordinates.z] = true;
        AddNeighbors(roomCoordinates.x, roomCoordinates.z);

        for (int i = 0; i < numRoomsOnFloor - 1; i++)
        {
            roomCoordinates = potentialRooms[Random.Range(0, potentialRooms.Count)];
            roomLayout[roomCoordinates.x, roomCoordinates.z] = true;
            AddNeighbors(roomCoordinates.x, roomCoordinates.z);
            potentialRooms.Remove(roomCoordinates);
        }
        potentialRooms.Clear();
    }

    private void AddNeighbors(int xIndex, int zIndex)
    {
        if (xIndex < numRoomsPerDirection.x - 1)
            if (!potentialRooms.Contains((xIndex + 1, zIndex)) && !roomLayout[xIndex + 1, zIndex])
                potentialRooms.Add((xIndex + 1, zIndex));

        if (xIndex > 0)
            if (!potentialRooms.Contains((xIndex - 1, zIndex)) && !roomLayout[xIndex - 1, zIndex])
                potentialRooms.Add((xIndex - 1, zIndex));

        if (zIndex < numRoomsPerDirection.y - 1)
            if (!potentialRooms.Contains((xIndex, zIndex + 1)) && !roomLayout[xIndex, zIndex + 1])
                potentialRooms.Add((xIndex, zIndex + 1));

        if (zIndex > 0)
            if (!potentialRooms.Contains((xIndex, zIndex - 1)) && !roomLayout[xIndex, zIndex - 1])
                potentialRooms.Add((xIndex, zIndex - 1));
    }

    private void PlaceRoom(int xIndex, int zIndex)
    {
        (float x, float z) roomCoordinates = (xIndex * roomSizes.x, zIndex * roomSizes.y);
        int roomTypeIndex = Random.Range(0, roomPrefabs.Length);
        rooms[(xIndex, zIndex)] = Instantiate(roomPrefabs[roomTypeIndex], new Vector3(roomCoordinates.x, 0, roomCoordinates.z), Quaternion.identity, roomParent.transform);
        roomIndices.Add(rooms[(xIndex, zIndex)], roomIndex);
        roomIndex++;
    }

    private void PlaceWalls(int xIndex, int zIndex)
    {
        int roomIndex = roomIndices[rooms[(xIndex, zIndex)]];
        (float x, float z) coordinates = (xIndex * roomSizes.x, zIndex * roomSizes.y);
        var offsets = new Vector3(roomSizes.x / 2, wallHeightOffset, roomSizes.y / 2);

        BuildWall(() => zIndex < numRoomsPerDirection.y - 1, (() => xIndex, () => zIndex + 1), roomIndex,
            (xIndex, zIndex + 1), new Vector3(coordinates.x, 0, coordinates.z + offsets.z), Quaternion.Euler(0, 180, 0));
        BuildWall(() => zIndex > 0, (() => xIndex, () => zIndex - 1), roomIndex, 
            (xIndex, zIndex - 1), new Vector3(coordinates.x, 0, coordinates.z - offsets.z), Quaternion.Euler(0, 0, 0));
        BuildWall(() => xIndex < numRoomsPerDirection.x - 1, (() => xIndex + 1, () => zIndex), roomIndex,
            (xIndex + 1, zIndex), new Vector3(coordinates.x + offsets.x, 0, coordinates.z), Quaternion.Euler(0, 270, 0));
        BuildWall(() => xIndex > 0, (() => xIndex - 1, () => zIndex), roomIndex, 
            (xIndex - 1, zIndex), new Vector3(coordinates.x - offsets.x, 0, coordinates.z), Quaternion.Euler(0, 90, 0));
    }

    private void BuildWall(Func<bool> isRoomOnEdge, (Func<int> xIndex, Func<int> zIndex) pathway,
        int roomIndex, (int, int) roomsIndex, Vector3 wallPosition, Quaternion wallRotation)
    {
        if (usedWallPositions.Contains(wallPosition))
            return;
        bool buildPathway = false;
        if (isRoomOnEdge())
            buildPathway = roomLayout[pathway.xIndex(), pathway.zIndex()];
        int index = rooms.ContainsKey(roomsIndex) ? roomIndices[rooms[roomsIndex]] : -1;
        ReduceToHamiltonianPath(ref buildPathway, roomIndex, index);
        Instantiate(buildPathway ? doorwayPrefab : wallPrefab, wallPosition, wallRotation, wallsParent.transform);
        usedWallPositions.Add(wallPosition);
    }

    private void ReduceToHamiltonianPath(ref bool direction, int roomIndex, int neighborIndex)
    {
        if (!direction)
            return;
        if (neighborIndex == -1)
            return;
        if (unionFind.AreUnioned(roomIndex, neighborIndex))
            direction = false;
        else
            unionFind.Union(roomIndex, neighborIndex);
    }
}
