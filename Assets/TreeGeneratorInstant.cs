public class TreeGeneratorInstant : ITreeGeneratorType
{
    private readonly TreeGeneratorBase _gen;
    private readonly IBranchCreator _branchCreator;
    private IBranchUpdator _branchUpdator;
    public TreeGeneratorInstant(TreeGeneratorBase gen)
    {
        _gen = gen;
        _branchUpdator = new BranchUpdator(gen);
        _branchCreator = new BranchCreator(gen, _branchUpdator);
    }


    public void Generate()
    {
        var obj = UnityEngine.Object.Instantiate(_gen.BranchPrefab, _gen.transform.position, _gen.transform.rotation, _gen.transform);
        _gen.Branch = new Branch
        {
            GameObject = obj,
            Branches = _branchCreator.CreateSection(obj),
        };
    }

    public void Update()
    {
        if (_gen.PreviousRotateAngle == _gen.RotateAngle)
            return;
        //if(_previousBranchRatio == BranchRatio)
        //    return;
        _gen.PreviousRotateAngle = _gen.RotateAngle;
        //_previousBranchRatio = BranchRatio;
        UpdateAngle(_gen.Branch);
    }

    void UpdateAngle(Branch root)
    {
        var mainAngle = 360f / _gen.BranchingCount;

        var slide = _gen.GetTop(root.GameObject);

        var end = slide.top - slide.add / 2f;

        for (var index = 0; index < root.Branches.Count; index++)
        {
            var rootBranch = root.Branches[index];
            _branchUpdator.UpdateBranch(root.GameObject, mainAngle * index, rootBranch.GameObject);

            UpdateAngle(rootBranch);
        }
    }
}