namespace SearchOutlets.JSON.Models
{
    /// <summary>
    /// Internal representation of an item from the Contacts.json file
    /// </summary>
    public class Contact : ProfileData
    {
        public int OutletId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Profile { get; set; }
    }
}