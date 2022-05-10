#pragma warning disable 649
using System.Collections;
using System.IO;
using System.Collections.Generic;
using TriLibCore.General;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TriLibCore.Samples
{
    public class LoadFbx : MonoBehaviour
    {
        private GameObject root;
        [SerializeField] private List<string> ModelPaths;
        [SerializeField] private List<string> ModelNames;

        public string Ordnername;
        public string Dateiformat;
        public GameObject loadRoot;
        public TMP_Dropdown Drop;
        public TMP_Text progressText;
        public Image progressBar;
        public float jumpWidht = 0.2f;

        private bool set = false;
        private bool val = false;

        public void Start()
        {
            progressBar.fillAmount = 0;
            List<string> AllPaths = new List<string>(Directory.GetFileSystemEntries(Application.streamingAssetsPath + "/" + Ordnername));
            ModelPaths = new List<string>();
            ModelNames = new List<string>();

            for (int i = 0; i < AllPaths.Count; i++)
            {
                if (AllPaths[i].EndsWith(Dateiformat))
                {
                    ModelPaths.Add(AllPaths[i]);
                }
            }

            if (ModelPaths.Count > 0)
            {
                set = true;

                foreach (string path in ModelPaths)
                {
                    string[] names = path.Split('/', '\\');
                    string name = names[names.Length - 1];
                    ModelNames.Add(name);
                }
                Drop.AddOptions(ModelNames);
            }
            else
            {
                progressText.text = "No Quick choose";
                progressBar.fillAmount = 1;
                progressBar.color = Color.yellow;
            }
        }

        public void LoadModel()
        {
            if (set)
            {
                var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();

                if (Drop.value == 0)
                {
                    var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
                    assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a Model file", OnLoad, OnMaterialsLoad, OnProgress, OnBeginLoad, OnError, null, assetLoaderOptions);
                }
                else
                {
                    AssetLoader.LoadModelFromFile(ModelPaths[Drop.value - 1], OnLoad, OnMaterialsLoad, OnProgress, OnError, null, assetLoaderOptions);
                }
            }
        }

        public void DeleteLastModel()
        {
            Destroy(root);
        }

        public void DeleteAllModels()
        {
            foreach (Transform model in loadRoot.transform)
            {
                Destroy(model.gameObject);
            }
        }

        public void MoveModel(string direction)
        {
            if (root != null)
            {
                if (direction == "x")
                    root.transform.position += jumpWidht * Vector3.right;
                else if (direction == "-x")
                    root.transform.position -= jumpWidht * Vector3.right;
                else if (direction == "y")
                    root.transform.position += jumpWidht * Vector3.up;
                else if (direction == "-y")
                    root.transform.position -= jumpWidht * Vector3.up;
                else if (direction == "z")
                    root.transform.position += jumpWidht * Vector3.forward;
                else if (direction == "-z")
                    root.transform.position -= jumpWidht * Vector3.forward;
            }
        }

        public void SetJumpWidth(float width)
        {
            jumpWidht = width;
        }

        public void RotateModel(float angle)
        {
            if (root != null)
            {
                root.transform.Rotate(Vector3.up, jumpWidht * angle);
            }
        }

        public void ScaleModel(float scale)
        {
            if (root != null)
            {
                root.transform.localScale = (1f + (jumpWidht * scale)) * root.transform.localScale;
            }
        }

        private void OnBeginLoad(bool filesSelected)
        {
            //_progressText.enabled = filesSelected;
        }

        // Called when any error occurs.
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
            progressBar.color = Color.red;
            progressBar.fillAmount = 1;
            progressText.text = "Error";
        }

        // Called when the Model loading progress changes.     
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            progressBar.color = Color.yellow;
            progressText.text = $"Progress: {progress:P}";
            progressBar.fillAmount = progress;

            if (progress >= 0.95)
            {
                InvokeRepeating("Validate", 2f, 0.3f);
                progressText.text = "Waiting on Validation";
            }
        }

        // Called when the Model (including Textures and Materials) has been fully loaded.       
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Materials loaded. Model fully loaded.");
            root.transform.SetParent(loadRoot.transform, false);
            val = true;
        }

        private void Validate()
        {
            if (val)
            {
                val = false;
                progressText.text = "Model fully loaded";
                progressBar.color = Color.green;
                CancelInvoke("Validate");
            }
        }

        // Called when the Model Meshes and hierarchy are loaded.       
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            Debug.Log("Model loaded. Loading materials.");
            root = assetLoaderContext.RootGameObject;
        }
    }
}
