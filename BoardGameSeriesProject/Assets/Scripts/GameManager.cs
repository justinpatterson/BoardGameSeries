using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public enum GamePhases { init, start, inGame, end, restart, settings }
    public GamePhases currentPhase = GamePhases.init;
    public GamePhaseBehavior[] gamePhaseBehaviors;
	public AdController adController;
	public PlayerDataController playerDataController;
	public GlitchController glitchController;

	public BoardInfo[] boards;
	int _selectedBoard = 0;

	public BoardModel boardModel
	{
		get { return boards[_selectedBoard].boardModel; }
		//set { boardModel = value; }
	}
	public BoardView boardViewer 
	{
		get {return boards[_selectedBoard].boardView;}
		//set {boardViewer = value;}
			
	}

    public SHARED_UIController sharedUIReference;

    GamePhaseBehavior _currentPhaseBehavior;
    Results _lastResults;

	public delegate void TileClickAction(Vector2 position);
	public static event TileClickAction OnTileClicked;
    
    public delegate void BackClickAction();
    public static event BackClickAction OnBackClicked;

    public delegate void RestartClickAction();
	public static event RestartClickAction OnRestartClicked;

	public delegate void GameUpdateAction();
	public static event BackClickAction OnGameUpdated;

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
        //if (!boardModel) boardModel = GetComponent<BoardModel>();
        TriggerPhaseTransition(GamePhases.init);
        TriggerPhaseTransition(GamePhases.start);
		adController.InitializeAds();
    }

    void Update()
    {
        if (_currentPhaseBehavior) _currentPhaseBehavior.UpdatePhase();
		if(OnGameUpdated!=null) OnGameUpdated();
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
				currentPhase = inputPhase;
				if(sharedUIReference) sharedUIReference.SetSharedUIDisplay(_currentPhaseBehavior.sharedUI);
				if(PlayerPrefs.GetFloat(GameDataModel.GameSettings.firewall.ToString(), 1f) == 0f) glitchController.QueueNextGlitchEventListener();
            }
        }
    }

    public void ReportGameStartPressed()
    {
        TriggerPhaseTransition(GamePhases.inGame);
    }

	public void ReportGameSettingsOptionPressed()
	{
		TriggerPhaseTransition(GamePhases.settings);
	}

	public void ReportBoardOptionPressed(int inputOption)
	{
		_selectedBoard = Mathf.Clamp(inputOption, 0, boards.Length);

	}
    


    public void TriggerResultsGeneration(int inputWinningPlayerNumber)
    {
        Results r = new Results();
        r.roundCount = boardModel.GetCurrentRoundCount();
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
    public void ReportTilePressed(Vector2 position)
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

public class GameDataModel
{
	public enum GameSettings { volume, sound, firewall } 
}