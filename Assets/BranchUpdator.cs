﻿using UnityEngine;

public class BranchUpdator : IBranchUpdator
{
    private readonly TreeGeneratorBase _gen;

    public BranchUpdator(TreeGeneratorBase gen)
    {
        _gen = gen;
    }

    public GameObject UpdateBranch(GameObject parent, float angle, GameObject currentBranch)
    {
        var slide = _gen.GetTop(parent);

        currentBranch.transform.position = slide.top;
        currentBranch.transform.rotation = parent.transform.rotation;
        currentBranch.transform.localScale = new Vector3(parent.transform.localScale.x * _gen.BranchRatio,
            parent.transform.localScale.y * _gen.BranchRatio, parent.transform.localScale.z * _gen.BranchRatio);

        var end = slide.top - slide.add / 2f;

        currentBranch.transform.RotateAround(end, parent.transform.forward, _gen.RotateAngle);

        currentBranch.transform.RotateAround(end, parent.transform.up, angle);

        return currentBranch;
    }


}