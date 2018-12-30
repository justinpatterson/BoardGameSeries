using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel_ConnectFour : BoardModel
{
    public override bool PlayerClaimsPosition(int inputPlayerNumber, Vector2 inputTargetPosition)
    {
        Vector2 inputTargetPosition_lowest = inputTargetPosition;
        bool foundLowest = false;
        for (int heightValue = 0; heightValue < height; heightValue++)
        {
            if (_boardState.ContainsKey(inputTargetPosition_lowest) && !foundLowest)
            {
                if (_boardState[inputTargetPosition_lowest] == -1)
                {
                    foundLowest = true;
                    inputTargetPosition_lowest.y = heightValue;
                }
            }
            else
            {
            }
        }
        return base.PlayerClaimsPosition(inputPlayerNumber, inputTargetPosition_lowest);
    }
}
