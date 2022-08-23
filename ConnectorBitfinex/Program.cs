using Bitfinex.Net.Clients;
using Bitfinex.Net.Enums;
using Bitfinex.Net.Interfaces.Clients;
using Bitfinex.Net.Objects;
using Bitfinex.Net.Objects.Models;
using ConnectorBitfinex;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

const string apiKey = "mrsRVEMEdMVg1FnuLPB5sDucOhW50iLIzAtXTIkF3xi";
const string secretKey = "mnUrTA6BilO7iywrm0bOk1g0lCde6fNiTx9w4ZmX03M";

var _restClient = new BitfinexClient(new BitfinexClientOptions()
{
    ApiCredentials = new ApiCredentials(apiKey,secretKey),
    LogLevel = LogLevel.Trace,
    RequestTimeout = TimeSpan.FromSeconds(10)
});
var _socketClient = new BitfinexSocketClient(new BitfinexSocketClientOptions()
{
    ApiCredentials = new ApiCredentials(apiKey, secretKey),
    LogLevel = LogLevel.Trace,
    AutoReconnect = true,
});

BufferBlock<BitfinexOrderBookEntry> _bufferblock = new BufferBlock<BitfinexOrderBookEntry>();
var _actionBlock = new ActionBlock<BitEntity>(data =>
{
    Console.WriteLine("Count = " + data.Count +
                       "\nPrice = " + data.Price +
                       "\nQuantity = " + data.Quantity + 
                       "\n----------------");
});

TransformBlock<BitfinexOrderBookEntry, BitEntity> _transformblock = new TransformBlock<BitfinexOrderBookEntry, BitEntity>(data =>
{
    return new BitEntity()
    {
        Count = data.Count,
        Price = data.Price,
        Quantity = data.Quantity
    };
});

var order = await _socketClient.SpotStreams.SubscribeToOrderBookUpdatesAsync("tBTCUST", Precision.PrecisionLevel0, Frequency.Realtime, 25, async order =>
{
    foreach (var orderItem in order.Data)
    {
        await _bufferblock.SendAsync(orderItem);
    }
});

_bufferblock.LinkTo(_transformblock);
_transformblock.LinkTo(_actionBlock);

Console.Read();
