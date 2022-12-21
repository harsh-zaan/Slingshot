using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private TextMeshProUGUI _leftPlayerText;
    [SerializeField] private TextMeshProUGUI _rightPlayerText;
    [SerializeField] private TextMeshProUGUI _waitingUnitsText;
    [SerializeField] private TextMeshProUGUI _winText;
    public TextMeshProUGUI unitInfoText;

    // instance
    public static GameUI instance;

    void Awake()
    {
        instance = this;
    }

    // The end turn button is called when pressed
    public void OnEndTurnButton()
    {
        PlayerController.me.EndTurn();
    }

    // adjusts the clickability of the end turn button
    public void ToggleEndTurnButton(bool toggle)
    {
        _endTurnButton.interactable = toggle;
        _waitingUnitsText.gameObject.SetActive(toggle);
    }

    // shows the number of units that can be used
    public void UpdateWaitingUnitsText(int waitingUnits)
    {
        _waitingUnitsText.text = waitingUnits + " Units Waiting";
    }

    // sets the player name text
    public void SetPlayerText(PlayerController player)
    {
        TextMeshProUGUI text = player == GameManager.instance.leftPlayer ? _leftPlayerText : _rightPlayerText;
        text.text = player.photonPlayer.NickName;
    }

    // sets the unit info text
    public void SetUnitInfoText(Unit unit)
    {
        unitInfoText.gameObject.SetActive(true);
        unitInfoText.text = "";

        unitInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.curHp, unit.maxHp);
        unitInfoText.text += string.Format("\n<b>Move Range:</b> {0}", unit.maxMoveDistance);
        unitInfoText.text += string.Format("\n<b>Attack Range:</b> {0}", unit.maxAttackDistance);
        unitInfoText.text += string.Format("\n<b>Damage:</b> {0} - {1}", unit.minDamage, unit.maxDamage);
    }

    // shows win text on the screen
    public void SetWinText(string winnerName)
    {
        _winText.gameObject.SetActive(true);
        _winText.text = winnerName + " Wins";
    }
}