﻿using UnityEngine;
using UnityEngine.UI;
using UIWidgets;
using System.Collections;
using CondOptions = ConstFile.ConditionOptions;
using System;

public class ValDropdown : MonoBehaviour
{
	public delegate void valDropDownDel(int arg);
	public Dropdown varCheck;
	public SpinnerFloat spin;
	public GameObject spinObj;
	public float currVal = 0;
	
	void Awake()
	{
		varCheck.ClearOptions();
		for (int i = 0; i < ConstFile.ConditionOptionsText.Length; i++)
		{
			Dropdown.OptionData opt = new Dropdown.OptionData(ConstFile.ConditionOptionsText[i]);
			varCheck.options.Add(opt);
		}

		varCheck.value = 0;
		
		spinObj.SetActive(varCheck.value == (int)CondOptions.VALUE);
	}
	

	// Update is called once per frame
	void Update ()
	{
		currVal = spin.Value;
		spinObj.SetActive(varCheck.value == (int)CondOptions.VALUE);
	}

	void OnDestroy()
	{
		//varCheck.onValueChanged.RemoveListener(DropDownChange);
	}
}
