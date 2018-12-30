using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class INIT_GamePhaseBehavior : GamePhaseBehavior {

    public override void StartPhase()
    {
        base.StartPhase();
		PlayerPrefs.SetInt( "GridSize", 3);

    }

    public override void UpdatePhase()
    {
        base.UpdatePhase();
    }

    public override void EndPhase()
    {
        base.EndPhase();
    }

}
