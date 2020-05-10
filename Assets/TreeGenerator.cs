using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public GameObject BranchPrefab;
    [Range(0,1)]
    public float BranchRatio = 0.5f;
    [Range(0,180)]
    public float RotateAngle = 45;

    [Range(0,1)]
    public float MinScale = 0.5f;

    public int MaxBranches = 500;

    private int BranchCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        var obj = Instantiate(BranchPrefab, transform.position, transform.rotation);

        CreateSection(obj);
    }

    private void CreateSection(GameObject obj)
    {
        if(BranchCount > MaxBranches)
            return;
        if (obj.transform.localScale.y < MinScale)
        {
            return;
        }

        var a = CreateBranch(obj, 0);
        var b = CreateBranch(obj, 1);
        var c = CreateBranch(obj, 2);


        CreateSection(a);
        CreateSection(b);
        CreateSection(c);
    }

    private GameObject CreateBranch(GameObject obj, int side)
    {
        var slide = (obj.transform.up * obj.transform.localScale.y * (1 + BranchRatio));

        var top = obj.transform.position + slide;

        var obj2 = Instantiate(BranchPrefab, top,
            obj.transform.rotation);

        obj2.transform.localScale = new Vector3(obj.transform.localScale.x * BranchRatio, obj.transform.localScale.y * BranchRatio, obj.transform.localScale.z * BranchRatio);


        obj2.transform.RotateAround(top - slide/ 2f, obj.transform.forward, RotateAngle);

        obj2.transform.RotateAround(top - slide/2f, obj.transform.up, 120 * side);
        BranchCount++;
        return obj2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
