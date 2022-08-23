using Bitfinex.Net.Clients;
using Bitfinex.Net.Objects;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectorBitfinex
{
    public class SocketConnection
    {
        BitfinexClient _restClient = new BitfinexClient(new BitfinexClientOptions()
        {
            ApiCredentials = new ApiCredentials("mrsRVEMEdMVg1FnuLPB5sDucOhW50iLIzAtXTIkF3xi", "mnUrTA6BilO7iywrm0bOk1g0lCde6fNiTx9w4ZmX03M"),
            LogLevel = LogLevel.Trace,
            RequestTimeout = TimeSpan.FromSeconds(10)
        });
        BitfinexSocketClient _socketClient = new BitfinexSocketClient(new BitfinexSocketClientOptions()
        {
            ApiCredentials = new ApiCredentials("mrsRVEMEdMVg1FnuLPB5sDucOhW50iLIzAtXTIkF3xi", "mnUrTA6BilO7iywrm0bOk1g0lCde6fNiTx9w4ZmX03M"),
            LogLevel = LogLevel.Trace,
            AutoReconnect = true,
        });
       
    }
}
