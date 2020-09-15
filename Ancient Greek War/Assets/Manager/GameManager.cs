using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int boardSize = 7;
    public int turn;

    public GameTile[][] board;

    public GameUnit initInfantry = new GameUnit();
    public GameUnit initMusketeer = new GameUnit();

    void InitBoard()
    {
        turn = 0;

        // First, wipe out everything.
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i][j].terrain = "Ground";

                board[i][j].property = 0;

                board[i][j].isBuilt = false;

                board[i][j].units = new GameUnit[4];

                board[i][j].distance = 0;
                board[i][j].direction = "";
            }
        }

        // Then, place me and opponent.
        board[0][0].property = 1;
        board[0][0].structure.property = 1;
        board[0][0].structure.generate = "Infantry";
        board[0][0].structure.counter = 1;
        board[0][0].isBuilt = true;

        board[6][6].property = 2;
        board[6][6].structure.property = 2;
        board[6][6].structure.generate = "Infantry";
        board[6][6].structure.counter = 1;
        board[6][6].isBuilt = true;

        // Then, board the terrain.
        char[,] boardTerrain =
        {
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'},
            {'G','G','G','G','G','G','G'}
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

                board[j][6-i].terrain = currTerrain;
            }
        }

        //UpdateLookupTable();

        /*
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[0][0].distance, board[0][1].distance, board[0][2].distance, board[0][3].distance, board[0][4].distance, board[0][5].distance, board[0][6].distance));
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[1][0].distance, board[1][1].distance, board[1][2].distance, board[1][3].distance, board[1][4].distance, board[1][5].distance, board[1][6].distance));
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[2][0].distance, board[2][1].distance, board[2][2].distance, board[2][3].distance, board[2][4].distance, board[2][5].distance, board[2][6].distance));
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[3][0].distance, board[3][1].distance, board[3][2].distance, board[3][3].distance, board[3][4].distance, board[3][5].distance, board[3][6].distance));
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[4][0].distance, board[4][1].distance, board[4][2].distance, board[4][3].distance, board[4][4].distance, board[4][5].distance, board[4][6].distance));
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[5][0].distance, board[5][1].distance, board[5][2].distance, board[5][3].distance, board[5][4].distance, board[5][5].distance, board[5][6].distance));
        Debug.Log(string.Format("[{0} {1} {2} {3} {4} {5} {6}]", board[6][0].distance, board[6][1].distance, board[6][2].distance, board[6][3].distance, board[6][4].distance, board[6][5].distance, board[6][6].distance));
        */

        // Then, place neutral units
        board[2][2].property = 0;
        board[2][2].structure.property = 1;
        board[2][2].structure.generate = "Musketeer";
        board[2][2].structure.counter = 1;
        board[2][2].isBuilt = true;
        
        board[4][4].property = 0;
        board[4][4].structure.property = 1;
        board[4][4].structure.generate = "Musketeer";
        board[4][4].structure.counter = 1;
        board[4][4].isBuilt = true;
    }

    void UpdateLookupTable()
    {
        // Clear the direction and distance
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                board[i][j].distance = 0;
                board[i][j].direction = "";
            }
        }
        
        // Then do the first iteration
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int property = board[i][j].property;

                if (property == 0)
                    continue;

                // Up
                if (j < boardSize - 1 && property != board[i][j + 1].property)
                {
                    board[i][j].distance = 1;
                    board[i][j].direction += "U";
                }

                // Down
                if (j > 0 && property != board[i][j - 1].property)
                {
                    board[i][j].distance = 1;
                    board[i][j].direction += "L";
                }

                // Right
                if (i < boardSize - 1 && property != board[i + 1][j].property)
                {
                    board[i][j].distance = 1;
                    board[i][j].direction += "U";
                }

                // Left
                if (i > 0 && property != board[i - 1][j].property)
                {
                    board[i][j].distance = 1;
                    board[i][j].direction += "D";
                }
            }
        }

        //then continue until nothing changes
        bool isChanged = true;
        while (isChanged)
        {
            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    int property = board[i][j].property;

                    if (property != 0 && board[i][j].distance == 0)
                    {
                        int minDistance = 15;
                        //check 4 directions to find out the shortest, then determine the way.
                        //up
                        if (j < 6 && board[i][j+1].distance != 0)
                        {
                            if (minDistance == board[i][j+1].distance)
                            {
                                board[i][j].direction += "U";
                            }
                            else if (minDistance > board[i][j+1].distance)
                            {
                                minDistance = board[i][j+1].distance;
                                board[i][j].direction = "U";
                            }
                        }
                        //down
                        if (j > 0 && board[i][j - 1].distance != 0)
                        {
                            if (minDistance == board[i][j - 1].distance)
                            {
                                board[i][j].direction += "D";
                            }
                            else if (minDistance > board[i][j - 1].distance)
                            {
                                minDistance = board[i][j - 1].distance;
                                board[i][j].direction = "D";
                            }
                        }
                        //right
                        if (i < 6 && board[i + 1][j].distance != 0)
                        {
                            if (minDistance == board[i + 1][j].distance)
                            {
                                board[i][j].direction += "R";
                            }
                            else if (minDistance > board[i + 1][j].distance)
                            {
                                minDistance = board[i + 1][j].distance;
                                board[i][j].direction = "R";
                            }
                        }
                        //left
                        if (i > 0 && board[i - 1][j].distance != 0)
                        {
                            if (minDistance == board[i - 1][j].distance)
                            {
                                board[i][j].direction += "L";
                            }
                            else if (minDistance > board[i - 1][j].distance)
                            {
                                minDistance = board[i - 1][j].distance;
                                board[i][j].direction = "L";
                            }
                        }

                        //update distance
                        if (minDistance != 15)
                            board[i][j].distance = minDistance + 1;
                        else
                            isChanged = false;
                    }
                }
            }
        }
        return;
    }


    // Start is called before the first frame update
    void Start()
    {
        turn = 0;
        board = new GameTile[boardSize][];
        for (int i = 0; i < boardSize; i++)
        {
            board[i] = new GameTile[boardSize];
        }

        initInfantry.property = 0;
        initInfantry.unitType = "Infantry";
        initInfantry.damage = 2;
        initInfantry.range = 1;
        initInfantry.currHealth = 0;
        initInfantry.unitHealth = 10;
        initInfantry.unitCount = 0;

        initMusketeer.property = 0;
        initMusketeer.unitType = "Musketeer";
        initMusketeer.damage = 3;
        initMusketeer.range = 2;
        initMusketeer.currHealth = 0;
        initMusketeer.unitHealth = 5;
        initMusketeer.unitCount = 0;

        InitBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public struct GameUnit
{
    public int property;

    public string unitType;

    public int damage;
    public int range;

    public int currHealth;
    public int unitHealth;
    public int unitCount;
}

public struct GameStructure
{
    public int property;
    
    public string generate;
    public int counter;
    public string enable;

    public bool isPolis;
}

public struct GameTile
{
    public int property;

    public string terrain;

    public GameStructure structure;
    public bool isBuilt;

    public GameUnit[] units;

    public string direction;
    public int distance;
}
