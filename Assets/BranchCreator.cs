using System.Collections.Generic;
using UnityEngine;

public class BranchCreator : IBranchCreator
{
    private readonly TreeGeneratorBase _gen;
    private readonly IBranchUpdator _branchUpdator;

    public BranchCreator(TreeGeneratorBase gen, IBranchUpdator branchUpdator)
    {
        _gen = gen;
        _branchUpdator = branchUpdator;
    }

    void CreateLeaf(GameObject obj)
    {
        if (!_gen.ShouldCreateLeaves)
            return;
        var top = _gen.GetTop(obj);
        GameObject.Instantiate(_gen.LeafPrefab, top.top - top.add / 2f, obj.transform.rotation, _gen.transform);
    }



    private GameObject CreateBranch(GameObject obj, float angle)
    {
        var obj2 = GameObject.Instantiate(_gen.BranchPrefab, Vector3.zero,
            Quaternion.identity, _gen.transform);
        return _branchUpdator.UpdateBranch(obj, angle, obj2);
    }
    public List<Branch> CreateSection(GameObject obj, int branchSectionCount = 1, int branchCount = 1)
    {
        if (obj.transform.localScale.y < _gen.MinScale)
        {
            CreateLeaf(obj);
            return new List<Branch>();
        }

        if (branchSectionCount > _gen.MaxBranchSections)
        {
            CreateLeaf(obj);
            return new List<Branch>();

        }
        if (branchCount > _gen.MaxBranches)
        {
            CreateLeaf(obj);
            return new List<Branch>();
        }

        var mainAngle = 360f / _gen.BranchingCount;

        GameObject[] branches = new GameObject[_gen.BranchingCount];

        for (int i = 0; i < _gen.BranchingCount; i++)
        {
            var a = CreateBranch(obj, i * mainAngle);


            branches[i] = a;
        }


        var list = new List<Branch>();
        foreach (var branch in branches)
        {
            var section = CreateSection(branch, branchSectionCount + 1, branchCount + _gen.BranchingCount);
            list.Add(new Branch() { GameObject = branch, Branches = section });
        }

        return list;
    }

    public List<Branch> CreateSection(GameObject parent)
    {
        return CreateSection(parent,1,1);
    }
}