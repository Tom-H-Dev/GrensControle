using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivedText : MonoBehaviour
{
    [SerializeField] private GameObject Text;
    [SerializeField] private GameObject ButtonContainer;

    public void AskQuistion()
    {
        Text.SetActive(true);
        ButtonContainer.SetActive(false);
    }
    
    
}
