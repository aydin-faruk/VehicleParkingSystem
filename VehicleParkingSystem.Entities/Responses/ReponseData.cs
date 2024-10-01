namespace VehicleParkingSystem.Models.Responses
{
    public class ReponseData<T>
    {
        public T? Data { get; set; }
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
    }
}
