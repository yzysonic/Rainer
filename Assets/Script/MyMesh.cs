using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMesh : MonoBehaviour {

    [SerializeField]
    private MeshFilter meshFilter;

    private Mesh mesh;
    private List<Vector3> vertextList = new List<Vector3>();
    private List<Vector2> uvList = new List<Vector2>();
    private List<int> indexList = new List<int>();

    void Start()
    {
        mesh = CreatePlaneMesh();
        meshFilter.mesh = mesh;
       
    }

    private Mesh CreatePlaneMesh()
    {
        var mesh = new Mesh();

        vertextList.Add(new Vector3(-1, -1, 0));//0番頂点
        vertextList.Add(new Vector3(1, -1, 0)); //1番頂点
        vertextList.Add(new Vector3(-1, 1, 0)); //2番頂点
        vertextList.Add(new Vector3(1, 1, 0));  //3番頂点

        uvList.Add(new Vector2(0, 0));
        uvList.Add(new Vector2(1, 0));
        uvList.Add(new Vector2(0, 1));
        uvList.Add(new Vector2(1, 1));

        indexList.AddRange(new[] { 0, 2, 1, 1, 2, 3 });//0-2-1の頂点で1三角形。 1-2-3の頂点で1三角形。

        mesh.SetVertices(vertextList);//meshに頂点群をセット
        mesh.SetUVs(0, uvList);//meshにテクスチャのuv座標をセット（今回は割愛)
        mesh.SetIndices(indexList.ToArray(), MeshTopology.Triangles, 0);//メッシュにどの頂点の順番で面を作るかセット
        return mesh;

    }
}
