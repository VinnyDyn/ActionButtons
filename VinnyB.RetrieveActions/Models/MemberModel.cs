using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveActions.Models
{
    [DataContract]
    public class MemberModel
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }
        
        [DataMember]
        public bool Required { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
