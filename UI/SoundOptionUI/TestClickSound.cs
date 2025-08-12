using UnityEngine;

public class TestClickSound : MonoBehaviour
{
        // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("왼쪽 클릭 - SFX 재생 시도");
            SoundManager.Instance.PlaySFX("Click");
        }
    }
}
