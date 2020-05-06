namespace Lappleken.Web.Models.Home
{
    public class TeamViewModel
    {
        public int TeamId { get; }
        public string Name { get; }

        public TeamViewModel(int teamId, string name)
        {
            TeamId = teamId;
            Name = name;
        }
    }
}