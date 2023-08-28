<h1>🌟 FindWay 🌟 </h1>

<h2>🚀 Proje Tanıtımı</h2>

<p>
    Bu proje, Unity 'One Week Challenge' konsepti altında geliştirilen bir projedir. Projede bulunanlar:
</p>

<ul>
    <li><a href="https://firebase.google.com">Install Firebase Realtime Database</li>
    <li><a href="https://firebase.google.com/docs/unity/setup?hl=tr">Install Firebase SDK</li>
</ul>

<ul>
    <li>
        <strong>Generic Singleton</strong>: Proje, singleton tasarım desenini kullanarak tek bir örnek üzerinden erişilebilen yöneticileri içerir. Bu, nesneleri verimli bir şekilde yönetmek için kullanılır. Generic singleton class tanımlama ve kullanım örneği aşağıda yer almaktadır.
    </li>
<pre>
<code>
public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                var objs = FindObjectsOfType (typeof(T)) as T[];
                if (objs.Length > 0)
                    _instance = objs[0];
                if (objs.Length > 1) {
                    Debug.LogError ("There is more than one " + typeof(T).Name + " in the scene.");
                }
                if (_instance == null) {
                    GameObject obj = new GameObject ();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    _instance = obj.AddComponent<T> ();
                }
            }
            return _instance;
        }
    }
}
</code>
</pre>
<img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/HowToUseSingleton.PNG"/>
<pre>
<code>
    PoolManager.Instance.GetPoolObject(PoolObjectType.Stone);
</code>    
</pre>  

<li>
        <strong>State Pattern</strong>: Oyun içi karakter durumlarını yönetmek için durum desenini uygular. Bu, karakter davranışlarını daha yönetilebilir ve genişletilebilir hale getirir. Aşağıda State Pattern oluşturup kullanma örneği verilmiştir. Play mode haricinde player controller hareket işlemlerinin çalışmasına izin verilmiyor.
    </li>
<pre>
<code>
using UnityEngine;

public class StateContext : Singleton<StateContext>
{
    [SerializeField] private State _currentState;
    public State GetCurrentState => _currentState;
    public void Transition(State state) =>_currentState = state;
}
</code>
</pre>

<pre>
    <code>
public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        StateContext.Instance.Transition(State.Playmode);
    }

    private void FixedUpdate()
    {
        if (StateContext.Instance.GetCurrentState is State.LoseState)
        {
            GameManager.Instance.LosePanelActive();
            Destroy(rb);
        }
        if (isDashing) return;
        if (StateContext.Instance.GetCurrentState is not State.Playmode)
            return;

        rb.MovePosition(transform.position + (transform.forward * input.magnitude) * Time.deltaTime * _speed);
        
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (transform.position.y < -5f)
        {
            StateContext.Instance.Transition(State.LoseState);
        }
    }
}
    </code>
</pre>
<li>
        <strong>Object Pooling</strong>: Nesne havuzlaması, performansı artırmak için sık kullanılan nesneleri önceden oluşturur ve yeniden kullanır. Bu, dinamik nesne oluşturmanın maliyetini azaltır.
    </li>
    <pre>
        <code>
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public ObjectOfPool[] objects;
    
    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].pooledObject = new Queue<GameObject>();
            for (int j = 0; j < objects[i].count; j++)
            {
                GameObject obj = Instantiate(objects[i].objectPrefab,transform);
                obj.SetActive(false);
                objects[i].pooledObject.Enqueue(obj);
            }
        }
    }

    public GameObject GetPoolObject(PoolObjectType type)
    {
        var foundObject = objects.FirstOrDefault(x => x.type == type).pooledObject.Dequeue();
        foundObject.SetActive(true);
        return foundObject;
    }

    public void SendPool(PoolObjectType type,GameObject poolObj)
    {
        poolObj.SetActive(false);
        objects.FirstOrDefault(x => x.type == type).pooledObject.Enqueue(poolObj);
    }
}
        </code>
    </pre>

<pre>
<code>
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjectOfPool
{
    public int count;
    public Queue<GameObject> pooledObject;
    public PoolObjectType type;
    public GameObject objectPrefab;
}
</code>
</pre>

