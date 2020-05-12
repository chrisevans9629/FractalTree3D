using System;
using UnityEngine;

public abstract class TreeGeneratorBase : MonoBehaviour
{
    public GameObject BranchPrefab;
    public GameObject LeafPrefab;
    public float FPS;
    public float MinFPS = 20;
    public bool ShouldCreateLeaves = true;

    [Range(0.001f,5)]
    public float AnimationSpeed = 0.01f;


    [Range(0, 1)]
    public float BranchRatio = 0.5f;
    [Range(0, 180)]
    public float RotateAngle = 45;

    [Range(0, 1)]
    public float MinScale = 0.5f;

    [Range(1, 10)]
    public int MaxBranchSections = 10;

    public int BranchingCount = 3;

    [Range(0, 12000)]
    public int MaxBranches = 10000;

    public Branch Branch { get; set; }

    public float PreviousRotateAngle;

    public Top GetTop(GameObject obj)
    {
        var slide = (obj.transform.up * obj.transform.localScale.y * (1 + BranchRatio));
        var top = obj.transform.position + slide;
        return new Top() { top = top, add = slide };
    }

    public void OnLeavesChanged(bool leaves)
    {
        ShouldCreateLeaves = leaves;
    }
    public void OnAngleChanged(float angle)
    {
        RotateAngle = angle;
    }
    public void OnAnimationChange(float animation)
    {
        AnimationSpeed = animation;
    }

    public void OnRatio(float ratio)
    {
        BranchRatio = ratio;
    }

    public void OnSectionChanged(float section)
    {
        MaxBranchSections = Convert.ToInt32(section);
    }

    public void OnBranchingCountChanged(string value)
    {
        if (int.TryParse(value, out var i))
        {
            BranchingCount = i;
        }
    }
}