using UnityEngine;

public class TreeCounter : MonoBehaviour
{
    void Start()
    {
        // Get the terrain component
        Terrain terrain = GetComponent<Terrain>();
        
        if (terrain != null)
        {
            // Get the number of tree instances
            int treeCount = terrain.terrainData.treeInstances.Length;
            Debug.Log("Total Trees: " + treeCount);
        }
    }
}