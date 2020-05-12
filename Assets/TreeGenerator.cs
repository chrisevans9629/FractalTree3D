using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch
{
    public GameObject GameObject;
    public List<Branch> Branches { get; set; } = new List<Branch>();
}

public class TreeGenerator : MonoBehaviour
{
    public GameObject BranchPrefab;
    public GameObject LeafPrefab;

    public bool ShouldCreateLeaves = true;

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
    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(BranchPrefab, transform.position, transform.rotation, transform);

        Branch = new Branch() { GameObject = obj, };

        Branch.Branches = CreateSection(obj);
        _previousRotateAngle = RotateAngle;
        //_previousBranchRatio = BranchRatio;
    }

    void CreateLeaf(GameObject obj)
    {
        if (!ShouldCreateLeaves)
            return;
        var top = GetTop(obj);
        Instantiate(LeafPrefab, top.top - top.add / 2f, obj.transform.rotation, transform);
    }

    private List<Branch> CreateSection(GameObject obj, int branchSectionCount = 1, int branchCount = 1)
    {
        if (obj.transform.localScale.y < MinScale)
        {
            CreateLeaf(obj);
            return new List<Branch>();
        }

        if (branchSectionCount > MaxBranchSections)
        {
            CreateLeaf(obj);
            return new List<Branch>();

        }
        if (branchCount > MaxBranches)
        {
            CreateLeaf(obj);
            return new List<Branch>();
        }

        var mainAngle = 360f / BranchingCount;

        GameObject[] branches = new GameObject[BranchingCount];

        for (int i = 0; i < BranchingCount; i++)
        {
            var a = CreateBranch(obj, i * mainAngle);


            branches[i] = a;
        }


        var list = new List<Branch>();
        foreach (var branch in branches)
        {
            var section = CreateSection(branch, branchSectionCount + 1, branchCount + BranchingCount);
            list.Add(new Branch() { GameObject = branch, Branches = section });
        }

        return list;
    }

    private GameObject CreateBranch(GameObject obj, float angle)
    {
        var obj2 = Instantiate(BranchPrefab, Vector3.zero,
            Quaternion.identity, transform);
        return UpdateBranch(obj, angle, obj2);
    }

    private GameObject UpdateBranch(GameObject parent, float angle, GameObject currentBranch)
    {
        var slide = GetTop(parent);

        currentBranch.transform.position = slide.top;
        currentBranch.transform.rotation = parent.transform.rotation;
        currentBranch.transform.localScale = new Vector3(parent.transform.localScale.x * BranchRatio,
            parent.transform.localScale.y * BranchRatio, parent.transform.localScale.z * BranchRatio);

        var end = slide.top - slide.add / 2f;

        currentBranch.transform.RotateAround(end, parent.transform.forward, RotateAngle);

        currentBranch.transform.RotateAround(end, parent.transform.up, angle);

        return currentBranch;
    }

    class Top
    {
        public Vector3 top;
        public Vector3 add;
    }

    private Top GetTop(GameObject obj)
    {
        var slide = (obj.transform.up * obj.transform.localScale.y * (1 + BranchRatio));
        var top = obj.transform.position + slide;
        return new Top() { top = top, add = slide };
    }
    private float _previousRotateAngle;

    //private float _previousBranchRatio;
    // Update is called once per frame
    void Update()
    {
        if (_previousRotateAngle == RotateAngle)
            return;
        //if(_previousBranchRatio == BranchRatio)
        //    return;
        _previousRotateAngle = RotateAngle;
        //_previousBranchRatio = BranchRatio;
        UpdateAngle(this.Branch);
    }

    void UpdateAngle(Branch root)
    {
        var mainAngle = 360f / BranchingCount;

        var slide = GetTop(root.GameObject);

        var end = slide.top - slide.add / 2f;

        for (var index = 0; index < root.Branches.Count; index++)
        {
            var rootBranch = root.Branches[index];
            UpdateBranch(root.GameObject, mainAngle * index, rootBranch.GameObject);

            UpdateAngle(rootBranch);
        }
    }
}
