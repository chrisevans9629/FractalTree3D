public class TreeUpdator : ITreeUpdator
{
    private readonly TreeGeneratorBase _gen;
    private readonly IBranchUpdator _branchUpdator;

    public TreeUpdator(TreeGeneratorBase gen, IBranchUpdator branchUpdator)
    {
        _gen = gen;
        _branchUpdator = branchUpdator;
    }

    public void Update()
    {
        if (_gen.PreviousRotateAngle == _gen.RotateAngle)
            return;
        _gen.PreviousRotateAngle = _gen.RotateAngle;
        UpdateAngle(_gen.Branch);
    }

    void UpdateAngle(Branch root)
    {
        var mainAngle = 360f / _gen.BranchingCount;

        var slide = _gen.GetTop(root.GameObject);


        for (var index = 0; index < root.Branches.Count; index++)
        {
            var rootBranch = root.Branches[index];
            _branchUpdator.UpdateBranch(root.GameObject, mainAngle * index, rootBranch.GameObject);

            UpdateAngle(rootBranch);
        }
    }
}