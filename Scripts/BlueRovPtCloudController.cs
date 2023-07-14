
using ROSBridgeLib;
using ROSBridgeLib.sensor_msgs;
using PointCloud;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

public class BlueRovPtCloudController : MonoBehaviour
{

    // Mesh stores the positions and colours of every point in the cloud
    // The renderer and filter are used to display it
    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;

    // The size, positions and colours of each of the pointcloud
    public float pointSize = 2f;

    public static List<Vector3> pcl_interpolated_list;
    public static Vector3[] pcl_interpolated;
    

    [Header("MAKE SURE THESE LISTS ARE MINIMISED OR EDITOR WILL CRASH")]
    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };


    public Transform offset;

    void Start()
    {
        // Give all the required components to the gameObject
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        mf = gameObject.AddComponent<MeshFilter>();
        meshRenderer.material = new Material(Shader.Find("Custom/PointCloudShader"));
        mesh = new Mesh
        {
            // Use 32 bit integer values for the mesh, allows for stupid amount of vertices (2,147,483,647 I think?)
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        transform.position = offset.position;
        transform.rotation = offset.rotation;
    }

    void UpdateMesh(){
        
        // pcl_interpolated = pcl_interpolated_list.ToArray();
        // Debug.Log(BlueRovCloudSubscriber.pcl_interpolated_list);
        positions = BlueRovCloudSubscriber.pcl;
        colours = BlueRovCloudSubscriber.pcl_color;
        // foreach (Vector3 inputPoint in positions)
        // {
        //         // Vector3 interpolatedPoint = MLSInterpolate(inputPoint, pcl);
        //         // pcl_interpolated_list.Add(new Vector3(0, 1, 2));
        //         Debug.Log(inputPoint);
        // }

        // Vector3 MLSInterpolate(Vector3 inputPoint, Vector3[] inputPoints)
        // {
        //     Vector3 interpolatedPoint = Vector3.zero;
        //     float totalWeight = 0f;

        //     foreach (Vector3 point in inputPoints)
        //     {
        //         float distance = Vector3.Distance(inputPoint, point);
        //         if (distance < searchRadius)
        //         {
        //             float weight = 1f / (distance * distance);
        //             interpolatedPoint += point * weight;
        //             totalWeight += weight;
        //         }
        //     }

        //     interpolatedPoint /= totalWeight;
        //     return interpolatedPoint;
        // }

        if (positions == null)
        {
            Debug.Log("e dey empty");
            return;
        }
        mesh.Clear();
        mesh.vertices = positions;
        mesh.colors = colours;
        int[] indices = new int[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            indices[i] = i;
        }

        mesh.SetIndices(indices, MeshTopology.Points, 0);
        // int nPoints = 65000;
        // mesh.uv = new Vector2[nPoints];
        // mesh.normals = new Vector3[nPoints];
        mf.mesh = mesh;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = offset.position;
        transform.rotation = offset.rotation;
        meshRenderer.material.SetFloat("_PointSize", pointSize);       
        UpdateMesh();
    }
   
}

