using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects {
  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  [AddComponentMenu("Image Effects/Edge Detection/Sobel Outline")]

  public class SobelOutline : PostEffectsBase {
    public float thickness = 1.0f;
    public float depthMultiplier = 1.0f;
    public float depthBias = 1.0f;
    public float normalMultiplier = 1.0f;
    public float normalBias = 10.0f;
    public Color color = Color.black;

    public Shader sobelShader;
    private Material sobelMaterial = null;


    void OnEnable() {
      GetComponent<Camera>().depthTextureMode = DepthTextureMode.None;
    }
    public override bool CheckResources() {
      CheckSupport(true);

      sobelMaterial = CheckShaderAndCreateMaterial(sobelShader, sobelMaterial);
      GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;

      if (!isSupported) {
        ReportAutoDisable();
      }

      return isSupported;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
      if (CheckResources() == false) {
        Graphics.Blit(source, destination);
        return;
      }

      sobelMaterial.SetFloat("_OutlineThickness", thickness);
      sobelMaterial.SetFloat("_OutlineDepthMultiplier", depthMultiplier);
      sobelMaterial.SetFloat("_OutlineDepthBias", depthBias);
      sobelMaterial.SetFloat("_OutlineNormalMultiplier", normalMultiplier);
      sobelMaterial.SetFloat("_OutlineNormalBias", normalBias);
      sobelMaterial.SetColor("_OutlineColor", color);
      sobelMaterial.SetTexture("_MainTex", source);

      Graphics.Blit(source, destination, sobelMaterial);
    }
  }
}