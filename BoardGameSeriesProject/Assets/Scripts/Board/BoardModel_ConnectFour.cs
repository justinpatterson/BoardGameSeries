using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel_ConnectFour : BoardModel
{

    public override Vector2 GetNextPlayerPosition(Vector2 inputDesiredPosition)
    {
        for (int heightValue = 0; heightValue < height; heightValue++)
        {
            Vector2 inputTargetPosition_lowest = inputDesiredPosition;
            inputTargetPosition_lowest.y = heightValue;

            if (_boardState.ContainsKey(inputTargetPosition_lowest))
            {
                if (_boardState[inputTargetPosition_lowest] == -1)
                {
                    inputTargetPosition_lowest.y = heightValue;
                    Debug.Log("LOWEST IS " + inputTargetPosition_lowest);
                    return inputTargetPosition_lowest;
                }
            }
        }
        Debug.Log("Unable to find a desired position");
        return inputDesiredPosition;
    }
}
