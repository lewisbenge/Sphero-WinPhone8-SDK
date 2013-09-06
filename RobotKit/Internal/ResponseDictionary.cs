using System;
using System.Collections.Generic;

namespace RobotKit.Internal
{
    public class ResponseDictionary
    {
        private Byte _deviceId;

        private Dictionary<Byte, MetaResponse> availableResponses = new Dictionary<Byte, MetaResponse>();

        public ResponseDictionary(Byte deviceId)
        {
            _deviceId = deviceId;
        }

        public void fillWithDemoData()
        {
            MetaResponse metaResponse = new MetaResponse(2, 34);
            metaResponse.addField(new MetaField("red"));
            metaResponse.addField(new MetaField("green"));
            metaResponse.addField(new MetaField("blue"));
        }

        public Response responseFromBytes(Byte[] response)
        {
            Byte num = response[0];
            Byte num1 = response[1];
            Byte num2 = response[2];
            Byte num3 = response[3];
            throw new NotImplementedException();
        }
    }
}