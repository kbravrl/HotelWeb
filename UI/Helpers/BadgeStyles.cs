using HotelWeb.Enums;
using Radzen;

namespace HotelWeb.UI.Helpers;

public static class BadgeStyles
{
    public static BadgeStyle ForRoomStatus(Enums.RoomStatus status) => status switch
    {
        Enums.RoomStatus.Available => BadgeStyle.Success,
        Enums.RoomStatus.Occupied => BadgeStyle.Primary,
        Enums.RoomStatus.Cleaning => BadgeStyle.Warning,
        Enums.RoomStatus.Maintenance => BadgeStyle.Danger,
        _ => BadgeStyle.Secondary
    };

    public static BadgeStyle ForRoomType(Enums.RoomType type) => type switch
    {
        Enums.RoomType.Single => BadgeStyle.Primary,
        Enums.RoomType.Double => BadgeStyle.Success,
        Enums.RoomType.Suite => BadgeStyle.Warning,
        _ => BadgeStyle.Secondary
    };

    public static BadgeStyle ForReservationStatus(Enums.ReservationStatus status) => status switch
    {
        Enums.ReservationStatus.Pending => BadgeStyle.Warning,
        Enums.ReservationStatus.Confirmed => BadgeStyle.Success,
        Enums.ReservationStatus.CheckedIn => BadgeStyle.Primary,
        Enums.ReservationStatus.CheckedOut => BadgeStyle.Secondary,
        Enums.ReservationStatus.Cancelled => BadgeStyle.Danger,
        _ => BadgeStyle.Secondary
    };

    public static BadgeStyle ForHousekeepingStatus(Enums.HousekeepingTaskStatus status) => status switch
    {
        Enums.HousekeepingTaskStatus.Pending => BadgeStyle.Warning,
        Enums.HousekeepingTaskStatus.InProgress => BadgeStyle.Primary,
        Enums.HousekeepingTaskStatus.Done => BadgeStyle.Success,
        _ => BadgeStyle.Secondary
    };
}