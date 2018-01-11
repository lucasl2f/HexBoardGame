using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	
	public enum PlayerActive {Player1, Player2};

	PlayerActive _playerActive;

	public PlayerActive playerActive {
		get {
			return _playerActive;
		}
	}

	[SerializeField]
	Text playerActiveText;
	
	public static GameController instance;
	
	void Awake () {
		instance = this;

		_playerActive = PlayerActive.Player1;
		playerActiveText.text = _playerActive.ToString() + " playing.";
	}
	
	void Start() {
		
	}

	public void ChangePlayer () {		
		if (_playerActive == PlayerActive.Player2) {
			_playerActive = PlayerActive.Player1;
		} else {
			_playerActive = PlayerActive.Player2;
		}

		playerActiveText.text = _playerActive.ToString() + " playing.";
	}
}