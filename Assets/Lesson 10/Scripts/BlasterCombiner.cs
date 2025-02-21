using UnityEngine;
using System.Collections.Generic;

public class BlasterCombiner : MonoBehaviour
{
    private Mesh _mesh;

    void Start()
    {
        CombineMeshes();
    }

    void CombineMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combine = new List<CombineInstance>();

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.gameObject == gameObject) continue; 

            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            ci.transform = mf.transform.localToWorldMatrix;
            combine.Add(ci);

            Destroy(mf.gameObject); // Delete the original object after merging
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine.ToArray(), false); // false - save submeshes, true - delete

        // Check MeshFilter and MeshRenderer maybe it's added
        MeshFilter mfMain = GetComponent<MeshFilter>();
        if (mfMain == null)
            mfMain = gameObject.AddComponent<MeshFilter>();

        MeshRenderer mrMain = GetComponent<MeshRenderer>();
        if (mrMain == null)
            mrMain = gameObject.AddComponent<MeshRenderer>();

        mfMain.mesh = combinedMesh;

        // === Фикс материалов ===
        List<Material> materials = new List<Material>();

        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            if (mr.gameObject == gameObject) continue; 
            materials.AddRange(mr.sharedMaterials); // add all materials
        }

        mrMain.sharedMaterials = materials.ToArray(); // assign material

        // Check in Console
        Debug.Log("Meshes are merged!");
        Debug.Log("Number of materials after fixing: " + mrMain.sharedMaterials.Length);
        Debug.Log("Number of vertices: " + combinedMesh.vertexCount);
    }
}
