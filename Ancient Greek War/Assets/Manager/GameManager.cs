using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class GameManager : MonoBehaviour
{
    public int boardSize = 7;
    public int turn;

    public GameTile[][] board;

    public GameObject boardManager;
    public GameObject userInterfaceManager;

    public GameUnit initInfantry = new GameUnit();
    public GameUnit initMusketeer = new GameUnit();

    public Dictionary<string, int> unitDictionary;

    float timer = 0;
    int currSeq = 1;
    int prevSeq = 0;
    int playerA = 5;
    bool isButtonActive = false;


    void InitBoard()
    {
        turn = 0;

        board = new GameTile[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            board[i] = new GameTile[boardSize];
        }

        // First, wipe out everything.
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i][j].property = 0;
                board[i][j].isBuilt = false;

                board[i][j].units = new GameUnit[20];
                board[i][j].unitCount = 0;

                board[i][j].distance = new int[7];
                board[i][j].direction = new string[7];
            }
        }

        // Then, place me and opponent.
        board[0][0].property = 1;
        board[0][0].structure.property = 1;
        board[0][0].structure.buildingType = "Polis";
        board[0][0].structure.generate = "Infantry";
        board[0][0].structure.counter = 1;
        board[0][0].isBuilt = true;

        board[6][6].property = 2;
        board[6][6].structure.property = 2;
        board[6][6].structure.buildingType = "Polis";
        board[6][6].structure.generate = "Infantry";
        board[6][6].structure.counter = 1;
        board[6][6].isBuilt = true;

        // Then, board the terrain.
        char[,] boardTerrain =
        {
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'M','M','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','W','G','G','G','G'},
            {'G','G','W','G','G','G','G'}
        };

        string currTerrain;
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                switch (boardTerrain[j, i])
                {
                    case 'G':
                        currTerrain = "Ground";
                        break;
                    case 'W':
                        currTerrain = "Water";
                        break;
                    case 'M':
                        currTerrain = "Mountain";
                        break;
                    default:
                        currTerrain = "Ground";
                        break;
                }

                board[i][6 - j].terrain = currTerrain;
                boardManager.SendMessage("ChangeTile", i.ToString() + "," + (6 - j).ToString() + "," + currTerrain);
            }
        }

        // Then, place neutral units
        board[2][2].property = 3;
        board[2][2].structure.property = 3;
        board[2][2].structure.buildingType = "Polis";
        board[2][2].structure.generate = "Infantry";
        board[2][2].structure.counter = 1;
        board[2][2].isBuilt = true;

        board[4][4].property = 4;
        board[4][4].structure.property = 4;
        board[4][4].structure.buildingType = "Polis";
        board[4][4].structure.generate = "Musketeer";
        board[4][4].structure.counter = 1;
        board[4][4].isBuilt = true;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                for (int k = 0; k < board[i][j].unitCount; k++)
                {
                    boardManager.SendMessage("CreateUnit", i.ToString() + "," + j.ToString() + "," + k.ToString() + "," + board[i][j].units[k].unitType + "," + board[i][j].units[k].property);
                }

                if (board[i][j].isBuilt)
                    boardManager.SendMessage("CreateBuilding", i.ToString() + "," + j.ToString() + "," + board[i][j].structure.buildingType + "," + board[i][j].property);
            }
        }
    }

    void UpdateBoard()
    {
        turn += 1;

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                for (int k = 0; k < board[i][j].unitCount; k++)
                {
                    boardManager.SendMessage("DestroyUnit", i.ToString() + "," + j.ToString() + "," + k.ToString());
                }

                if (board[i][j].isBuilt)
                    boardManager.SendMessage("DestroyBuilding", i.ToString() + "," + j.ToString());
            }
        }

        UpdateLookupTable(1);
        UpdateLookupTable(2);
        UpdateLookupTable(3);
        UpdateLookupTable(4);

        GenerateUnit();

        UpdateUnit(1);
        UpdateUnit(2);
        UpdateUnit(3);
        UpdateUnit(4);

        Battlements();

        CollapseBuilding();

        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                for (int k = 0; k < board[i][j].unitCount; k++)
                {
                    boardManager.SendMessage("CreateUnit", i.ToString() + "," + j.ToString() + "," + k.ToString() + "," + board[i][j].units[k].unitType + "," + board[i][j].units[k].property);
                    boardManager.SendMessage("SetUnitHealth", i.ToString() + "," + j.ToString() + "," + k.ToString() + "," + ((float)board[i][j].units[k].currentHP / 2 / (float)board[i][j].units[k].unitHP));
                }

                if (board[i][j].isBuilt)
                    boardManager.SendMessage("CreateBuilding", i.ToString() + "," + j.ToString() + "," + board[i][j].structure.buildingType + "," + board[i][j].property);
            }
        }
    }

    //added 10/10
    bool moveable(int i, int j)
    {
        //tile is movable if it is not Mountain or Water
        if (board[i][j].terrain == "Water" || board[i][j].terrain == "Mountain")
        {
            //UnityEngine.Debug.Log(i.ToString() + "," + j.ToString() + ", " + board[i][j].terrain + "is not walkable");
            return false;
        }

        return true;
    }

    void UpdateLookupTable(int targetProperty)
    {
        // Clear the direction and distance
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i][j].distance[targetProperty] = 0;
                board[i][j].direction[targetProperty] = "";
            }
        }

        // Then do the first iteration
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int property = board[i][j].property;

                if (property != targetProperty || !moveable(i, j))
                    continue;

                // Up
                if (j < boardSize - 1 && property != board[i][j + 1].property && moveable(i, j + 1))
                {
                    board[i][j].distance[targetProperty] = 1;
                    board[i][j].direction[targetProperty] += "U";
                }

                // Down
                if (j > 0 && property != board[i][j - 1].property && moveable(i, j - 1))
                {
                    board[i][j].distance[targetProperty] = 1;
                    board[i][j].direction[targetProperty] += "D";
                }

                // Left
                if (i > 0 && property != board[i - 1][j].property && moveable(i - 1, j))
                {
                    board[i][j].distance[targetProperty] = 1;
                    board[i][j].direction[targetProperty] += "L";
                }

                // Right
                if (i < boardSize - 1 && property != board[i + 1][j].property && moveable(i + 1, j))
                {
                    board[i][j].distance[targetProperty] = 1;
                    board[i][j].direction[targetProperty] += "R";
                }
            }
        }

        // Then continue until nothing changes
        int changeCount = 1;
        while (changeCount > 0)
        {
            changeCount = 0;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int property = board[i][j].property;

                    if (property == targetProperty && board[i][j].distance[targetProperty] == 0 && moveable(i, j))
                    {
                        int minDistance = 35;
                        // Check 4 directions to find out the shortest, then determine the way.
                        // Up
                        if (j < boardSize - 1 && board[i][j + 1].distance[targetProperty] != 0)
                        {
                            if (minDistance == board[i][j + 1].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "U";
                            }
                            else if (minDistance > board[i][j + 1].distance[targetProperty])
                            {
                                minDistance = board[i][j + 1].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "U";
                            }
                        }
                        // Down
                        if (j > 0 && board[i][j - 1].distance[targetProperty] != 0)
                        {
                            if (minDistance == board[i][j - 1].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "D";
                            }
                            else if (minDistance > board[i][j - 1].distance[targetProperty])
                            {
                                minDistance = board[i][j - 1].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "D";
                            }
                        }
                        // Right
                        if (i < boardSize - 1 && board[i + 1][j].distance[targetProperty] != 0)
                        {
                            if (minDistance == board[i + 1][j].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "R";
                            }
                            else if (minDistance > board[i + 1][j].distance[targetProperty])
                            {
                                minDistance = board[i + 1][j].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "R";
                            }
                        }
                        // Left
                        if (i > 0 && board[i - 1][j].distance[targetProperty] != 0)
                        {
                            if (minDistance == board[i - 1][j].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "L";
                            }
                            else if (minDistance > board[i - 1][j].distance[targetProperty])
                            {
                                minDistance = board[i - 1][j].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "L";
                            }
                        }

                        // Update distance
                        if (minDistance != 35)
                        {
                            board[i][j].distance[targetProperty] = minDistance + 1;
                            changeCount++;
                        }
                    }
                }
            }
        }

        // Negative iteration
        changeCount = 1;
        while (changeCount > 0)
        {
            changeCount = 0;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int property = board[i][j].property;

                    if (property != targetProperty && board[i][j].distance[targetProperty] == 0 && moveable(i, j))
                    {
                        int maxDistance = -35;

                        // Check 4 directions to find out the shortest, then determine the way.
                        // Up
                        if (j < boardSize - 1 && board[i][j + 1].distance[targetProperty] != 0)
                        {
                            if (maxDistance == board[i][j + 1].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "U";
                            }
                            else if (maxDistance < board[i][j + 1].distance[targetProperty])
                            {
                                maxDistance = board[i][j + 1].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "U";
                            }
                        }
                        // Down
                        if (j > 0 && board[i][j - 1].distance[targetProperty] != 0)
                        {
                            if (maxDistance == board[i][j - 1].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "D";
                            }
                            else if (maxDistance < board[i][j - 1].distance[targetProperty])
                            {
                                maxDistance = board[i][j - 1].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "D";
                            }
                        }
                        // Right
                        if (i < boardSize - 1 && board[i + 1][j].distance[targetProperty] != 0)
                        {
                            if (maxDistance == board[i + 1][j].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "R";
                            }
                            else if (maxDistance < board[i + 1][j].distance[targetProperty])
                            {
                                maxDistance = board[i + 1][j].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "R";
                            }
                        }
                        // Left
                        if (i > 0 && board[i - 1][j].distance[targetProperty] != 0)
                        {
                            if (maxDistance == board[i - 1][j].distance[targetProperty])
                            {
                                board[i][j].direction[targetProperty] += "L";
                            }
                            else if (maxDistance < board[i - 1][j].distance[targetProperty])
                            {
                                maxDistance = board[i - 1][j].distance[targetProperty];
                                board[i][j].direction[targetProperty] = "L";
                            }
                        }

                        // Update distance
                        if (maxDistance != -35)
                        {
                            if (maxDistance == 1)
                            {
                                board[i][j].distance[targetProperty] = -1;
                            }
                            else
                            {
                                board[i][j].distance[targetProperty] = maxDistance - 1;
                            }

                            changeCount++;
                        }

                    }
                }
            }
        }

        // Now, handle with negative tiles directions.
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int property = board[i][j].property;

                if (property != targetProperty)
                {
                    // Check minDistance tiles around it and get the directions
                    int minDistance = 35;

                    // Up
                    if (j < 6 && board[i][j + 1].distance[targetProperty] != 0)
                    {
                        if (minDistance == board[i][j + 1].distance[targetProperty])
                        {
                            board[i][j].direction[targetProperty] += "U";
                        }
                        else if (minDistance > board[i][j + 1].distance[targetProperty])
                        {
                            minDistance = board[i][j + 1].distance[targetProperty];
                            board[i][j].direction[targetProperty] = "U";
                        }
                    }
                    // Down
                    if (j > 0 && board[i][j - 1].distance[targetProperty] != 0)
                    {
                        if (minDistance == board[i][j - 1].distance[targetProperty])
                        {
                            board[i][j].direction[targetProperty] += "D";
                        }
                        else if (minDistance > board[i][j - 1].distance[targetProperty])
                        {
                            minDistance = board[i][j - 1].distance[targetProperty];
                            board[i][j].direction[targetProperty] = "D";
                        }
                    }
                    // Right
                    if (i < 6 && board[i + 1][j].distance[targetProperty] != 0)
                    {
                        if (minDistance == board[i + 1][j].distance[targetProperty])
                        {
                            board[i][j].direction[targetProperty] += "R";
                        }
                        else if (minDistance > board[i + 1][j].distance[targetProperty])
                        {
                            minDistance = board[i + 1][j].distance[targetProperty];
                            board[i][j].direction[targetProperty] = "R";
                        }
                    }
                    // Left
                    if (i > 0 && board[i - 1][j].distance[targetProperty] != 0)
                    {
                        if (minDistance == board[i - 1][j].distance[targetProperty])
                        {
                            board[i][j].direction[targetProperty] += "L";
                        }
                        else if (minDistance > board[i - 1][j].distance[targetProperty])
                        {
                            minDistance = board[i - 1][j].distance[targetProperty];
                            board[i][j].direction[targetProperty] = "L";
                        }
                    }

                    // Updating the distances are enough. quitting.
                }
            }
        }
    }

    void GenerateUnit()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                // For every tile, check if the tile has Polis structure.
                if (board[i][j].isBuilt && board[i][j].structure.buildingType == "Polis")
                {
                    // Then, check the counter to make sure the unit is generated.
                    board[i][j].structure.counter -= 1;

                    if (board[i][j].structure.counter == 0)
                    {
                        // If generated, add the corresponding unit to the list
                        GameUnit referenceUnit;

                        switch (board[i][j].structure.generate)
                        {
                            case "Infantry":
                                referenceUnit = initInfantry;
                                break;
                            case "Musketeer":
                                referenceUnit = initMusketeer;
                                break;
                            default:
                                referenceUnit = initInfantry;
                                break;
                        }

                        GameUnit newUnit = new GameUnit();

                        // If there is not corresponding group
                        // Create new one
                        newUnit.property = board[i][j].structure.property;
                        newUnit.unitType = referenceUnit.unitType;
                        newUnit.damage = referenceUnit.damage;
                        newUnit.range = referenceUnit.range;
                        newUnit.currentHP = referenceUnit.currentHP;
                        newUnit.unitHP = referenceUnit.unitHP;

                        board[i][j].units[board[i][j].unitCount] = newUnit;
                        board[i][j].unitCount += 1;

                        // Then reset the counter value according to unit.
                        board[i][j].structure.counter = unitDictionary[referenceUnit.unitType];
                    }
                }
            }
        }
    }

    void UpdateUnit(int targetProperty)
    {
        int threshold = 2;

        for (int currDistance = 2 * (boardSize - 1); currDistance >= -2 * (boardSize - 1); currDistance--)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int unitCount = 0;

                    for (int u = 0; u < board[i][j].unitCount; u++)
                        if (board[i][j].units[u].property == targetProperty)
                            unitCount++;

                    if (unitCount > threshold)
                    {
                        int diff = unitCount - threshold;

                        int dividend = 0;
                        int divisor = board[i][j].direction[targetProperty].Length;

                        for (int u = 0; u < board[i][j].unitCount && diff > 0; u++)
                        {
                            GameUnit unitMoved = board[i][j].units[u];

                            if (unitMoved.property != targetProperty)
                                continue;

                            switch (board[i][j].direction[targetProperty][(dividend++) % divisor])
                            {
                                case 'U':
                                    board[i][j + 1].units[board[i][j + 1].unitCount] = unitMoved;
                                    board[i][j + 1].unitCount++;
                                    break;
                                case 'D':
                                    board[i][j - 1].units[board[i][j - 1].unitCount] = unitMoved;
                                    board[i][j - 1].unitCount++;
                                    break;
                                case 'L':
                                    board[i - 1][j].units[board[i - 1][j].unitCount] = unitMoved;
                                    board[i - 1][j].unitCount++;
                                    break;
                                case 'R':
                                    board[i + 1][j].units[board[i + 1][j].unitCount] = unitMoved;
                                    board[i + 1][j].unitCount++;
                                    break;
                            }

                            for (int k = u + 1; k < board[i][j].unitCount; k++)
                            {
                                GameUnit unitNext = board[i][j].units[k];

                                board[i][j].units[k - 1] = unitNext;
                            }

                            u -= 1;
                            diff -= 1;
                            board[i][j].unitCount -= 1;
                        }
                    }
                }
            }
        }
    }

    void Battlements()
    {
        // Find tile with 2 or more property units
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                bool isBattle = false;

                if (board[i][j].unitCount == 0) { continue; }

                int zeroProperty = board[i][j].units[0].property;

                for (int u = 1; u < board[i][j].unitCount; u++)
                {
                    if (board[i][j].units[u].property != zeroProperty)
                    {
                        isBattle = true;
                        break;
                    }
                }

                if (isBattle == true)
                {
                    int[] separateCount = new int[7];
                    GameUnit[][] propertyUnit = new GameUnit[7][];

                    for (int p = 0; p < 7; p++)
                    {
                        propertyUnit[p] = new GameUnit[board[i][j].unitCount];
                        separateCount[p] = 0;
                    }

                    for (int u = 0; u < board[i][j].unitCount; u++)
                    {
                        int currentProperty = board[i][j].units[u].property;
                        propertyUnit[currentProperty][separateCount[currentProperty]] = board[i][j].units[u];
                        separateCount[currentProperty]++;
                    }

                    for (int p = 0; p < 7; p++)
                    {
                        Array.Sort(propertyUnit[p], (a, b) => ((a.range < b.range) || ((a.range == b.range) && (a.currentHP < b.currentHP))) ? 1 : -1);
                    }

                    bool isOver = false;
                    int maxRange = 0;

                    for (int p = 0; p < 7; p++)
                    {
                        if (separateCount[p] > 0 && propertyUnit[p][0].range > maxRange)
                        {
                            maxRange = propertyUnit[p][0].range;
                        }
                    }

                    while (isOver != true)
                    {
                        // Do it for each property
                        for (int p = 0; p < 7; p++)
                        {
                            // Find all units with maximum range
                            for (int currentIndex = 0; currentIndex < separateCount[p]; currentIndex++)
                            {
                                if (propertyUnit[p][currentIndex].range == maxRange)
                                {

                                    // Find the target with minimum range, minimum health.
                                    int targetProperty = -1;
                                    int targetIndex = -1;
                                    int targetRange = 500;
                                    int targetHP = 500;

                                    for (int t = 0; t < 7; t++)
                                    {
                                        // If the same property, skip
                                        if (t == p) { continue; }

                                        // If no units in the target property, skip
                                        else if (separateCount[t] == 0) { continue; }

                                        for (int nextIndex = separateCount[p] - 1; nextIndex > -1; nextIndex--)
                                        {
                                            // If lower range & alive unit is found, it is new target
                                            // If same range & alive & lower hp unit is found, it is new target
                                            if ((propertyUnit[t][nextIndex].range < targetRange && propertyUnit[t][nextIndex].currentHP > 0)
                                                || (propertyUnit[t][nextIndex].range == targetRange && propertyUnit[t][nextIndex].currentHP > 0 && propertyUnit[t][nextIndex].currentHP < targetHP))
                                            {
                                                targetProperty = t;
                                                targetIndex = nextIndex;
                                                targetRange = propertyUnit[t][nextIndex].range;
                                                targetHP = propertyUnit[t][nextIndex].currentHP;
                                                break;
                                            }
                                        }
                                    }

                                    // Found target, now really deal damage.

                                    // If no target is found, skip
                                    if (targetProperty == -1 || targetIndex == -1 || targetRange == 500 || targetHP < 0)
                                    {
                                        continue;
                                    }

                                    // If target is valid, shoot.
                                    int damage = propertyUnit[p][currentIndex].damage;
                                    propertyUnit[targetProperty][targetIndex].currentHP -= damage;
                                }
                            }
                        }

                        // Find all dead units
                        for (int p = 0; p < 7; p++)
                        {
                            if (separateCount[p] == 0) { continue; }

                            for (int index = separateCount[p] - 1; index >= 0; index--)
                            {
                                // Check if dead, then decrease separateCount
                                if (propertyUnit[p][index].currentHP <= 0) { separateCount[p] -= 1; UnityEngine.Debug.Log(p.ToString() + " died " + index); }
                            }
                        }

                        // find Next maxRanges
                        int newMaxRange = 0;

                        for (int p = 0; p < 7; p++)
                        {
                            for (int ind = 0; ind < separateCount[p]; ind++)
                            {
                                if (separateCount[p] > 0 && propertyUnit[p][ind].range < maxRange && propertyUnit[p][ind].range > newMaxRange)
                                {
                                    newMaxRange = propertyUnit[p][ind].range;
                                }
                            }
                        }
                        // If none, find whole maxRange again
                        if (newMaxRange == 0)
                        {
                            maxRange = 0;

                            for (int p = 0; p < 7; p++)
                            {
                                if (separateCount[p] > 0 && propertyUnit[p][0].range > maxRange)
                                {
                                    maxRange = propertyUnit[p][0].range;
                                }
                            }
                        }
                        else
                        {
                            maxRange = newMaxRange;
                        }

                        // Evaluate isOver boolean
                        int alivePropertyCount = 0;
                        for (int p = 0; p < 7; p++)
                        {
                            if (separateCount[p] > 0) { alivePropertyCount += 1; }
                        }

                        if (alivePropertyCount < 2) { isOver = true; }
                    }

                    // Now, return all alive units to the units section
                    int aliveProperty = 0;
                    for (int p = 0; p < 7; p++)
                    {
                        if (separateCount[p] > 0) { aliveProperty = p; break; }
                    }

                    if (aliveProperty == 0) { board[i][j].unitCount = 0; }
                    else
                    {
                        board[i][j].unitCount = separateCount[aliveProperty];

                        for (int u = 0; u < board[i][j].unitCount; u++)
                        {
                            board[i][j].units[u] = propertyUnit[aliveProperty][u];
                        }
                    }
                }
            }
        }
    }

    void CollapseBuilding()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (board[i][j].unitCount != 0)
                {
                    int conqueredProperty = board[i][j].units[0].property;
                    board[i][j].property = conqueredProperty;
                }
                if (board[i][j].isBuilt && board[i][j].property != board[i][j].structure.property)
                {
                    // Destroy the building
                    board[i][j].isBuilt = false;
                }
            }
        }
        return;
    }

    public void ButtonClicked(string input)
    {
        string[] inputs = input.Split(',');
        string majorButton = inputs[0];
        string minorButton = inputs[1];
        int gridX, gridY;

        int property = 1;
        
        switch(majorButton)
        {
            case "AbilityButton":
                break;
            case "BuildButton":
                gridX = System.Convert.ToInt32(inputs[2]);
                gridY = System.Convert.ToInt32(inputs[3]);

                board[gridX][gridY].property = property;
                board[gridX][gridY].structure.property = property;
                board[gridX][gridY].structure.buildingType = minorButton;
                board[gridX][gridY].structure.generate = "Musketeer";
                board[gridX][gridY].structure.counter = 1;
                board[gridX][gridY].isBuilt = true;

                break;
        }

        // Deactivate button
        isButtonActive = false;

        // SendMessage()
    }

    // Start is called before the first frame update
    void Start()
    {
        initInfantry.property = 0;
        initInfantry.unitType = "Infantry";
        initInfantry.damage = 2;
        initInfantry.range = 1;
        initInfantry.unitHP = 10;
        initInfantry.currentHP = 10;

        initMusketeer.property = 0;
        initMusketeer.unitType = "Musketeer";
        initMusketeer.damage = 3;
        initMusketeer.range = 2;
        initMusketeer.unitHP = 5;
        initMusketeer.currentHP = 5;

        unitDictionary = new Dictionary<string, int>()
        {
            {"Infantry", 2},
            {"Musketeer", 3}
        };

        InitBoard();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (!(playerA > currSeq))
        {
            if (isButtonActive == false)
            {
                // Activate button at UIManager

                // SendMessage()

                isButtonActive = true;
            }
            timer = 0;
        }
        else
        {
            if (currSeq != prevSeq)
            {
                UpdateBoard();
                prevSeq = currSeq;
                userInterfaceManager.SendMessage("SetTurn", currSeq.ToString());
            }

            if (timer >= 1)
            {
                currSeq += 1;
                timer = 0;
            }
        }
    }

    void PrintDebug(string which, int who)
    {
        switch (which)
        {
            case "distance":
                {
                    UnityEngine.Debug.Log("----- turn: " + turn.ToString() + " < distance > property : " + who.ToString() + " -----");
                    UnityEngine.Debug.Log(board[0][6].distance[who] + "\t" + board[1][6].distance[who] + "\t" + board[2][6].distance[who] + "\t" + board[3][6].distance[who] + "\t" + board[4][6].distance[who] + "\t" + board[5][6].distance[who] + "\t" + board[6][6].distance[who]);
                    UnityEngine.Debug.Log(board[0][5].distance[who] + "\t" + board[1][5].distance[who] + "\t" + board[2][5].distance[who] + "\t" + board[3][5].distance[who] + "\t" + board[4][5].distance[who] + "\t" + board[5][5].distance[who] + "\t" + board[6][5].distance[who]);
                    UnityEngine.Debug.Log(board[0][4].distance[who] + "\t" + board[1][4].distance[who] + "\t" + board[2][4].distance[who] + "\t" + board[3][4].distance[who] + "\t" + board[4][4].distance[who] + "\t" + board[5][4].distance[who] + "\t" + board[6][4].distance[who]);
                    UnityEngine.Debug.Log(board[0][3].distance[who] + "\t" + board[1][3].distance[who] + "\t" + board[2][3].distance[who] + "\t" + board[3][3].distance[who] + "\t" + board[4][3].distance[who] + "\t" + board[5][3].distance[who] + "\t" + board[6][3].distance[who]);
                    UnityEngine.Debug.Log(board[0][2].distance[who] + "\t" + board[1][2].distance[who] + "\t" + board[2][2].distance[who] + "\t" + board[3][2].distance[who] + "\t" + board[4][2].distance[who] + "\t" + board[5][2].distance[who] + "\t" + board[6][2].distance[who]);
                    UnityEngine.Debug.Log(board[0][1].distance[who] + "\t" + board[1][1].distance[who] + "\t" + board[2][1].distance[who] + "\t" + board[3][1].distance[who] + "\t" + board[4][1].distance[who] + "\t" + board[5][1].distance[who] + "\t" + board[6][1].distance[who]);
                    UnityEngine.Debug.Log(board[0][0].distance[who] + "\t" + board[1][0].distance[who] + "\t" + board[2][0].distance[who] + "\t" + board[3][0].distance[who] + "\t" + board[4][0].distance[who] + "\t" + board[5][0].distance[who] + "\t" + board[6][0].distance[who]);
                    break;
                }
            case "direction":
                {
                    UnityEngine.Debug.Log("----- turn: " + turn.ToString() + " < direction > property : " + who.ToString() + " -----");
                    UnityEngine.Debug.Log(board[0][6].direction[who] + "\t" + board[1][6].direction[who] + "\t" + board[2][6].direction[who] + "\t" + board[3][6].direction[who] + "\t" + board[4][6].direction[who] + "\t" + board[5][6].direction[who] + "\t" + board[6][6].direction[who]);
                    UnityEngine.Debug.Log(board[0][5].direction[who] + "\t" + board[1][5].direction[who] + "\t" + board[2][5].direction[who] + "\t" + board[3][5].direction[who] + "\t" + board[4][5].direction[who] + "\t" + board[5][5].direction[who] + "\t" + board[6][5].direction[who]);
                    UnityEngine.Debug.Log(board[0][4].direction[who] + "\t" + board[1][4].direction[who] + "\t" + board[2][4].direction[who] + "\t" + board[3][4].direction[who] + "\t" + board[4][4].direction[who] + "\t" + board[5][4].direction[who] + "\t" + board[6][4].direction[who]);
                    UnityEngine.Debug.Log(board[0][3].direction[who] + "\t" + board[1][3].direction[who] + "\t" + board[2][3].direction[who] + "\t" + board[3][3].direction[who] + "\t" + board[4][3].direction[who] + "\t" + board[5][3].direction[who] + "\t" + board[6][3].direction[who]);
                    UnityEngine.Debug.Log(board[0][2].direction[who] + "\t" + board[1][2].direction[who] + "\t" + board[2][2].direction[who] + "\t" + board[3][2].direction[who] + "\t" + board[4][2].direction[who] + "\t" + board[5][2].direction[who] + "\t" + board[6][2].direction[who]);
                    UnityEngine.Debug.Log(board[0][1].direction[who] + "\t" + board[1][1].direction[who] + "\t" + board[2][1].direction[who] + "\t" + board[3][1].direction[who] + "\t" + board[4][1].direction[who] + "\t" + board[5][1].direction[who] + "\t" + board[6][1].direction[who]);
                    UnityEngine.Debug.Log(board[0][0].direction[who] + "\t" + board[1][0].direction[who] + "\t" + board[2][0].direction[who] + "\t" + board[3][0].direction[who] + "\t" + board[4][0].direction[who] + "\t" + board[5][0].direction[who] + "\t" + board[6][0].direction[who]);
                    break;
                }
            case "property":
                {
                    UnityEngine.Debug.Log("----- turn: " + turn.ToString() + " < property > property : " + who.ToString() + " -----");
                    UnityEngine.Debug.Log(board[0][6].property + "\t" + board[1][6].property + "\t" + board[2][6].property + "\t" + board[3][6].property + "\t" + board[4][6].property + "\t" + board[5][6].property + "\t" + board[6][6].property);
                    UnityEngine.Debug.Log(board[0][5].property + "\t" + board[1][5].property + "\t" + board[2][5].property + "\t" + board[3][5].property + "\t" + board[4][5].property + "\t" + board[5][5].property + "\t" + board[6][5].property);
                    UnityEngine.Debug.Log(board[0][4].property + "\t" + board[1][4].property + "\t" + board[2][4].property + "\t" + board[3][4].property + "\t" + board[4][4].property + "\t" + board[5][4].property + "\t" + board[6][4].property);
                    UnityEngine.Debug.Log(board[0][3].property + "\t" + board[1][3].property + "\t" + board[2][3].property + "\t" + board[3][3].property + "\t" + board[4][3].property + "\t" + board[5][3].property + "\t" + board[6][3].property);
                    UnityEngine.Debug.Log(board[0][2].property + "\t" + board[1][2].property + "\t" + board[2][2].property + "\t" + board[3][2].property + "\t" + board[4][2].property + "\t" + board[5][2].property + "\t" + board[6][2].property);
                    UnityEngine.Debug.Log(board[0][1].property + "\t" + board[1][1].property + "\t" + board[2][1].property + "\t" + board[3][1].property + "\t" + board[4][1].property + "\t" + board[5][1].property + "\t" + board[6][1].property);
                    UnityEngine.Debug.Log(board[0][0].property + "\t" + board[1][0].property + "\t" + board[2][0].property + "\t" + board[3][0].property + "\t" + board[4][0].property + "\t" + board[5][0].property + "\t" + board[6][0].property);
                    break;
                }
            case "unitCount":
                {
                    UnityEngine.Debug.Log("----- turn: " + turn.ToString() + " < property > unitCounts : " + who.ToString() + " -----");
                    UnityEngine.Debug.Log(board[0][6].unitCount + "\t" + board[1][6].unitCount + "\t" + board[2][6].unitCount + "\t" + board[3][6].unitCount + "\t" + board[4][6].unitCount + "\t" + board[5][6].unitCount + "\t" + board[6][6].unitCount);
                    UnityEngine.Debug.Log(board[0][5].unitCount + "\t" + board[1][5].unitCount + "\t" + board[2][5].unitCount + "\t" + board[3][5].unitCount + "\t" + board[4][5].unitCount + "\t" + board[5][5].unitCount + "\t" + board[6][5].unitCount);
                    UnityEngine.Debug.Log(board[0][4].unitCount + "\t" + board[1][4].unitCount + "\t" + board[2][4].unitCount + "\t" + board[3][4].unitCount + "\t" + board[4][4].unitCount + "\t" + board[5][4].unitCount + "\t" + board[6][4].unitCount);
                    UnityEngine.Debug.Log(board[0][3].unitCount + "\t" + board[1][3].unitCount + "\t" + board[2][3].unitCount + "\t" + board[3][3].unitCount + "\t" + board[4][3].unitCount + "\t" + board[5][3].unitCount + "\t" + board[6][3].unitCount);
                    UnityEngine.Debug.Log(board[0][2].unitCount + "\t" + board[1][2].unitCount + "\t" + board[2][2].unitCount + "\t" + board[3][2].unitCount + "\t" + board[4][2].unitCount + "\t" + board[5][2].unitCount + "\t" + board[6][2].unitCount);
                    UnityEngine.Debug.Log(board[0][1].unitCount + "\t" + board[1][1].unitCount + "\t" + board[2][1].unitCount + "\t" + board[3][1].unitCount + "\t" + board[4][1].unitCount + "\t" + board[5][1].unitCount + "\t" + board[6][1].unitCount);
                    UnityEngine.Debug.Log(board[0][0].unitCount + "\t" + board[1][0].unitCount + "\t" + board[2][0].unitCount + "\t" + board[3][0].unitCount + "\t" + board[4][0].unitCount + "\t" + board[5][0].unitCount + "\t" + board[6][0].unitCount);
                    break;
                }
        }
    }
}

public struct GameUnit
{
    public int property;

    public string unitType;

    public int damage;
    public int range;

    public int currentHP;
    public int unitHP;
}

public struct GameStructure
{
    public int property;

    public string buildingType;
    
    public string generate;
    public int counter;
    public string enable;
}

public struct GameTile
{
    public int property;

    public string terrain;

    public GameStructure structure;
    public bool isBuilt;

    public GameUnit[] units;
    public int unitCount;

    public string[] direction;
    public int[] distance;
}