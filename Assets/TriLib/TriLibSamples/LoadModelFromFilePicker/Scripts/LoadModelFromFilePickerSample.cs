#pragma warning disable 649
using TriLibCore.General;
using UnityEngine;
using TriLibCore.Extensions;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


namespace TriLibCore.Samples
{
    /// <summary>
    /// Represents a sample that loads a Model from a file-picker.
    /// </summary>
    public class LoadModelFromFilePickerSample : MonoBehaviour
    {
        /// <summary>
        /// The last loaded GameObject.
        /// </summary>
      //  private GameObject _loadedGameObject;
        public GameObject _loadedGameObject;
        public GameObject setPositionSphere;
        public Camera Cam;

        public Slider mainSlider;
        public Slider Y_mainSlider;
        public Slider Scale_mainSlider;

        public float x;
        public float y;
        public float z;

        private bool setPositionSign;

        private float xAngle;
        private float yAngle;
        private float zAngle;

        private float Y_Slider;
        private float scale;

        public Transform parent;


        /// <summary>
        /// The load Model Button.
        /// </summary>
        [SerializeField]
        private Button _loadModelButton;
        //private Vector3 scale;
        private Vector3 scaleChange;

        /// <summary>
        /// The progress indicator Text;
        /// </summary>
        [SerializeField]
        private Text _progressText;
        void Start()
        {
            //_loadedGameObject.AddComponent<Rigidbody>();
            //_loadedGameObject.AddComponent<XRGrabInteractable>();
            //Rigidbody rigidbody = _loadedGameObject.AddComponent<Rigidbody>() as Rigidbody;
            //vector3 += newVector3(0.00053, 0.00053, 0.00053);

            mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            mainSlider.minValue = 0;
            mainSlider.maxValue = 360;

            Y_mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            Y_mainSlider.minValue = -2;
            Y_mainSlider.maxValue = 2;

            Scale_mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            Scale_mainSlider.minValue = 0;
            Scale_mainSlider.maxValue = 2;

        }
        void Awake()
        {
            //scale = new Vector3(0.00053f, 0.00053f, 0.00053f);

        }
        void Update()
        {
            if (setPositionSign == true)
            {
                float smooth = 5.0f;
                yAngle = mainSlider.value;
                //_loadedGameObject.transform.Rotate(33, yAngle, 0, Space.Self);
                //_loadedGameObject.transform.rotation +=new Vector3(0, yAngle, 0);

                // Smoothly tilts a transform towards a target rotation.

                //float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
                //float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

                // Rotate the cube by converting the angles into a quaternion.
                Quaternion target = Quaternion.Euler(0, yAngle, 0);

                // Dampen towards the target rotation
                //_loadedGameObject.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

            }
            
        }
        public void ValueChangeCheck()
        {
            //float smooth = 5.0f;

            /*Debug.Log(mainSlider.value);
            Quaternion target = Quaternion.Euler(0, yAngle, 0);
            _loadedGameObject.transform.rotation = _loadedGameObject.transform.rotation + Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
            */
            yAngle = mainSlider.value;
            //_loadedGameObject.transform.Rotate(0, yAngle, 0, Space.Self);
            _loadedGameObject.transform.localRotation = Quaternion.Euler(0, yAngle, 0);

            Y_Slider = Y_mainSlider.value;
            //_loadedGameObject.transform.Translate(0, Y_Slider, 0, Space.Self);
            //_loadedGameObject.transform.Rotate(0, Y_Slider, 0, Space.Self);

            //_loadedGameObject.transform.position = setPositionSphere.transform.position + new Vector3(x, y, z);
            _loadedGameObject.transform.localPosition = new Vector3(0, Y_Slider, 0);

            scale= Scale_mainSlider.value;
            scaleChange = new Vector3(scale, scale, scale);
            _loadedGameObject.transform.localScale = scaleChange;
        }

        public void SetPositionSign()
        {
            setPositionSign = true;
        }

