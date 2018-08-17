using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharactersButton : MonoBehaviour
{
    public int count = 1;
    public Transform character;

    private Button _button;
    private CharacterManager _characterManager;

	void Start ()
	{
	    _button = GetComponent<Button>();
	    _characterManager = GameObject.FindObjectOfType<CharacterManager>();

        _button.onClick.AddListener(HandleClick);
	}

    public void HandleClick()
    {
        _characterManager.CreateCharacters(count, character);
    }
}
