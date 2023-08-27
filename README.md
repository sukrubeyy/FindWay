<h1>🌟 FindWay 🌟 </h1>

<h2>🚀 Proje Tanıtımı</h2>

<p>
    Bu proje, Unity 'One Week Challenge' konsepti altında geliştirilen bir projedir. Projede bulunanlar:
</p>

<ul>
    <li>
        <strong>Generic Singleton</strong>: Proje, singleton tasarım desenini kullanarak tek bir örnek üzerinden erişilebilen yöneticileri içerir. Bu, nesneleri verimli bir şekilde yönetmek için kullanılır. 
    </li>
    <li>
```csharp
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
    </li>
    <li>
        <strong>State Pattern</strong>: Oyun içi karakter durumlarını yönetmek için durum desenini uygular. Bu, karakter davranışlarını daha yönetilebilir ve genişletilebilir hale getirir.
    </li>
    <li>
        <strong>Object Pooling</strong>: Nesne havuzlaması, performansı artırmak için sık kullanılan nesneleri önceden oluşturur ve yeniden kullanır. Bu, dinamik nesne oluşturmanın maliyetini azaltır.
    </li>
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

