namespace EppLib.Entities
{
    public class PostalInfo
    {
        public string m_type;
        public string m_name;

        /// <summary>
        /// If the contact name is an individual, an organization can be entered. Otherwise, it must be omitted.
        /// </summary>
        public string m_org;

        public PostalAddress m_address;
    }
}