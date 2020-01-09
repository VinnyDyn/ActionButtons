using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveActions.Models
{
    [DataContract]
    public class RetrieveActionsResponseModel
    {
        public RetrieveActionsResponseModel(List<ActionModel> actions)
        {
            Actions = actions;
        }

        [DataMember]
        public List<ActionModel> Actions { get; set; }
    }
}
