using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class MainGameController : MonoBehaviour
{
	public SingleLevel[] Levels;

	private UIController _uiController;
	private PlayerMovementController _payerMovementController;
	private BuildController _buildController;
	private CameraController _cameraController;
	private HoleController _holeController;
	private TimerController _timerController;

	private string _currentLevel = "CurrentlevelFinal";
	private string _absoluteLevel = "LevelsCompleted";

	private bool _gameStarted;

	private void Awake()
	{
	}
	private void Start()
	{
		Application.targetFrameRate = 60;
		_uiController = GetComponent<UIController>();
		_payerMovementController = GetComponent<PlayerMovementController>();
		_buildController = GetComponent<BuildController>();
		_cameraController = GetComponent<CameraController>();
		_holeController = GetComponent<HoleController>();
		_timerController = GetComponent<TimerController>();
		_payerMovementController.onFirstInputTaken += StartCollectingPart;
		_uiController.SetLevelNumber(PlayerPrefs.GetInt(_absoluteLevel));
		_uiController.ShowStartUI();
		BuildLevel();
		_holeController.SetMaxAmountOfBlocks(Levels[PlayerPrefs.GetInt(_currentLevel)].GetComponentsInChildren<SingleBlock>().Length);
		_holeController.onCollectingComplete += StartBuildingPart;
		_timerController.onTimeEnd += StartBuildingPart;
		_buildController.onBuildingComplete += FinishLevel;
	}
	private void Update()
	{
		if (!_gameStarted)
		{

		}
	}
	[Button]
	public void StartCollectingPart()
	{
		_gameStarted = true; 
		_timerController.StartTimer();
		_uiController.ShowCollectUI();
		_uiController.EnablePlayButton(false);
	}
	[Button]
	public void StartBuildingPart()
	{
		_cameraController.StartChangeCameraRotation(_buildController.GetCameraGoalTransform());
		_uiController.ShowFightUI();
		_payerMovementController.StartMoveToBuildPosition();
		_buildController.StartBuilding(_holeController.GetCollectedBlocks(), Levels[PlayerPrefs.GetInt(_currentLevel)].LevelFigure);
	}
	public void FinishLevel(bool isFullyBuild)
	{
		//_uiController.ShowResultUI();
		//Invoke(nameof(RestartScene), 1f);
		Invoke(nameof(TempLevelFinish), 3f);
	}
	private void TempLevelFinish()
	{
		_uiController.ShowResultUI();
		Invoke(nameof(RestartScene), 1f);
	}
	private void BuildLevel()
	{
		for (int i = 0; i < Levels.Length; i++)
		{
			Levels[i].gameObject.SetActive(false);
		}
		if (PlayerPrefs.GetInt(_currentLevel) >= Levels.Length)
		{
			PlayerPrefs.SetInt(_currentLevel, 0);
		}
		Levels[PlayerPrefs.GetInt(_currentLevel)].gameObject.SetActive(true);
	}
	public void LevelLose()
	{
		Invoke(nameof(TurnOnLoseUI), 2f);
	}
	public void LevelWin()
	{
		_uiController.SetWinLevelNumber(PlayerPrefs.GetInt(_absoluteLevel));
		Savelevel();
		Invoke(nameof(MakeCameraConfettiParticles), 0.5f);
		Invoke(nameof(TurnOnWinUI), 2f);
	}
	private void MakeCameraConfettiParticles()
	{
		//ParticlesManager.Current.MakeConfettiParticles(_cameraController.CameraTransform.position + _cameraController.CameraTransform.forward * 3f + Vector3.up);
	}
	private void TurnOnWinUI()
	{
	}
	private void TurnOnLoseUI()
	{
	}
	public void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	private void Savelevel()
	{
		if (PlayerPrefs.GetInt(_currentLevel) + 1 == Levels.Length)
		{
			PlayerPrefs.SetInt(_currentLevel, 0);
			PlayerPrefs.SetInt(_absoluteLevel, PlayerPrefs.GetInt(_absoluteLevel) + 1);
		}
		else
		{
			PlayerPrefs.SetInt(_currentLevel, PlayerPrefs.GetInt(_currentLevel) + 1);
			PlayerPrefs.SetInt(_absoluteLevel, PlayerPrefs.GetInt(_absoluteLevel) + 1);
		}
	}
}
