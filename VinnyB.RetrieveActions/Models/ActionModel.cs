using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VinnyB.RetrieveActions.Earlybounds;

namespace VinnyB.RetrieveActions.Models
{
    [DataContract]
    public class ActionModel
    {
        public ActionModel(Workflow workflow)
        {
            Id = workflow.Id;
            UniqueName = workflow.UniqueName;
            this.DeserializeWorkflowXAML(workflow.Xaml);
        }

        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string UniqueName { get; set; }
        [DataMember]
        public List<MemberModel> InputParameters { get; set; }
        [DataMember]
        public List<MemberModel> OutputParameters { get; set; }


        private void DeserializeWorkflowXAML(string XAML)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Activity));
            using (TextReader reader = new StringReader(XAML))
            {
                Activity result = (Activity)serializer.Deserialize(reader);
                //Filter valid members
                var members = result.Members.Where(w => w.PropertyAttributes != null).ToList();

                if (members != null)
                {
                    this.InputParameters = members
                        .Where(w => w.PropertyAttributes.ArgumentDirectionAttribute.Value == "Input")
                        .Select(s => new MemberModel()
                        {
                            Name = s.Name,
                            Type = s.Type,
                            Required = Convert.ToBoolean(s.PropertyAttributes.ArgumentRequiredAttribute.Value),
                            Description = s.PropertyAttributes.ArgumentDescriptionAttribute != null ? s.PropertyAttributes.ArgumentDescriptionAttribute.Value : string.Empty
                        }).ToList();

                    this.OutputParameters = members
                        .Where(w => w.PropertyAttributes.ArgumentDirectionAttribute.Value == "Output")
                        .Select(s => new MemberModel()
                        {
                            Name = s.Name,
                            Type = s.Type,
                            Required = Convert.ToBoolean(s.PropertyAttributes.ArgumentRequiredAttribute.Value),
                            Description = s.PropertyAttributes.ArgumentDescriptionAttribute != null ? s.PropertyAttributes.ArgumentDescriptionAttribute.Value : string.Empty
                        }).ToList();
                }
            }
        }
    }
}
