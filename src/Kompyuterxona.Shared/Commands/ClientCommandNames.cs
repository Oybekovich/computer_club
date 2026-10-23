namespace Kompyuterxona.Shared.Commands;

/// <summary>
/// Server client'ga yuboradigan buyruqlarning nomlari.
/// Satrni ikki joyda (serverda va client'da) qo'lda yozsak, bitta harf xato bo'lsa
/// buyruq jim turib ishlamaydi. Shuning uchun nomlar faqat shu yerda yoziladi.
/// </summary>
public static class ClientCommandNames
{
    /// <summary>Sessiya boshlandi: qolgan vaqt va ruxsat etilgan dasturlar ro'yxati.</summary>
    public const string SessionStarted = nameof(SessionStarted);

    /// <summary>Vaqt o'zgardi (admin vaqt qo'shdi).</summary>
    public const string TimeUpdated = nameof(TimeUpdated);

    /// <summary>Sessiya tugadi: sabab bilan.</summary>
    public const string SessionEnded = nameof(SessionEnded);

    /// <summary>Ruxsat etilgan dasturlar ro'yxati o'zgardi.</summary>
    public const string AllowedAppsChanged = nameof(AllowedAppsChanged);

    /// <summary>Belgilangan dasturni ochish.</summary>
    public const string LaunchApp = nameof(LaunchApp);

    /// <summary>Ekranda mijozga xabar ko'rsatish.</summary>
    public const string ShowMessage = nameof(ShowMessage);

    /// <summary>Kompyuterni o'chirish.</summary>
    public const string Shutdown = nameof(Shutdown);

    /// <summary>Kompyuterni qayta yuklash.</summary>
    public const string Restart = nameof(Restart);
}
