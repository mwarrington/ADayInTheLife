/*
  Created by:
  Juan Sebastian Munoz Arango
  naruse@gmail.com
  All rights reserved
 */

namespace ProDrawCall {
    using UnityEngine;
    using UnityEditor;
    using System.Collections;
    using System.Collections.Generic;

    public sealed class ObjectsGUI {
        static readonly ObjectsGUI instance = new ObjectsGUI();
        public static  ObjectsGUI Instance { get { return instance; } }
        private ObjectsGUI() { Initialize(); }

        private int objectsSize = 1;

	    private Vector2 arraysScrollPos = Vector2.zero;
	    private static string customAtlasName = "";
        public static string CustomAtlasName { get { return customAtlasName; } }

	    private static List<bool> unfoldedObjects;

	    private static GUIStyle normalStyle;
	    private static GUIStyle errorStyle;
        private static GUIStyle smallTextStyle;
	    private static GUIStyle smallTextErrorStyle;
		private static GUIStyle smallTextWarningStyle;
	    private static GUIStyle warningStyle;

        public void Initialize() {
	        normalStyle = new GUIStyle();

            errorStyle = new GUIStyle();
	        errorStyle.normal.textColor = Color.red;

            smallTextStyle = new GUIStyle();
	        smallTextStyle.fontSize = 9;

            smallTextErrorStyle = new GUIStyle();
	        smallTextErrorStyle.normal.textColor = Color.red;
	        smallTextErrorStyle.fontSize = 9;

            smallTextWarningStyle = new GUIStyle();
			smallTextWarningStyle.normal.textColor = new Color(0.7725f, 0.5255f, 0);//~ dark yellow
			smallTextWarningStyle.fontSize = 9;

            warningStyle = new GUIStyle();
	        warningStyle.normal.textColor = Color.yellow;
	        warningStyle.fontSize = 8;

            customAtlasName = "";
            unfoldedObjects = new List<bool>();
	        unfoldedObjects.Add(false);
        }

        //used when changing scenes to automatically clear objs
        public void UnfoldObjects() {
            for(int i = 0; i < unfoldedObjects.Count; i++)
                unfoldedObjects[i] = true;
        }

        //Fills the array of textures with the selected objects in the hierarchy view
	    //adds to the end all the objects.
	    public void FillArrayWithSelectedObjects(GameObject[] arr) {
	        //dont include already optimized objects
	        List<GameObject> filteredArray = new List<GameObject>();
	        for(int i = 0; i < arr.Length; i++)
	            if(!arr[i].name.Contains(Constants.OptimizedObjIdentifier))
	                filteredArray.Add(arr[i]);
	            else
	                Debug.LogWarning("Skipping " + arr[i].name + " game object as is already optimized.");


	        bool filledTexture = false;
	        for(int i = 0; i < filteredArray.Count; i++) {
	            filledTexture = false;
	            for(int j = 0; j < ObjSorter.GetOptShaders().Count; j++) {
	                for(int k = 0; k < ObjSorter.GetOptShaders()[j].Objects.Count; k++) {
	                    if(ObjSorter.GetOptShaders()[j].Objects[k] == null) {
	                        if(!ObjectRepeated(filteredArray[i])) {
	                            ObjSorter.GetOptShaders()[j].SetObjectAtIndex(k, new OptimizableObject(filteredArray[i]));
	                            filledTexture = true;
	                            break;
	                        } else {
	                            Debug.LogWarning("Game Object " + filteredArray[i].name + " is already in the list.");
	                        }
	                    }
	                }
	                if(filledTexture)
	                    break;
	            }
	            //if we didnt find an empty spot in the array, lets just add it to the texture list.
	            if(!filledTexture) {
	                if(!ObjectRepeated(filteredArray[i])) {
	                    ObjSorter.AddObject(filteredArray[i]);//adds also null internally to increase space for textures
	                    filledTexture = true;
	                    objectsSize++;
	                } else {
	                    Debug.LogWarning("Game Object " + filteredArray[i].name + " is already in the list.");
	                }
	            }
	        }
	    }

	    //checks if a gameObject is already in the list.
	    private bool ObjectRepeated(GameObject g) {
	        if(g == null)
	            return false;
	        int instanceID = g.GetInstanceID();
	        for(int i = 0; i < ObjSorter.GetOptShaders().Count; i++) {
	            for(int j = 0; j < ObjSorter.GetOptShaders()[i].Objects.Count; j++) {
	                if(ObjSorter.GetOptShaders()[i].Objects[j] != null && instanceID == ObjSorter.GetOptShaders()[i].Objects[j].GameObj.GetInstanceID())
	                    return true;
	            }
	        }
	        return false;
	    }

