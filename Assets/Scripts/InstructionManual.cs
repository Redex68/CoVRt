using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManual : MonoBehaviour
{
    private int pageIndex;
    public GameObject cover;
    public List<GameObject> pages;

    // Start is called before the first frame update
    void Start()
    {
        pageIndex = -2;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextPage()
    {
        // deactivate current pages
        cover.SetActive(false);
        if (pageIndex >= 0)
        {
            pages[pageIndex].SetActive(false);
            pages[pageIndex + 1].SetActive(false);
        }

        pageIndex += 2;
        if (pageIndex >= pages.Count) pageIndex = pages.Count - 2;

        pages[pageIndex].SetActive(true);
        pages[pageIndex + 1].SetActive(true);
    }

    public void PrevPage()
    {
        // deactivate current pages
        cover.SetActive(false);
        if (pageIndex >= 0)
        {
            pages[pageIndex].SetActive(false);
            pages[pageIndex + 1].SetActive(false);
        }

        // decrement index
        pageIndex -= 2;
        if (pageIndex < -2) pageIndex = -2;

        // update manual; display cover if index = -1, else current index page + next
        if (pageIndex == -2) cover.SetActive(true);
        else
        {
            pages[pageIndex].SetActive(true);
            pages[pageIndex + 1].SetActive(true);
        }
    }
}
