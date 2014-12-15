/*
  Created by:
  Juan Sebastian Munoz Arango
  naruse@gmail.com
  All rights reserved
 */

namespace ProDrawCall {
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;

	public sealed class ProDrawCallOptimizerMenu : EditorWindow {
	    private Atlasser generatedAtlas;

	    private static int selectedMenuOption = 0;
	    private static string[] menuOptions;

	    private static ProDrawCallOptimizerMenu window;
	    [MenuItem("Window/ProDrawCallOptimizer")]
	    private static void Init() {
	        ObjSorter.Initialize();

	        window = (ProDrawCallOptimizerMenu) EditorWindow.GetWindow(typeof(ProDrawCallOptimizerMenu));
	        window.minSize = new Vector2(445, 200);
	        window.Show();

            ObjectsGUI.Instance.Initialize();
            AdvancedMenuGUI.Instance.Initialize();

	        menuOptions = new string[] { "Objects",  "Advanced" };
	        selectedMenuOption = 0;
	    }

	    void OnGUI() {
	        if(NeedToReload())
	            ReloadDataStructures();
	        selectedMenuOption = GUI.SelectionGrid(new Rect(5,8,window.position.width-10, 20), selectedMenuOption, menuOptions, 2);
	        switch(selectedMenuOption) {
	            case 0:
	                ObjectsGUI.Instance.DrawGUI(window);
                    AdvancedMenuGUI.Instance.ClearConsole();
                    menuOptions[0] = "Objects";
	                break;
	            case 1:
                    AdvancedMenuGUI.Instance.DrawGUI(window);
                    menuOptions[0] = "Objects(" + ObjSorter.GetTotalSortedObjects() + ")";
                    break;
                default:
	                Debug.LogError("Unrecognized menu option: " + selectedMenuOption);
	                break;
	        }

	        if(GUI.Button(new Rect(5, window.position.height - 35, window.position.width/2 - 10, 33), "Clear Atlas")) {
	            GameObject[] objsInHierarchy = Utils.GetAllObjectsInHierarchy();
	            foreach(GameObject obj in objsInHierarchy) {
	                if(obj.name.Contains(Constants.OptimizedObjIdentifier))
	                    DestroyImmediate(obj);
                    else
                        if(obj.GetComponent<MeshRenderer>() != null)
	                        obj.GetComponent<MeshRenderer>().enabled = true;
	            }
	            // delete the folder where the atlas reside.
	            string folderOfAtlas = EditorApplication.currentScene;
				if(folderOfAtlas == "") { //scene is not saved yet.
					folderOfAtlas = Constants.NonSavedSceneFolderName + ".unity";
					Debug.LogWarning("WARNING: Scene has not been saved, clearing baked objects from NOT_SAVED_SCENE folder");
				}
	            folderOfAtlas = folderOfAtlas.Substring(0, folderOfAtlas.Length-6) + "-Atlas";//remove the ".unity"
	            if(Directory.Exists(folderOfAtlas)) {
	                FileUtil.DeleteFileOrDirectory(folderOfAtlas);
	                AssetDatabase.Refresh();
	            }
	        }

	        GUI.enabled = CheckEmptyArray(); //if there are no textures deactivate the GUI
	        if(GUI.Button(new Rect(window.position.width/2 , window.position.height - 35, window.position.width/2 - 5, 33), "Bake Atlas")) {
	            //Remove objects that are already optimized and start over.
	            if(AdvancedMenuGUI.Instance.RemoveObjectsBeforeBaking) {
	                GameObject[] objsInHierarchy = Utils.GetAllObjectsInHierarchy();
	                foreach(GameObject obj in objsInHierarchy) {
	                    if(obj.name.Contains(Constants.OptimizedObjIdentifier))
	                        GameObject.DestroyImmediate(obj);
	                }
	            }

	            string progressBarInfo = "Please wait...";
                int shadersToOptimize = ObjSorter.GetOptShaders().Count;
	            float pace = 1/(float)shadersToOptimize;
	            float progress = pace;

                for(int i = 0; i < shadersToOptimize; i++) {
                    EditorUtility.DisplayProgressBar("Optimization in progress... " +
					                                 (AdvancedMenuGUI.Instance.CreatePrefabsForObjects ? " Get coffee this will take some time..." : ""), progressBarInfo, progress);
                    progressBarInfo = "Processing shader: " + ObjSorter.GetOptShaders()[i].ShaderName;
                    ObjSorter.GetOptShaders()[i].OptimizeShader(AdvancedMenuGUI.Instance.ReuseTextures, AdvancedMenuGUI.Instance.CreatePrefabsForObjects);
                    progress += pace;
                }

	            EditorUtility.ClearProgressBar();
	            AssetDatabase.Refresh();//reimport the created atlases so they get displayed in the editor.
	        }
	    }

        //used to deactivate the "Bake Atlas" button if we dont have anything to bake
	    private bool CheckEmptyArray() {
	        for(int i = 0; i < ObjSorter.GetOptShaders().Count; i++)
	            if(ObjSorter.GetOptShaders()[i].Objects.Count > 1 ||//check that at least there are 2 objects (regardless if tex are null) OR
                   (ObjSorter.GetOptShaders()[i].Objects.Count == 1 && (ObjSorter.GetOptShaders()[i].Objects[0] != null && ObjSorter.GetOptShaders()[i].Objects[0].ObjHasMoreThanOneMaterial)))//there is at least 1 object that has multiple materials
	                return true;
	        return false;
	    }

	    void OnInspectorUpdate() {
	        Repaint();
	    }

	    private void OnDidOpenScene() {
	        //unfold all the objects to automatically clear objs from other scenes
            ObjectsGUI.Instance.UnfoldObjects();
	    }

	    private static void ReloadDataStructures() {
	        Init();

	    }

	    private bool NeedToReload() {
	        if(ObjSorter.GetOptShaders() == null)
	            return true;
	        else
	            return false;
	    }
	}
}