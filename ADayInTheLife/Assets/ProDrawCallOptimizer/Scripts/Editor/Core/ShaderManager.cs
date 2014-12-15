/*
  singleton class for managing the shader defines logic.

  Created by:
  Juan Sebastian Munoz
  naruse@gmail.com
  All rights reserved

 */

namespace ProDrawCall {
	using System.IO;
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	using System.Collections.Generic;

	public class ShaderManager {

	    private static ShaderManager instance;
	    public static ShaderManager Instance {
	        get {
	            if(instance == null)
	                instance = new ShaderManager();
	            return instance;
	        }
	    }

        // this is basically a list of dictionaries that contain a string(key) for
        // the shader name and a List of string for the shader defines of the string(key) in the dic
        // this is used for caching shader defines so we dont have to (automatically) go trough the shader properties
        // and get the shader defines for the available textures.
        Dictionary<string, List<string>> shaderInfoCache;
	    private ShaderManager() {
            InitializeShaderInfoCache();
        }

        private void InitializeShaderInfoCache() {
            shaderInfoCache = new Dictionary<string, List<string>>();
        }

        private void ClearCache() {
            shaderInfoCache.Clear();
        }

        private void CacheShaderInfo(string shaderName, List<string> shaderDefines) {
            if(!shaderInfoCache.ContainsKey(shaderName)) {//if the shader is not cached
                shaderInfoCache.Add(shaderName, shaderDefines);//cache it.
            }

        }

	    public List<Texture2D> GetTexturesForObject(GameObject g, string shaderName, bool generateTexturesIfNecessary = false) {
	        List<string> defines = GetShaderTexturesDefines(shaderName);
	        List<Texture2D> materialTextures = new List<Texture2D>();
	        if(defines != null) {
	            for(int i = 0; i < defines.Count; i++) {
	                Texture2D tex = g.GetComponent<MeshRenderer>().sharedMaterials[0].GetTexture(defines[i]) as Texture2D;
	                if(tex == null && generateTexturesIfNecessary) {
	                    tex = Utils.GenerateTexture(g.GetComponent<MeshRenderer>().sharedMaterials[0].color);//TODO GET THE PROPER COLOR FOR EACH SHADER.
	                }
	                materialTextures.Add(tex);
	            }
	            return materialTextures;
	        }
	        return null;
	    }

	    public Texture2D GetTextureForObjectSpecificShaderDefine(GameObject g, string shaderDefine, bool generateTexturesIfNecessary = false) {
	        Texture2D result = g.GetComponent<MeshRenderer>().sharedMaterials[0].GetTexture(shaderDefine) as Texture2D;
	        if(result == null && generateTexturesIfNecessary) {
	            if(g.GetComponent<MeshRenderer>().sharedMaterials[0].HasProperty("_Color")) {
	                Color shaderColor = g.GetComponent<MeshRenderer>().sharedMaterials[0].GetColor("_Color");
	                result = Utils.GenerateTexture(shaderColor);
	            } else {
	                Debug.LogWarning("Shader for GameObject " + g.name + " doesnt have a '_Color' property, using white color by default");
	                result = Utils.GenerateTexture(Color.white);
	            }
	        }
	        return result;
	    }

	    public Vector2 GetScaleForObjectSpecificShaderDefine(GameObject g, string shaderDefine) {
	        return g.GetComponent<MeshRenderer>().sharedMaterials[0].GetTextureScale(shaderDefine);
	    }

	    public Vector2 GetOffsetForObjectSpecificShaderDefine(GameObject g, string shaderDefine) {
	        return g.GetComponent<MeshRenderer>().sharedMaterials[0].GetTextureOffset(shaderDefine);
	    }


        // returns the shader defines for a specific shaderName.
        // if the shaderName is not found (which should never happen) then it returns null;
	    public List<string> GetShaderTexturesDefines(string shaderName) {
            Material mat = new Material(Shader.Find(shaderName));
            if(mat == null) {
                Debug.LogError("Unknown Shader: " + shaderName);
                return null;
            }

            if(shaderInfoCache.ContainsKey(shaderName)) {//if shader is not catched, cache it.
                return shaderInfoCache[shaderName];
            } else {//shader not cached, calculate the shader defines and cache the shader properties
                List<string> shaderTextureDefines = new List<string>();
                int count = ShaderUtil.GetPropertyCount(mat.shader);
                for(int i = 0; i < count; i++) {
                    if(ShaderUtil.GetPropertyType(mat.shader, i) == ShaderUtil.ShaderPropertyType.TexEnv) {
                        if(ShaderUtil.GetTexDim(mat.shader, i) == ShaderUtil.ShaderPropertyTexDim.TexDim2D) {
                            shaderTextureDefines.Add(ShaderUtil.GetPropertyName(mat.shader, i));
                        }
                    }
                }
                CacheShaderInfo(shaderName, shaderTextureDefines);
                return shaderTextureDefines;
            }

	    }
	}
}