using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class textWriter : MonoBehaviour
{
    private static textWriter instance;

    private List<textWriterSingle> TextWriterSinglesList;

    void Awake()
    {
        instance = this;
        TextWriterSinglesList = new List<textWriterSingle>();
    }

    public static void AddWriter_static(Text UIText, string TextToWrite, float TimePerCharacter,
        bool InvisibleCharacters)
    {
        instance.addWriter(UIText, TextToWrite, TimePerCharacter, InvisibleCharacters);
    }

    public void addWriter(Text UIText, string TextToWrite, float TimePerCharacter, bool InvisibleCharacters)
    {
        TextWriterSinglesList.Add(new textWriterSingle(UIText, TextToWrite, TimePerCharacter, InvisibleCharacters));
    }

    void Update()
    {
        for (int i = 0; i < TextWriterSinglesList.Count; i++)
        {
            bool destroyInstance = TextWriterSinglesList[i].Update();
            if (destroyInstance)
            {
                TextWriterSinglesList.RemoveAt(i);
                i--;
            }
        }
    }

    public class textWriterSingle
    {
        private Text uiText;
        private string TextToWrite;
        private int CharacterIndex;
        private float TimePerCharacter;
        private float Timer;
        private bool InvisibleCharacters;

        public textWriterSingle(Text UIText, string TextToWrite, float TimePerCharacter, bool InvisibleCharacters)
        {
            this.uiText = UIText;
            this.TextToWrite = TextToWrite;
            this.TimePerCharacter = TimePerCharacter;
            this.InvisibleCharacters = InvisibleCharacters;
            CharacterIndex = 0;
        }

        public bool Update()
        {
            Timer -= Time.deltaTime;
            {
                Timer += TimePerCharacter;
                CharacterIndex++;
                string text = TextToWrite.Substring(0, CharacterIndex);
                if (InvisibleCharacters)
                {
                    text += "<color=#00000000>" + TextToWrite.Substring(CharacterIndex) + "</color>";
                }

                if (CharacterIndex >= TextToWrite.Length) ;
                {
                    return true;
                }
                return false;
            }
        }
    }

}

