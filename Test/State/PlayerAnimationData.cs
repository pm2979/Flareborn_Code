using UnityEngine;
using System;

[Serializable]
public class PlayerAnimationData
{
    [SerializeField] private string walkParameterName = "IsWalk";
    public string walkParameterHash { get; private set; }
    [SerializeField] private string dashParameterName = "IsDash";
    public string dashParameterHash { get; private set; }
    [SerializeField] private string jumpParameterName = "IsJump";
    public string jumpParameterHash { get; private set; }
    [SerializeField] private string attackParameterName = "IsAttack";
    public string attackParameterHash { get; private set; }

    public void Initialize()
    {
        walkParameterHash = walkParameterName;
        dashParameterHash = dashParameterName;
        jumpParameterHash = jumpParameterName;
        attackParameterHash = attackParameterName;
    }
}
