using System;
using UnityEngine;
using System.Collections;

public class DownloadAssetBundle : MonoBehaviour {
	public string BundleURL;
	public string AssetName;
	public int version;
	
	void Start() {
		StartCoroutine (DownloadAndCache());
	}
	
	public IEnumerator  DownloadAndCache (){
		GameObject test;
		// Wait for the Caching system to be ready
		while (!Caching.ready)
			yield return null;
		
		// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
		using(WWW www = WWW.LoadFromCacheOrDownload (BundleURL, version)){
			yield return www;
			if (www.error != null)
				throw new Exception("WWW download had an error:" + www.error);
			AssetBundle bundle = www.assetBundle;
			if (AssetName == "")
			{
				test = (GameObject)Instantiate(bundle.mainAsset);
			}
			else
			{
			 test = (GameObject)Instantiate(bundle.LoadAsset(AssetName));
			}
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload(false);
				
		} // memory is freed from the web stream (www.Dispose() gets called implicitly)
	yield	return test;
	}
}
