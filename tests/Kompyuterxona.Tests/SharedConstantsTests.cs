using Kompyuterxona.Shared;

namespace Kompyuterxona.Tests;

/// <summary>
/// 0-bosqich: test tizimi ishlayotganini tekshiradigan oddiy testlar.
/// Haqiqiy hisob-kitob testlari 1-bosqichda (BillingService bilan) qo'shiladi.
/// </summary>
public class SharedConstantsTests
{
    [Fact]
    public void DefaultServerPort_IsFiveThousand()
    {
        Assert.Equal(5000, Constants.DefaultServerPort);
    }

    [Fact]
    public void SecondsPerHour_IsCorrect()
    {
        // Pul va vaqt hisoblash shu qiymatga tayanadi, shuning uchun tekshiramiz.
        Assert.Equal(60 * 60, Constants.SecondsPerHour);
    }

    [Fact]
    public void ClientDataFolder_IsInsideDataFolder()
    {
        Assert.StartsWith(Constants.DataFolder, Constants.ClientDataFolder);
    }
}
