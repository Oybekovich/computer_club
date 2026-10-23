namespace Kompyuterxona.Shared;

/// <summary>
/// Admin va Client uchun umumiy o'zgarmas qiymatlar.
/// Bitta joyda turganligi uchun manzil yoki port o'zgarsa, faqat shu faylni tahrirlash kifoya.
/// </summary>
public static class Constants
{
    /// <summary>Server standart holatda shu portda tinglaydi.</summary>
    public const int DefaultServerPort = 5000;

    /// <summary>Client kompyuterlar ulanadigan SignalR manzili.</summary>
    public const string ClientHubPath = "/hubs/client";

    /// <summary>Admin paneli (brauzer) ulanadigan SignalR manzili.</summary>
    public const string AdminHubPath = "/hubs/admin";

    /// <summary>Client har necha soniyada "men tirikman" signalini yuboradi.</summary>
    public const int HeartbeatSeconds = 15;

    /// <summary>
    /// Server client'ni oflayn deb belgilashdan oldin necha soniya kutadi.
    /// Bitta heartbeat yo'qolsa ham client darhol oflayn bo'lib qolmasligi uchun zahira bilan olingan.
    /// </summary>
    public const int OfflineAfterSeconds = HeartbeatSeconds * 3;

    /// <summary>Bir soatdagi soniyalar soni — pul va vaqt hisoblashda ishlatiladi.</summary>
    public const int SecondsPerHour = 3600;

    /// <summary>
    /// Ma'lumotlar papkasi: Windows'da C:\ProgramData\Kompyuterxona.
    /// Baza, loglar va zaxiralar shu yerda turadi (dastur fayllaridan alohida).
    /// </summary>
    public static string DataFolder => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
        "Kompyuterxona");

    /// <summary>Client kompyuterdagi ma'lumotlar papkasi (config va loglar).</summary>
    public static string ClientDataFolder => Path.Combine(DataFolder, "Client");
}
