using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineMesh : MonoBehaviour {
    
    // Function for combining mesh in the editor, mainly use for reducing colliders in a big structure
    [ContextMenu("Combine")]
	void Combine () {
        // Need to save old position of the parent object
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;

        // Translate parent to 0, 0, 0 and zero rotations
        transform.position = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;

        // Finds all meshfilters for all children of this object
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;

        // Loops through all meshfilters and saves them in a combineinstance array
        while (i < meshFilters.Length)
        {
            if (meshFilters[i].GetComponent<MeshCollider>())
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            }
            i++;
        }
        
        // Adds a meshfilter to the parent
        transform.gameObject.AddComponent<MeshFilter>();
        transform.GetComponent<MeshFilter>().mesh = new Mesh();

        // Combines all the meshes extracted from the child objects
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

        // Creates a mesh collider
        transform.gameObject.AddComponent<MeshCollider>();
        transform.GetComponent<MeshCollider>().sharedMesh = null;

        // Sets the mesh of the collider to the newly generated one
        transform.GetComponent<MeshCollider>().sharedMesh = transform.GetComponent<MeshFilter>().mesh;

        i = 0;
        // Loops through all child meshes, destroying them so we don't have duplicate mesh rendering
        while (i < meshFilters.Length)
        {
            DestroyImmediate(meshFilters[i].gameObject.GetComponent<MeshCollider>());
            i++;
        }

        // Translate parent back to original position and rotation
        transform.position = oldPosition;
        transform.rotation = oldRotation;
	}
	
}
