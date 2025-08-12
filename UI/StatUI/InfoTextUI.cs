using TMPro;
using UnityEngine;

public class InfoTextUI : MonoBehaviour
{
    [field:SerializeField] public TextMeshProUGUI Text { get; private set; }

    public void Set(string text)
    {
        Text.text = text;
    }
}
