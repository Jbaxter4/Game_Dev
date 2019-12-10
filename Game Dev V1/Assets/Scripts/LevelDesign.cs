using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelDesign : MonoBehaviour
{
    [SerializeField] int levelSize,rotation,rotationz,lavaOffset,bossRoomSize, enemyNumber;
    [SerializeField] float offset;
    [SerializeField] bool isIce, isLava;
    [SerializeField] GameObject bossRoom,Floor, BoundaryRL, WidthBoundary, sandBag1, cornerCover, arch, window1, window2, window3, doorFrame, barrel, barrelPile1, iceFloor1,iceFloor2,iceFloor3,iceFloor,hedge,lavaSpurt;
    [SerializeField]
    GameObject[] enemies;
    public float overLapRadius;
    Vector3 Boundary1, Boundary2, Boundary3, Boundary4, floorLocation;
    public int currentObjectCount, failedAtempt;
    List<Room> Rooms = new List<Room>();
    int[,] map;
    public NavMeshSurface surface;
    int numberOfEnemies;

    class Room
    {
        int maxX, maxZ;
        int minX, minZ;
        public Room(int positionX, int positionZ, int xSize, int zSize)
        {
            maxX = positionX + zSize;
            maxZ = positionZ + xSize;
            minX = positionX - zSize;
            minZ = positionZ - xSize;
        }

        public int getMaxX()
        {
            return maxX;
        }
        public int getMaxZ()
        {
            return maxZ;
        }
        public int getMinX()
        {
            return minX;
        }
        public int getMinZ()
        {
            return minZ;
        }
    }

    public LevelDesign(){
        levelSize = levelSize;
        }

    public int getLevelSize()
    {
        return levelSize;
    }

    private void Start()
    {
        if (isLava)
        {
            offset = offset + -1;
        }
        spawnBoundaryAndFloor(levelSize);
        spawnBossRoom();
        
        for(int x = 0; x < Random.Range(levelSize,levelSize*(levelSize/2));x++)
        {
            spawnRoom();
        }        
        spawnObjectCollider(levelSize, sandBag1, true, levelSize * 8,0);
        spawnObjectCollider(levelSize, cornerCover, true, levelSize * 8,0);
        spawnObjectCollider(levelSize, hedge, true, levelSize * 8,0);
        spawnObjectCollider(levelSize, barrel, true, levelSize * 10,4);
        if (isLava)
        {
            spawnObjectCollider(levelSize, lavaSpurt, false, levelSize * 8, 0);
        }
        surface.BuildNavMesh();
        for(int x = 0; x< enemyNumber; x++)
        {
            spawnEnemys();
        }
    }

    void spawnEnemys()
    {
        GameObject enemy = enemies[Random.Range(0,enemies.Length)];
        
        for (int y = 0; y < 100; y++)
        {
            Ray ray = new Ray(transform.position + new Vector3(Random.Range(-levelSize * 5 + 4, levelSize * 5 - 4), 3, Random.Range(-levelSize * 5 + 4, levelSize * 5 - 4)), Vector3.down);
            RaycastHit hit = new RaycastHit();
            LayerMask layerMask = 1 << 8;
            if (Physics.Raycast(ray, out hit, 100));
            {
                if (hit.collider.gameObject.layer != 8)
                {
                    Instantiate(enemy, hit.point, Quaternion.identity);
                    numberOfEnemies++;
                    break;
                }
            }
        }

    }

    void spawnBoundaryAndFloor(int levelSize)
    {
        //Boundary Walls
        for(int x = 0; x < levelSize *2 ; x++)
        {
            Boundary1 = new Vector3(levelSize * 5 + 1-offset, -.2f, levelSize * 5 - (x * 5)- lavaOffset);
            Instantiate(BoundaryRL, Boundary1, Quaternion.Euler(new Vector3(0, rotationz)));
        }

        for (int x = 0; x < levelSize * 2; x++)
        {
            Boundary3 = new Vector3(-levelSize * 5 + 1-offset, -.2f, levelSize * 5 - (x * 5 ) - lavaOffset);
            Instantiate(BoundaryRL, Boundary3, Quaternion.Euler(new Vector3(0, rotationz)));
        }

        for (int x = 0; x < levelSize * 2; x++)
        {
            if (x < (levelSize - 2) || x > (levelSize + 2))
            {
                Boundary2 = new Vector3(levelSize * 5 - (x * 5), -.2f, levelSize * 5);
                Instantiate(WidthBoundary, Boundary2, Quaternion.Euler(new Vector3(0, rotation)));
            }
        }

        for (int x = 0; x < levelSize * 2; x++)
        {
            Boundary4 = new Vector3(levelSize * 5 -(x*5), -.2f, -levelSize * 5 );
            Instantiate(WidthBoundary, Boundary4, Quaternion.Euler(new Vector3(0, rotation)));
        }

        int randomFloor;
        if (isIce)
        {
            spawnIceFloor();
        }
        for (int x = 0; x < levelSize * 2; x++)
        {
            for(int y = 0;y < levelSize * 2; y++)
            {
                if (isIce)
                {
                    spawnIceLake(x, y);
                }
                else
                {
                    floorLocation = new Vector3(levelSize * 5 - (x * 5), -.2f, levelSize * 5 - (y * 5));
                    Instantiate(Floor, floorLocation, Quaternion.identity);
                }
            }
        }
    }

    void spawnBossRoom()
    {
        Instantiate(bossRoom, new Vector3(0, 0, levelSize * 5 + bossRoomSize), Quaternion.identity);
    }

    void spawnObjectCollider(int levelSize,GameObject objectToSpawn,bool rotate, int number,int offSetY)
    {
        currentObjectCount = 0;
        failedAtempt = 0;
        for(int x = 0; x < levelSize * 10; x++)
        {
            if (failedAtempt > 100)
            {
                return;
            }
            Ray ray = new Ray(transform.position + new Vector3(Random.Range(-levelSize * 5 + 4, levelSize * 5 - 4), 3, Random.Range(-levelSize * 5 + 4, levelSize * 5 - 4)), Vector3.down);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 200))
            {
                LayerMask layerMask = 1 << 8;
                Collider[] hitColliders = Physics.OverlapBox(hit.point, new Vector3(5, 2, 5), Quaternion.identity, layerMask);
                if (hitColliders.Length == 0)
                {               
                    Collider[] collider = Physics.OverlapSphere(hit.point, overLapRadius);
                    bool overlaped = false;
                    foreach(Collider col in collider)
                    {
                        if (col.gameObject.layer == 8) overlaped = true;
                    }
                    if (!overlaped)
                    {
                        Vector3 pos = hit.point;
                        pos.y = offSetY;
                        if (rotate)
                        {
                            Instantiate(objectToSpawn, pos , Quaternion.Euler(new Vector3(0, Random.Range(0, 360))));

                        }
                        else
                        {
                            Instantiate(objectToSpawn, pos, Quaternion.identity);

                        }
                        currentObjectCount++;
                    }
                    else
                    {
                        failedAtempt++;
                    }
                }
                else
                {
                    failedAtempt++;
                }
            }        
        }       
    }

    bool isThereSpace(int randomX,int randomZ,int xSize,int zSize)
    {
        xSize = xSize * 5;
        zSize = zSize * 5;
        int layerMask = 1 << 8;
        Collider[] hitColliders = Physics.OverlapBox(new Vector3(randomX, 3, randomZ), new Vector3(xSize + 5,2,zSize + 5), Quaternion.identity, layerMask);
        if (hitColliders.Length == 0)
        {            
            return true;
        }
        else
        {
            //print("collision");
            return false;
        }
    }
    void spawnRoom()
    {
        int wallsX, wallsZ;
        wallsX = Random.Range(3, levelSize / 4);
        wallsZ = Random.Range(3, levelSize / 4);
        
        List<int> wallType = new List<int>();
        bool door = false;
        for (int y = 0; y < (wallsX + wallsZ) * 2; y++)
        {
            wallType.Add(Random.Range(0, 40));
            if (wallType[y] >= 1 && wallType[y] <= 2)
            {
                door = true;
            }
        }

        if (!door)
        {
            wallType[Random.Range(0, (wallsX + wallsZ) * 2)] = 2;
        }
        List<Vector3> RoomPositions = new List<Vector3>();
        List<int> Rotation = new List<int>();
        Vector3 roomPosition;
        int randomX = 0, randomZ = 0;
        bool roomSpawned = false;
        for ( int P = 0; P < 20;P++)
        {
     
            randomX = Random.Range(-levelSize * 5 + wallsZ * 5, levelSize * 5 - wallsX * 5);
            randomZ = Random.Range(-levelSize * 5 + wallsZ * 5, levelSize * 5 - wallsX * 5);

            for (int x = 0; x < wallsX * 2; x++)
            {
                //randomWalls(randomX, randomZ - wallsX * 5 + ((x / 2) * 5), wallType[x], 90);
                roomPosition = new Vector3(randomX, 0, randomZ - wallsX * 5 + ((x / 2) * 5) );
                RoomPositions.Add(roomPosition);
                Rotation.Add(90+rotation);
                x++;
                //randomWalls(randomX - wallsY* 5, randomZ - wallsX * 5 + (((x - 1) / 2) * 5), wallType[x], 90);
                roomPosition = new Vector3(randomX - wallsZ * 5, 0, randomZ - wallsX * 5 + (((x - 1) / 2) * 5) );
                RoomPositions.Add(roomPosition);
                Rotation.Add(90+rotation);
            }
            for (int x = 0; x < wallsZ * 2; x++)
            {
                //randomWalls(randomX - wallsY * 5 + ((x / 2) * 5), randomZ, wallType[x + wallsX * 2], 180);
                roomPosition = new Vector3(randomX - wallsZ * 5 + ((x / 2) * 5)+offset, 0, randomZ+offset);
                RoomPositions.Add(roomPosition);
                Rotation.Add(180 + rotation);
                x++;
                //randomWalls(randomX - wallsY * 5 + (((x - 1) / 2) * 5), randomZ - wallsX * 5, wallType[x + wallsX * 2], 180);
                roomPosition = new Vector3(randomX - wallsZ * 5 + (((x - 1) / 2) * 5) + offset, 0, randomZ - wallsX * 5+offset);
                RoomPositions.Add(roomPosition);
                Rotation.Add(180 + rotation);
            }
            //print(isThereSpace(randomX, randomZ, wallsX, wallsZ));
            if (isThereSpace(randomX, randomZ, wallsX, wallsZ))
            {
                for (int x = 0; x < RoomPositions.Count; x++)
                {
                    //print(RoomPositions.Count + "    " + wallType.Count);
                    randomWalls(RoomPositions[x], wallType[x], Rotation[x]);
                    roomSpawned = true;
                }
                break;
            }
            RoomPositions.Clear();
            Rotation.Clear();
        }
        //Should check for true
        if (roomSpawned) {
            if (wallsX + wallsZ >= 8)
            {
               
            }
        }        
    }       
    

    void randomWalls(Vector3 position, int wallType,int rotation)
    {
        GameObject objectToSpawn = null;
        bool spawnDoor = false;

        //print(wallType);
        if(wallType >= 0 && wallType <= 1)
        {
            //door
            objectToSpawn = doorFrame;
            spawnDoor = true;
        }
        else if(wallType >= 2 && wallType <= 2)
        {
            //arch
            objectToSpawn = arch;
        }
        else if (wallType >= 3 && wallType <= 3)
        {
            //window1
            objectToSpawn = window1;
        }
        else if (wallType >= 4 && wallType <= 4)
        {
            //window2
            objectToSpawn = window2;
        }
        else if (wallType >= 5 && wallType <= 5)
        {
            //window3
            objectToSpawn = window3;
        }
        else if (wallType >= 6 && wallType <= 40)
        {
            //wall
            objectToSpawn = WidthBoundary;
        }
        GameObject roomWall = Instantiate(objectToSpawn, position, Quaternion.Euler(new Vector3(0, rotation)));
    }

    void spawnIceFloor()
    {
        map = new int[levelSize * 2, levelSize * 2];
        string seed = Time.time.ToString();


        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < levelSize * 2; x++)
        {
            for (int y = 0; y < levelSize * 2; y++)
            {
                if (x == 0 || x == levelSize * 2 - 1 || y == 0 || y == levelSize * 2 - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (Random.Range(0, 100) < 46) ? 1 : 0;
                }
            }
        }
        for(int x = 0; x < 1; x++)
        {
            smooth();
        }
    }
    void smooth() { 

        for (int x = 0; x < levelSize * 2; x++)
        {
            for (int y = 0; y < levelSize * 2; y++)
            {
                int neighbourWallTiles = GetSurroundingFloorCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingFloorCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (IsInMapRange(neighbourX, neighbourY))
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                   
                }
            }
        }

        return wallCount;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < levelSize*2 && y >= 0 && y < levelSize*2;
    }

    void spawnIceLake(int x, int y)
    {
        int pos1 = 0, pos2 = 0, pos3 = 0, pos4 = 0, tileValue;
        floorLocation = new Vector3(levelSize * 5 - (x * 5), -.2f, levelSize * 5 - (y * 5));
        if (map[x,y] == 1)
        {
            if (IsInMapRange(x, y+1))
            {
                pos1 = (map[x, y + 1] == 1) ? 1 : 0;
            }
            if (IsInMapRange(x + 1, y))
            {
                pos2 = map[x + 1, y] == 1 ? 2 : 0;
            }
            if (IsInMapRange(x, y - 1))
            {
                pos3 = map[x, y - 1] == 1 ? 4 : 0;
            }
            if (IsInMapRange(x - 1, y))
            {
                pos4 = map[x - 1, y] == 1 ? 8 : 0;
            }
            tileValue = pos1 + pos2 + pos3 + pos4;
            switch (tileValue)
            {
                case 0:
                    Instantiate(Floor, floorLocation, Quaternion.identity);
                    break;
                //1 point
                case 1:
                    floorLocation.z -= 5;
                    Instantiate(iceFloor1, floorLocation, Quaternion.Euler(new Vector3(0,90)));
                    break;
                case 2:
                    floorLocation.x -= 5; floorLocation.z -= 5;
                    Instantiate(iceFloor1, floorLocation, Quaternion.Euler(new Vector3(0,180)));
                    break;
                case 4:
                    floorLocation.x -= 5;
                    Instantiate(iceFloor1, floorLocation, Quaternion.Euler(new Vector3(0,270)));
                    break;
                case 8:
                    Instantiate(iceFloor1, floorLocation, Quaternion.Euler(new Vector3(0,0)));
                    break;
                //2 points
                case 3:
                    floorLocation.x -= 5; floorLocation.z -= 5;
                    Instantiate(iceFloor2, floorLocation, Quaternion.Euler(new Vector3(0, 180)));
                    break;
                case 6:
                    floorLocation.x -= 5;
                    Instantiate(iceFloor2, floorLocation, Quaternion.Euler(new Vector3(0,270)));
                    break;
                case 12:
                    Instantiate(iceFloor2, floorLocation, Quaternion.Euler(new Vector3(0,0)));
                    break;
                case 9:
                    floorLocation.z-= 5;
                    Instantiate(iceFloor2, floorLocation, Quaternion.Euler(new Vector3(0,90)));
                    break;
                case 5:
                    floorLocation.z -= 5;
                    Instantiate(iceFloor3, floorLocation, Quaternion.Euler(new Vector3(0,90)));
                    break;
                case 10:
                    floorLocation.x -= 5; floorLocation.z -= 5;
                    Instantiate(iceFloor3, floorLocation, Quaternion.Euler(new Vector3(0,180)));
                    break;
                //3 points
                case 7:
                    Instantiate(iceFloor, floorLocation, Quaternion.identity);
                    break;
                case 14:
                    floorLocation.z -= 5;
                    Instantiate(iceFloor, floorLocation, Quaternion.Euler(new Vector3(0,90)));
                    break;
                case 13:
                    floorLocation.x -= 5; floorLocation.z -= 5;
                    Instantiate(iceFloor, floorLocation, Quaternion.Euler(new Vector3(0,180)));
                    break;
                case 11:
                    floorLocation.x -= 5; 
                    Instantiate(iceFloor, floorLocation, Quaternion.Euler(new Vector3(0,270)));
                    break;
                //4 points
                case 15:
                    Instantiate(iceFloor, floorLocation, Quaternion.identity);
                    break;
            }
        }
        else
        {
            Instantiate(Floor, floorLocation, Quaternion.identity);
        }
    }
}

