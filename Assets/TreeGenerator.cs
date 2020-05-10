using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(BranchPrefab, transform.position, transform.rotation);

        CreateSection(obj, 1);
    }

    void CreateLeaf(GameObject obj)
    {
        if(!ShouldCreateLeaves)
            return;
        var top = GetTop(obj);
        Instantiate(LeafPrefab, top.top - top.add / 2f, obj.transform.rotation);
    }

    private void CreateSection(GameObject obj, int branchCount)
    {
        if (obj.transform.localScale.y < MinScale)
        {
            CreateLeaf(obj);
            return;
        }

        if (branchCount > MaxBranchSections)
        {
            CreateLeaf(obj);
            return;

        }
        if (branchCount * BranchingCount > MaxBranches)
        {
            CreateLeaf(obj);
            return;
        }

        var mainAngle = 360f / BranchingCount;

        GameObject[] branches = new GameObject[BranchingCount];

        for (int i = 0; i < BranchingCount; i++)
        {
            var a = CreateBranch(obj, i * mainAngle);


            branches[i] = a;
        }



        foreach (var branch in branches)
        {
            CreateSection(branch, branchCount + 1);
        }

    }

    private GameObject CreateBranch(GameObject obj, float angle)
    {
        var slide = GetTop(obj);

        var obj2 = Instantiate(BranchPrefab, slide.top,
            obj.transform.rotation);

        obj2.transform.localScale = new Vector3(obj.transform.localScale.x * BranchRatio, obj.transform.localScale.y * BranchRatio, obj.transform.localScale.z * BranchRatio);


        obj2.transform.RotateAround(slide.top - slide.add / 2f, obj.transform.forward, RotateAngle);

        obj2.transform.RotateAround(slide.top - slide.add / 2f, obj.transform.up, angle);
        return obj2;
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
        return new Top(){top = top, add = slide};
    }

    // Update is called once per frame
    void Update()
    {

    }
}
