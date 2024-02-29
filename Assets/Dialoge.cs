using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialoge : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextComponent;
    

    [SerializeField] private float textspeed = 0.5f;

    [SerializeField] private GameObject button;
    
    private int index;
    private int Lineindex;
    private int closestmadnessIndex;

    [SerializeField] public List<customlist> mylist = new List<customlist>();

    public int npcMadness;

    [SerializeField] private GameObject Startd;
    private StartDialoge STD;
    
    void Start()
    {
        STD = Startd.GetComponent<StartDialoge>();
        TextComponent.text = string.Empty;
        startDailogo();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TextComponent.text == mylist[index].lines[Lineindex].ToString())
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                TextComponent.text = mylist[index].lines[Lineindex].ToString();
            }
        }
        
    }

    void startDailogo()
    {
        int minDifference = int.MaxValue;

        for (int i = 0; i < mylist.Count; i++)
        {
            int differnece = Mathf.Abs(npcMadness - mylist[i].madness);

            if (differnece < minDifference)
            {
                minDifference = differnece;
                closestmadnessIndex = i;
            }
        }
        
        if (npcMadness >= mylist[closestmadnessIndex].madness)
        {
            StartCoroutine(TypeLine(mylist[closestmadnessIndex].lines[Lineindex]));     
        }
    }

    IEnumerator TypeLine(string line)
    {
        foreach (char c in line)
        {
            TextComponent.text += c;
            yield return new WaitForSeconds(textspeed);
        }
    }
    void NextLine()
    {
        Lineindex++;
        if (Lineindex < mylist[index].lines.Length)
        {
            TextComponent.text = string.Empty;
            StartCoroutine(TypeLine(mylist[index].lines[Lineindex]));
        }
        else
        {
            Lineindex = 0;
            index++;
            if (index < mylist.Count)
            {
                startDailogo();
            }
            else
            {
                STD.enabled = true;
                button.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
