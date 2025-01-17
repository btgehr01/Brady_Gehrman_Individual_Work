using System.Collections.Generic;
using NWH.NPhysics;
using NWH.VehiclePhysics2.Effects;
using NWH.VehiclePhysics2.Input;
using NWH.VehiclePhysics2.Modules;
using NWH.VehiclePhysics2.Powertrain;
using NWH.VehiclePhysics2.Powertrain.Wheel;
using NWH.VehiclePhysics2.Sound;
using NWH.WheelController3D;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using NWH.VehiclePhysics2.Sound.SoundComponents;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace NWH.VehiclePhysics2
{
    /// <summary>
    ///     Main class controlling all the other parts of the vehicle.
    /// </summary>
    [RequireComponent(typeof(NRigidbody))]
    public partial class VehicleController : Vehicle
    {
        public const string defaultResourcesPath = "NWH Vehicle Physics/Defaults/";

        public Brakes                          brakes          = new Brakes();
        public DamageHandler                   damageHandler   = new DamageHandler();
        public EffectManager                   effectsManager  = new EffectManager();
        public GroundDetection.GroundDetection groundDetection = new GroundDetection.GroundDetection();
        public VehicleInputHandler             input           = new VehicleInputHandler();
        public ModuleManager                   moduleManager   = new ModuleManager();
        public Powertrain.Powertrain           powertrain      = new Powertrain.Powertrain();
        public SoundManager                    soundManager    = new SoundManager();
        public Steering                        steering        = new Steering();

        /// <summary>
        ///     Position of the engine relative to the vehicle. Turn on gizmos to see the marker.
        /// </summary>
        [Tooltip("    Position of the engine relative to the vehicle. Turn on gizmos to see the marker.")]
        public Vector3 enginePosition = new Vector3(0f, 0.4f, 1.5f);

        /// <summary>
        ///     Position of the exhaust relative to the vehicle. Turn on gizmos to see the marker.
        /// </summary>
        [Tooltip("    Position of the exhaust relative to the vehicle. Turn on gizmos to see the marker.")]
        public Vector3 exhaustPosition = new Vector3(0f, 0.1f, -2f);

        /// <summary>
        ///     Used as a threshold value for lateral slip. When absolute lateral slip of a wheel is
        ///     lower than this value wheel is considered to have no lateral slip (wheel skid). Used mostly for effects and sound.
        /// </summary>
        [Tooltip(
            "Used as a threshold value for lateral slip. When absolute lateral slip of a wheel is\r\nlower than this value wheel is considered to have no lateral slip (wheel skid). Used mostly for effects and sound.")]
        public float lateralSlipThreshold = 0.2f;

        /// <summary>
        ///     Used as a threshold value for longitudinal slip. When absolute longitudinal slip of a wheel is
        ///     lower than this value wheel is considered to have no longitudinal slip (wheel spin). Used mostly for effects and
        ///     sound.
        /// </summary>
        [Tooltip(
            "Used as a threshold value for longitudinal slip. When absolute longitudinal slip of a wheel is\r\nlower than this value wheel is considered to have no longitudinal slip (wheel spin). Used mostly for effects and sound.")]
        public float longitudinalSlipThreshold = 0.7f;

        /// <summary>
        ///     State settings for the current vehicle.
        ///     State settings determine which components are enabled or disabled, as well as which LOD they belong to.
        /// </summary>
        [Tooltip(
            "State settings for the current vehicle.\r\nState settings determine which components are enabled or disabled, as well as which LOD they belong to.")]
        public StateSettings stateSettings;

        /// <summary>
        ///     Position of the transmission relative to the vehicle. Turn on gizmos to see the marker.
        /// </summary>
        [Tooltip("    Position of the transmission relative to the vehicle. Turn on gizmos to see the marker.")]
        public Vector3 transmissionPosition = new Vector3(0f, 0.2f, 0.2f);

        /// <summary>
        /// Makes vehicle kinematic so that the vehicle is fully immobile when
        /// velocity is near 0 and there is no input. Improves performance since
        /// almost no physics code is run when this option is enabled.
        /// </summary>
        [Tooltip("    Constrains vehicle rigidbody position and rotation so that the vehicle is fully immobile when\r\nvelocity is near 0 and there is no input.")]
        public bool freezeWhileIdle = true;

        /// <summary>
        /// Can be enabled to only freeze the vehicle when idle and on slope, to prevent any creep from slip-based approach to friction.
        /// </summary>
        [Tooltip("Can be enabled to only freeze the vehicle when idle and on slope, to prevent any creep from slip-based approach to friction.")]
        public bool freezeOnSlopeOnly = true;

        // ************************
        // ** Sub-stepping
        // ************************

        /// <summary>
        ///     Vehicle NRigidbody (substepped version of Rigidbody).
        /// </summary>
        [Tooltip("    Vehicle NRigidbody (substepped version of Rigidbody).")]
        public NRigidbody vehicleNRigidbody;

        public enum PhysicsUpdateRate { 
            F50Hz = 50,
            F60Hz = 60,
            F100Hz = 100,
            F200Hz = 200,
            F400Hz = 400,
            F600Hz = 600,
            F800Hz = 800,
            F1000Hz = 1000,
            F1200Hz = 1200,
            F1400Hz = 1400,
            F1600Hz = 1600,
        }

        public PhysicsUpdateRate awakePhysicsUpdateRate = PhysicsUpdateRate.F400Hz;
        public PhysicsUpdateRate asleepPhysicsUpdateRate = PhysicsUpdateRate.F50Hz;

        // ************************
        // ** Physical properties
        // ************************

        /// <summary>
        ///     Mass of the vehicle in [kg].
        /// </summary>
        [Tooltip("    Mass of the vehicle in [kg].")]
        public float mass = 1400f;

        /// <summary>
        ///     Maximum angular velocity of the rigidbody. Use to prevent vehicle spinning unrealistically fast on collisions.
        ///     Can also be used to artificially limit tank's rotation speed.
        /// </summary>
        [Tooltip(
            "Maximum angular velocity of the rigidbody. Use to prevent vehicle spinning unrealistically fast on collisions.\r\nCan also be used to artificially limit tank's rotation speed.")]
        public float maxAngularVelocity = 8f;

        /// <summary>
        ///     Drag of the vehicle rigidbody.
        /// </summary>
        [Tooltip("    Drag of the vehicle rigidbody.")]
        public float drag;

        /// <summary>
        ///     Angular drag of the vehicle rigidbody.
        /// </summary>
        [Tooltip("    Angular drag of the vehicle rigidbody.")]
        public float angularDrag;

        /// <summary>
        ///     Material that will be used on all vehicle colliders
        /// </summary>
        [Tooltip("    Material that will be used on all vehicle colliders")]
        public PhysicMaterial physicsMaterial;

        /// <summary>
        ///     Center of mass of the rigidbody. Needs to be readjusted when new colliders are added.
        /// </summary>
        [Tooltip(
            "Center of mass of the rigidbody. Needs to be readjusted when new colliders are added.")]
        public Vector3 centerOfMass = Vector3.zero;

        /// <summary>
        ///     Vector by which the inertia tensor of the rigidbody will be scaled on Start().
        ///     Due to the uniform density of the rigidbodies, versus the very non-uniform density of a vehicle, inertia can feel
        ///     off.
        ///     Use this to adjust inertia tensor values.
        /// </summary>
        [Tooltip(
            "    Vector by which the inertia tensor of the rigidbody will be scaled on Start().\r\n    Due to the unform density of the rigidbodies, versus the very non-uniform density of a vehicle, inertia can feel\r\n    off.\r\n    Use this to adjust inertia tensor values.")]
        public Vector3 inertiaTensor = new Vector3(370f, 1640f, 1150f);


        /// <summary>
        ///     Vehicle dimensions in [m]. X - width, Y - height, Z - length.
        /// </summary>
        [Tooltip("    Vehicle dimensions in [m]. X - width, Y - height, Z - length.")]
        public Vector3 vehicleDimensions = new Vector3(1.5f, 1.5f, 4.6f);

        /// <summary>
        ///     Interpolation of the vehicle Rigidbody.
        /// </summary>
        [UnityEngine.Tooltip("    Interpolation of the vehicle Rigidbody.")]
        public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;


        // ************************
        // ** LODs
        // ************************

        /// <summary>
        ///     Distance between camera and vehicle used for determining LOD.
        /// </summary>
        [System.NonSerialized]
        [Tooltip("    Distance between camera and vehicle used for determining LOD.")]
        public float vehicleToCamDistance;

        /// <summary>
        ///     Currently active LOD.
        /// </summary>
        [System.NonSerialized]
        [Tooltip("    Currently active LOD.")]
        public LOD activeLOD;

        /// <summary>
        ///     Currently active LOD index.
        /// </summary>
        [System.NonSerialized]
        [Tooltip("    Currently active LOD index.")]
        public int activeLODIndex;

        /// <summary>
        ///     LODs will only be updated when this value is true.
        ///     Does not affect sleep LOD.
        /// </summary>
        [Tooltip("    LODs will only be updated when this value is true.\r\n    Does not affect sleep LOD.")]
        public bool updateLODs = true;

        /// <summary>
        ///     When enabled Camera.main will be used as lod camera.
        /// </summary>
        [Tooltip("    When enabled Camera.main will be used as lod camera.")]
        public bool useCameraMainForLOD = true;

        /// <summary>
        ///     Camera from which the LOD distance will be measured.
        ///     To use Camera.main instead, set 'useCameraMainForLOD' to true instead.
        /// </summary>
        [Tooltip(
            "Camera from which the LOD distance will be measured.\r\nTo use Camera.main instead, set 'useCameraMainForLOD' to true instead.")]
        public Camera LODCamera;

        /// <summary>
        ///     Valid only for 4-wheeled vehicles with 2 axles (i.e. cars).
        ///     For other vehicles this value will be 0.
        /// </summary>
        [Tooltip(
            "    Valid only for 4-wheeled vehicles with 2 axles (i.e. cars).\r\n    For other vehicles this value will be 0.")]
        public float wheelbase = 4f;

        /// <summary>
        ///     Cached Time.fixedDeltaTime.
        /// </summary>
        [System.NonSerialized]
        [Tooltip("    Cached Time.fixedDeltaTime.")]
        public float fixedDeltaTime = 0.02f;

        /// <summary>
        ///     Cached Time.deltaTime;
        /// </summary>
        [System.NonSerialized]
        [Tooltip("    Cached Time.deltaTime;")]
        public float deltaTime = 0.02f;

        /// <summary>
        /// Amount of time before Freeze While Idle freezes the vehicle.
        /// </summary>
        [UnityEngine.Tooltip("Amount of time before Freeze While Idle freezes the vehicle.")]
        public float inactivityTimeout = 1.3f;

        /// <summary>
        ///     Called after vehicle has finished initializing.
        /// </summary>
        [System.NonSerialized]
        [UnityEngine.Tooltip("    Called after vehicle has finished initializing.")]
        public UnityEvent OnVehicleInitialized = new UnityEvent();

        /// <summary>
        /// Struct that holds multiplayer state to be synced over the network.
        /// </summary>
        [System.NonSerialized]
        protected MultiplayerState _multiplayerState;


        private Transform            _cameraTransform;
        private RigidbodyConstraints _initialRbConstraints = RigidbodyConstraints.None;
        private bool                 _constraintsApplied;
        private int                  _lodCount;
        private float                _prevAngularDrag;
        private Vector3              _prevCenterOfMass;
        private float                _prevDrag;
        private Vector3              _prevInertiaTensor;
        private float                _prevMass;
        private float                _prevMaxAngularVelocity;
        private float _inactivityTimer;


        /// <summary>
        ///     Wheel groups (i.e. axles) on this vehicle.
        /// </summary>
        public List<WheelGroup> WheelGroups
        {
            get { return powertrain.wheelGroups; }
            private set { powertrain.wheelGroups = value; }
        }

        /// <summary>
        ///     List of all wheels attached to this vehicle.
        /// </summary>
        public List<WheelComponent> Wheels
        {
            get { return powertrain.wheels; }
            private set { powertrain.wheels = value; }
        }

        /// <summary>
        ///     Position of the engine in world coordinates. Used for effects and sound.
        /// </summary>
        public Vector3 WorldEnginePosition
        {
            get { return transform.TransformPoint(enginePosition); }
        }

        /// <summary>
        ///     Position of the exhaust in world coordinates. Used for effects and sound.
        /// </summary>
        public Vector3 WorldExhaustPosition
        {
            get { return transform.TransformPoint(exhaustPosition); }
        }

        /// <summary>
        ///     Position of the transmission in world coordinates. Used for effects and sound.
        /// </summary>
        public Vector3 WorldTransmissionPosition
        {
            get { return transform.TransformPoint(transmissionPosition); }
        }


        public override void Awake()
        {
            #if NVP2_DEBUG
            Debug.Log($"Awake() [{name}]");
            #endif
            
            base.Awake();

            vehicleTransform = transform;

            vehicleNRigidbody = GetComponent<NRigidbody>();
            if (vehicleNRigidbody == null)
            {
                vehicleNRigidbody = gameObject.AddComponent<NRigidbody>();
                if (vehicleNRigidbody == null)
                {
                    Debug.LogError($"NRigidbody could not be added to object {name}.");
                }
            }

            if (OnVehicleInitialized == null)
            {
                OnVehicleInitialized = new UnityEvent();
            }
        }


        private void OnDestroy()
        {
            StopAllCoroutines();
        }


        public virtual void Start()
        {
            #if NVP2_DEBUG
            Debug.Log($"Start() [{name}]");
            #endif
            
            Debug.Assert(vehicleTransform != null,  "Vehicle Transform is null.");
            Debug.Assert(vehicleRigidbody != null,  "Vehicle Rigidbody is null.");
            Debug.Assert(vehicleNRigidbody != null, "Vehicle NRigidbody is null.");

            input.Awake(this);
            steering.Awake(this);
            powertrain.Awake(this);
            soundManager.Awake(this);
            effectsManager.Awake(this);
            damageHandler.Awake(this);
            brakes.Awake(this);
            groundDetection.Awake(this);
            moduleManager.Awake(this);

            ApplyInitialRigidbodyValues();
            SetMultiplayerInstanceType(_multiplayerInstanceType);
            InvokeRepeating("LODCheck", Random.Range(0.2f, 0.4f), Random.Range(0.3f, 0.5f));

            vehicleNRigidbody.OnPrePhysicsSubstep  += OnPrePhysicsSubstep;
            vehicleNRigidbody.OnPhysicsSubstep     += OnPhysicsSubstep;
            vehicleNRigidbody.OnPostPhysicsSubstep += OnPostPhysicsSubstep;

            CheckComponentStates();

            // Calculate wheelbase if there are 2x2 wheels
            if (powertrain.wheelGroups.Count == 2
                && powertrain.wheelGroups[0].Wheels.Count == 2
                && powertrain.wheelGroups[1].Wheels.Count == 2)
            {
                wheelbase = Vector3.Distance(
                    powertrain.wheelGroups[0].LeftWheel.wheelController.transform.position,
                    powertrain.wheelGroups[1].LeftWheel.wheelController.transform.position);
            }

            SetColliderMaterial();
            
            #if NVP2_DEBUG
            Debug.Log("+++ Invoke OnVehicleInitialized +++");
            #endif
            OnVehicleInitialized.Invoke();

            // Put to sleep immediately after initializing 
            if (!awakeOnStart)
            {
                Sleep();
            }

            // Only apply low speed fix to local vehicles!
            if (freezeWhileIdle && _multiplayerInstanceType == MultiplayerInstanceType.Local)
            {
                StartCoroutine(FreezeWhileIdleCoroutine());
            }
        }


        public virtual void Update()
        {
            deltaTime = Time.deltaTime;

            CheckComponentStates();

            if (_multiplayerInstanceType == MultiplayerInstanceType.Local)
            {
                input.Update();
                effectsManager.Update();
                soundManager.Update();
                damageHandler.Update();
                moduleManager.Update();
            }
            else
            {
                effectsManager.Update();
                soundManager.Update();
            }
        }

        private int GetSubstepCount()
        {
            if (_multiplayerInstanceType == MultiplayerInstanceType.Local)
            {
                int targetUpdateFrequency = isAwake ? (int)awakePhysicsUpdateRate : (int)asleepPhysicsUpdateRate;
                int fixedUpdateFrequency = Mathf.RoundToInt(1f / Time.fixedDeltaTime);
                if (targetUpdateFrequency < fixedUpdateFrequency)
                {
                    return 1;
                }
                else
                {
                    int substeps = Mathf.RoundToInt((float)targetUpdateFrequency / (float)fixedUpdateFrequency);
                    if (substeps < 1) substeps = 1;
                    return substeps;
                }
            }
            else
            {
                return 1;
            }
        }


        public virtual void OnPrePhysicsSubstep(float t, float dt)
        {
            fixedDeltaTime = dt;

            vehicleNRigidbody.Substeps = GetSubstepCount();

            // Adjust inertia tensor for increased stability around 0
            float inertiaScale = Mathf.Lerp(4f, 1f, VelocityMagnitude * 0.25f + vehicleRigidbody.angularVelocity.sqrMagnitude * 1.1f);
            vehicleRigidbody.inertiaTensor = inertiaTensor * inertiaScale;

            // Run FixedUpdate on components
            if (_multiplayerInstanceType == MultiplayerInstanceType.Local)
            {
                input.FixedUpdate();
                brakes.FixedUpdate();
                steering.FixedUpdate();
                powertrain.OnPrePhysicsSubstep(t, dt);
                moduleManager.FixedUpdate();
            }
            else
            {
                steering.FixedUpdate();
            }
        }


        public virtual void OnPhysicsSubstep(float t, float dt, int i)
        {
            if (_multiplayerInstanceType == MultiplayerInstanceType.Local)
            {
                powertrain.OnPhysicsSubstep(t, dt, i);
            }
        }


        public virtual void OnPostPhysicsSubstep(float t, float dt)
        {
            powertrain.OnPostPhysicsSubstep(t, dt);
        }


        public override void Sleep()
        {

            if (!isAwake) return;

#if NVP2_DEBUG
            Debug.Log($"Sleep() [{name}]");
#endif

            base.Sleep();

            int lodCount = stateSettings.LODs.Count;
            if (stateSettings != null && lodCount > 0)
            {
                activeLODIndex = lodCount - 1;
                activeLOD      = stateSettings.LODs[activeLODIndex];
            }

            if (vehicleNRigidbody != null)
            {
                vehicleNRigidbody.Substeps = GetSubstepCount();
            }
        }


        public override void Wake()
        {
            if (isAwake) return;

#if NVP2_DEBUG
            Debug.Log($"Wake() [{name}]");
#endif
            base.Wake();

            LODCheck();

            if (vehicleNRigidbody != null)
            {
                vehicleNRigidbody.Substeps = GetSubstepCount();
            }
        }


        private void OnDrawGizmosSelected()
        {
            #if UNITY_EDITOR
            // Draw COM
            if (vehicleRigidbody == null)
            {
                vehicleRigidbody = GetComponent<Rigidbody>();
            }

            Gizmos.color = Color.green;

            Gizmos.color = Color.white;
            Vector3 worldComPosition = transform.TransformPoint(centerOfMass);
            Gizmos.DrawWireSphere(worldComPosition, 0.07f);
            Handles.Label(worldComPosition, new GUIContent("  CoM"));

            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(WorldEnginePosition, 0.04f);
            Handles.Label(WorldEnginePosition, new GUIContent("  Engine"));

            Gizmos.DrawWireSphere(WorldTransmissionPosition, 0.04f);
            Handles.Label(WorldTransmissionPosition, new GUIContent("  Transmission"));

            Gizmos.DrawWireSphere(WorldExhaustPosition, 0.04f);
            Handles.Label(WorldExhaustPosition, new GUIContent("  Exhaust"));

            Gizmos.color = Color.white;

            // Assumes that the model is positioned on top of the XZ plane.
            Matrix4x4 initMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(transform.up * vehicleDimensions.y * 0.5f, vehicleDimensions);
            Gizmos.matrix = initMatrix;

            steering.OnDrawGizmosSelected(this);
            powertrain.OnDrawGizmosSelected(this);
            soundManager.OnDrawGizmosSelected(this);
            effectsManager.OnDrawGizmosSelected(this);
            damageHandler.OnDrawGizmosSelected(this);
            brakes.OnDrawGizmosSelected(this);
            groundDetection.OnDrawGizmosSelected(this);
            moduleManager.OnDrawGizmosSelected(this);
            #endif
        }


        /// <summary>
        ///     Calculates a center of mass of the vehicle based on wheel positions.
        ///     Returned value is good enough for general use but manual setting of COM is always recommended if possible.
        /// </summary>
        /// <returns>Center of mass of the vehicle's Rigidbody</returns>
        public void UpdateCenterOfMass()
        {
            Vector3 centerOfMass = Vector3.zero;
            if (vehicleRigidbody == null)
            {
                vehicleRigidbody = GetComponent<Rigidbody>();
            }

            Vector3 centerPoint = Vector3.zero;
            Vector3 pointSum    = Vector3.zero;
            int     count       = 0;

            foreach (WheelController wheelController in GetComponentsInChildren<WheelController>())
            {
                pointSum += transform.InverseTransformPoint(wheelController.transform.position);
                count++;
            }

            if (count > 0)
            {
                centerOfMass =  pointSum / count;
            }

            centerOfMass -= Wheels[0].wheelController.springLength * 0.15f * transform.up;
            centerOfMass += vehicleDimensions.y * 0.05f * transform.up;
            this.centerOfMass = centerOfMass;
            vehicleRigidbody.centerOfMass = centerOfMass;
        }


        public void UpdateInertiaTensor(float xCoeff = 1f, float yCoeff = 1f, float zCoeff = 1f)
        {
            // Very very approximate as the positions of the individual components are not really known.
            // Still more correct than the Unity calculation which assumes uniform density of all colliders.
            Vector3 bodyInertia = new Vector3(
                (vehicleDimensions.y * vehicleDimensions.y + vehicleDimensions.z * vehicleDimensions.z) * 0.024f * mass * xCoeff,
                (vehicleDimensions.z * vehicleDimensions.z + vehicleDimensions.x * vehicleDimensions.x) * 0.04f * mass * yCoeff,
                (vehicleDimensions.x * vehicleDimensions.x + vehicleDimensions.y * vehicleDimensions.y) * 0.025f * mass * zCoeff
            );
            Vector3 wheelInertia = Vector3.zero;
            foreach (WheelComponent wheelComponent in Wheels)
            {
                Vector3 wheelLocalPos =
                    transform.InverseTransformPoint(wheelComponent.wheelController.Visual.transform.position);
                wheelInertia.x += (wheelLocalPos.y * wheelLocalPos.y + wheelLocalPos.z * wheelLocalPos.z) *
                                  wheelComponent.wheelController.wheel.mass;
                wheelInertia.y += (wheelLocalPos.x * wheelLocalPos.x + wheelLocalPos.z * wheelLocalPos.z) *
                                  wheelComponent.wheelController.wheel.mass;
                wheelInertia.z += (wheelLocalPos.x * wheelLocalPos.x + wheelLocalPos.y * wheelLocalPos.y) *
                                  wheelComponent.wheelController.wheel.mass;
            }

            inertiaTensor = bodyInertia + wheelInertia;
            vehicleRigidbody.inertiaTensor = inertiaTensor;
        }


        /// <summary>
        ///     True if all of the wheels are touching the ground.
        /// </summary>
        public bool IsFullyGrounded()
        {
            int wheelCount = Wheels.Count;
            for (int i = 0; i < wheelCount; i++)
            {
                if (!Wheels[i].IsGrounded)
                {
                    return false;
                }
            }

            return true;
        }


        /// <summary>
        ///     True if any of the wheels are touching ground.
        /// </summary>
        public bool IsGrounded()
        {
            int wheelCount = Wheels.Count;
            for (int i = 0; i < wheelCount; i++)
            {
                if (Wheels[i].IsGrounded)
                {
                    return true;
                }
            }

            return false;
        }


        public void LODCheck()
        {
            #if NVP2_DEBUG
            //Debug.Log($"LODCheck() [{name}]");
            #endif
            
            if (stateSettings == null)
            {
                return;
            }

            _lodCount = stateSettings.LODs.Count;

            if (!isAwake && _lodCount > 0) // Vehicle is sleeping, force the highest lod
            {
                activeLODIndex = _lodCount - 1;
                activeLOD      = stateSettings.LODs[activeLODIndex];
            }
            else if (updateLODs) // Vehicle is awake, determine LOD based on distance
            {
                if (useCameraMainForLOD)
                {
                    LODCamera = Camera.main;
                }
                else
                {
                    if (LODCamera == null)
                    {
                        Debug.LogWarning(
                            "LOD camera is null. Set the LOD camera or enable 'useCameraMainForLOD' instead. Falling back to Camera.main.");
                        LODCamera = Camera.main;
                    }
                }

                if (_lodCount > 0 && LODCamera != null)
                {
                    _cameraTransform = LODCamera.transform;
                    stateSettings.LODs[_lodCount - 2].distance =
                        Mathf.Infinity; // Make sure last non-sleep LOD is always matched

                    vehicleToCamDistance = Vector3.Distance(vehicleTransform.position, _cameraTransform.position);
                    for (int i = 0; i < _lodCount - 1; i++)
                    {
                        if (stateSettings.LODs[i].distance > vehicleToCamDistance)
                        {
                            activeLODIndex = i;
                            activeLOD      = stateSettings.LODs[i];
                            break;
                        }
                    }
                }
                else
                {
                    activeLODIndex = -1;
                    activeLOD      = null;
                }
            }
        }


        public virtual void Reset()
        {
            SetDefaults();
        }


        public virtual void SetColliderMaterial()
        {
            if (physicsMaterial == null)
            {
                return;
            }

            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.material = physicsMaterial;
            }
        }


        /// <summary>
        ///     Resets the vehicle to default state.
        ///     Sets default values for all fields and assign default objects from resources folder.
        /// </summary>
        public virtual void SetDefaults()
        {
            #if NVP2_DEBUG
            Debug.Log($"SetDefaults() [{name}]");
            #endif
            
            ApplyInitialRigidbodyValues();

            steering.SetDefaults(this);
            powertrain.SetDefaults(this);
            soundManager.SetDefaults(this);
            effectsManager.SetDefaults(this);
            damageHandler.SetDefaults(this);
            brakes.SetDefaults(this);
            groundDetection.SetDefaults(this);
            moduleManager.SetDefaults(this);

            if (stateSettings == null)
            {
                stateSettings =
                    Resources.Load(defaultResourcesPath + "DefaultStateSettings") as StateSettings;
            }

            if (physicsMaterial == null)
            {
                physicsMaterial = Resources.Load(defaultResourcesPath + "VehicleMaterial") as PhysicMaterial;
            }

            UpdateCenterOfMass();
            UpdateInertiaTensor();
        }


        public void Validate()
        {
            #if NVP2_DEBUG
            Debug.Log($"Validate() [{name}]");
            #endif
            
            Debug.Log(
                $"{gameObject.name}: Validating VehicleController setup. If no other messages show up after this one, " +
                "the vehicle is good to go.");

            if (transform.localScale != Vector3.one)
            {
                Debug.LogWarning(
                    "VehicleController Transform scale is other than [1,1,1]. It is recommended to avoid " +
                    " scaling the vehicle parent object" +
                    " and use Scale Factor from Unity model import settings instead.");
            }

            steering.Validate(this);
            powertrain.Validate(this);
            soundManager.Validate(this);
            effectsManager.Validate(this);
            damageHandler.Validate(this);
            brakes.Validate(this);
            groundDetection.Validate(this);
            moduleManager.Validate(this);
        }


        public List<VehicleComponent> GetAllComponents()
        {
            List<VehicleComponent> components = new List<VehicleComponent>();

            components.Add(steering);
            components.Add(powertrain);
            components.Add(soundManager);
            components.Add(effectsManager);
            components.Add(damageHandler);
            components.Add(brakes);
            components.Add(groundDetection);
            components.Add(moduleManager);

            return components;
        }


        public virtual void ApplyInitialRigidbodyValues()
        {
            if (vehicleRigidbody == null)
            {
                vehicleRigidbody = GetComponent<Rigidbody>();
                Debug.Assert(vehicleRigidbody != null, "Vehicle does not have a Rigidbody.");
            }

            // Apply initial rigidbody values
            vehicleRigidbody.interpolation          = RigidbodyInterpolation.Interpolate;
            vehicleRigidbody.maxAngularVelocity     = maxAngularVelocity;
            vehicleRigidbody.drag                   = drag;
            vehicleRigidbody.mass                   = mass;
            vehicleRigidbody.angularDrag            = angularDrag;
            vehicleRigidbody.centerOfMass           = centerOfMass;
            vehicleRigidbody.inertiaTensor          = inertiaTensor;
            vehicleRigidbody.sleepThreshold         = 0;
            vehicleRigidbody.interpolation          = interpolation;
            _initialRbConstraints                   = vehicleRigidbody.constraints;
        }


        private IEnumerator FreezeWhileIdleCoroutine()
        {
            _inactivityTimer = inactivityTimeout;

            while(true)
            {
                // Find the slope 
                float slope = Vector3.Angle(transform.up, Vector3.up);

                // Determine if vehicle is inactive
                float d = Mathf.Abs(input.Vertical) * 10f + Mathf.Abs(input.Steering) * 10f + vehicleRigidbody.velocity.magnitude * 4f + vehicleRigidbody.angularVelocity.magnitude * 12f;

                if (d < 0.85f && !(freezeOnSlopeOnly && slope < 5f))
                {
                    _inactivityTimer -= fixedDeltaTime;
                }
                else
                {
                    _inactivityTimer = inactivityTimeout;
                    vehicleRigidbody.isKinematic = false;
                    vehicleRigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    vehicleRigidbody.drag = 0;
                    vehicleRigidbody.angularDrag = 0;
                }

                if (_inactivityTimer < 0f)
                {
                    _inactivityTimer = 0f;
                }

                if (_inactivityTimer <= 0f)
                {
                    vehicleRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    vehicleRigidbody.isKinematic = true;
                }
                else if (_inactivityTimer < 1f)
                {
                    vehicleRigidbody.drag = (1f - _inactivityTimer) * 60f;
                    vehicleRigidbody.angularDrag = (1f - _inactivityTimer) * 20f;
                }

                yield return new WaitForFixedUpdate();
            }
        }


        public virtual void CheckComponentStates()
        {
            input.CheckState(activeLODIndex);
            steering.CheckState(activeLODIndex);
            powertrain.CheckState(activeLODIndex);
            soundManager.CheckState(activeLODIndex);
            effectsManager.CheckState(activeLODIndex);
            damageHandler.CheckState(activeLODIndex);
            brakes.CheckState(activeLODIndex);
            groundDetection.CheckState(activeLODIndex);
            moduleManager.CheckState(activeLODIndex);
        }


        public virtual void OnCollisionEnter(Collision collision)
        {
            damageHandler.HandleCollision(collision);
            vehicleRigidbody.drag        = drag;
            vehicleRigidbody.angularDrag = angularDrag;
        }


        public virtual void OnEnable()
        {
#if NVP2_DEBUG
            Debug.Log($"OnEnable() [{name}]");
#endif
            
            vehicleTransform = transform;
            vehicleRigidbody = GetComponent<Rigidbody>();

            Wake();
        }


        public virtual void OnDisable()
        {
#if NVP2_DEBUG
            Debug.Log($"OnDisable() [{name}]");
#endif
            
            Sleep();
        }


        public override void SetMultiplayerInstanceType(MultiplayerInstanceType instanceType, bool isKinematic = true)
        {
            #if NVP2_DEBUG
            Debug.Log($"SetMultiplayerInstanceType() [{name}]");
            #endif
            
            _multiplayerInstanceType = instanceType;

            if (_multiplayerInstanceType == MultiplayerInstanceType.Remote)
            {
                vehicleRigidbody.isKinematic            = isKinematic;
                if (isKinematic)
                {
                    vehicleRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }

                input.autoSetInput                      = false;

                foreach (WheelComponent wheelComponent in Wheels)
                {
                    wheelComponent.wheelController.visualOnlyUpdate = isKinematic;
                    wheelComponent.wheelController.UseInternalUpdate();
                }
            }
            else
            {
                vehicleRigidbody.isKinematic            = false;

                foreach (WheelComponent wheelComponent in Wheels)
                {
                    wheelComponent.wheelController.visualOnlyUpdate = false;
                }
            }

            InitMutliplayerState();

            base.SetMultiplayerInstanceType(instanceType);
        }

        /// <summary>
        /// Should be called after sound initialization.
        /// </summary>
        public void InitMutliplayerState()
        {
            _multiplayerState = new MultiplayerState();

            int soundComponentCount = soundManager.components.Count;
            _multiplayerState.soundVolumeArray = new float[soundComponentCount];
            _multiplayerState.soundPitchArray = new float[soundComponentCount];
            _multiplayerState.soundIsPlayingArray = new bool[soundComponentCount];

            _multiplayerState.initialized = true;
        }

        public MultiplayerState GetMultiplayerState()
        {
            return _multiplayerState;
        }

        public void UpdateMultiplayerState()
        {
            // Physics
            _multiplayerState.velocity = vehicleRigidbody.velocity;
            _multiplayerState.angVelocity = vehicleRigidbody.angularVelocity;

            // Input
            _multiplayerState.throttleInput = input.Throttle;
            _multiplayerState.brakeInput = input.Brakes;
            _multiplayerState.steeringInput = input.Steering;

            // Sound
            for (int i = 0; i < soundManager.components.Count; i++)
            {
                soundManager.components[i].GetNetworkState(out bool playing, out float volume, out float pitch);
                _multiplayerState.soundVolumeArray[i] = volume;
                _multiplayerState.soundPitchArray[i] = pitch;
                _multiplayerState.soundIsPlayingArray[i] = playing;
            }

            // Effects
            _multiplayerState.lightState = effectsManager.lightsManager.GetIntState();
        }

        public bool SetMultiplayerState(MultiplayerState inboundState)
        {
            // Physics
            vehicleRigidbody.velocity = inboundState.velocity;
            vehicleRigidbody.angularVelocity = inboundState.angVelocity;

            // Input
            input.Throttle = inboundState.throttleInput;
            input.Brakes = inboundState.brakeInput;
            input.Steering =  inboundState.steeringInput;

            // Sound
            for (int i = 0; i < soundManager.components.Count; i++)
            {
                float volume = inboundState.soundVolumeArray[i];
                float pitch = inboundState.soundPitchArray[i];
                bool isPlaying = inboundState.soundIsPlayingArray[i];
                soundManager.components[i].SetNetworkState(isPlaying, volume, pitch);
            }

            // Effects
            effectsManager.lightsManager.SetStateFromInt(inboundState.lightState);

            return true;
        }

        [System.Serializable]
        public struct MultiplayerState
        {
            public bool initialized;
            public Vector3 velocity;
            public Vector3 angVelocity;
            public float throttleInput;
            public float brakeInput;
            public float steeringInput;
            public float[] soundVolumeArray;
            public float[] soundPitchArray;
            public bool[] soundIsPlayingArray;
            public int lightState;
        }
        
        // dir1 dir2 = need to be unit vectors
        // Assumed to start at the exact same point; their direction matters
        // The result is in degrees
        // Source: https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/
        public static float CalculateSignedCentralAngle(Vector3 dir1, Vector3 dir2, Vector3 normal)
            => Mathf.Atan2(Vector3.Dot(Vector3.Cross(dir1, dir2), normal), Vector3.Dot(dir1, dir2)) * Mathf.Rad2Deg;
    }
}