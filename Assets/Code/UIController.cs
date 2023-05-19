using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using TMPro;

public class UIController : MonoBehaviour
{

	[Foldout("StartUI")]
	[SerializeField] Button _startGameButton;

	[Foldout("StartUI")]
	[SerializeField] Button _timerUpgradeButton;
	[Foldout("StartUI")]
	[SerializeField] Image _timerUpgradeImage;
	[Foldout("StartUI")]
	[SerializeField] GameObject _timerCostObject;
	[Foldout("StartUI")]
	[SerializeField] GameObject _timerRewardedObject;
	[Foldout("StartUI")]
	[SerializeField] GameObject _timerCostText;

	[Foldout("StartUI")]
	[SerializeField] Button _sizeUpgradeButton;
	[Foldout("StartUI")]
	[SerializeField] Image _sizeUpgradeImage;
	[Foldout("StartUI")]
	[SerializeField] GameObject _sizeCostObject;
	[Foldout("StartUI")]
	[SerializeField] GameObject _sizeRewardedObject;
	[Foldout("StartUI")]
	[SerializeField] GameObject _sizeCostText;

	[Foldout("StartUI")]
	[SerializeField] Button _powerUpgradeButton;
	[Foldout("StartUI")]
	[SerializeField] Image _powerUpgradeImage;
	[Foldout("StartUI")]
	[SerializeField] GameObject _powerCostObject;
	[Foldout("StartUI")]
	[SerializeField] GameObject _powerRewardedObject;
	[Foldout("StartUI")]
	[SerializeField] GameObject _powerCostText;



	[Foldout("InGameUI")]
	[SerializeField] TMP_Text _levelNumber;

	[Foldout("WinUI")]
	[SerializeField] TMP_Text _winLevelNumber;

	[SerializeField] private TMP_Text _timerText;
	[SerializeField] private Slider _timerSlider;

	[SerializeField] private Animator _startUIAnimator;
	[SerializeField] private Animator _collectUIAnimator;
	[SerializeField] private Animator _fightUIAnimator;
	[SerializeField] private Animator _resultUIAnimator;
	[SerializeField] private Animator _moneyUIAnimator;

	private MainGameController _mainGameController;
	private int _minutes;

	private string _hide = "Hide";
	private string _show = "Show";

	private void Awake()
	{
		_mainGameController = GetComponent<MainGameController>();
		_startGameButton.onClick.AddListener(_mainGameController.StartCollectingPart);
	}
	public void EnablePlayButton(bool state)
	{
		_startGameButton.gameObject.SetActive(state);
	}
	public void SetLevelNumber(int number)
	{
		//_levelNumber.text = "Level " + (number + 1).ToString();
	}
	public void SetWinLevelNumber(int number)
	{
		_winLevelNumber.text = "Level " + (number + 1).ToString();
	}
	public void ShowStartUI()
	{
		_startUIAnimator.SetTrigger(_show);
		_moneyUIAnimator.SetTrigger(_show);
		_collectUIAnimator.SetTrigger(_show);
	}
	public void ShowCollectUI()
	{
		_startUIAnimator.SetTrigger(_hide);
	}
	public void ShowFightUI()
	{
		_collectUIAnimator.SetTrigger(_hide);
		_fightUIAnimator.SetTrigger(_show);
	}
	public void ShowResultUI()
	{
		_resultUIAnimator.SetTrigger(_show);
		_fightUIAnimator.SetTrigger(_hide);
	}
	public void SetTimerAmount(int timerAmount, float maxAmount, float currentAmount)
	{
		_timerSlider.value = currentAmount / maxAmount;
		_minutes = timerAmount / 60;
		_timerText.text = _minutes.ToString() + ":" + (timerAmount - _minutes * 60).ToString();
	}
}