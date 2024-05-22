namespace Reservio.Helper
{
    public enum Result
    {
        Success,
        UserNotFound,
        EmailSendFailure,
        EmailAlreadyExists,
        UserAlreadyExists,
        UserNotVerified,
        PasswordNotMatch,
        ChangePasswordFailure,
        InvalidPassword,
        BadRequest,
        CreateRoomFailure,
        UploadImageFailure,
        UpdateRoomFailure,
        RoomNotFound,
        EquipmentNotFound,
        UserCannotReserve,
        RoomNotAvailable,
        ReservationFailure

    }
}
