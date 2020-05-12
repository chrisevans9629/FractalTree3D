using System;
using System.Collections;


public class TreeGeneratorAnimated : ITreeGeneratorType
{
    public void Generate()
    {
    }

    public void Update()
    {
    }
}


public class TreeGenerator : TreeGeneratorBase
{
    private ITreeGeneratorType instant;


    // Start is called before the first frame update
    void Start()
    {
        instant = new TreeGeneratorInstant(new BranchCreator(this, new BranchUpdator(this)), new TreeUpdator(this, new BranchUpdator(this)));

        instant.Generate();
        PreviousRotateAngle = RotateAngle;
        //_previousBranchRatio = BranchRatio;
    }

    //private float _previousBranchRatio;
    // Update is called once per frame
    void Update()
    {
        instant.Update();
    }


}
