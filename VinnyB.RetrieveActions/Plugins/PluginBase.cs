using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveActions.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        /// <summary>
        /// LocalPluginContext
        /// </summary>
        protected class LocalPluginContext
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "LocalPluginContext")]
            internal IServiceProvider ServiceProvider { get; private set; }
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "LocalPluginContext")]
            internal IOrganizationService OrganizationService { get; private set; }
            internal IOrganizationService OrganizationServiceAdmin { get; private set; }
            internal IPluginExecutionContext PluginExecutionContext { get; private set; }
            internal IServiceEndpointNotificationService NotificationService { get; private set; }
            internal ITracingService TracingService { get; private set; }
            private LocalPluginContext() { }
            internal LocalPluginContext(IServiceProvider serviceProvider)
            {
                if (serviceProvider == null)
                {
                    throw new ArgumentNullException("serviceProvider");
                }

                PluginExecutionContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                TracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                NotificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));
                IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                OrganizationService = factory.CreateOrganizationService(PluginExecutionContext.UserId);
                OrganizationServiceAdmin = factory.CreateOrganizationService(null);
            }
            internal void Trace(string message)
            {
                if (string.IsNullOrWhiteSpace(message) || TracingService == null)
                {
                    return;
                }

                if (PluginExecutionContext == null)
                {
                    TracingService.Trace(message);
                }
                else
                {
                    TracingService.Trace(
                        "{0}, Correlation Id: {1}, Initiating User: {2}",
                        message,
                        PluginExecutionContext.CorrelationId,
                        PluginExecutionContext.InitiatingUserId);
                }
            }
            internal object InputParameters()
            {
                return PluginExecutionContext.InputParameters["Target"];
            }
            internal T GetPreImage<T>(string preImageName = "PreImage") where T : Entity
            {
                var entity = PluginExecutionContext.PreEntityImages[preImageName] as Entity;
                return entity.ToEntity<T>();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            internal T GetTarget<T>() where T : Entity
            {
                var target = PluginExecutionContext.InputParameters["Target"] as Entity;

                return target.ToEntity<T>();
            }

            internal T GetPostImage<T>(string postImageName = "PostImage") where T : Entity
            {
                var image = PluginExecutionContext.PostEntityImages[postImageName] as Entity;

                return image.ToEntity<T>();
            }

            internal T GetMergePreImage<T>(string preImageName = "PreImage") where T : Entity
            {
                var target = PluginExecutionContext.InputParameters["Target"] as Entity;
                var preImage = PluginExecutionContext.PreEntityImages[preImageName] as Entity;
                var entity = GetDeepCopy<Entity>(preImage);


                foreach (var attribute in target.Attributes)
                {
                    if (entity.Contains(attribute.Key))
                    {
                        entity[attribute.Key] = attribute.Value;
                    }
                    else
                    {
                        entity.Attributes.Add(attribute.Key, attribute.Value);
                    }
                }


                return entity.ToEntity<T>();
            }



            public static T GetDeepCopy<T>(Entity objectToCopy) where T : Entity
            {
                var entity = new Entity
                {
                    EntityState = objectToCopy.EntityState,
                    Id = objectToCopy.Id,
                    LogicalName = objectToCopy.LogicalName,
                    RowVersion = objectToCopy.RowVersion
                };


                foreach (var attribute in objectToCopy.Attributes)
                    entity.Attributes.Add(attribute.Key, attribute.Value);


                return entity.ToEntity<T>();
            }

            internal EntityReference GetEntityReference()
            {
                var target = PluginExecutionContext.InputParameters["Target"] as EntityReference;
                return target;
            }

            internal Relationship GetRelationship()
            {
                Relationship target = (Relationship)PluginExecutionContext.InputParameters["Relationship"];
                return target;
            }
            internal void AddSharedVariable(string key, object value)
            {
                this.PluginExecutionContext.SharedVariables.Add(key, value);
            }
            internal T GetSharedVariable<T>(string key)
            {
                return this.GetSharedVariableFromContext<T>(key, this.PluginExecutionContext);
            }
            private T GetSharedVariableFromContext<T>(string key, IPluginExecutionContext context)
            {
                T item = default(T);
                if (context != null && context.SharedVariables.Contains(key))
                {
                    item = (T)context.SharedVariables[key];
                }
                return item;
            }
        }

        /// <summary>
        /// ChildClassName
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "PluginBase")]
        protected string ChildClassName { get; private set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "PluginBase")]
        internal PluginBase(Type childClassName)
        {
            ChildClassName = childClassName.ToString();
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="serviceProvider"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "CrmVSSolution411.NewProj.PluginBase+LocalPluginContext.Trace(System.String)", Justification = "Execute")]
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            LocalPluginContext localcontext = new LocalPluginContext(serviceProvider);


            localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Entered {0}.Execute()", this.ChildClassName));
            try
            {
                ExecuteCrmPlugin(localcontext);
                return;
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Exception: {0}", e.ToString()));
                throw;
            }
            finally
            {
                localcontext.Trace(string.Format(CultureInfo.InvariantCulture, "Exiting {0}.Execute()", this.ChildClassName));
            }
        }

        /// <summary>
        /// ExecuteCrmPlugin
        /// </summary>
        /// <param name="localcontext"></param>
        protected virtual void ExecuteCrmPlugin(LocalPluginContext localcontext)
        {

        }
    }
}
