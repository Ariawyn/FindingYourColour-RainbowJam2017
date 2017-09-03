using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayscalePostEffect : MonoBehaviour 
{

	public Material grayscalePostEffectMaterial;

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{

		Graphics.Blit(src, dest, this.grayscalePostEffectMaterial);

	}
}