        private void EmptyObjsAndTexturesArray() {
	        objectsSize = 1;
	        ObjSorter.AdjustArraysSize(objectsSize);
	        for(int i = 0; i < ObjSorter.GetOptShaders().Count; i++) {
	            for(int j = 0; j < ObjSorter.GetOptShaders()[i].Objects.Count; j++) {
	                ObjSorter.GetOptShaders()[i].SetObjectAtIndex(j, null);
	            }
	            ObjSorter.GetOptShaders()[i].Objects.Clear();
	        }
	    }

	    private void AdjustArraysWithObjSorter() {
	        if(unfoldedObjects.Count != ObjSorter.GetOptShaders().Count) {
	            int offset = ObjSorter.GetOptShaders().Count - unfoldedObjects.Count;
	            bool removing = false;
	            if(offset < 0) {
	                offset *= -1;
	                removing = true;
	            }
	            for(int i = 0; i < (offset < 0 ? offset*-1 : offset); i++) {
	                if(removing) {
	                    unfoldedObjects.RemoveAt(unfoldedObjects.Count-1);
	                } else {
	                    unfoldedObjects.Add(true);
	                }
	            }
	        }
	    }

        public void DrawGUI(ProDrawCallOptimizerMenu window) {
            GUILayout.BeginArea(new Rect(5,30, window.position.width-10, 75));
            GUILayout.Space(3);
            GUILayout.BeginHorizontal();
                if(GUILayout.Button("Add all scene\nobjects", GUILayout.Width(85), GUILayout.Height(32))) {
                    EmptyObjsAndTexturesArray();
                    FillArrayWithSelectedObjects(Utils.GetAllObjectsInHierarchy());
                    return;//wait for next frame to recalculate objects
                }
                GUI.enabled = (Selection.activeGameObject != null);
                if(GUILayout.Button("Add selected\nobjects", GUILayout.Width(85), GUILayout.Height(32))) {
                    FillArrayWithSelectedObjects(Selection.gameObjects);
                    return;
                }
                if(GUILayout.Button("Add selected\nand children", GUILayout.Width(85), GUILayout.Height(32))) {
                    GameObject[] selectedGameObjects = Selection.gameObjects;

                    List<GameObject> objsToAdd = new List<GameObject>();
                    for(int i = 0; i < selectedGameObjects.Length; i++) {
                        Transform[] selectedObjs = selectedGameObjects[i].GetComponentsInChildren<Transform>(true);
                        for(int j = 0; j < selectedObjs.Length; j++)
                            objsToAdd.Add(selectedObjs[j].gameObject);
                    }
                    FillArrayWithSelectedObjects(objsToAdd.ToArray());
                    return;
                }
                GUI.enabled = true;
                GUILayout.BeginVertical();
                    GUILayout.Space(-0.5f);
                    EditorGUILayout.HelpBox("- Click \'Advanced\' to search objects by tags or layers." +
                                            (AdvancedMenuGUI.Instance.CreatePrefabsForObjects ? "\n- You have marked \"Create prefabs\" on the advanced tab, this bake will be SLOW..." : ""), MessageType.Info);
                GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            objectsSize = ObjSorter.GetTotalSortedObjects();
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(140));
                    GUILayout.Space(6);
                    GUILayout.Label("Objects to optimize: " + objectsSize, GUILayout.Width(140));
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Width(25));
                    if(GUILayout.Button("+", GUILayout.Width(23), GUILayout.Height(12))) {
                        objectsSize++;
                    }
                    GUILayout.Space(-2);
                    if(GUILayout.Button("-", GUILayout.Width(23), GUILayout.Height(12))) {
                        objectsSize--;
                    }
                GUILayout.EndVertical();
                GUILayout.Space(-3);
                GUILayout.BeginVertical(GUILayout.Width(55));
                    GUILayout.Space(-0.5f);
                    if(GUILayout.Button("Clear", GUILayout.Width(55), GUILayout.Height(24))) {
                        EmptyObjsAndTexturesArray();
                    }
                    GUILayout.EndVertical();
                    GUILayout.BeginVertical();
                        GUILayout.Space(-6);
                        GUILayout.Label("Atlasses prefix name(Optional):");
                        GUILayout.Space(-3);
                        customAtlasName = GUILayout.TextField(customAtlasName);
                    GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            GUILayout.EndArea();

