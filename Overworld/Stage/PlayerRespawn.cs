using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public void SetPlayerPosition()
    {
        GameObject player = PlayerManager.Instance.gameObject;

        if (player != null)
        {
            Debug.Log("[LoadVillageAndMovePlayer] 플레이어와 리턴포인트 찾음");
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            Debug.Log("[LoadVillageAndMovePlayer] 플레이어 위치 이동 완료");
        }
        else
        {
            if (player == null)
                Debug.LogWarning("[LoadVillageAndMovePlayer] Player를 찾지 못함");
        }
    }
}