<pre>
<code>
public enum PoolObjectType
{
    Stone
}
</code>
</pre>

<pre>
<code>
   GameObject stone = poolManager.GetPoolObject(PoolObjectType.Stone);
</code>
</pre>

<pre>
<code>
    PoolManager.Instance.SendPool(PoolObjectType.Stone, gameObject);
</code>
</pre>

<li>
        <strong>Firebase Realtime Database</strong>: Firebase veritabanı, oyun içi kullanıcı verilerini depolamak ve senkronize etmek için kullanılır. Oyuncu ilerlemesi, skorlar vb. gibi verileri saklamak için kullanılır. Bu projede ise, CoinCount, GetDailyBonusSystem, Customization kavramları için kullanılmıştır.
    </li>
<pre>
    <code>
using System;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
public class FirebaseManager : Singleton<FirebaseManager>
{
    private DatabaseReference reference;
    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                GetUserInformationFromFirebaseDatabase();
            }
        });
    }
    public void GetUserInformationFromFirebaseDatabase()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("Users").Child(DataManager.Instance.userInformation.GetUserId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("IS FAULTED");
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string json = snapshot.GetRawJsonValue();
                    var cloudData = JsonUtility.FromJson<UserInformation>(json);
                    if (cloudData is not null)
                    {
                        Debug.LogWarning("Firebase User Data Not Null");
                        DataManager.Instance.userInformation = cloudData;
                        MenuManager.Instance.IntializeElementsOfUI();
                        CustomizationObject.Instance.Initialize();
                        try
                        {
                            DataManager.Instance.InstanceOnDailyBonusEvent(cloudData.GetLastLoginDate);
                        }
                        catch (Exception e)
                        {
                            print(e.Message);
                            throw;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Firebase User Data Null");
                        DataManager.Instance.userInformation.Initialize();
                        CustomizationObject.Instance.Initialize();
                        Save();
                    }
                }
            });
    }
    public void Reset()
    {
        DeleteData();
        DataManager.Instance.userInformation.Initialize();
    }

    public void DeleteData()
    {
        reference.Child("Users").Child(DataManager.Instance.userInformation.GetUserId).RemoveValueAsync();
        Debug.LogWarning("User Data Delete From Cloud");
        DataManager.Instance.userInformation.SetUserID();
        DataManager.Instance.userInformation.Initialize();
        MenuManager.Instance.IntializeElementsOfUI();
    }
    public void Save()
    {
        reference.Child("Users").Child(DataManager.Instance.userInformation.GetUserId).SetRawJsonValueAsync(JsonUtility.ToJson(DataManager.Instance.userInformation));
        GetUserInformationFromFirebaseDatabase();
        MenuManager.Instance.IntializeElementsOfUI();
    }
}
    </code>