        /// <summary>
        /// Creates the AssetLoaderOptions instance and displays the Model file-picker.
        /// </summary>
        /// <remarks>
        /// You can create the AssetLoaderOptions by right clicking on the Assets Explorer and selecting "TriLib->Create->AssetLoaderOptions->Pre-Built AssetLoaderOptions".
        /// </remarks>
        public void LoadModel()
        {
            var assetLoaderOptions = AssetLoader.CreateDefaultLoaderOptions();
            var assetLoaderFilePicker = AssetLoaderFilePicker.Create();
            assetLoaderFilePicker.LoadModelFromFilePickerAsync("Select a Model file", OnLoad, OnMaterialsLoad, OnProgress, OnBeginLoad, OnError, null, assetLoaderOptions);

            //scaleChange = new Vector3(-0.01f, -0.01f, -0.01f);
            //_loadedGameObject.transform.localScale = scale;

            //_loadedGameObject.AddComponent<Rigidbody>();
            //var type = Type.GetType("XRGrabInteractable");
            //gameObject.AddComponent(type);
            //_loadedGameObject.AddComponent<XRGrabInteractable>();
            //Rigidbody gameObjectRigidBody = _loadedGameObject.AddComponent<Rigidbody>();
            //Rigidbody gameObjectsRigidBody = _loadedGameObject.AddComponent<Rigidbody>();
            //XRGrabInteractable xrGrabInt = _loadedGameObject.AddComponent(typeof(XRGrabInteractable)) as XRGrabInteractable;
            //SphereCollider sc = _loadedGameObject.AddComponent<SphereCollider>() as SphereCollider;


        }


        /// <summary>
        /// Called when the the Model begins to load.
        /// </summary>
        /// <param name="filesSelected">Indicates if any file has been selected.</param>
        private void OnBeginLoad(bool filesSelected)
        {
            _loadModelButton.interactable = !filesSelected;
            _progressText.enabled = filesSelected;
        }
        public void SetPosition()
        {
            //_loadedGameObject.transform.position = _loadedGameObject.transform.position + new Vector3(x, y, z);
            parent.transform.position = setPositionSphere.transform.position + new Vector3(x, y, z);
            _loadedGameObject.transform.position = setPositionSphere.transform.position + new Vector3(x, y, z);
           
        }

        /// <summary>
        /// Called when any error occurs.
        /// </summary>
        /// <param name="obj">The contextualized error, containing the original exception and the context passed to the method where the error was thrown.</param>
        private void OnError(IContextualizedError obj)
        {
            Debug.LogError($"An error occurred while loading your Model: {obj.GetInnerException()}");
        }

        /// <summary>
        /// Called when the Model loading progress changes.
        /// </summary>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        /// <param name="progress">The loading progress.</param>
        private void OnProgress(AssetLoaderContext assetLoaderContext, float progress)
        {
            _progressText.text = $"Progress: {progress:P}";
        }

        /// <summary>
        /// Called when the Model (including Textures and Materials) has been fully loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnMaterialsLoad(AssetLoaderContext assetLoaderContext)
        {
            if (assetLoaderContext.RootGameObject != null)
            {
                Debug.Log("Model fully loaded.");

                //    _loadedGameObject.transform.localScale = scale;
                 _loadedGameObject.AddComponent<XRGrabInteractable>();
                //Rigidbody gameObjectsRigidBody_loadedGameObject.AddComponent<XRGrabInteract>();
                _loadedGameObject.GetComponent<Rigidbody>().useGravity = false;
                //gameObjectsRigidBody.useGravity = false;

                _loadedGameObject.transform.SetParent(parent, false);


            }
            else
            {
                Debug.Log("Model could not be loaded.");
            }
            _loadModelButton.interactable = true;
            _progressText.enabled = false;
        }

        /// <summary>
        /// Called when the Model Meshes and hierarchy are loaded.
        /// </summary>
        /// <remarks>The loaded GameObject is available on the assetLoaderContext.RootGameObject field.</remarks>
        /// <param name="assetLoaderContext">The context used to load the Model.</param>
        private void OnLoad(AssetLoaderContext assetLoaderContext)
        {
            if (_loadedGameObject != null)
            {
                Destroy(_loadedGameObject);
            }
            _loadedGameObject = assetLoaderContext.RootGameObject;
            if (_loadedGameObject != null)
            {
                Cam.FitToBounds(assetLoaderContext.RootGameObject, 2f);
            }
        }
    }



}
