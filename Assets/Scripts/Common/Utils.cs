using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Utils {
  public static Regex ValidInputChars = new Regex("[^A-Za-z0-9 ]");
  public static Dictionary<KeyCode, char> TextInputKeyCodes = new Dictionary<KeyCode, char>() {
		//Letters
		{ KeyCode.A,'a'},
    { KeyCode.B,'b'},
    { KeyCode.C,'c'},
    { KeyCode.D,'d'},
    { KeyCode.E,'e'},
    { KeyCode.F,'f'},
    { KeyCode.G,'g'},
    { KeyCode.H,'h'},
    { KeyCode.I,'i'},
    { KeyCode.J,'j'},
    { KeyCode.K,'k'},
    { KeyCode.L,'l'},
    { KeyCode.M,'m'},
    { KeyCode.N,'n'},
    { KeyCode.O,'o'},
    { KeyCode.P,'p'},
    { KeyCode.Q,'q'},
    { KeyCode.R,'r'},
    { KeyCode.S,'s'},
    { KeyCode.T,'t'},
    { KeyCode.U,'u'},
    { KeyCode.V,'v'},
    { KeyCode.W,'w'},
    { KeyCode.X,'x'},
    { KeyCode.Y,'y'},
    { KeyCode.Z,'z'},
     
		//KeyPad Numbers
		{ KeyCode.Keypad1,'1'},
    { KeyCode.Keypad2,'2'},
    { KeyCode.Keypad3,'3'},
    { KeyCode.Keypad4,'4'},
    { KeyCode.Keypad5,'5'},
    { KeyCode.Keypad6,'6'},
    { KeyCode.Keypad7,'7'},
    { KeyCode.Keypad8,'8'},
    { KeyCode.Keypad9,'9'},
    { KeyCode.Keypad0,'0'},
  
		//Alpha Numbers
		{ KeyCode.Alpha1,'1'},
    { KeyCode.Alpha2,'2'},
    { KeyCode.Alpha3,'3'},
    { KeyCode.Alpha4,'4'},
    { KeyCode.Alpha5,'5'},
    { KeyCode.Alpha6,'6'},
    { KeyCode.Alpha7,'7'},
    { KeyCode.Alpha8,'8'},
    { KeyCode.Alpha9,'9'},
    { KeyCode.Alpha0,'0'},

		//Other Symbols
		{ KeyCode.Space, ' ' },
		/*
		//Other Symbols
		{'!', KeyCode.Exclaim}, //1
		{'"', KeyCode.DoubleQuote},
		{'#', KeyCode.Hash}, //3
		{'$', KeyCode.Dollar}, //4
		{'&', KeyCode.Ampersand}, //7
		{'\'', KeyCode.Quote}, //remember the special forward slash rule... this isnt wrong
		{'(', KeyCode.LeftParen}, //9
		{')', KeyCode.RightParen}, //0
		{'*', KeyCode.Asterisk}, //8
		{'+', KeyCode.Plus},
		{',', KeyCode.Comma},
		{'-', KeyCode.Minus},
		{'.', KeyCode.Period},
		{'/', KeyCode.Slash},
		{':', KeyCode.Colon},
		{';', KeyCode.Semicolon},
		{'<', KeyCode.Less},
		{'=', KeyCode.Equals},
		{'>', KeyCode.Greater},
		{'?', KeyCode.Question},
		{'@', KeyCode.At}, //2
		{'[', KeyCode.LeftBracket},
		{'\\', KeyCode.Backslash}, //remember the special forward slash rule... this isnt wrong
		{']', KeyCode.RightBracket},
		{'^', KeyCode.Caret}, //6
		{'_', KeyCode.Underscore},
		{'`', KeyCode.BackQuote},

		{'A', KeyCode.KeypadPeriod},
		{'B', KeyCode.KeypadDivide},
		{'C', KeyCode.KeypadMultiply},
		{'D', KeyCode.KeypadMinus},
		{'F', KeyCode.KeypadPlus},
		{'G', KeyCode.KeypadEquals},
		*/
	};
  public static string RemoveBadChars(string word) {
    return ValidInputChars.Replace(word, "");
  }

  public static List<T> Shuffle<T>(List<T> list) {
    List<T> oldList = list != null ? new List<T>(list) : new List<T>();
    List<T> newList = new List<T>();
    int i;
    while (oldList.Count > 0) {
      i = UnityEngine.Random.Range(0, oldList.Count);
      newList.Add(oldList[i]);
      oldList.RemoveAt(i);
    }

    return newList;
  }
}