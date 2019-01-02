using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardModel : MonoBehaviour {
    public int height = 3;
    public int width = 3;
    public Vector2[] winningDirections;
    public int winningCount = 3;

    //players can own a board entry, but by default -1 means it's empty
    protected Dictionary<Vector2, int> _boardState = new Dictionary<Vector2, int>();
    protected List<Vector2> _boardTurnHistory = new List<Vector2>();
    protected int _lastPlayerNumber;
    protected Vector2 _lastPlayerPosition;
	protected WinVectorData _lastWinVectorData;

    public virtual void Init()
    {
        _boardTurnHistory.Clear();
		/*
        width = PlayerPrefs.GetInt("GridSize");
        height = PlayerPrefs.GetInt("GridSize");
        winningCount = PlayerPrefs.GetInt("GridSize");
        */
        GenerateBoard(width, height);
		GameManager.instance.boardViewer.GenerateBoardGridElements(_boardState);
    }

    void GenerateBoard(int w, int h)
    {
        ClearBoard();

        for (int widthValue = 0; widthValue < w; widthValue++)
        {
            for (int heightValue = 0; heightValue < h; heightValue++)
            {
                _boardState.Add(new Vector2(widthValue, heightValue), -1);
            }
        }
    }

    public void ClearBoard()
    {
        _boardState.Clear();
        GameManager.instance.boardViewer.ClearBoardGridElements();
    }

    public virtual Vector2 GetNextPlayerPosition(Vector2 inputDesiredPosition)
    {
        return inputDesiredPosition;
    }

    public virtual bool PlayerClaimsPosition(int inputPlayerNumber, Vector2 inputTargetPosition)
    {
        if (_boardState.ContainsKey(inputTargetPosition))
        {
            if (_boardState[inputTargetPosition] == -1)
            {
                _lastPlayerNumber = inputPlayerNumber;
                _lastPlayerPosition = inputTargetPosition;
                if (inputPlayerNumber != -1) _boardTurnHistory.Add(inputTargetPosition);
                _boardState[inputTargetPosition] = inputPlayerNumber;
                return true;
            }
            else
            {
                _lastPlayerNumber = -1;
                _lastPlayerPosition = Vector2.one * -1;
                return false;
            }
        }
        else
        {
            _lastPlayerNumber = -1;
            _lastPlayerPosition = Vector2.one * -1;
            return false;
        }
    }

	public bool CheckWinState()
    {
        if (_lastPlayerNumber == -1)
        {
            return false;
        }
        return CheckWinState_AllPositions();
    }
    bool CheckWinState_AllPositions()
    {
        bool win = false;
		_lastWinVectorData = new WinVectorData();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 checkposition = new Vector2(x, y);
                if (_boardState.ContainsKey(checkposition))
                {
                    if (_boardState[checkposition] == _lastPlayerNumber)
                    {
						if (win == false) 
						{
							win = CheckWinState_AtPosition(checkposition, _lastPlayerNumber);
						}
                    }
                }
            }
        }

        return win;
    }
    bool CheckWinState_AtPosition(Vector2 inputPosition, int inputPlayerNumber)
    {
        int maxWinCount = 0;
        foreach (Vector2 direction in winningDirections)
        {
			int currentWinCount = 1;
			for (int i = 1; i < winningCount; i++)
            {
                string output = "Checking Space:" + direction + "...";
                Vector2 targetPosition = new Vector2();
                targetPosition.x = inputPosition.x + direction.x * i;
                targetPosition.y = inputPosition.y + direction.y * i;

                if (targetPosition.x < 0 || targetPosition.x >= width || targetPosition.y < 0 || targetPosition.y >= height)
                {
                    output += "INVALID";
                }
                else
                {
                    output += "VALID:" + inputPlayerNumber + " vs " + _boardState[targetPosition];

                    if (_boardState[targetPosition] == inputPlayerNumber)
                    {
                        currentWinCount++;
                        if (currentWinCount > maxWinCount)
                        {
                            maxWinCount = currentWinCount;
							if((maxWinCount >= winningCount))
							{
								_lastWinVectorData = new WinVectorData(inputPosition * GameManager.instance.boardViewer.spriteSize, targetPosition * GameManager.instance.boardViewer.spriteSize);
							}
                        }
                    }

                }

				//Debug.Log(output);
            }
        }
        return (maxWinCount >= winningCount);
    }

    public int GetCurrentRoundCount()
    {
        if (_boardTurnHistory.Count > 0)
            return _boardTurnHistory.Count / 2;
        else
            return 0;
    }
    public int GetCurrentTurnCount()
    {
        return _boardTurnHistory.Count;
    }
	public WinVectorData GetWinVectorData()
	{
		return _lastWinVectorData;
	}
}

