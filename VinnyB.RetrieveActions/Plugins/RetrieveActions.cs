using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveActions.Earlybounds;
using VinnyB.RetrieveActions.Models;
using VinnyB.RetrieveActions.Plugins;

namespace VinnyB.RetrieveActions.Plugins
{
    public class RetrieveActions : PluginBase
    {
        public RetrieveActions() : base(typeof(RetrieveActions)) { }

        /// <summary>
        /// Classe principal que chama os métodos.
        /// </summary>
        /// <param name="localContext"> Contexto de execução. </param>

        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
        {
            if (localContext == null) { throw new InvalidPluginExecutionException("Contexto não localizado!"); };

            //Logical Name
            string primaryEntity = (string)localContext.PluginExecutionContext.InputParameters["PrimaryEntity"];

            //Retrieve All Action By 
            using (CrmServiceContext crmServiceContext = new CrmServiceContext(localContext.OrganizationServiceAdmin))
            {
                var actions = crmServiceContext.WorkflowSet
                    .Join(crmServiceContext.SdkMessageSet,
                        wfl => wfl.SdkMessageId.Id,
                        sdk => sdk.Id,
                        (wfl, sdk) => new { Workflow = wfl, SdkMessage = sdk })
                    .Where(w => w.Workflow.StateCode == WorkflowState.Activated
                            && w.Workflow.CategoryEnum == Workflow_Category.Action
                            && w.Workflow.TypeEnum == Workflow_Type.Definition
                            && w.Workflow.PrimaryEntity == primaryEntity)
                    .Select(s => new Workflow()
                    {
                        Id = s.Workflow.Id,
                        UniqueName = s.SdkMessage.Name,
                        Xaml = s.Workflow.Xaml,
                    }).ToList();

                var actionModels = new List<ActionModel>();
                foreach (var workflow_ in actions)
                {
                    ActionModel actionModel = new ActionModel(workflow_);
                    actionModels.Add(actionModel);
                }

                RetrieveActionsResponseModel response = new RetrieveActionsResponseModel(actionModels);

                string jsonSerializer = string.Empty;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(response.GetType());
                    serializer.WriteObject(memoryStream, response);
                    jsonSerializer = Encoding.UTF8.GetString(memoryStream.ToArray());
                }

                localContext.PluginExecutionContext.OutputParameters["RetrieveActionsResponseModel"] = jsonSerializer;
            }
        }
    }
}