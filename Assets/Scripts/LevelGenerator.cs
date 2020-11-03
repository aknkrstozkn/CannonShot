using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [Header("Platform")]
    [SerializeField] GameObject platformPrefab = null;
    [SerializeField] Vector2 platformCountRange =  new Vector2(2, 6);
    // distance x is for max x distance and z for max z;
    [SerializeField] Vector2 platformSpawnDistance = new Vector2(10, 10);   

    [Header("Target")]
    [SerializeField] GameObject targetPrefab = null;
    [SerializeField] Vector2 targetCountRange = new Vector2(1, 5);
    [SerializeField] float targetYRange = 5f;
    public static int currentTargetCount = 0;

    [Header("Cannon")]
    [SerializeField] GameObject cannon = null;
    [SerializeField] float gapFromSurfece = 0.34f;
    [SerializeField] float cannonJumpHeight = 10f;
    [SerializeField] float cannonJumpTime = 2f;
    
    // List for stor platforms
    private List<GameObject> platformList = new List<GameObject>();
    // Which platform cannon is on now
    private int currentPlatformIndex = 0;
    // Sizes of Platform
    private float platSizeX = 0;
    private float platSizeZ = 0;

    // Sizes of Target
    private float targetSizeX = 0f;
    private float targetSizeZ = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Assigning platform sizes
        platSizeX = platformPrefab.GetComponent<Renderer>().bounds.size.x;
        platSizeZ = platformPrefab.GetComponent<Renderer>().bounds.size.z;

        // Assigning target sizes
        targetSizeX = targetPrefab.GetComponent<Renderer>().bounds.size.x;
        targetSizeZ = targetPrefab.GetComponent<Renderer>().bounds.size.z;

        CreatePlatforms();
        SpawnCannon();
    }

    // Update is called once per frame
    void Update()
    {
        // Control if current platform is finished
        if(platformList[currentPlatformIndex].GetComponent<TargetManager>().targetCount == 0)
        {   
            // If this platform is last platform than restart
            if(currentPlatformIndex == platformList.Count - 1)
            {
                Restart();
            }
            // If this is not the last one than continue to new platform
            else
            {
                currentPlatformIndex++;
                JumpCannon();                
            }            
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void JumpCannon()
    {
        cannon.transform.DOJump(CalcCannonPos(), cannonJumpHeight, 1, cannonJumpTime, false);
    }

    void SpawnCannon()
    {
        cannon.transform.position = CalcCannonPos();
        cannon.transform.rotation = Quaternion.identity;
    }

    Vector3 CalcCannonPos()
    {

        var platform = platformList[currentPlatformIndex];
        var platformPos = platform.transform.position;

        var xPos = Random.Range((platformPos.x - (platSizeX / 2)), (platformPos.x + (platSizeX / 2)));
        var yPos = gapFromSurfece;
        var zPos = Random.Range((platformPos.z - (platSizeZ / 2)), (platformPos.z - (platSizeZ / 4)));
        
        return new Vector3(xPos, yPos, zPos);
    }

    void CreateTargets(GameObject plat, int targetCount)
    {
        var platPos = plat.transform.position;
        // Targets line length
        var range = platSizeX / targetSizeX;
        // Creating a position list for targets to prevent collisions
        List<int> positionIndexes = Enumerable.Range(1, (int)range + 1).ToList();

        for (int i = 0; i < targetCount; i++)
        {
            // Get a position from position list
            var index = Random.Range(0, positionIndexes.Count);
            // Calculate localposition of target
            var xPos = ((positionIndexes[index] - 1) * targetSizeX) + (targetSizeX / 2);
            // Add platforms global x position to targets local position 
            // To calculate global position of target
            xPos += platPos.x - (platSizeX / 2);

            // Remove selected position from position list
            // To prevent unwanted collisions
            positionIndexes.RemoveAt(index);

            // Y position
            var yPos = Random.Range(0f, targetYRange);
            // Z position
            var zPos = Random.Range(platPos.z + (platSizeZ / 4), platPos.z + (platSizeZ / 2));

            // Creating final position
            var pos = new Vector3(xPos, yPos, zPos);

            // Creating target
            GameObject target = Instantiate(targetPrefab, pos, targetPrefab.transform.rotation) as GameObject;
            // Assigning targets parent
            target.transform.parent = plat.transform;
        }
    }
    
    void CreatePlatforms()
    {
        // First platform
        GameObject firstPlatform = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        platformList.Add(firstPlatform);
        // Get random target count
        var targetCount = Random.Range((int)targetCountRange.x, (int)targetCountRange.y);
        // Set platforms target count
        firstPlatform.GetComponent<TargetManager>().targetCount = targetCount;

        CreateTargets(firstPlatform, targetCount);
        // Platform Count
        var platformCount = Random.Range((int)platformCountRange.x, (int)platformCountRange.y);

        // Creating Platforms one by one
        for(int i = 1; i <= platformCount; ++i)
        {
            // Random X position in range
            var xPos = Random.Range((int)(-1 * platformSpawnDistance.x), (int)platformSpawnDistance.x);
            // Y Position is always zero
            var yPos = 0;
            // To avoid collision when creating platforms
            // Creating new platform according to last platform
            var lastPlatPosZ = platformList[i - 1].transform.position.z;
            var zStartPos = lastPlatPosZ + platSizeZ;
            var zPos = Random.Range((int)(zStartPos), (int)(zStartPos + platformSpawnDistance.y));

            // Getting result position ready
            var position = new Vector3(xPos, yPos, zPos);

            // Create Platform and store
            GameObject platform = Instantiate(platformPrefab, position, Quaternion.identity) as GameObject;
            platformList.Add(platform);

            // Get random target count
            targetCount = Random.Range((int)targetCountRange.x, (int)targetCountRange.y);
            // Set platforms target count
            platform.GetComponent<TargetManager>().targetCount = targetCount;

            CreateTargets(platform, targetCount);
        }

    }
}
