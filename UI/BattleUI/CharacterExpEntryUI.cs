using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterExpEntryUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI nameText; // 이름
    [SerializeField] private TextMeshProUGUI levelText; // 레벨
    [SerializeField] private TextMeshProUGUI expStatusText; // 경험치 텍스트
    [SerializeField] private Slider expSlider; // 경험치 슬라이더

    // 캐릭터의 초기 상태를 UI에 설정
    public void SetData(CharacterInstance character)
    {
        nameText.text = character.Name;
        levelText.text = $"Lv. {character.Level}";

        int requiredExp = ExpTable.GetRequiredExp(character.Level);
        expStatusText.text = $"{character.CurrentExp} / {requiredExp}";
        expSlider.value = (float)character.CurrentExp / requiredExp;
    }

    // 경험치 획득 애니메이션을 재생하는 코루틴
    public IEnumerator AnimateExpGain(CharacterInstance character, int gainedExp)
    {
        // 애니메이션 시작 전의 초기 상태 계산
        int initialLevel = character.Level;
        int initialExp = character.CurrentExp;
        int expToSubtract = gainedExp;
      
        while (expToSubtract > 0)
        {
            if (initialExp >= expToSubtract)
            {
                initialExp -= expToSubtract;
                expToSubtract = 0;
            }
            else
            {
                expToSubtract -= initialExp;
                initialLevel--;
                if (initialLevel < 1) // 레벨 1 미만으로 내려가지 않도록 방어
                {
                    initialLevel = 1;
                    initialExp = 0;
                    break;
                }
                initialExp = ExpTable.GetRequiredExp(initialLevel);
            }
        }

        // 계산된 초기 상태를 기반으로 애니메이션 시작
        float fullBarAnimationTime = 1.5f; // 경험치 바가 차오르는 데 걸리는 시간
        int animatingLevel = initialLevel;
        int currentAnimatingExp = initialExp;
        int totalExpToAdd = gainedExp;

        // 애니메이션 전, UI를 초기 상태로 설정
        levelText.text = $"Lv. {animatingLevel}";

        yield return new WaitForSeconds(0.5f); // 애니메이션 대기

        while (totalExpToAdd > 0)
        {
            // 최대 레벨에 도달하면 애니메이션 중단
            //if (ExpTable.IsMaxLevel(animatingLevel))
            //{
            //    break;
            //}

            int requiredExp = ExpTable.GetRequiredExp(animatingLevel);
            int expToNextLevel = requiredExp - currentAnimatingExp;
            int amountToAddThisLoop = Mathf.Min(totalExpToAdd, expToNextLevel);

            float startFill = (float)currentAnimatingExp / requiredExp;
            float endFill = (float)(currentAnimatingExp + amountToAddThisLoop) / requiredExp;

            // 이번 루프에서 채울 양에 비례하여 애니메이션 시간 계산
            float duration = fullBarAnimationTime * (endFill - startFill);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / duration);
                float newFill = Mathf.Lerp(startFill, endFill, progress);

                expSlider.value = newFill;
                expStatusText.text = $"{(int)(newFill * requiredExp)} / {requiredExp}";

                yield return null;
            }

            // 루프 종료 후 정확한 값으로 보정
            expSlider.value = endFill;
            expStatusText.text = $"{currentAnimatingExp + amountToAddThisLoop} / {requiredExp}";

            currentAnimatingExp += amountToAddThisLoop;
            totalExpToAdd -= amountToAddThisLoop;

            // 레벨업 체크
            if (currentAnimatingExp >= requiredExp)
            {
                animatingLevel++;
                currentAnimatingExp = 0;

                // UI 업데이트
                levelText.text = $"Lv. {animatingLevel}";
                //if (ExpTable.IsMaxLevel(animatingLevel))
                //{
                //    expStatusText.text = "MAX";
                //    expSlider.value = 1;
                //}
                //else
                //{
                //    int nextRequiredExp = ExpTable.GetRequiredExp(animatingLevel);
                //    expStatusText.text = $"0 / {nextRequiredExp}";
                //    expSlider.value = 0;
                //}

                int nextRequiredExp = ExpTable.GetRequiredExp(animatingLevel);
                expStatusText.text = $"0 / {nextRequiredExp}";
                expSlider.value = 0;

                // 이펙트 대기
                yield return new WaitForSeconds(0.3f);
            }
        }

        // 모든 애니메이션 종료 후, 최종 데이터로 UI 완벽하게 동기화
        SetData(character);
    }
}