</pre>
<p>
    Firebase Realtime Database'den GetUserInformationFromFirebaseDatabase() methodu içerisinde verilerimizi çekiyoruz. Eğer verimiz varsa deserialize işlemi yapıp DataManager'a bu verileri gönderiyoruz (Verileri çekip UserInformation sınfıına deserialize edilip DataManager'da bulunan UserInformation değişkenine eşitleniyor). Eğer Database'de verimiz yok ise tüm veri sınıflarını initialize methodunu çalıştırıp default değerlerine eşitleyip firebase'e gönderiyoruz.
</p>
<pre>
    <code>
        ResetAllDataButton.onClick.AddListener(() =>
        {
            //Account ResetAllData Button
            FirebaseManager.Instance.Reset();
            ListLevel();
        });
    </code>
</pre>
<p>
    Proje içerisinde daily bonus sistemi bulunmaktadır. Oyuna ilk girdiğinizde Firebase'e bu veri "yyyy-MM-dd HH:mm:ss" formatında kaydedilir. Ardından oyuna her girişinizde Firebase'den alınan veri UserInformation tipine deserialize edildiğinde TimeSpan sınıfı kullanılarak şimdiki zaman ve kaydedilen zaman arasındaki geçen süre 1 gün ise kullanıcıya daily bonus verilmektedir. Daily bonus ekranında ise iki butonumuz yer almakta. Claim butonu default 500 coin vermekte eğer altında yer alan Double butonuna tıklarsanız 2x coin almaktasınız. Projeye reklam eklendiğinde Double butonuna reklam atamasası yapabilmek için böyle bir şey yaptım.
</p>
    <li>
        <strong>JSON Serialize ve Deserialize</strong>: Proje, oyun içi verileri JSON formatında saklamak, okumak ve yazmak için JSON serialize ve deserialize işlemlerini kullanır. Bu projede JsonUtility kullanıldı fakat sizler projenizde NewtonSoftJson kullanmaya özen gösteriniz.
    </li>
    <p>
    Levels menusunde listelenen level buttonları içerisinde serialize işlemi yapılmakta. Firebase üzerinden almış olduğumuz customization ayarlarını butona tıkladığımızda 'Assets/Customization/CustomizationSettings.json' olarak kayıt edilmekte. Kaydedilen bu json dosyası oyun sahnesine girince player objesine eklenen CharacterCustomization sınıfı içerisinde Deserialize edilmekte ve karakterin renklendirilmesi yapılmaktadır.    
    </p>
    
<pre>
<code>
public class LevelButton : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Button button;
    private int SceneIndex;
    
    public void Initialize(int sceneIndex)
    {
        SceneIndex = sceneIndex;
        button.interactable = sceneIndex <= DataManager.Instance.userInformation.GetLevelIndex;
        text.text = SceneIndex.ToString();
        button.onClick.AddListener(() =>
        {
                if (button.IsInteractable())
                {
                    string json = JsonUtility.ToJson(DataManager.Instance.userInformation.GetCustomizationSettings);
                    System.IO.File.WriteAllText(PathHelper.Path.CustomizationFolderPath+PathHelper.FileName.CustomizationJsonName,json);
                    MenuManager.Instance.SceneLoadingMenu.SetActive(true);
                    StartCoroutine(LoadLevel(SceneIndex));
                }
        });
    }

    private IEnumerator LoadLevel(int index)
    {
        AsyncOperation operation =  SceneManager.LoadSceneAsync(index);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            MenuManager.Instance.loadingBar.value = progress;
            yield return null;
        }
    }
}
</code>
</pre>

<pre>
    <code>
public class CharacterCustomization : MonoBehaviour
{
    public CustomizationSettings _customizationSettings;
    public Renderer EyesRenderer;
    public Renderer BodyRenderer;
    public List<Renderer> ArmsRenderer;
    
    private void Start()
    {
        if (File.Exists(PathHelper.Path.CustomizationFolderPath+PathHelper.FileName.CustomizationJsonName))
        {
            string path = PathHelper.Path.CustomizationFolderPath + PathHelper.FileName.CustomizationJsonName;
            string json = File.ReadAllText(path);
            _customizationSettings = JsonUtility.FromJson<CustomizationSettings>(json);
        }
        else
        {
            _customizationSettings.Initialize();
        }
         
        EyesRenderer.material.color = _customizationSettings.EyesColor;
        BodyRenderer.material.color = _customizationSettings.BodyColor;
        foreach (var arm in ArmsRenderer)
            arm.material.color = _customizationSettings.ArmsColor;
        
    }
}
    </code>
</pre>

<pre>
    <code>
    public static class PathHelper
    {
        public static class Path
        {
            public static string CustomizationFolderPath = Application.dataPath + "/Customization/";
            public static string LevelsPath = Application.dataPath + "/Levels/";
        }
        
        public static class FileName
        {
            public static string CustomizationJsonName = "CustomizationSettings.json";
        }
    }
    </code>
</pre>

<li>
    <strong>Customization</strong>: Karakterin vücut, göz ve kollarının rengini custimaze yapabilmekteyiz. Basit bir customization sistemi kurdum.
</li>
<p>
    Home ekranında Eyes, Body ve Arms butonları yer almakta. Bu butonlara tıklayarak açılan paletten renk seçimleri yapabilirsiniz. Burada düzeltilmesi gereken önemli şeylerden biri her renk değişimde UserInformation sınıfı komple firebase'e kaydedilmektedir. Burada FirebaseManager içerisinden sadece renk güncellemesi için bir method yazılıp optimize edilebilir.
    
