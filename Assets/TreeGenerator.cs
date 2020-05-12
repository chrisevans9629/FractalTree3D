using System;
using System.Collections;


public class TreeGenerator : TreeGeneratorBase
{
    private ITreeGeneratorType instant;


    // Start is called before the first frame update
    void Start()
    {
        instant = new TreeGeneratorInstant(this);

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
