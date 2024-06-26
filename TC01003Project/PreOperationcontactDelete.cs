
// <copyright file="PreOperationcontactDelete.cs" company="">
// Copyright (c) 2024 All Rights Reserved
// </copyright>
// <author></author>
// <date>6/21/2024 5:50:24 PM</date>
// <summary>Implements the PreOperationcontactDelete Plugin.</summary>
// <auto-generated>
//     This code was generated by a tool.
// </auto-generated>

using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace TC_01003.TC01003Project
{

    /// <summary>
    /// PreOperationcontactDelete Plugin.
    /// </summary>    
    public class PreOperationcontactDelete: PluginBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreOperationcontactDelete"/> class.
        /// </summary>
        /// <param name="unsecure">Contains public (unsecured) configuration information.</param>
        /// <param name="secure">Contains non-public (secured) configuration information.</param>
        public PreOperationcontactDelete(string unsecure, string secure)
            : base(typeof(PreOperationcontactDelete))
        {
            
           // TODO: Implement your custom configuration handling.
        }


        /// <summary>
        /// Main entry point for he business logic that the plug-in is to execute.
        /// </summary>
        /// <param name="localContext">The <see cref="LocalPluginContext"/> which contains the
        /// <see cref="IPluginExecutionContext"/>,
        /// <see cref="IOrganizationService"/>
        /// and <see cref="ITracingService"/>
        /// </param>
        /// <remarks>
        /// </remarks>
        protected override void ExecuteCdsPlugin(ILocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new InvalidPluginExecutionException(nameof(localContext));
            }           
            // Obtain the tracing service
            ITracingService tracingService = localContext.TracingService;

            try
            { 
                // Obtain the execution context from the service provider.  
                IPluginExecutionContext context = (IPluginExecutionContext)localContext.PluginExecutionContext;

                // Obtain the organization service reference for web service calls.  
                IOrganizationService currentUserService = localContext.CurrentUserService;

                if (context.InputParameters.Contains("Target") &&
                    context.InputParameters["Target"] is EntityReference)
                {
                    Entity preImageContact = context.PreEntityImages["DeleteImage"];
                    var emailAddress = preImageContact.GetAttributeValue<string>("emailaddress1");
                    var fullName = preImageContact.GetAttributeValue<string>("fullname");
                    var contactParams = context.InputParameters["Target"] as EntityReference;
                    var testCustomAction = new OrganizationRequest()
                    {
                        RequestName = "new_SendCustomEmailAction"
                    };
                    testCustomAction.Parameters.Add("Sender",
                        new EntityReference("systemuser", context.UserId));
                    testCustomAction.Parameters.Add("Subject", $"Contact {fullName} was deleted {DateTime.Now}");
                    testCustomAction.Parameters.Add("Body", $"Contact was deleted!");
                    testCustomAction.Parameters.Add("RecepientEmail", emailAddress);

                    var response = currentUserService.Execute(testCustomAction);
                }
            }	
            // Only throw an InvalidPluginExecutionException. Please Refer https://go.microsoft.com/fwlink/?linkid=2153829.
            catch (Exception ex)
            {
                tracingService?.Trace("An error occurred executing Plugin TC_01003.TC01003Project.PreOperationcontactDelete : {0}", ex.ToString());
                throw new InvalidPluginExecutionException("An error occurred executing Plugin TC_01003.TC01003Project.PreOperationcontactDelete .", ex);
            }	
        }
    }
}