</p>

<pre>
    <code>
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    public Color color;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private CustomizationButtonType type; 
   
    void Start()
    {
        image.color=color;
        button.onClick.AddListener(() =>
        {
            switch(type)
            {
                case CustomizationButtonType.Body:
                    CustomizationObject.Instance.SetBodyColor(color);
                    break;
                case CustomizationButtonType.Eyes:
                    CustomizationObject.Instance.SetEyesColor(color);
                    break;
                case CustomizationButtonType.Arms:
                    CustomizationObject.Instance.SetArmsColor(color);
                    break;
            }
        });
    }
}
    </code>
</pre>

<pre>
    <code>
public enum CustomizationButtonType
{
    Body,
    Eyes,
    Arms
}
    </code>
</pre>

<pre>
    <code>
using System;
using UnityEngine;

[Serializable]
public struct CustomizationSettings
{
    public Color32 EyesColor;
    public Color32 BodyColor;
    public Color32 ArmsColor;
    public void Initialize()
    {
        EyesColor = Color.red;
        BodyColor = Color.black;
        ArmsColor = Color.magenta;
    }
    public void SetEyesColor(Color32 value) => EyesColor = value;
    public void SetBodyColor(Color32 value) => BodyColor = value;
    public void SetArmsColor(Color32 value) => ArmsColor = value;
}
    </code>
</pre>

<pre>
    <code>
using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class UserInformation
{
    public int coinCount;
    [field: SerializeField] private int levelIndex;
    [field: SerializeField] private string Id { get; set; }
    [field: SerializeField] private PlayerSettings settings;
    [field: SerializeField] private string lastLoginTime;
    [field: SerializeField] private CustomizationSettings _customizationSettings;

    public CustomizationSettings GetCustomizationSettings => _customizationSettings;
    public void Initialize()
    {
        coinCount = 0;
        levelIndex = 1;
        lastLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        settings.Initialize();
        _customizationSettings.Initialize();
    }

    public void SetUserID()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
    }
    public int GetCoinCount => coinCount;
    public string GetUserId => Id;
    public void IncreaseCoinCount() => coinCount++;
    public void DecriseCoinCount() => coinCount--;
    public void IncreaseLevel() => levelIndex++;
    public int GetLevelIndex => levelIndex;
    public DateTime GetLastLoginDate => DateTime.ParseExact(lastLoginTime, "yyyy-MM-dd HH:mm:ss", null);
    public void SetCoin(int value) => coinCount = value;
    public void SetVolume(float value) => settings.SetVolume(value);
    public float GetVolume() => settings.MusicVolume;
    public bool IsVibrate() => settings.IsVibrate;
    public bool IsAdmob() => settings.isAdmob;
    public void SetVibrate(bool toggleValue) => settings.SetVibrate(toggleValue);
    public void SetAdmob(bool toggleValue) => settings.SetAdmob(toggleValue);
    public void UpdateLastLogin()
    {
        lastLoginTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        FirebaseManager.Instance.Save();
    }

    public void SetEyesColor(Color32 value)
    {
        _customizationSettings.SetEyesColor(value);  
    }

    public Color32 GetEyesColor => _customizationSettings.EyesColor;
    public Color32 GetBodyColor => _customizationSettings.BodyColor;
    public Color32 GetArmsColor => _customizationSettings.ArmsColor;
    public void SetBodyColor(Color32 value)
    {
        _customizationSettings.SetBodyColor(value);
    }

    public void SetArmsColor(Color32 value)
    {
        _customizationSettings.SetArmsColor(value);
    }
}
    </code>
</pre>

<li>
    <strong>Interfaces</strong>: IMovable ve IFracturable adında iki tane interface bulunmakta. 
</li>

<p>
    <b>IMovable</b> taşların atılabileceği objelerde bulunmakta örneğin buton tarzı gibi bişey düşünebilirsiniz. MovableButton sınıfı ise bu interface'den kalıtım almakta. Target obje ve pozisyon bilgilerine göre execute methodu tetiklendiğinde obje hedef pozisyonuna gönderilmektedir.
