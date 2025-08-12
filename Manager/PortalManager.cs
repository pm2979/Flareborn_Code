using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private Portal[] portals;
    private bool isAllPortalInitialized;



    void OnEnable()
    {
        if (!isAllPortalInitialized)
        {
            InitializePortals();
            isAllPortalInitialized = true;
        }
    }

    private void InitializePortals()
    {
        int count = gameObject.transform.childCount;

        // 포탈 배열 초기화
        portals = new Portal[count];

        // 자식 오브젝트에서 포탈 컴포넌트를 가져와 배열에 저장
        for (int i = 0; i < count; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            Portal portal = child.GetComponent<Portal>();

            portals[i] = portal;
        }
    }

    public Portal GetPortalById(string portalId)
    {
        foreach (var portal in portals)
        {
            if (portal.portalID == portalId)
            {
                return portal;
            }
        }
        Debug.LogWarning($"Portal with ID {portalId} not found.");
        return null;
    }
}
