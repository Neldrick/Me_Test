using ProtoBuf;
namespace MatchingEngine.Modules.DataType
{
    [ProtoContract]
    public class Market{
        [ProtoMember(1)]
        public string name{get;set;}
        [ProtoMember(2)]
        public string stock{get;set;}
        [ProtoMember(3)]
        public string money {get;set;}
        [ProtoMember(4)]
        public decimal min_price{get;set;}     
        [ProtoMember(5)]
        public decimal fees {get;set;}   

    }

    /*{ "name": "BTCUSD",
        "stock":"BTC",
        "money":"USD",
        "min_amount": "0.5"
      }, */
}