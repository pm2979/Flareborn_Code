using UnityEngine;

[ExecuteAlways] // 씬 뷰에서도 작동
public class HitboxGizmoDrawer : MonoBehaviour
{
    [SerializeField] private Color gizmoColor = new Color(1, 0, 0, 0.3f); // 반투명 빨강

    private void OnDrawGizmos()
    {
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) return;

        Gizmos.color = gizmoColor;

        // 콜라이더 위치와 회전을 고려한 월드 좌표 변환
        Matrix4x4 cubeTransform = Matrix4x4.TRS(
            col.transform.position + col.center,
            col.transform.rotation,
            col.size
        );

        Gizmos.matrix = cubeTransform;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
