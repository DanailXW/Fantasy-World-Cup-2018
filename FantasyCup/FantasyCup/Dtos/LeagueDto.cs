namespace FantasyCup.Dtos
{
    public class LeagueDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasPotMoney { get; set; }
        public decimal PotAmount { get; set; }
        public int Code { get; set; }
    }

    public class LeagueDtoSecure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasPotMoney { get; set; }
        public decimal PotAmount { get; set; }
    }
}