</p>

<pre>
    <code>
public interface IMovable
{
    public void Execute();
}
    </code>
</pre>

<pre>
    <code>
public class MoveableButton : MonoBehaviour, IMovable
{
    [SerializeField] private GameObject targetObject;
    public Vector3 targetPos;
    public void Execute()
    {
        LeanTween.move(targetObject,targetPos,1.5f);
    }
}
    </code>
</pre>
<p>
    Aşağıda Stone prefabının scripti bulunmakta. Stone objesinin temas ettiği obje IMovable interface barındırıyorsa execute methodu çalıştırılmakta.
</p>
<pre>
    <code>
public class Stone : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<IMovable>()?.Execute();
        StartCoroutine(CoroutineSendPool());
    }

    private IEnumerator CoroutineSendPool()
    {
        yield return new WaitForSeconds(0.5f);
        PoolManager.Instance.SendPool(PoolObjectType.Stone, gameObject);
    }
}
    </code>
    </pre>
    <p>
    <b>IFracturable</b> dash atarak çarptığımız objenin fırlamasını sağlayan interface'dir. Execute methodu transform bulundurmaktadır, bu parametrede karakterin direction forward bilgisini gönderip bu  bilgi doğrultusunda objeyi fırlatıyoruz.
    </p>
    
<pre>
    <code>
    public interface IFracturable
    {
        public void ExecuteFracture(Transform direction);
    }
    </code>
</pre>

<pre>
    <code>
public class Fracture : MonoBehaviour, IFracturable
{
    private float throwForce = 30f;
    private Rigidbody rb;
    public void ExecuteFracture(Transform direction)
    {
        if (rb is null)
            rb = gameObject.AddComponent<Rigidbody>();
        
        rb.AddForce(direction.forward * throwForce, ForceMode.Impulse);
        StartCoroutine(RemoveRigidbody());
        IEnumerator RemoveRigidbody()
        {
            yield return new WaitForSeconds(2f);
            GetComponent<BoxCollider>().isTrigger = true;
            Destroy(rb);
        }
    }
}
    </code>
</pre>

</ul>

<h2> FindWay Images And Tutorials </h2>
<ol>
    <li>
       <h3>Tutorial State</h3>
        <ul>
             <li>Movement</li>
             <img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/MovementTutorial.PNG"/>
             <li>Dash</li>
             <img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/DashGif.gif"/>
             <img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/MovementTutorial.PNG"/>
             <li>Stone Throw</li>
             <img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/ThrowGif.gif"/>
             <li>Coyo Jumping</li>
             <img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/CoyoJumping.gif"/>
        </ul>
       <li>
            <h3>Customization</h3>
           <ul>
               <img src="https://github.com/sukrubeyy/FindWay/blob/main/Assets/Images/CustomizationGif.gif"/>
           </ul>
       </li>
    </li>
</ol>

<h2>🛠️ Nasıl Başlanır?</h2>

<p>
    Projeyi klonlamak için aşağıdaki komutu kullanabilirsiniz:
</p>

<code>git clone https://github.com/sukrubeyy/FindWay.git</code>

<h2>👥 Katkı Sağlama</h2>

<p>
    Eğer bu projeye katkıda bulunmak isterseniz, aşağıdaki adımları izleyebilirsiniz:
</p>

<ol>
    <li>Bu repo'yu forklayın.</li>
    <li>Yeni bir feature branch oluşturun: <code>git checkout -b yeni-özellik</code></li>
    <li>Yaptığınız değişiklikleri commitleyin: <code>git commit -am 'Yeni özellik: Açıklama'</code></li>
    <li>Branch'inizi uzak sunucuya pushlayın: <code>git push origin yeni-özellik</code></li>
    <li>Pull request oluşturun ve değişikliklerinizi açıklayın.</li>
</ol>

<b>Eklenebilecek Örnek Özellikler</b>
<ol type="A">
    <li>Admob Reklamları</li>
    <li>Firebase Notification & Analytics</li>
    <li>EndPoint ve MovableButton İçin Shader Duzenlemeleri</li>
</ol>

