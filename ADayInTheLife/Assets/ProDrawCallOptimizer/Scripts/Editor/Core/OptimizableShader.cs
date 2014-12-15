/*
  This class is in charge of containing objects that share the same shader. (even tho the AddObject(OptimizableObject) method doesnt
  care if the shader of the objects[] matches with the shader of the class.

  Later on, ObjSorter sorts the objects inside each OptimizableShader to match the shader.

  Created by:
  Juan Sebastian Munoz Arango
  naruse@gmail.com
  All rights reserved
*/
namespace ProDrawCall {

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class OptimizableShader {
    private string shaderName = "";
    public string ShaderName { get { return shaderName; } }

    private List<OptimizableObject> objects;
    public List<OptimizableObject> Objects { get { return objects; } }

    public OptimizableShader(string name) {
        shaderName = name;
        objects = new List<OptimizableObject>();
    }

    //adds an object to the objects list.
    //WARNING: this doesnt care if the optimizable object obj matches the shader name
    // of this object.
    // Later on the ObjSorter.cs->SortObjects() organizes them to match
    public void AddObject(OptimizableObject obj) {
        /*if(shaderName != "" && obj != null && shaderName != obj.ShaderName) {
            Debug.LogWarning("ERROR: Adding obj to a not matching optimizable shader");
            return;
        }*/
        objects.Add(obj);
    }

    public void RemoveObjectAt(int index) {
        objects.RemoveAt(index);
    }

    public void RemoveObject(OptimizableObject obj) {
        if(!objects.Remove(obj))
            Debug.LogError("Couldnt remove object");
//      else
//          Debug.Log("Added obj");
    }

    public void SetObjectAtIndex(int index, OptimizableObject obj) {
        objects[index] = obj;
    }

    public void OptimizeShader(bool reuseTextures, bool generatePrefabs) {
        if(shaderName == "")//unknown shader doesnt need to be optimed
            return;
        int currentAtlasSize = CalculateAproxAtlasSize(reuseTextures);//(reuseTextures) ? aproxAtlasSizeReuseTextures : aproxAtlasSize;
        if((objects.Count > 1 || //more than 1 obj or 1 obj with multiple mat
            (objects.Count == 1 && objects[0] != null && objects[0].ObjHasMoreThanOneMaterial)) &&
           currentAtlasSize < Constants.MaxAtlasSize) { //check the generated atlas size doesnt exceed max supported texture size

            List<Rect> texturePositions = new List<Rect>();//creo que puede morir porque el atlasser tiene adentro un rect.
            Node resultNode = null;//nodes for the tree for atlasing


            Atlasser generatedAtlas = new Atlasser(currentAtlasSize, currentAtlasSize);
            int resizeTimes = 1;

            TextureReuseManager textureReuseManager = new TextureReuseManager();

            for(int j = objects.Count-1; j >= 0; j--) {//start from the largest to the shortest textures
                if(objects[j].ObjHasMoreThanOneMaterial)//before atlassing multiple materials obj, combine it.
                    objects[j].ProcessAndCombineMaterials();

                Vector2 textureToAtlasSize = objects[j].TextureSize;
                if(reuseTextures) {
                    //if texture is not registered already
                    if(!textureReuseManager.TextureRefExists(objects[j])) {
                        //generate a node
                        resultNode = generatedAtlas.Insert(Mathf.RoundToInt((textureToAtlasSize.x != Constants.NULLV2.x) ? textureToAtlasSize.x : Constants.NullTextureSize),
                                                           Mathf.RoundToInt((textureToAtlasSize.y != Constants.NULLV2.y) ? textureToAtlasSize.y : Constants.NullTextureSize));
                        if(resultNode != null) { //save node if fits in atlas
                            textureReuseManager.AddTextureRef(objects[j], resultNode.NodeRect, j);
                        }
                    }
                } else {
                    resultNode = generatedAtlas.Insert(Mathf.RoundToInt((textureToAtlasSize.x != Constants.NULLV2.x) ? textureToAtlasSize.x : Constants.NullTextureSize),
                                                       Mathf.RoundToInt((textureToAtlasSize.y != Constants.NULLV2.y) ? textureToAtlasSize.y : Constants.NullTextureSize));
                }
                if(resultNode == null) {
                    int resizedAtlasSize = currentAtlasSize + Mathf.RoundToInt((float)currentAtlasSize * Constants.AtlasResizeFactor * resizeTimes);
                    generatedAtlas = new Atlasser(resizedAtlasSize, resizedAtlasSize);
                    j = objects.Count;//Count and not .Count-1 bc at the end of the loop it will be substracted j-- and we want to start from Count-1

                    texturePositions.Clear();
                    textureReuseManager.ClearTextureRefs();
                    resizeTimes++;
                } else {
                    if(reuseTextures) {
                        texturePositions.Add(textureReuseManager.GetTextureRefPosition(objects[j]));
                    } else {
                        texturePositions.Add(resultNode.NodeRect);//save the texture rectangle
                    }
                }
            }
            Material atlasMaterial = CreateAtlasMaterialAndTexture(generatedAtlas, shaderName, textureReuseManager);
            OptimizeDrawCalls(ref atlasMaterial,
                              generatedAtlas.GetAtlasSize().x,
                              generatedAtlas.GetAtlasSize().y,
                              texturePositions,
                              reuseTextures,
                              textureReuseManager,
                              generatePrefabs);
            //after the game object has been organized, remove the combined game objects.
            for(int i = 0; i < objects.Count; i++) {
                if(objects[i].ObjWasCombined)
                    objects[i].ClearCombinedObject();
            }
        }
    }

    private void OptimizeDrawCalls(ref Material atlasMaterial,  float atlasWidth, float atlasHeight, List<Rect> texturePos, bool reuseTextures, TextureReuseManager texReuseMgr, bool generatePrefabsForObjects) {
        GameObject trash = new GameObject("Trash");//stores unnecesary objects that might be cloned and are children of objects

        // // // when generating prefabs // // //
        string folderToSavePrefabs = EditorApplication.currentScene;
        if(generatePrefabsForObjects) {
            if(folderToSavePrefabs == "") { //scene is not saved yet.
                folderToSavePrefabs = Constants.NonSavedSceneFolderName + ".unity";
            }
            folderToSavePrefabs = folderToSavePrefabs.Substring(0, folderToSavePrefabs.Length-6) + "-Atlas";//remove the ".unity"
            folderToSavePrefabs += Path.DirectorySeparatorChar + "Prefabs";
            if(!Directory.Exists(folderToSavePrefabs)) {
                Directory.CreateDirectory(folderToSavePrefabs);
                AssetDatabase.Refresh();
            }
        }
        ///////////////////////////////////////////

        for(int i = 0; i < objects.Count; i++) {
            string optimizedObjID = objects[i].GameObj.name + Constants.OptimizedObjIdentifier;

            objects[i].GameObj.GetComponent<MeshRenderer>().enabled = true;//activate renderers for instantiating
            GameObject instance = GameObject.Instantiate(objects[i].GameObj,
                                                         objects[i].GameObj.transform.position,
                                                         objects[i].GameObj.transform.rotation) as GameObject;
            Undo.RegisterCreatedObjectUndo(instance,"CreateObj" + optimizedObjID);

            //remove children of the created instance.
            Transform[] children = instance.GetComponentsInChildren<Transform>();
            for(int j = 0; j < children.Length; j++)
                children[j].transform.parent = trash.transform;

            instance.transform.parent = objects[i].GameObj.transform.parent;
            instance.transform.localScale = objects[i].GameObj.transform.localScale;
            instance.renderer.sharedMaterial = atlasMaterial;
            instance.name = optimizedObjID;

            instance.GetComponent<MeshFilter>().sharedMesh = Utils.CopyMesh(objects[i].GameObj.GetComponent<MeshFilter>().sharedMesh);

            //Remap uvs
            Mesh remappedMesh = instance.GetComponent<MeshFilter>().sharedMesh;
            Vector2[] remappedUVs = instance.GetComponent<MeshFilter>().sharedMesh.uv;
            for(int j = 0; j < remappedUVs.Length; j++) {
                if(reuseTextures) {
                    remappedUVs[j] = Utils.ReMapUV(remappedUVs[j],
                                                   atlasWidth,
                                                   atlasHeight,
                                                   texReuseMgr.GetTextureRefPosition(objects[i]),
                                                   instance.name);
                } else {
                    remappedUVs[j] = Utils.ReMapUV(remappedUVs[j], atlasWidth, atlasHeight, texturePos[i], instance.name);
                }
            }
            remappedMesh.uv = remappedUVs;

            instance.GetComponent<MeshFilter>().sharedMesh = remappedMesh;

            Undo.RecordObject(objects[i].GameObj.GetComponent<MeshRenderer>(), "Active Obj");

            //if the gameObject has multiple materials, search for the original one (the uncombined) in order to deactivate it
            if(objects[i].ObjWasCombined) {
                objects[i].UncombinedObject.GetComponent<MeshRenderer>().enabled = false;
            } else {
                objects[i].GameObj.GetComponent<MeshRenderer>().enabled = false;
            }

            if(generatePrefabsForObjects) {
                string prefabName = Utils.GetValiName(instance.name) + " " + instance.GetInstanceID();
                string  assetPath = folderToSavePrefabs + Path.DirectorySeparatorChar + prefabName;
                AssetDatabase.CreateAsset(instance.GetComponent<MeshFilter>().sharedMesh, assetPath + ".asset");
                PrefabUtility.CreatePrefab(assetPath + ".prefab", instance, ReplacePrefabOptions.ConnectToPrefab);
            }
        }
        GameObject.DestroyImmediate(trash);
    }

    private Material CreateAtlasMaterialAndTexture(Atlasser generatedAtlas, string shaderToAtlas, TextureReuseManager textureReuseManager) {
        string fileName = ((ObjectsGUI.CustomAtlasName == "") ? "Atlas " : (ObjectsGUI.CustomAtlasName + " ")) + shaderToAtlas.Replace('/','_');
        string folderToSaveAssets = EditorApplication.currentScene;
        if(folderToSaveAssets == "") { //scene is not saved yet.
            folderToSaveAssets = Constants.NonSavedSceneFolderName + ".unity";
            Debug.LogWarning("WARNING: Scene has not been saved, saving baked objects to: " + Constants.NonSavedSceneFolderName + " folder");
        }

        folderToSaveAssets = folderToSaveAssets.Substring(0, folderToSaveAssets.Length-6) + "-Atlas";//remove the ".unity" and add "-Atlas"
        if(!Directory.Exists(folderToSaveAssets)) {
            Directory.CreateDirectory(folderToSaveAssets);
            AssetDatabase.ImportAsset(folderToSaveAssets);
        }

        string atlasTexturePath = folderToSaveAssets + Path.DirectorySeparatorChar + fileName;
        //create the material in the project and set the shader material to shaderToAtlas
        Material atlasMaterial = new Material(Shader.Find(shaderToAtlas));
        //save the material to the project view
        AssetDatabase.CreateAsset(atlasMaterial, atlasTexturePath + "Mat.mat");
        AssetDatabase.ImportAsset(atlasTexturePath + "Mat.mat");
        //load a reference from the project view to the material (this is done to be able to set the texture to the material in the project view)
        atlasMaterial = (Material) AssetDatabase.LoadAssetAtPath(atlasTexturePath + "Mat.mat", typeof(Material));

        List<string> shaderDefines = ShaderManager.Instance.GetShaderTexturesDefines(shaderToAtlas);
        for(int k = 0; k < shaderDefines.Count; k++) {//go trough each property of the shader.
            List<Texture2D> texturesOfShader = GetTexturesToAtlasForShaderDefine(shaderDefines[k]);//Get thtextures for the property shderDefines[k] to atlas them
            List<Vector2> scales = GetScalesToAtlasForShaderDefine(shaderDefines[k]);
            List<Vector2> offsets = GetOffsetsToAtlasForShaderDefine(shaderDefines[k]);
            if(AdvancedMenuGUI.Instance.ReuseTextures) {
                texturesOfShader = Utils.FilterTexsByIndex(texturesOfShader, textureReuseManager.GetTextureIndexes());
                scales = Utils.FilterVec2ByIndex(scales, textureReuseManager.GetTextureIndexes());
                offsets = Utils.FilterVec2ByIndex(offsets, textureReuseManager.GetTextureIndexes());
            }
            generatedAtlas.SaveAtlasToFile(atlasTexturePath + k.ToString() + ".png", texturesOfShader, scales, offsets);//save the atlas with the retrieved textures
            AssetDatabase.ImportAsset(atlasTexturePath + k.ToString() + ".png");
            Texture2D tex = (Texture2D) AssetDatabase.LoadAssetAtPath(atlasTexturePath + k.ToString() + ".png", typeof(Texture2D));

            atlasMaterial.SetTexture(shaderDefines[k], //set property shderDefines[k] for shader shaderToAtlas
                                     tex);
        }
        return atlasMaterial;
    }

    //this method returns a list of texture2D by the textures defines of the shader of each object.
    private List<Texture2D> GetTexturesToAtlasForShaderDefine(string shaderDefine) {
        List<Texture2D> textures = new List<Texture2D>();
        for(int i = 0; i < objects.Count; i++) {//for each object lets get the shaderDefine texture.
            Texture2D texToAdd = ShaderManager.Instance.GetTextureForObjectSpecificShaderDefine(objects[i].GameObj, shaderDefine, true/*if null generate texture*/);
            textures.Add(texToAdd);
        }
        return textures;
    }

    private List<Vector2> GetScalesToAtlasForShaderDefine(string shaderDefine) {
        List<Vector2> scales = new List<Vector2>();
        for(int i = 0; i < objects.Count; i++) {//for each object lets get the shaderDefine texture.
            Vector2 scale = ShaderManager.Instance.GetScaleForObjectSpecificShaderDefine(objects[i].GameObj, shaderDefine);
            scales.Add(scale);
        }
        return scales;
    }
    private List<Vector2> GetOffsetsToAtlasForShaderDefine(string shaderDefine) {
        List<Vector2> offsets = new List<Vector2>();
        for(int i = 0; i < objects.Count; i++) {//for each object lets get the shaderDefine texture.
            Vector2 offset = ShaderManager.Instance.GetOffsetForObjectSpecificShaderDefine(objects[i].GameObj, shaderDefine);
            offsets.Add(offset);
        }
        return offsets;
    }
    //calculates aprox atlas sizes with and without reusing textures
    public int CalculateAproxAtlasSize(bool reuseTextures) {
        int aproxAtlasSize = 0;
        if(shaderName == "")//we dont need to calculate atlas size on non-optimizable objects
            return aproxAtlasSize;

        if(reuseTextures) {
            //atlas size reuse textures
            TextureReuseManager textureReuseManager = new TextureReuseManager();
            for(int i = 0; i < objects.Count; i++) {
                if(objects[i] != null) {
                    if(!textureReuseManager.TextureRefExists(objects[i])) {
                        textureReuseManager.AddTextureRef(objects[i]);
                        aproxAtlasSize += objects[i].TextureArea;
                    }
                }
            }
        } else {
            //atlas size without reusing textures
            for(int i = 0; i < objects.Count; i++) {
                if(objects[i] != null)
                    aproxAtlasSize += objects[i].TextureArea;
            }
        }
        aproxAtlasSize = Mathf.RoundToInt(Mathf.Sqrt(aproxAtlasSize));
        return aproxAtlasSize;
    }
}
}