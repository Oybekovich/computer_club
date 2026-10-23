namespace Kompyuterxona.Shared.Dtos;

/// <summary>
/// Kompyuterning joriy holati. Bazada saqlanmaydi — server xotirasida hisoblanadi
/// (client ulanganmi va unda faol sessiya bormi).
/// </summary>
public enum ComputerState
{
    /// <summary>Client server bilan ulanmagan (o'chiq yoki tarmoqda yo'q).</summary>
    Offline = 0,

    /// <summary>Client ulangan, lekin sessiya yo'q — kompyuter qulflangan.</summary>
    Idle = 1,

    /// <summary>Faol sessiya bor, vaqt sanalmoqda.</summary>
    Active = 2
}
