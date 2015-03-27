using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;

public class PageSingleton {

	private int pageNumber;
	private int numberOfPages;
	private int numberOfMazesToShow;
	private string[] mazeFiles;
	private static  PageSingleton instance;
	
	public static PageSingleton Instance {
		get 
		{
			if (instance == null)
			{
				instance = new PageSingleton();
			}
			return instance;
		}
	}

	private PageSingleton() {

		refresh();

	}

	public void refresh(){
		pageNumber = 1;
		
		// Classer les fichiers en ordre décroissant de la date de modification
		DirectoryInfo directory = new DirectoryInfo("./mazes/");
		string[] myFiles = directory.GetFiles().OrderByDescending(f => f.LastWriteTime).Select(f => f.Name).ToArray();
		
		mazeFiles = new string[myFiles.Length];
		for(int k=0; k<myFiles.Length; k++) {
			if(myFiles[k].Substring(myFiles[k].Length-5) == ".maze") {
				mazeFiles[k] = myFiles[k];
			}
		}
		
		//Enlever l'extension
		for(int k=0; k<mazeFiles.Length; k++) {
			mazeFiles[k] = mazeFiles[k].Substring(0,mazeFiles[k].Length-5);
		}
		
		if(mazeFiles.Length < 5) {
			numberOfMazesToShow = mazeFiles.Length;
		}
		else {
			numberOfMazesToShow = 5;
		}
		
		if(mazeFiles.Length < 5) {
			numberOfPages = 1;
		}
		else {
			numberOfPages = mazeFiles.Length / 5 + 1;
		}
	}

	public void showNextPage() {
		if(pageNumber < numberOfPages){
			++pageNumber;
			showPage();
		}
	}

	public void showPreviousPage() {
		if(pageNumber > 1){
			--pageNumber;
			showPage();
		}
	}

	public int getPageNumber(){
		return pageNumber;
	}

	public void showPage(){
		// show the current page 
		for(int j=0; j<numberOfMazesToShow; j++) {
			if(mazeFiles.Length > ((pageNumber-1)* numberOfMazesToShow) + j){
			   	((TextMesh)(GameObject.Find("Maze"+(j+1).ToString()).GetComponent("TextMesh"))).text = mazeFiles[((pageNumber-1)* numberOfMazesToShow) + j];
			}else{
				((TextMesh)(GameObject.Find("Maze"+(j+1).ToString()).GetComponent("TextMesh"))).text = "";
			}
		}

	}

}
