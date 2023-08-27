<h1>🌟 FindWay 🌟 </h1>

<h2>🚀 Proje Tanıtımı</h2>

<p>
    Bu proje, Unity 'One Week Challenge' konsepti altında geliştirilen bir projedir. Projede bulunanlar:
</p>

<ul>
    <li>
        <strong>Generic Singleton</strong>: Proje, singleton tasarım desenini kullanarak tek bir örnek üzerinden erişilebilen yöneticileri içerir. Bu, nesneleri verimli bir şekilde yönetmek için kullanılır. Generic singleton class tanımlama ve kullanım örneği aşağıda yer almaktadır.
    </li>
<pre>
<code>
using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance is null)
                _instance = FindAnyObjectByType(typeof(T)) as T;
            return _instance;
        }
    }
}
</code>
</pre>
    
<pre>
<code>
    public class PoolManager : Singleton<PoolManager>{}
</code>
</pre>

<pre>
<code>
    PoolManager.Instance.GetPoolObject(PoolObjectType.Stone);
</code>    
</pre>  

<li>
        <strong>State Pattern</strong>: Oyun içi karakter durumlarını yönetmek için durum desenini uygular. Bu, karakter davranışlarını daha yönetilebilir ve genişletilebilir hale getirir. Aşağıda State Pattern oluşturup kullanma örneği verilmiştir. Playemode haricinde player controller hareket işlemlerinin çalışmasına izin verilmiyor.
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
            gameManager.LosePanelActive();
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
    <li>
        <strong>Firebase Realtime Database</strong>: Firebase veritabanı, oyun içi kullanıcı verilerini depolamak ve senkronize etmek için kullanılır. Oyuncu ilerlemesi, skorlar vb. gibi verileri saklamak için kullanılır.
    </li>
     <li>
        <strong>Firebase Realtime Database</strong>: Firebase veritabanı, oyun içi kullanıcı verilerini depolamak ve senkronize etmek için kullanılır. Oyuncu ilerlemesi, skorlar vb. gibi verileri saklamak için kullanılır.
    </li>
    <li>
        <strong>JSON Serialize ve Deserialize</strong>: Proje, oyun içi verileri JSON formatında saklamak, okumak ve yazmak için JSON serialize ve deserialize işlemlerini kullanır.
    </li>
    <li>
        <strong>Customization</strong>: Oyunculara karakterlerini, araçlarını veya dünyalarını özelleştirme imkanı sunar. Bu, kullanıcı deneyimini kişiselleştirmeyi destekler.
    </li>
    <li>
        <strong>Google AdMob Reklamları</strong>: Proje, Google AdMob'u entegre ederek reklam gösterimini yönetir. Bu, oyun gelirini artırmak için kullanılır.
    </li>

    
    
</ul>

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

