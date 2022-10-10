using UnityEngine;
using System.IO;
using System.Reflection;

namespace BodhiDonselaar
{
	[RequireComponent(typeof(Camera))]
	public class EquiCam : MonoBehaviour
	{
		private static Material equi;
		public Size RenderResolution = Size.Default;
		private RenderTexture cubemap;
		private Camera cam;
		private GameObject child;
		public enum Size
		{
			High = 2048,
			Default = 1024,
			Low = 512,
			Minimum = 256
		}
		void OnEnable()
		{
			if (equi == null)
			{
				var str = Assembly.GetExecutingAssembly().GetManifestResourceStream("Lualts-Camera-Mod.Resources.lcm-equi-shader");
				if (str == null)
				    return;

				var bundle = AssetBundle.LoadFromStream(str);
				if (bundle == null)
				    return;

				equi = new Material(bundle.LoadAsset<Shader>("EquiCam"));
				bundle.Unload(false);
			}

			child = new GameObject();
			child.hideFlags = HideFlags.HideInHierarchy;
			child.transform.SetParent(transform);
			child.transform.localPosition = Vector3.zero;
			child.transform.localEulerAngles = Vector3.zero;
			cam = child.AddComponent<Camera>();
			cam.CopyFrom(GetComponent<Camera>());
			child.SetActive(false);
			New();
		}
		void OnDisable()
		{
			ab.Unload(true);
			if (child != null) DestroyImmediate(child);
			if (cubemap != null)
			{
				cubemap.Release();
				DestroyImmediate(cubemap);
			}
		}
		void OnRenderImage(RenderTexture src, RenderTexture des)
		{
			if (equi != null && cubemap != null && cam != null)
			{
				if (cubemap.width != (int)RenderResolution) New();
				cam.RenderToCubemap(cubemap);
				Shader.SetGlobalFloat("FORWARD", cam.transform.eulerAngles.y * 0.01745f);
				Graphics.Blit(cubemap, des, equi);
			}
		}
		private void New()
		{
			cam.targetTexture = null;
			if (cubemap != null)
			{
				cubemap.Release();
				DestroyImmediate(cubemap);
			}
			cubemap = new RenderTexture((int)RenderResolution, (int)RenderResolution, 0, RenderTextureFormat.ARGB32);
			cubemap.antiAliasing = 1;
			cubemap.filterMode = FilterMode.Bilinear;
			cubemap.anisoLevel = 0;
			cubemap.dimension = UnityEngine.Rendering.TextureDimension.Cube;
			cubemap.autoGenerateMips = false;
			cubemap.useMipMap = false;
			cam.targetTexture = cubemap;
		}
	}
}
