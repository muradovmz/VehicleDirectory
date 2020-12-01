namespace Core.Specifications
{
    public class VehicleSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } =1;
        private int _pageSize = 5;
        public int PageSize
        {
            get=>_pageSize;
            set=>_pageSize=(value>MaxPageSize)?MaxPageSize:value;
        }

        public int? ManufacturerId { get; set; }
        public int? ModelId { get; set; }
        public int? ColorId { get; set; }
        public int? FuelTypeId { get; set; }
        public string Sort { get; set; }

        private string _search;
        public string Search
        {
            get => _search;
            set => _search=value.ToLower();
        }
    }
}