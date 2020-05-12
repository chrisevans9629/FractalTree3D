using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoBranchCreator : IBranchCreator
{
    private readonly BranchAnimated _branchAnimated;

    public NoBranchCreator(BranchAnimated branchAnimated)
    {
        _branchAnimated = branchAnimated;
    }

    public void Generate()
    {
        
    }

    public List<Branch> CreateSection(Branch parent)
    {
        return _branchAnimated.Branches;
    }
}

public class BranchAnimated : Branch
{
    private IBranchCreator _creator;

    public BranchAnimated(IBranchCreator creator)
    {
        _creator = creator;
    }

    public void AddBranches()
    {
        Branches = _creator.CreateSection(this);
        _creator = new NoBranchCreator(this);
    }

    public float MaxScaleY { get; set; }

    public IEnumerable<BranchAnimated> AnimatedBranches => Branches.OfType<BranchAnimated>();

    public bool IsAnimationComplete => GameObject.transform.localScale.y > MaxScaleY;

}

public class BranchCreatorAnimated : IBranchCreator
{
    private readonly TreeGeneratorBase _gen;
    private readonly IBranchUpdator _branchUpdator;

    public BranchCreatorAnimated(TreeGeneratorBase gen, IBranchUpdator branchUpdator)
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
    public List<Branch> CreateSection(Branch obj)
    {
        if (obj.GameObject.transform.localScale.y < _gen.MinScale)
        {
            CreateLeaf(obj.GameObject);
            return new List<Branch>();
        }
        if (obj.BranchSection > _gen.MaxBranchSections)
        {
            CreateLeaf(obj.GameObject);
            return new List<Branch>();
        }
        if (obj.BranchCount > _gen.MaxBranches)
        {
            CreateLeaf(obj.GameObject);
            return new List<Branch>();
        }

        var mainAngle = 360f / _gen.BranchingCount;

        GameObject[] branches = new GameObject[_gen.BranchingCount];

        for (int i = 0; i < _gen.BranchingCount; i++)
        {
            var a = CreateBranch(obj.GameObject, i * mainAngle);
            branches[i] = a;
        }


        var list = new List<Branch>();
        for (var index = 0; index < branches.Length; index++)
        {
            var branch = branches[index];
            
            //var section = CreateSection(branch, branchSectionCount + 1, branchCount + _gen.BranchingCount);
            list.Add(new BranchAnimated(this) {GameObject = branch, BranchSection = obj.BranchSection + 1, BranchCount = obj.BranchCount + _gen.BranchingCount + index, MaxScaleY = branch.transform.localScale.y});
            branch.transform.position -= branch.transform.up * branch.transform.localScale.y;
            branch.transform.localScale = new Vector3(branch.transform.localScale.x, 0, branch.transform.localScale.z);

        }

        return list;
    }

    public void Generate()
    {
        var obj = UnityEngine.Object.Instantiate(_gen.BranchPrefab, _gen.transform.position, _gen.transform.rotation, _gen.transform);
        _gen.Branch = new Branch
        {
            GameObject = obj,
            
        };
        _gen.Branch.Branches = CreateSection(_gen.Branch);
    }
    
}
public class TreeGeneratorAnimated : ITreeGeneratorType
{
    private readonly TreeGeneratorBase _gen;
    private readonly ITreeUpdator _treeUpdator;

    public TreeGeneratorAnimated(TreeGeneratorBase gen, ITreeUpdator treeUpdator)
    {
        _gen = gen;
        _treeUpdator = treeUpdator;
    }

    public void Generate()
    {
        var t = GameObject.Instantiate(_gen.BranchPrefab, _gen.transform.position, _gen.transform.rotation, _gen.transform);
        t.transform.localScale -= Vector3.up;
        t.transform.position -= t.transform.up;
        _gen.Branch = new BranchAnimated(new BranchCreatorAnimated(_gen, new BranchUpdator(_gen))) {GameObject = t, MaxScaleY = 1};
    }
    public BranchAnimated Root => _gen.Branch as BranchAnimated;
    public void Update()
    {
        
        UpdateBranch(Root);
        _treeUpdator.Update();
    }

    public void UpdateBranch(BranchAnimated branch)
    {
        if (branch.IsAnimationComplete)
        {
            branch.AddBranches();
            foreach (var branchAnimatedBranch in branch.AnimatedBranches)
            {
                UpdateBranch(branchAnimatedBranch);
            }
        }
        else
        {
            branch.GameObject.transform.localScale += Vector3.up * _gen.AnimationSpeed * Time.deltaTime;
            branch.GameObject.transform.position += branch.GameObject.transform.up * _gen.AnimationSpeed * Time.deltaTime;
        }
    }
}


public class TreeGenerator : TreeGeneratorBase
{
    private ITreeGeneratorType instant;


    // Start is called before the first frame update
    void Start()
    {
        //instant = new TreeGeneratorInstant(new BranchCreator(this, new BranchUpdator(this)), new TreeUpdator(this, new BranchUpdator(this)));
        instant = new TreeGeneratorAnimated(this, new TreeUpdator(this, new BranchUpdator(this)));

        instant.Generate();
        PreviousRotateAngle = RotateAngle;
        //_previousBranchRatio = BranchRatio;
    }

    //private float _previousBranchRatio;
    // Update is called once per frame
    void Update()
    {
        FPS = 1 / Time.deltaTime;

        if(FPS < MinFPS)
            return;

        instant.Update();
    }

    public void Generate()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        instant.Generate();
    }




}
