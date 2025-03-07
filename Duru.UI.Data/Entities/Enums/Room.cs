namespace Duru.UI.Data.Entities.Enums;

public enum RoomStatus
{
    Available,       // Müsait
    Booked,          // Rezerve edilmiş
    Occupied,        // Dolu (Misafir konaklıyor)
    Cleaning,        // Temizlik yapılıyor
    Maintenance,     // Bakımda
    OutOfService     // Kullanılamaz (Sorunlu)
}

public enum RoomType
{
    Single,         // Tek kişilik
    Double,         // Çift kişilik
    Twin,           // İki ayrı yataklı
    Suite,          // Süit oda
    Family,         // Aile odası
    Deluxe,         // Lüks oda
    Presidential    // Başkanlık süiti
}