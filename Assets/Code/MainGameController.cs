using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class MainGameController : MonoBehaviour
{
	public SingleLevel[] Levels;

	private UIController _uiController;
	private EnemyController _enemyController;

	private string _currentLevel = "CurrentlevelFinal";
	private string _absoluteLevel = "LevelsCompleted";

	private bool _gameStarted;

	private void Awake()
	{
		_enemyController = GetComponent<EnemyController>();
	}
	private void Start()
	{
		_uiController = GetComponent<UIController>();
		_uiController.SetLevelNumber(PlayerPrefs.GetInt(_absoluteLevel));
		_uiController.ShowStartUI();
		_enemyController.OnEnemyComplete += FinishLevel;
		BuildLevel();
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
		_uiController.ShowCollectUI();
	}
	[Button]
	public void StartFightingPart()
	{
		_uiController.ShowFightUI();
	}
	public void FinishLevel(bool IsEnemyDefeated)
	{
		_uiController.ShowResultUI();
	}
	private void BuildLevel()
	{
		for (int i = 0; i < Levels.Length; i++)
		{
			Levels[i].LevelObject.SetActive(false);
		}
		if (PlayerPrefs.GetInt(_currentLevel) >= Levels.Length)
		{
			PlayerPrefs.SetInt(_currentLevel, 0);
		}
		Levels[PlayerPrefs.GetInt(_currentLevel)].LevelObject.SetActive(true);
		_enemyController.SetHealth(Levels[PlayerPrefs.GetInt(_currentLevel)].EnemyHealth);
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
