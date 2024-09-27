namespace MBS.Core.Enums;

public enum EventStatus
{
    Confirmed,   // Event is confirmed and scheduled
    Tentative,   // Event is tentative and not yet confirmed
    Cancelled  // Event has been cancelled
}