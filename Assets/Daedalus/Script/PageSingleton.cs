using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class PageSingleton
{

    int pageNumber;
    int numberOfPages;
    int numberOfMazesToShow;
    string[] mazeFiles;
    static  PageSingleton instance;
	
    public static PageSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PageSingleton();
            }
            return instance;
        }
    }

    PageSingleton()
    {
        Refresh();
    }

    public void Refresh()
    {
        pageNumber = 1;
		
        // Classer les fichiers en ordre décroissant de la date de modification
        DirectoryInfo directory = new DirectoryInfo("./mazes/");
        string[] myFiles = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Select(f => f.Name).ToArray();
		
        mazeFiles = new string[myFiles.Length];
        for (int k = 0; k < myFiles.Length; k++)
        {
            if (myFiles [k].Substring(myFiles [k].Length - 5) == ".maze")
            {
                mazeFiles [k] = myFiles [k];
            }
        }
		
        //Enlever l'extension
        for (int k=0; k<mazeFiles.Length; k++)
        {
            mazeFiles [k] = mazeFiles [k].Substring(0, mazeFiles [k].Length - 5);
        }
		
        if (mazeFiles.Length < 5)
        {
            numberOfMazesToShow = mazeFiles.Length;
        }
        else
        {
            numberOfMazesToShow = 5;
        }
		
        if (mazeFiles.Length < 5)
        {
            numberOfPages = 1;
        }
        else
        {
            numberOfPages = mazeFiles.Length / 5 + 1;
        }
    }

    public void ShowNextPage()
    {
        if (pageNumber < numberOfPages)
        {
            ++pageNumber;
            ShowPage();
        }
    }

    public void ShowPreviousPage()
    {
        if (pageNumber > 1)
        {
            --pageNumber;
            ShowPage();
        }
    }

    public void ShowPage()
    {
        // show the current page 
        for (int j=0; j<numberOfMazesToShow; j++)
        {
            TextMesh textMesh = ((TextMesh)(GameObject.Find("Maze" + (j + 1)).GetComponent("TextMesh")));
            if (mazeFiles.Length > ((pageNumber - 1) * numberOfMazesToShow) + j)
            {
                textMesh.text = mazeFiles [((pageNumber - 1) * numberOfMazesToShow) + j];
            }
            else
            {
                textMesh.text = "";
            }
        }
    }

}
