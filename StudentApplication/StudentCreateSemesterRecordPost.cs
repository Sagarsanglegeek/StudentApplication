using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentApplication
{
    public class StudentCreateSemesterRecordPost : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            //Plugin context
            IPluginExecutionContext context=(IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            ITracingService tracingService=(ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                // D365 service object
                IOrganizationServiceFactory organizationServiceFactory=(IOrganizationServiceFactory)serviceProvider.GetService (typeof(IOrganizationServiceFactory));
                IOrganizationService service = organizationServiceFactory.CreateOrganizationService(context.UserId);

                int sem=0;

                if(context.MessageName.ToString() !="create" && context.Stage !=40 )
                {
                    return;
                }

                if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
                {
                    Entity entity = (Entity)context.InputParameters["Target"];

                    //string fetchXml = @"<fetch>
                    //                    <entity name='ss_student'>
                    //                    <attribute name='ss_studentid'/>
                    //                    <attribute name='ss_name'/>
                    //                    <attribute name='ss_course'/>
                    //                    <filter type='and'>
                    //                    <condition attribute='statecode' operator='eq' value='0'/>
                    //                    <condition attribute='ss_studentid' operator='eq' value='"+entity.Id+@"'/>
                    //                    </filter>
                    //                    <link-entity name='ss_course' from='ss_courseid' to='ss_course' link-type='inner' alias='cou'>
                    //                    <attribute name='ss_semester'/>
                    //                    <filter type='and'>
                    //                    <condition attribute='ss_semester' operator='not-null'/>
                    //                    </filter>
                    //                    </link-entity>
                    //                    </entity>
                    //                    </fetch>";

                    Guid courseGuid = Guid.Empty;//= ((Guid)(entity.GetAttributeValue<Guid>("")));

                    if(entity.Attributes.Contains("ss_course") && entity.Attributes["ss_course"] !=null)
                    {
                        courseGuid = ((EntityReference)(entity.Attributes["ss_course"])).Id;
                    }


                    string fetchXml = @"<fetch version='1.0'>
                                        <entity name='ss_course'>
                                        <attribute name='ss_courseid'/>
                                        <attribute name='ss_semester'/>
                                        <filter type='and'>
                                        <condition attribute='ss_courseid' operator='eq' value='"+ courseGuid +@"'/>
                                        </filter>
                                        </entity>
                                        </fetch>";

                    EntityCollection collection = service.RetrieveMultiple(new FetchExpression(fetchXml));

                    foreach(Entity entityRecord in collection.Entities)
                    {
                        if(entityRecord.Attributes.Contains("ss_semester") && entityRecord.Attributes["ss_semester"] !=null)
                        {
                            sem = (int)entityRecord.Attributes["ss_semester"];
                        }
                        break;
                    }

                    for(int i = 0; i<sem; i++)
                    {
                        Entity semRecord = new Entity("ss_semester");
                        semRecord["ss_name"] = "Sem" + i;
                        service.Create(semRecord);
                    }
                }

            }
            catch (Exception ex)
            {

                throw new InvalidPluginExecutionException();
            
            }

        }
    }
}
