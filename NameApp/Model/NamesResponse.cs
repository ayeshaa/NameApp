using System.Collections.Generic;

namespace NameApp.Model
{
    public class NamesResponse
    {
        public DefaultResponse DefaultResponse
        {
            get;
            set;
        }
        public List<Names> Names
        {
            get; set;
        }
    }
}
