using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = RainerLib.Tree;
using RainerLib;

public class Seed : MonoBehaviour {

    public Tree Tree { get; private set; }

	// Use this for initialization
	void Awake () {
        Tree = GetComponentInChildren<Tree>();
    }

    private void OnDisable()
    {
        transform.Find("Model").gameObject.SetActive(false);
        //transform.Find("Icon").gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "RainArea")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().enabled = false;
            Tree.GetComponent<Collider>().enabled = true;
        }
    }

    public void StartFadeOut()
    {
        Tree.enabled = false;
        Tree.GetComponent<CapsuleCollider>().center = Vector3.down * 1000.0f;
        transform.Find("Icon").gameObject.SetActive(false);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Timer fadeTimer = new Timer(0.3f);
        var renderers = GetComponentsInChildren<Renderer>();

        foreach(var renderer in renderers)
        {
            var material = renderer.material;
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }

        while (!fadeTimer.TimesUp())
        {
            fadeTimer++;
            foreach(var renderer in renderers)
            {
                renderer.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f - fadeTimer.Progress);
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}
