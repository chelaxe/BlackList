using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BlackList
{
    public class ZapretSOAPServices
    {
        public static Int64 LastDumpDate()
        {
            Int64 lastDumpDate = 0;

            using (ChannelFactory<ServiceReference.OperatorRequestPortType> scf = new ChannelFactory<ServiceReference.OperatorRequestPortType>(
                new BasicHttpBinding(), new EndpointAddress("http://vigruzki.rkn.gov.ru/services/OperatorRequest/")))
            {
                ServiceReference.OperatorRequestPortType channel = scf.CreateChannel();
                ServiceReference.getLastDumpDateResponse glddr = channel.getLastDumpDate(new ServiceReference.getLastDumpDateRequest());
                lastDumpDate = glddr.lastDumpDate;
            }

            return lastDumpDate;
        }

        public static Boolean SendRequest(out String resultComment, out String code, Byte[] requestFile, Byte[] signatureFile)
        {
            Boolean result = false;
            code = null;

            using (ChannelFactory<ServiceReference.OperatorRequestPortType> scf = new ChannelFactory<ServiceReference.OperatorRequestPortType>(
                new BasicHttpBinding(), new EndpointAddress("http://vigruzki.rkn.gov.ru/services/OperatorRequest/")))
            {
                ServiceReference.OperatorRequestPortType channel = scf.CreateChannel();
                ServiceReference.sendRequestRequestBody srrb = new ServiceReference.sendRequestRequestBody();

                srrb.requestFile = requestFile;
                srrb.signatureFile = signatureFile;

                ServiceReference.sendRequestResponse srr = channel.sendRequest(new ServiceReference.sendRequestRequest(srrb));

                resultComment = srr.Body.resultComment;

                if (result = srr.Body.result)
                {
                    code = srr.Body.code;
                }
            }

            return result;
        }

        public static Boolean GetResult(out String resultComment, out Byte[] registerZipArchive, String code)
        {
            Boolean result = false;
            registerZipArchive = null;

            using (ChannelFactory<ServiceReference.OperatorRequestPortType> scf = new ChannelFactory<ServiceReference.OperatorRequestPortType>(
                new BasicHttpBinding(), new EndpointAddress("http://vigruzki.rkn.gov.ru/services/OperatorRequest/")))
            {
                ServiceReference.OperatorRequestPortType channel = scf.CreateChannel();
                ServiceReference.getResultRequestBody grrb = new ServiceReference.getResultRequestBody();

                grrb.code = code;

                ServiceReference.getResultResponse grr = channel.getResult(new ServiceReference.getResultRequest(grrb));

                resultComment = grr.Body.resultComment;

                if (result = grr.Body.result)
                {
                    registerZipArchive = grr.Body.registerZipArchive;
                }
            }

            return result;
        }
    }
}
