# Redis Types
## String & Commands
Redis'te String (Dizgi) veri yapısı, Redis'in en temel türüdür ve bir anahtarla ilişkilendirilmiş tek bir değeri (metin, tamsayı veya ondalık sayı) saklar. Genellikle önbellekleme, sayım ve oturum verilerini depolama için kullanılır.

- Belirtilen anahtara bir değer atar. Önceden varsa üzerine yazar.
**set key value**
set name oguz

- Belirtilen anahtarın değerini döndürür.Yoksa (nil) döndürür.
**get key**
get name

- Belirtilen anahtarı siler. (Tüm veri tipleri için geçerlidir)
**DEL key**
DEL kullanici:token

- Anahtarın belirtilen index aralığını okur.
**getrange key 0 2**
getrange name 0 2

- Anahtarın tamsayı değerini 1 artırır.
**INCR key**
INCR ziyaretciSayisi

- Anahtarın tamsayı değerini belirtilen miktarda artırır.
**INCRBY key number**
INCRBY ziyaretciSayisi 10

- Anahtarın tamsayı değerini 1 azaltır.
**DECR key**
DECR ziyaretciSayisi

- Anahtarın tamsayı değerini belirtilen miktarda azaltır.
**DECRBY key number**
DECRBY ziyaretciSayisi 10

- Belirtilen dizeyi mevcut değerin sonuna ekler.
**APPEND key value**
APPEND name akpinar


## List & Commands
Redis'teki List (Liste) veri yapısı, dizgilerin (string) eklenme sırasına göre tutulduğu sıralı koleksiyonlardır. Listeler, hem baştan (sol) hem de sondan (sağ) hızlı ekleme ve çıkarma (push/pop) işlemlerini destekler, bu da onları kuyruk (queue) ve yığın (stack) gibi veri yapılarını uygulamak için ideal hale getirir.

Listeler, bağlantılı listeler (linked lists) olarak uygulandığı için, eleman sayısından bağımsız olarak listenin başına veya sonuna ekleme/çıkarma işlemleri çok hızlıdır (O(1) karmaşıklığa sahiptir).

- Yeni elemanları listenin başına (soluna) ekler. Birden fazla eleman eklenebilir.
LPUSH

- Yeni elemanları listenin sonuna (sağına) ekler. Birden fazla eleman eklenebilir.
RPUSH

- Listenin belirtilen aralıktaki (başlangıç ve bitiş indeksleri) elemanlarını döndürür. Listenin tamamını almak için 0 -1 kullanılır.
LRANGE

- Listenin sahip olduğu eleman sayısını (uzunluğunu) döndürür.
LLEN

- Listenin başındaki (solundaki) ilk elemanı çıkarır ve döndürür.
LPOP

- Listenin sonundaki (sağındaki) son elemanı çıkarır ve döndürür.
RPOP

- Listenin belirtilen indeksindeki elemanı döndürür.
LINDEX

## Set & Commands
Set (Küme), Redis'teki String'lerden oluşan, sırasız ve her elemanın benzersiz olduğu bir koleksiyondur.

- **Sırasız:** Listelerden farklı olarak, kümedeki elemanların belirli bir sırası yoktur; indeksleme mümkün değildir.

- **Benzersiz:** Bir eleman, bir kümede yalnızca bir kez bulunabilir. Aynı elemanı tekrar eklemeye çalışmak Redis'te herhangi bir hata vermez ancak küme boyutunu artırmaz.

- Belirtilen kümelere bir veya birden fazla benzersiz eleman ekler.
SADD

- Kümedeki tüm elemanları döndürür.
SMEMBERS

- Kümeden bir veya birden fazla elemanı çıkarır.
SREM

## Sorted Set & Commands
Sorted Set (Sıralı Küme), Redis'in benzersiz ve en güçlü veri yapılarından biridir. Bu yapı, normal Set'lerin benzersizlik özelliğini List'lerin sıralama özelliğiyle birleştirir.

**Sıralı kümedeki her eleman (member):**

- Benzersizdir (Redis'teki diğer küme yapıları gibi).

- Bir skor (score) ile ilişkilendirilir. Bu skor, elemanın küme içindeki konumunu belirleyen bir kayan nokta (float) değeridir.

**Temel Kullanım Alanları:**
Liderlik Tabloları, Öncelikli Kuyruklar, Zaman Serileri

- Küme üyesini belirtilen skor ile ekler. Üye zaten varsa, skoru günceller.
ZADD

- Belirtilen indeks aralığındaki üyeleri döndürür. Skor sırasına (küçükten büyüğe) göre sıralanır.
ZRANGE
ZRANGE WITHSCORES

- Kümeden bir veya daha fazla üyeyi kaldırır.
ZREM

## Hash & Commands
Hash (Karma Tablo), Redis'te bir anahtar altında birden çok alan-değer (field-value) çifti saklamanızı sağlayan bir veri yapısıdır. Geleneksel olarak programlama dillerinde kullanılan sözlük (dictionary), karma harita (hash map) veya nesne (object) yapılarına benzer.

**Yapı:** Her bir Redis anahtarı, yüzlerce veya binlerce alan-değer çiftini barındırabilir.

**Kullanım Alanı:** Hash'ler, bir veritabanı kaydının veya bir nesnenin özelliklerini tek bir Redis anahtarı altında toplamak için idealdir. Örneğin, bir kullanıcının adı, soyadı, e-posta adresi gibi tüm nitelikleri tek bir kullanici:123 anahtarı altında tutulabilir.

**Faydası:** İlişkili verileri tek bir yerde tutarak belleği daha verimli kullanır ve veri okuma/yazma işlemlerini daha hızlı hale getirir.


- Belirtilen Hash anahtarındaki bir alana (field) değer atar. Alan yoksa oluşturur, varsa günceller.
HSET

- Belirtilen Hash anahtarındaki tek bir alanın değerini döndürür.
HGET

- Hash anahtarından bir veya birden fazla alanı siler.
HDEL

- Hash anahtarındaki tüm alan-değer çiftlerini döndürür.
HGETALL