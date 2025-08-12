using System.Collections;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    [Header("Player Rotation Settings")]
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private float rotateDegree = 90f; // 회전할 각도

    [Header("Virtual Camera")]
    [SerializeField] private Cinemachine.CinemachineVirtualCamera rotatedCamera;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera originalCamera;

    [Header("Triggers")]
    [SerializeField] private Collider entryTrigger; // virtualCamera로 바뀌는 콜라이더
    [SerializeField] private Collider exitTrigger; // originalVirtualCamera로 바뀌는 콜라이더

    private bool isRotated = false;
    private Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 오브젝트가 아니라면 리턴
        if (!other.CompareTag("Player")) return;

        playerTransform = other.transform;

        if (other.gameObject == entryTrigger.gameObject && !isRotated)
        {
            // entryTrigger에 들어오면 플레이어 오브젝트를 회전
            RotatePlayerObject(playerTransform);

            // 카메라 우선순위 설정
            rotatedCamera.Priority = 10;
            originalCamera.Priority = 0;
            isRotated = true;
        }
        else if (other.gameObject == exitTrigger.gameObject && isRotated)
        {
            // exitTrigger에 들어오면 플레이어 오브젝트를 원래대로 돌림
            RotatePlayerObject(playerTransform);

            // 카메라 우선순위 설정
            rotatedCamera.Priority = 0;
            originalCamera.Priority = 10;
            isRotated = false;

            // entryTrigger와 exitTrigger의 트리거 위치를 스왑
            SwapTriggers();
        }
    }

    private void RotatePlayerObject(Transform playerTransform)
    {
        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.Euler(0, playerTransform.eulerAngles.y + rotateDegree, 0);
        
        // Smoothly rotate the player towards the target rotation
        StartCoroutine(RotateTowards(playerTransform, targetRotation));
    }

    private IEnumerator RotateTowards(Transform playerTransform, Quaternion targetRotation)
    {
        while (Quaternion.Angle(playerTransform.rotation, targetRotation) > 0.01f)
        {
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }

        playerTransform.rotation = targetRotation; // Ensure final rotation is set
    }

    private void SwapTriggers()
    {
        // entryTrigger와 exitTrigger의 위치를 스왑
        Vector3 tempPosition = entryTrigger.transform.position;
        entryTrigger.transform.position = exitTrigger.transform.position;
        exitTrigger.transform.position = tempPosition;
    }
}
