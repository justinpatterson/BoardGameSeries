using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour {
    GameObject _boardContainer;
    public GameObject gridElementPrefab;
    Dictionary<Vector2, BoardElement> _boardElements = new Dictionary<Vector2, BoardElement>();
    public float spriteSize = 1.2f;
    public Sprite[] playerSprites;

    public virtual void GenerateBoardGridElements(Dictionary<Vector2, int> inputBoard)
    {
        if (!_boardContainer)
        {
            _boardContainer = new GameObject();
            _boardContainer.name = "boardContainer";
        }

        ClearBoardGridElements();

        foreach (Vector2 position in inputBoard.Keys)
        {
            GameObject newGridElement = Instantiate(gridElementPrefab, position * spriteSize, Quaternion.identity);
            newGridElement.transform.SetParent(_boardContainer.transform);
            if (newGridElement.GetComponent<BoardElement>())
            {
                _boardElements.Add(position, newGridElement.GetComponent<BoardElement>());
                _boardElements[position].AssignPlayerSlot(-1);
                _boardElements[position].AssignPosition(position);
            }
        }
    }

    public void ClearBoardGridElements()
    {
        if (!_boardContainer) return;

        _boardElements.Clear();
        foreach (Transform t in _boardContainer.transform) { Destroy(t.gameObject); }
    }

    public void PlayerClaimedGridAtPosition(int inputPlayerNubmer, Vector2 inputPosition)
    {
        if (_boardElements.ContainsKey(inputPosition))
            _boardElements[inputPosition].AssignPlayerSlot(inputPlayerNubmer);
    }
}
