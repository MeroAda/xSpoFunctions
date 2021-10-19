using CloudNative.CloudEvents;
using Google.Cloud.Functions.Framework;
using Google.Events.Protobuf.Cloud.PubSub.V1;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ada.Net.Lib;
using Ada.Net.Lib.Models.AdaPools;
using System.Collections.Generic;
using xSpoFunctions.Services;

namespace xSpoFunctions.Triggers
{
    public class GetxSpoDetails : ICloudEventFunction<MessagePublishedData>
    {

        public async Task HandleAsync(CloudEvent cloudEvent, MessagePublishedData data, CancellationToken cancellationToken)
        {

            xSpoService xSS = new xSpoService();

            //Get json details
            var pools = await xSS.GetxSpoExtendedDetails();

            if(pools == null)
            {
                Console.WriteLine("null Pools returned -- quitting sadly");
                return;
            }

            //Save to GCS
            var uploaded = await xSS.SavePoolDetailsToGCS(pools);
            if(uploaded)
                Console.WriteLine("Details updated!");
            else
                Console.WriteLine("Problem updating details");
        }

    }
}
