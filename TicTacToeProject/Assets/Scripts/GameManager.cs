using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TicTacToeBoardController))]
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public enum GamePhases { init, start, inGame, end, restart }
    public GamePhases currentPhase = GamePhases.init;
    public GamePhaseBehavior[] gamePhaseBehaviors;
    public TicTacToeBoardController ticTacToeBoardReference;
    public SHARED_UIController sharedUIReference;

    GamePhaseBehavior _currentPhaseBehavior;
    Results _lastResults;

	public delegate void TileClickAction(Vector2 position);
	public static event TileClickAction OnTileClicked;
    
    public delegate void BackClickAction();
    public static event BackClickAction OnBackClicked;

    public delegate void RestartClickAction();
    public static event RestartClickAction OnRestartClicked;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (!ticTacToeBoardReference) ticTacToeBoardReference = GetComponent<TicTacToeBoardController>();
        TriggerPhaseTransition(GamePhases.init);
        TriggerPhaseTransition(GamePhases.start);
    }

    void Update()
    {
        if (_currentPhaseBehavior) _currentPhaseBehavior.UpdatePhase();
    }

    public void TriggerPhaseTransition(GamePhases inputPhase)
    {

        if (_currentPhaseBehavior)
        {
            _currentPhaseBehavior.EndPhase();
        }

        foreach (GamePhaseBehavior gpb in gamePhaseBehaviors)
        {
            if (gpb.phase == inputPhase)
            {
                gpb.StartPhase();
                _currentPhaseBehavior = gpb;
                if(sharedUIReference) sharedUIReference.SetSharedUIDisplay(_currentPhaseBehavior.sharedUI);
            }
        }
    }

    public void ReportGameStartPressed()
    {
        TriggerPhaseTransition(GamePhases.inGame);
    }
    


    public void TriggerResultsGeneration(int inputWinningPlayerNumber)
    {
        Results r = new Results();
        r.roundCount = ticTacToeBoardReference.GetCurrentRoundCount();
        r.winningPlayerNumber = inputWinningPlayerNumber;
        _lastResults = r;
    }
    public Results GetResults()
    {
        return _lastResults;
    }

    #region DELEGATES
    public void ReportTicTacToeTilePressed(Vector2 position)
    {
        if (OnTileClicked != null)
            OnTileClicked(position);
    }
    public void ReportBackPressed()
    {
        if (OnBackClicked != null)
            OnBackClicked();
    }
    public void ReportRestartPressed()
    {
        if (OnRestartClicked != null)
            OnRestartClicked();
    }
    #endregion
}
