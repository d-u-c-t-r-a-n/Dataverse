using System;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;

namespace Common.Actions.Entities.ChangeRequest
{
    public class ReplaceDoubleQuotationMarks : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName != "Create" || context.MessageName != "Update" || context.Stage != 20) return;
            if (context.Depth > 1) return; // guard recursion
            if (context.InputParameters.Contains("Target") && !(context.InputParameters["Target"] is Entity)) return;

            Entity entity = (Entity)context.InputParameters["Target"];
            if (!entity.LogicalName.Equals("changerequest", StringComparison.OrdinalIgnoreCase)) return;

            try
            {
                string logMessage = $"Start finding double quotation marks.{Environment.NewLine}";
                bool shouldLog = false;

                foreach (var field in new List<string> { "description", "changereason", "technicalassessment", "impactandrisk", "securityassessment" })
                {
                    if (entity.Contains(field) &&
                    entity[field] != null &&
                    entity[field].ToString().Contains("\""))
                    {
                        entity[field] = entity[field].ToString().Replace("\"", "'");
                        tracingService.Trace($"Found in {field}");
                        logMessage += $"Found in {field}.{Environment.NewLine}";
                        shouldLog = true;
                    }
                }

                if (shouldLog) tracingService.Trace(string.Join(logMessage, "Finished loop. Exit plugin."));

            }
            catch (Exception ex)
            {
                tracingService.Trace("Error when trying to replace double quotation marks.");
                tracingService.Trace(ex.Message);
                tracingService.Trace(ex.ToString());
            }

        }
    }
}

