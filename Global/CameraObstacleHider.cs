using System.Collections.Generic;
using UnityEngine;

public class CameraObstacleHider : MonoBehaviour
{
    public Transform player;
    public Transform cameraPivot;
    public LayerMask obstacleLayer; // "Obstacle" 레이어를 포함하는 마스크

    private List<Renderer> currentObstacles = new List<Renderer>();
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    
    void Start()
    {
        // 자동으로 플레이어와 카메라 찾아서 할당
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (cameraPivot == null)
            cameraPivot = Camera.main?.transform;

        if (player == null || cameraPivot == null)
            Debug.LogWarning("[CameraObstacleHider] Player 또는 CameraPivot이 씬에 없습니다.");
    }
    void LateUpdate()
    {
        ClearObstacles();

        Vector3 dir = player.position - cameraPivot.position;
        float dist = dir.magnitude;

        // 카메라와 플레이어 사이에 있는 장애물 감지
        Ray ray = new Ray(cameraPivot.position, dir.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, dist, obstacleLayer);

        foreach (var hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null && !originalMaterials.ContainsKey(rend))
            {
                originalMaterials[rend] = rend.materials;
                MakeTransparent(rend);
                currentObstacles.Add(rend);
            }
        }
    }

    void MakeTransparent(Renderer rend)
    {
        foreach (Material mat in rend.materials)
        {
            mat.shader = Shader.Find("Standard");
            mat.SetFloat("_Mode", 3); // Transparent 모드
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;

            Color c = mat.color;
            c.a = 0.3f;
            mat.color = c;
        }
    }

    void ClearObstacles()
    {
        foreach (var rend in currentObstacles)
        {
            if (rend != null && originalMaterials.ContainsKey(rend))
            {
                rend.materials = originalMaterials[rend];
            }
        }

        currentObstacles.Clear();
        originalMaterials.Clear();
    }
}