            /*EditorGUI.HelpBox(new Rect(237, 75, 133, 30),
              "If checked, each time there is an atlas baking process starting all the optimized objects get destroyed, un check this when you want manually to keep track of your optimized objects",
              MessageType.Info);*/

            objectsSize = objectsSize < 1 ? 1 : objectsSize;//no neg size

            ObjSorter.AdjustArraysSize(objectsSize);
            ObjSorter.SortObjects();
            AdjustArraysWithObjSorter();

            arraysScrollPos = GUI.BeginScrollView(new Rect(0, 100, window.position.width, window.position.height - 138),
                                                  arraysScrollPos,
                                                  new Rect(0,0, window.position.width-20, (ObjSorter.GetTotalSortedObjects() + ObjSorter.GetOptShaders().Count)*(32.5f)));

            int drawingPos = 0;
            for(int i = 0; i < ObjSorter.GetOptShaders().Count; i++) {
                string shaderName = (ObjSorter.GetOptShaders()[i].Objects[0] != null && ObjSorter.GetOptShaders()[i].Objects[0].IsCorrectlyAssembled) ? ObjSorter.GetOptShaders()[i].Objects[0].ShaderName : "";
                bool positionIsAShader = (shaderName != "");
                string shaderLabel = (i+1).ToString() + ((positionIsAShader) ? ". Shader: " + shaderName + "." : ". Not optimizable: ") + " (" + ObjSorter.GetOptShaders()[i].Objects.Count + ")";
                unfoldedObjects[i] = EditorGUI.Foldout(new Rect(3, drawingPos*30+(positionIsAShader ? 19 : 24), 300, 15),
                                                       unfoldedObjects[i],
                                                       "");
                GUI.Label(new Rect(20, drawingPos*30+(positionIsAShader ? 19 : 24), 300, 15),
                          shaderLabel,
                          (positionIsAShader) ? normalStyle : errorStyle);
                if(positionIsAShader) {
                    if(ObjSorter.GetOptShaders()[i].Objects.Count > 1 || //array has at least more than one texture OR
                       (ObjSorter.GetOptShaders()[i].Objects.Count == 1 && ObjSorter.GetOptShaders()[i].Objects[0].ObjHasMoreThanOneMaterial)) {//if there is 1 object that has multiple materials
                        int aproxAtlasSize = ObjSorter.GetAproxAtlasSize(i, AdvancedMenuGUI.Instance.ReuseTextures);
                        string msg = " Aprox Atlas Size: ~(" + aproxAtlasSize + "x" + aproxAtlasSize + ")+" + (Constants.AtlasResizeFactor*100) + "%+";
                        GUIStyle msgStyle = smallTextStyle;
                        if(aproxAtlasSize > Constants.MaxAtlasSize) {
                            msg += " TOO BIG!!!";
                            msgStyle = smallTextErrorStyle;
                        } else if(aproxAtlasSize > Constants.MaxSupportedUnityTexture) {
                            msg += " Texture will be imported to 4096x4096max";
                            msgStyle = smallTextWarningStyle;
                        }
                        GUI.Label(new Rect(15, drawingPos * 30 + 33, 300, 10), msg, msgStyle);
                    } else {
                        GUI.Label(new Rect(15, drawingPos*30+33, 300, 10),"Not optimizing as there needs to be at least 2 textures to atlas.", warningStyle);
                    }
                }



                if(GUI.Button(new Rect(window.position.width-40, drawingPos*30+23, 23,20),"X")) {
                    if(ObjSorter.GetOptShaders().Count > 1) {
                        unfoldedObjects.RemoveAt(i);
                        ObjSorter.Remove(i);
                    } else {
                        ObjSorter.GetOptShaders()[0].Objects.Clear();
                        ObjSorter.GetOptShaders()[0].Objects.Add(null);
                        //ObjSorter.GetObjs()[0].Clear();
                        //ObjSorter.GetObjs()[0].Add(null);
                    }
                    return;
                }
                drawingPos++;
                if(unfoldedObjects[i]) {
                    for(int j = 0; j < ObjSorter.GetOptShaders()[i].Objects.Count; j++) {
                        GUI.Label(new Rect(20, drawingPos*30+20 + 6, 30, 25), (j+1).ToString() +":");
                        GameObject testObj = (GameObject) EditorGUI.ObjectField(new Rect(41, drawingPos*30 + 24, 105, 17),
                                                                                "",
                                                                                (ObjSorter.GetOptShaders()[i].Objects[j] != null) ? ObjSorter.GetOptShaders()[i].Objects[j].GameObj : null,
                                                                                typeof(GameObject),
                                                                                true);
                        //dont let repeated game objects get inserted in the list.
                        if(testObj != null) {
                            if(ObjSorter.GetOptShaders()[i].Objects[j] == null ||
                               testObj.GetInstanceID() != ObjSorter.GetOptShaders()[i].Objects[j].GameObj.GetInstanceID()) {
                                if(!ObjectRepeated(testObj))
                                    ObjSorter.GetOptShaders()[i].Objects[j] = new OptimizableObject(testObj);
                                else
                                    Debug.LogWarning("Game Object " + testObj.name + " is already in the list.");
                            }
                        }
                        if(ObjSorter.GetOptShaders()[i].Objects[j] != null) {
                            if(ObjSorter.GetOptShaders()[i].Objects[j].GameObj != null) {
                                if(ObjSorter.GetOptShaders()[i].Objects[j].IsCorrectlyAssembled) {
                                    if(ObjSorter.GetOptShaders()[i].Objects[j].MainTexture != null) {
                                        EditorGUI.DrawPreviewTexture(new Rect(170, drawingPos*30+18, 25, 25),
                                                                     ObjSorter.GetOptShaders()[i].Objects[j].MainTexture,
                                                                     null,
                                                                     ScaleMode.StretchToFill);

                                        GUI.Label(new Rect(198,drawingPos*30 + 24, 105, 25),
                                                  ((ObjSorter.GetOptShaders()[i].Objects[j].ObjHasMoreThanOneMaterial)?"~":"")+
                                                  "(" + ObjSorter.GetOptShaders()[i].Objects[j].TextureSize.x +
                                                  "x" +
                                                  ObjSorter.GetOptShaders()[i].Objects[j].TextureSize.y + ")" +
                                                  ((ObjSorter.GetOptShaders()[i].Objects[j].ObjHasMoreThanOneMaterial)? "+":""));
                                    } else {
                                        GUI.Label(new Rect(178, drawingPos*30 + 16, 85, 25),
                                                  ((ObjSorter.GetOptShaders()[i].Objects[j].ObjHasMoreThanOneMaterial)? "Aprox":"null"));
                                        GUI.Label(new Rect(170,drawingPos*30 + 28, 85, 25),
                                                  "(" + ObjSorter.GetOptShaders()[i].Objects[j].TextureSize.x +
                                                  "x" +
                                                  ObjSorter.GetOptShaders()[i].Objects[j].TextureSize.y + ")" +
                                                  ((ObjSorter.GetOptShaders()[i].Objects[j].ObjHasMoreThanOneMaterial)? "+":""));
                                        GUI.Label(new Rect(257,drawingPos*30 + 17, 125, 20), "No texture found;\ncreating a texture\nwith the color", warningStyle);
                                    }
                                    if(ObjSorter.GetOptShaders()[i].Objects[j].ObjHasMoreThanOneMaterial) {
                                        GUI.Label(new Rect(330, drawingPos*30 + 17, 59, 30), " Multiple\nMaterials");
                                    }
                                } else {//obj not correctly assembled, display log
                                    GUI.Label(new Rect(170, drawingPos*30 + 18, 125, 14), ObjSorter.GetOptShaders()[i].Objects[j].IntegrityLog[0], errorStyle);
                                    GUI.Label(new Rect(170, drawingPos*30 + 28, 125, 20), ObjSorter.GetOptShaders()[i].Objects[j].IntegrityLog[1], errorStyle);
                                }
                            } else {
                                ObjSorter.RemoveAtPosition(i, j);
                            }
                        }
                        if(GUI.Button(new Rect(150, drawingPos*30+20, 18,22), "-")) {
                            if(ObjSorter.GetTotalSortedObjects() > 1) {
                                ObjSorter.GetOptShaders()[i].RemoveObjectAt(j);
                            } else {
                                ObjSorter.GetOptShaders()[0].SetObjectAtIndex(0, null);
                            }
                        }
                        drawingPos++;
                    }
                }
            }
            GUI.EndScrollView();
        }
    }
}